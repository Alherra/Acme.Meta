using Microsoft.AspNetCore.Http;
using SqlSugar.IOC;
using SqlSugar;
using System.ComponentModel;
using System.Linq.Expressions;
using Meta.Meta.Audit;

namespace Meta
{
    /// <summary>
    /// 服务基类
    /// </summary>
    [AutoInject]
    [Description("服务基类")]
    public abstract class MetaService
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        [Description("当前用户信息")]
        protected static CacheUser CurrentUser
        {
            get
            {
                var key = Activator.CreateInstance<HttpContextAccessor>().HttpContext.Connection.Id;
                return CacheServer.Find(key);
            }
        }

        /// <summary>
        /// Creates an instance of the specified type using that type's parameterless constructor.
        /// </summary>
        /// <typeparam name="TClass">type</typeparam>
        /// <returns></returns>
        [Description("Creates an instance of the specified type using that type's parameterless constructor")]
        protected static TClass CreateInstance<TClass>(string func = "", params Object[] objects) => 
            func.Equals("") 
            ? Activator.CreateInstance<TClass>()
            : (TClass)(typeof(TClass).GetMethod(func)?.Invoke(null, objects) ?? default(TClass))!;

        /// <summary>
        /// 数据服务
        /// </summary>
        [Description("数据服务")]
        private static readonly SqlSugarScope db = DbScoped.SugarScope;

        /// <summary>
        /// 辅助数据服务
        /// </summary>
        [Description("辅助数据服务")]
        private static readonly SqlSugarScope NormalServer = DbService.Db;

        #region ADO
        /// <summary>
        /// ADO服务
        /// </summary>
        [Description("ADO服务")]
        protected static IAdo NormalAdo => NormalServer.Ado;

        /// <summary>
        /// ADO服务
        /// </summary>
        [Description("ADO服务")]
        protected static IAdo Ado => db.Ado;
        #endregion

        #region Query
        /// <summary>
        /// 数据查询服务
        /// </summary>
        [Description("数据查询服务")]
        protected static ISugarQueryable<T> Queryable<T>()
        {
            Type type = typeof(T);
            var service = DbService.QueryScope.Queryable<T>();

            var conModels = new List<IConditionalModel>();
            if (type.IsAssignableTo(typeof(ISoftDelete)))
                conModels.Add(new ConditionalModel
                {
                    FieldName = "DeletedTime",
                    ConditionalType = ConditionalType.EqualNull,
                    FieldValue = null
                });
            if (type.IsAssignableTo(typeof(IMultiTenant)))
                conModels.Add(new ConditionalModel
                {
                    FieldName = "TenantId",
                    ConditionalType = ConditionalType.EqualNull,
                    FieldValue = CurrentUser.TenantId.ToString(),
                    FieldValueConvertFunc = it => Convert.ToInt64(it)
                });
            return service.Where(conModels);
        }
        #endregion

