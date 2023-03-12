using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Tools
{
    /// <summary>
    /// 创建动态类型
    /// </summary>
    public class TypeCreator
    {
        /// <summary>
        /// 获取动态类型对象
        /// </summary>
        /// <returns></returns>
        public static Type Create(string className, Dictionary<string, Type> keyValues)
        {
            //程序集名
            AssemblyName assemblyName = new("DynamicAssembly");
            //程序集
            AssemblyBuilder dyAssembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            //模块
            ModuleBuilder dyModule = dyAssembly.DefineDynamicModule("DynamicModule");
            //类
            TypeBuilder dyClass = dyModule.DefineType(className, TypeAttributes.Public | TypeAttributes.Serializable | TypeAttributes.Class | TypeAttributes.AutoClass);
            foreach (var item in keyValues)
            {
                //字段
                var fb = dyClass.DefineField(item.Key, item.Value, FieldAttributes.Public);
                //get方法
                MethodBuilder mbNumberGetAccessor = dyClass.DefineMethod("get_" + item.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    item.Value, Type.EmptyTypes);
                ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
                numberGetIL.Emit(OpCodes.Ldarg_0);
                numberGetIL.Emit(OpCodes.Ldfld, fb);
                numberGetIL.Emit(OpCodes.Ret);
                //set方法
                MethodBuilder mbNumberSetAccessor = dyClass.DefineMethod("set_" + item.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
               null, new Type[] { item.Value });
                ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
                numberSetIL.Emit(OpCodes.Ldarg_0);
                numberSetIL.Emit(OpCodes.Ldarg_1);
                numberSetIL.Emit(OpCodes.Stfld, fb);
                numberSetIL.Emit(OpCodes.Ret);
                //属性
                PropertyBuilder pbNumber = dyClass.DefineProperty(item.Key, PropertyAttributes.HasDefault,
               item.Value, null);
                //绑定get，set方法
                pbNumber.SetGetMethod(mbNumberGetAccessor);
                pbNumber.SetSetMethod(mbNumberSetAccessor);
            }
            //创建类型信息
            return dyClass.CreateTypeInfo()!;
        }
    }
}
