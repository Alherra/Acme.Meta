using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Newtonsoft.Json;
using SqlSugar;
using Microsoft.AspNetCore.Http;

namespace System
{
    /// <summary>
    /// DB Application
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
        /// Sugar Client
        /// </summary>
        private readonly SqlSugarClient _db = DbService.Client;

        private UserInfo? _user;

        /// <summary>
        /// Current user
        /// </summary>
        private UserInfo? User
        {
            get
            {
                _user ??= JsonConvert.DeserializeObject<UserInfo>(ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "CurrentUser")?.Value! ?? string.Empty);
                return _user;
            }
        }

        /// <summary>
        /// If begin tran
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
        /// Execute func with db
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="func">Function codes</param>
        /// <param name="doTran">If use the tran</param>
        /// <returns></returns>
        public static async Task<TResult> Execute<TResult>(Func<AppDB, Task<TResult>> func, bool doTran = true)
        {
            // Create an instance
            using var db = new AppDB(doTran);

            try
            {
                // Execute
                return await func(db);
            }
            catch (Exception ex)
            {
                // Rollback and throw the exception
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
        /// ISugarQueryable
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <returns></returns>
        /// <remarks>Auto filter</remarks>
        public ISugarQueryable<TableClass> Queryable<TableClass>(params Type[] ignores)
        {
            var queryable = _db.Queryable<TableClass>();
            var list = new List<IConditionalModel>();
            var type = typeof(TableClass);
            if (type.IsAssignableTo(typeof(ISoftDelete)) && !ignores.Contains(typeof(ISoftDelete)))
            {
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
        /// Insert data
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="datas">Data to insert</param>
        /// <returns></returns>
        public async Task<bool> Insert<TableClass>(TableClass data)
            where TableClass : class, new()
        {
            Type type = typeof(TableClass);
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
                while (Queryable<TableClass>().Where("Id='" + prop.GetValue(data) + "'").Any())
                {
                    prop.SetValue(data, Guid.NewGuid());
                }
            #endregion

            return await _db.Insertable(data).ExecuteCommandAsync() == 1;
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="datas">Data to insert</param>
        /// <returns></returns>
        public Task<int> Insert<TableClass>(params TableClass[] datas)
            where TableClass : class, new()
        {
            Type type = typeof(TableClass);
            // Audit
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

            // Insert
            return _db.Insertable(datas).ExecuteCommandAsync();
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="datas">Data to insert</param>
        /// <returns></returns>
        public Task<int> Insert<TableClass>(IEnumerable<TableClass> datas)
            where TableClass : class, new()
        {
            Type type = typeof(TableClass);
            // Audit
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

            // Insert
            return _db.Insertable(datas.ToArray()).ExecuteCommandAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// Update data
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="datas">Data to insert</param>
        /// <returns></returns>
        public Task<int> Update<TableClass>(params TableClass[] datas)
            where TableClass : class, new()
        {
            // Audit
            Type type = typeof(TableClass);
            if (type.IsAssignableTo(typeof(IModifyAudited)))
            {
                foreach (var data in datas)
                {
                    type.GetProperty("LastModificationTime")?.SetValue(data, DateTime.Now);
                    type.GetProperty("LastModifierId")?.SetValue(data, User?.Id ?? 0);
                }
            }

            // Update
            return _db.Updateable(datas).ExecuteCommandAsync();
        }

        /// <summary>
        /// Update data
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="target">Fields to updates</param>
        /// <param name="condition">Update condition</param>
        /// <returns></returns>
        public Task<int> Update<TableClass>(object target, Expression<Func<TableClass, bool>> condition)
            where TableClass : class, new()
        {
            if (target is null)
                return Task.FromResult(0);

            // Make a new dictionary
            var columnValues = target
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(target));

            Type type = typeof(TableClass);
            // Audit
            if (type.IsAssignableTo(typeof(IModifyAudited)))
            {
                if (User != null)
                    columnValues["LastModifierId"] = User.Id;
                columnValues["LastModificationTime"] = DateTime.Now;
            }

            // Update
            return _db.Updateable<TableClass>(columnValues)
                .Where(condition)
                .ExecuteCommandAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletion
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="ids">Key ids</param>
        /// <returns></returns>
        public Task<bool> Delete<TableClass>(params int[] ids)
            where TableClass : class, new()
        {
            Type type = typeof(TableClass);

            if (type.IsAssignableTo(typeof(IDeletionAudited)))
                return _db.Updateable<TableClass>(new { DeletedTime = DateTime.Now, DeleterId = User?.Id ?? 0 }).Where("Id IN (" + String.Join(",", ids.Select(key => "'" + key + "'")) + ")").ExecuteCommandHasChangeAsync();
            else if (type.IsAssignableTo(typeof(ISoftDelete)))
                return _db.Updateable<TableClass>(new { DeletedTime = DateTime.Now }).Where("Id IN (" + String.Join(",", ids.Select(key => "'" + key + "'")) + ")").ExecuteCommandHasChangeAsync();
            else
                return _db.Deleteable<TableClass>().Where("Id IN (" + String.Join(",", ids.Select(key => "'" + key + "'")) + ")").ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Deletion
        /// </summary>
        /// <typeparam name="TableClass">Table</typeparam>
        /// <param name="condition">Deletion condition</param>
        /// <returns></returns>
        public Task<bool> Delete<TableClass>(Expression<Func<TableClass, bool>> condition)
            where TableClass : class, new()
        {
            Type type = typeof(TableClass);

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
                return _db.Updateable<TableClass>(new { DeletedTime = DateTime.Now, DeleterId = User?.Id ?? 0 }).Where(list).Where(condition).ExecuteCommandHasChangeAsync();
            else if (type.IsAssignableTo(typeof(ISoftDelete)))
                return _db.Updateable<TableClass>(new { DeletedTime = DateTime.Now }).Where(list).Where(condition).ExecuteCommandHasChangeAsync();
            else
                return _db.Deleteable<TableClass>().Where(condition).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Exception
        /// <summary>
        /// Throw the exceptions
        /// </summary>
        /// <param name="exception"></param>
        private void ThrowException(Exception exception)
        {
            // Throw set
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
        public static explicit operator SqlSugarClient(AppDB db) => db._db;

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_doTran)
                    {
                        // TODO: Dispose
                        if (_exception is null)
                        {
                            // Commit
                            _db.CommitTran();
                        }
                        else
                        {
                            // Rollback
                            _db.RollbackTran();
                        }
                    }

                    // Dispose
                    _db.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