        #region Creation
        /// <summary>
        /// 增加数据
        /// </summary>
        [Description("增加数据")]
        protected static async Task<int> Insertor<T>(params T[] entities)
            where T : class, new()
        {
            return await DbService.QueryScope.Insertable(entities.Select(e => SetCreationProperty(e)).ToList()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 增加数据
        /// </summary>
        [Description("增加数据")]
        protected static async Task<int> Insertor<T>(bool autoSave = false, params T[] entities)
            where T : class, new()
        {
            if (autoSave)
                return await NormalServer.Insertable(entities.Select(e => SetCreationProperty(e)).ToList()).ExecuteCommandAsync();
            else
                return await Insertor(entities);
        }

        /// <summary>
        /// 增加数据服务返回长整数Id
        /// </summary>
        [Description("增加数据")]
        protected static async Task<long> InsertReturnBigIdentity<T>(T entity, bool autoSave = false)
            where T : class, new()
        {
            if (autoSave)
                return await NormalServer.Insertable(SetCreationProperty(entity)).ExecuteReturnBigIdentityAsync();
            else
                return await DbService.QueryScope.Insertable(SetCreationProperty(entity)).ExecuteReturnBigIdentityAsync();
        }

        /// <summary>
        /// 配置待增加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Description("配置待增加数据")]
        private static T SetCreationProperty<T>(T entity)
        {
            Type type = typeof(T);
            #region Audit
            if (type.IsAssignableTo(typeof(ICreationAudited)))
            {
               type.GetProperty("CreatedTime")?.SetValue(entity, DateTime.Now);
               type.GetProperty("CreatorId")?.SetValue(entity, CurrentUser.Id);
            }
            else if (type.IsAssignableTo(typeof(ICreatedTime)))
                type.GetProperty("CreatedTime")?.SetValue(entity, DateTime.Now);

            if (type.IsAssignableTo(typeof(IMultiTenant)))
                type.GetProperty("TenantId")?.SetValue(entity, CurrentUser.TenantId);
            #endregion

            #region Id Check
            var prop = type.GetProperty("Id");
            if (prop?.PropertyType == typeof(Guid))
                while (Queryable<T>().Where("Id='" + prop.GetValue(entity) + "'").Any())
                {
                    prop.SetValue(entity, Guid.NewGuid());
                }
            #endregion

            return entity;
        }
        #endregion

        #region Modify
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <typeparam name="T">更新的表实体类</typeparam>
        /// <param name="propertities">更新的字段</param>
        /// <param name="expression">更新条件</param>
        /// <param name="autoSave">自动保存</param>
        /// <returns></returns>
        [Description("数据更新")]
        protected static async Task<int> Updator<T>(object propertities, Expression<Func<T, bool>> expression, bool autoSave = false)
            where T : class, new()
        {
            var props = propertities.GetType().GetProperties();
            var TPropNames = typeof(T).GetProperties().Select(x => x.Name).ToHashSet();
            if (props.Any(x => !TPropNames.Contains(x.Name)))
                throw new InvalidArgumentException("Fields Not Matched: " + String.Join(",", props.Where(x => !typeof(T).GetProperties().Contains(x)).Select(x => x.Name)));

            Dictionary<string, object> vals = new();

            foreach (var property in props)
                vals.Add(property.Name, property.GetValue(propertities)!);

            if (typeof(T).IsAssignableTo(typeof(IModifyAudited)))
            {
                vals.Add("LastModificationTime", DateTime.Now);
                vals.Add("LastModifierId", CurrentUser.Id);
            }

            if (autoSave)
                return await NormalServer.Updateable<T>(vals).Where(expression).ExecuteCommandAsync();
            else
                return await db.Updateable<T>(vals).Where(expression).ExecuteCommandAsync();

        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <typeparam name="T">更新的表实体类</typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        [Description("数据更新")]
        protected static async Task<int> Updator<T>(params T[] entities)
            where T : class, new()
        {
            return await db.Updateable(entities.Select(e => SetModifyProperty(e)).ToList()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <typeparam name="T">更新的表实体类</typeparam>
        /// <param name="autoSave">自动保存</param>
        /// <param name="entities">一个或者多个数据实体</param>
        /// <returns></returns>
        [Description("数据更新")]
        protected static async Task<int> Updator<T>(bool autoSave = false, params T[] entities)
            where T : class, new()
        {
            if (autoSave)
                return await NormalServer.Updateable(entities.Select(e => SetModifyProperty(e)).ToList()).ExecuteCommandAsync();
            else
                return await Updator(entities);
        }

        /// <summary>
        /// 配置待增加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Description("配置待增加数据")]
        private static T SetModifyProperty<T>(T entity)
        {
            Type type = typeof(T);
            if (type.IsAssignableTo(typeof(IModifyAudited)))
            {
                type.GetProperty("LastModificationTime")?.SetValue(entity, DateTime.Now);
                type.GetProperty("LastModifierId")?.SetValue(entity, CurrentUser.Id);
            }

            return entity;
        }

        /// <summary>
        /// 数据更新的服务
        /// </summary>
        [Description("数据更新的服务")]
        protected static IUpdateable<T> Updateable<T>(bool autoSave = false)
            where T : class, new()
        {
            if (autoSave)
                return NormalServer.Updateable<T>();
            else
                return db.Updateable<T>();
        }
        #endregion

        #region Deletion
        /// <summary>
        /// 数据删除
        /// </summary>
        [Description("数据删除")]
        protected static async Task<int> Deletor<T>(Expression<Func<T, bool>> expression)
            where T : class, new()
        {
            Type type = typeof(T);

            if (type.IsAssignableTo(typeof(IDeletionAudited)))
                return await db.Updateable<T>(new { DeletedTime = DateTime.Now, DeleterId = CurrentUser.Id }).Where("DeletedTime IS NULL").Where(expression).ExecuteCommandAsync();
            else if (type.IsAssignableTo(typeof(ISoftDelete)))
                return await db.Updateable<T>(new { DeletedTime = DateTime.Now }).Where("DeletedTime IS NULL").Where(expression).ExecuteCommandAsync();
            else
                return await db.Deleteable<T>().Where(expression).ExecuteCommandAsync();
        }

        /// <summary>
        /// 数据删除
        /// </summary>
        [Description("数据删除")]
        protected static async Task<int> Deletor<T>(params object[] TKeys)
            where T : class, new()
        {
            Type type = typeof(T);

            if (type.IsAssignableTo(typeof(IDeletionAudited)))
                return await db.Updateable<T>(new { DeletedTime = DateTime.Now, DeleterId = CurrentUser.Id }).Where("Id IN (" + String.Join(",", TKeys.Select(key => "'" + key + "'")) + ")").ExecuteCommandAsync();
            else if (type.IsAssignableTo(typeof(ISoftDelete)))
                return await db.Updateable<T>(new { DeletedTime = DateTime.Now }).Where("Id IN (" + String.Join(",", TKeys.Select(key => "'"+ key + "'"))+ ")").ExecuteCommandAsync();
            else
                return await db.Deleteable<T>().Where("Id IN (" + String.Join(",", TKeys.Select(key => "'" + key + "'")) + ")").ExecuteCommandAsync();
        }
        #endregion
    }
}
