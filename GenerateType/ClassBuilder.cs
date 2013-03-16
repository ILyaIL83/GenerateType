using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace GenerateType
{
    public class ClassBuilder
    {
        public static readonly MethodAttributes getSetAttr =
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual;

        public static Type Build(Type typeToImplement)
        {
            AssemblyName assemblyName = new AssemblyName("Btracs.Common.DTO");
            AssemblyBuilder assemBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemBuilder.DefineDynamicModule("DataModuleBuilder");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                string.Format("{0}Implementation", typeToImplement.FullName), TypeAttributes.Class, typeof(BaseDataClass));
            typeBuilder.AddInterfaceImplementation(typeToImplement);

            var properties = typeToImplement.GetProperties();
            foreach (var property in properties)
            {
                BuildProperty(typeBuilder, property.Name, property.PropertyType);
            }

            Type type = typeBuilder.CreateType();
            return type;
        }

        private static void BuildProperty(TypeBuilder typeBuilder, string name, Type type)
        {
            FieldBuilder field = typeBuilder.DefineField("m" + name, type, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, null);            

            MethodBuilder getter = typeBuilder.DefineMethod("get_" + name, getSetAttr, type, Type.EmptyTypes);
            ILGenerator getIL = getter.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, field);
            getIL.Emit(OpCodes.Ret);

            MethodBuilder setter = typeBuilder.DefineMethod("set_" + name, getSetAttr, null, new Type[] { type });
            ILGenerator setIL = setter.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, field);
            setIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getter);
            propertyBuilder.SetSetMethod(setter);
        }
    }
}
