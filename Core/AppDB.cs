﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Security.Claims;
using SqlSugar;
using SqlSugar.IOC;
using Microsoft.AspNetCore.Http;

namespace System
{
    /// <summary>
    /// 数据应用功能
    /// 
    /// Example:
    ///     (Sync): 
    ///         return AppDB.Execute((db) => { return AppResult.Success(); });
    ///     (Async): 
    ///         return await AppDB.Execute(async (db) => { return AppResult.Success(); });
    /// </summary>
    public class AppDB : IDisposable
    {
        /// <summary>
        /// Sugar 客户端
        /// </summary>
        private readonly SqlSugarScope _db = DbScoped.SugarScope;

        /// <summary>
        /// 当前用户信息
        /// </summary>
        private static UserInfo? User => JsonConvert.DeserializeObject<UserInfo>(ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "CurrentUser")?.Value! ?? string.Empty);

        /// <summary>
        /// 是否开启事务
        /// </summary>
        private readonly bool _doTran;

        /// <summary>
        /// 析构参数
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// 缓存异常
        /// </summary>
        private Exception? _exception = null;

        /// <summary>
        /// 执行代码块
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="func">待执行的代码块包装方法</param>
        /// <param name="doTran">是否开启事务</param>
        /// <returns>代码块执行结果</returns>
        public static async Task<TResult> Execute<TResult>(Func<AppDB, Task<TResult>> func, bool doTran = true)
        {
            // 创建实例
            using var db = new AppDB(doTran);

            try
            {
                // 执行并等待析构提交
                return await func(db);
            }
            catch (Exception ex)
            {
                // 回滚数据并抛出异常
                db.ThrowException(ex);
                throw;
            }

        }

        /// <summary>
        /// ADO
        /// </summary>
        public IAdo Ado => _db.Ado;

        #region Query
        /// <summary>
        /// 单表查询功能
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <returns></returns>
        /// <remarks>自动化过滤</remarks>
        public ISugarQueryable<TabelClass> Queryable<TabelClass>(params Type[] ignores)
        {
            var queryable = _db.Queryable<TabelClass>();
            var list = new List<IConditionalModel>();
            var type = typeof(TabelClass);
            if (type.IsAssignableTo(typeof(ISoftDelete)) && !ignores.Contains(typeof(ISoftDelete)))
            {
                /**************    软删除    ***************/
                list.Add(new ConditionalModel
                {
                    FieldName = "DeletedTime",
                    ConditionalType = ConditionalType.EqualNull
                });
            }
            if (type.IsAssignableTo(typeof(IMultiTenant)) && !ignores.Contains(typeof(IMultiTenant)))
                list.Add(new ConditionalModel
                {
                    FieldName = "TenantId",
                    ConditionalType = ConditionalType.Equal,
                    FieldValue = (User?.TenantId ?? 0).ToString(),
                    FieldValueConvertFunc = it => Convert.ToInt32(it)
                });
            return queryable.Where(list);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="datas">待插入数据</param>
        /// <returns></returns>
        public async Task<bool> Insert<TabelClass>(TabelClass data)
            where TabelClass : class, new()
        {
            Type type = typeof(TabelClass);
            #region Audit
            if (type.IsAssignableTo(typeof(ICreationAudited)))
            {
                type.GetProperty("CreatedTime")?.SetValue(data, DateTime.Now);
                type.GetProperty("CreatorId")?.SetValue(data, User?.Id ?? 0);
            }
            else if (type.IsAssignableTo(typeof(ICreatedTime)))
                type.GetProperty("CreatedTime")?.SetValue(data, DateTime.Now);

            if (type.IsAssignableTo(typeof(IMultiTenant)))
                type.GetProperty("TenantId")?.SetValue(data, User?.TenantId ?? 0);
            #endregion

            #region Id Check
            var prop = type.GetProperty("Id");
            if (prop?.PropertyType == typeof(Guid))
                while (Queryable<TabelClass>().Where("Id='" + prop.GetValue(data) + "'").Any())
                {
                    prop.SetValue(data, Guid.NewGuid());
                }
            #endregion

            return await _db.Insertable(data).ExecuteCommandAsync() == 1;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="datas">待插入数据</param>
        /// <returns></returns>
        public Task<int> Insert<TabelClass>(params TabelClass[] datas)
            where TabelClass : class, new()
        {
            Type type = typeof(TabelClass);
            // 审计信息
            foreach (var data in datas)
            {
                #region Audit
                if (type.IsAssignableTo(typeof(ICreationAudited)))
                {
                    type.GetProperty("CreatedTime")?.SetValue(data, DateTime.Now);
                    type.GetProperty("CreatorId")?.SetValue(data, User?.Id ?? 0);
                }
                else if (type.IsAssignableTo(typeof(ICreatedTime)))
                    type.GetProperty("CreatedTime")?.SetValue(data, DateTime.Now);

                if (type.IsAssignableTo(typeof(IMultiTenant)))
                    type.GetProperty("TenantId")?.SetValue(data, User?.TenantId ?? 0);
                #endregion
            }

            // 插入方法
            return _db.Insertable(datas).ExecuteCommandAsync();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="datas">待插入数据</param>
        /// <returns></returns>
        public Task<int> Insert<TabelClass>(IEnumerable<TabelClass> datas)
            where TabelClass : class, new()
        {
            Type type = typeof(TabelClass);
            // 审计信息
            foreach (var data in datas)
            {
                #region Audit
                if (type.IsAssignableTo(typeof(ICreationAudited)))
                {
                    type.GetProperty("CreatedTime")?.SetValue(data, DateTime.Now);
                    type.GetProperty("CreatorId")?.SetValue(data, User?.Id ?? 0);
                }
                else if (type.IsAssignableTo(typeof(ICreatedTime)))
                    type.GetProperty("CreatedTime")?.SetValue(data, DateTime.Now);

                if (type.IsAssignableTo(typeof(IMultiTenant)))
                    type.GetProperty("TenantId")?.SetValue(data, User?.TenantId ?? 0);
                #endregion
            }

            // 插入方法
            return _db.Insertable(datas.ToArray()).ExecuteCommandAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="datas">待更新数据</param>
        /// <returns></returns>
        public Task<int> Update<TabelClass>(params TabelClass[] datas)
            where TabelClass : class, new()
        {
            // 审计信息
            Type type = typeof(TabelClass);
            if (type.IsAssignableTo(typeof(IModifyAudited)))
            {
                foreach (var data in datas)
                {
                    type.GetProperty("LastModificationTime")?.SetValue(data, DateTime.Now);
                    type.GetProperty("LastModifierId")?.SetValue(data, User?.Id ?? 0);
                }
            }

            // 更新方法
            return _db.Updateable(datas).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="target">目标字段</param>
        /// <param name="condition">更新条件</param>
        /// <returns></returns>
        public Task<int> Update<TabelClass>(object target, Expression<Func<TabelClass, bool>> condition)
            where TabelClass : class, new()
        {
            if (target is null)
                return Task.FromResult(0);

            // 生成更新字典
            var columnValues = target
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(target));

            Type type = typeof(TabelClass);
            // 审计信息
            if (type.IsAssignableTo(typeof(IModifyAudited)))
            {
                if (User != null)
                    columnValues["LastModifierId"] = User.Id;
                columnValues["LastModificationTime"] = DateTime.Now;
            }

            // 更新方法
            return _db.Updateable<TabelClass>(columnValues)
                .Where(condition)
                .ExecuteCommandAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public Task<bool> Delete<TabelClass>(params int[] ids)
            where TabelClass : class, new()
        {
            Type type = typeof(TabelClass);

            if (type.IsAssignableTo(typeof(IDeletionAudited)))
                return _db.Updateable<TabelClass>(new { DeletedTime = DateTime.Now, DeleterId = User?.Id ?? 0 }).Where("Id IN (" + String.Join(",", ids.Select(key => "'" + key + "'")) + ")").ExecuteCommandHasChangeAsync();
            else if (type.IsAssignableTo(typeof(ISoftDelete)))
                return _db.Updateable<TabelClass>(new { DeletedTime = DateTime.Now }).Where("Id IN (" + String.Join(",", ids.Select(key => "'" + key + "'")) + ")").ExecuteCommandHasChangeAsync();
            else
                return _db.Deleteable<TabelClass>().Where("Id IN (" + String.Join(",", ids.Select(key => "'" + key + "'")) + ")").ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TabelClass">表数据类</typeparam>
        /// <param name="condition">条件表达式</param>
        /// <returns></returns>
        public Task<bool> Delete<TabelClass>(Expression<Func<TabelClass, bool>> condition)
            where TabelClass : class, new()
        {
            Type type = typeof(TabelClass);

            var list = new List<IConditionalModel>();
            if (type.IsAssignableTo(typeof(ISoftDelete)))
            {
                list.Add(new ConditionalModel
                {
                    FieldName = "DeletedTime",
                    ConditionalType = ConditionalType.EqualNull
                });
            }
            if (type.IsAssignableTo(typeof(IDeletionAudited)))
                return _db.Updateable<TabelClass>(new { DeletedTime = DateTime.Now, DeleterId = User?.Id ?? 0 }).Where(list).Where(condition).ExecuteCommandHasChangeAsync();
            else if (type.IsAssignableTo(typeof(ISoftDelete)))
                return _db.Updateable<TabelClass>(new { DeletedTime = DateTime.Now }).Where(list).Where(condition).ExecuteCommandHasChangeAsync();
            else
                return _db.Deleteable<TabelClass>().Where(condition).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Exception
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="exception"></param>
        private void ThrowException(Exception exception)
        {
            // 抛出
            _exception = exception;
            Dispose();
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        private AppDB(bool doTran = true)
        {
            _doTran = doTran;

            if (_doTran) _db.BeginTran();
        }

        /// <summary>
        /// Get client.
        /// </summary>
        /// <param name="db"></param>
        public static explicit operator SqlSugarScope(AppDB db) => db._db;

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_doTran)
                    {
                        // TODO: 释放托管状态(托管对象)
                        if (_exception is null)
                        {
                            // 提交事务
                            _db.CommitTran();
                        }
                        else
                        {
                            // 事务回滚
                            _db.RollbackTran();
                        }
                    }

                    // 释放客户端
                    _db.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~AppDB()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
