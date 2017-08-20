using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using ARMD.DataContracts.Api.ReferenceData.ReferenceDataVersionTypesContainers;
using ARMD.DataContracts.ToStations.ReferenceData.RatesRoutes;
using ARMD.DataContracts.ToStations.ReferenceData.SupportingTables;
using TestApp.Core;

namespace TestApp.Emitting
{
    public class Emitter
    {
        //public delegate IEnumerable<T> Getter<out T>(object target);

        public Type CreateType()
        {
            //create the builder
            AssemblyName assembly = new AssemblyName("FileHelpersTests");
            AppDomain appDomain = System.Threading.Thread.GetDomain();
            AssemblyBuilder assemblyBuilder = appDomain.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.Name);
            /*Assembly assembly = Assembly.GetExecutingAssembly();
            AppDomain appDomain = System.Threading.Thread.GetDomain();
            AssemblyBuilder assemblyBuilder = appDomain.DefineDynamicAssembly(assembly.GetName(), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.GetName().Name);*/

            //create the class
            TypeBuilder typeBuilder = moduleBuilder.DefineType("BaseReferenceDataVersionTablesProxy", TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                                                                         TypeAttributes.BeforeFieldInit, typeof(BaseReferenceDataVersionTables));
            //create the firstName field
            //FieldBuilder firstNameField = typeBuilder.DefineField("firstName", typeof(System.String), FieldAttributes.Private);
            FieldBuilder deserializerField = typeBuilder.DefineField("CollectionDeserializer", typeof(CollectionDeserializer), FieldAttributes.Public | FieldAttributes.InitOnly);

            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new []{typeof(CollectionDeserializer)});
            ILGenerator ctorIL = constructorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, deserializerField);
            ctorIL.Emit(OpCodes.Ret);
            //create the Delimiter attribute

            var deserializeMethod = typeof(CollectionDeserializer).GetMethod("Deserialize", BindingFlags.Public | BindingFlags.Instance);
            var genericMethod = deserializeMethod.MakeGenericMethod(typeof(Tariff));
            //var info = typeof(CollectionDeserializer).GetMethod("GetSomeValue", BindingFlags.Public | BindingFlags.Instance);
            //TypeBuilder.GetMethod()

            //create the firstName attribute [FieldOrder(0)]

            //create the FirstName property
            /*string[] typeParameterNames = { "T" };
            GenericTypeParameterBuilder[] typeParameters = typeBuilder.DefineGenericParameters(typeParameterNames);

            GenericTypeParameterBuilder TInput = typeParameters[0];*/
            PropertyBuilder firstNameProperty = typeBuilder.DefineProperty("Tariffs", PropertyAttributes.None, CallingConventions.Any, typeof(IEnumerable<Tariff>), null);

            //create the FirstName Getter
            MethodBuilder firstNamePropertyGetter = typeBuilder.DefineMethod("get_Tariffs", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Final |
                                                                                              MethodAttributes.HideBySig, firstNameProperty.PropertyType, Type.EmptyTypes);

            ILGenerator firstNamePropertyGetterIL = firstNamePropertyGetter.GetILGenerator();
            firstNamePropertyGetterIL.Emit(OpCodes.Ldarg_0);
            firstNamePropertyGetterIL.Emit(OpCodes.Ldfld, deserializerField);
            firstNamePropertyGetterIL.Emit(OpCodes.Callvirt, genericMethod);
            //firstNamePropertyGetterIL.Emit(OpCodes.Ldfld, deserializerField);
            firstNamePropertyGetterIL.Emit(OpCodes.Ret);

            //create the FirstName Setter
            /*MethodBuilder firstNamePropertySetter = typeBuilder.DefineMethod("set_GypTypes", MethodAttributes.Public | MethodAttributes.SpecialName |
                                                                                              MethodAttributes.HideBySig, null, new [] { typeof(IEnumerable<GypType>) });
            ILGenerator firstNamePropertySetterIL = firstNamePropertySetter.GetILGenerator();
            firstNamePropertySetterIL.Emit(OpCodes.Ldarg_0);
            firstNamePropertySetterIL.Emit(OpCodes.Ldarg_1);
            firstNamePropertySetterIL.Emit(OpCodes.Stfld, firstNameField);
            firstNamePropertySetterIL.Emit(OpCodes.Ret);*/

            //assign getter and setter
            firstNameProperty.SetGetMethod(firstNamePropertyGetter);
            //firstNameProperty.SetSetMethod(firstNamePropertySetter);


            //create the lastName field
            FieldBuilder lastNameField = typeBuilder.DefineField("lastName", typeof(System.String), FieldAttributes.Private);

            //create the lastName attribute [FieldOrder(1)]

            //create the LastName property
            PropertyBuilder lastNameProperty = typeBuilder.DefineProperty("LastName", PropertyAttributes.HasDefault, typeof(System.String), null);

            //create the LastName Getter
            MethodBuilder lastNamePropertyGetter = typeBuilder.DefineMethod("get_LastName", MethodAttributes.Public | MethodAttributes.SpecialName |
                                                                                            MethodAttributes.HideBySig, typeof(System.String), Type.EmptyTypes);
            ILGenerator lastNamePropertyGetterIL = lastNamePropertyGetter.GetILGenerator();
            lastNamePropertyGetterIL.Emit(OpCodes.Ldarg_0);
            lastNamePropertyGetterIL.Emit(OpCodes.Ldfld, lastNameField);
            lastNamePropertyGetterIL.Emit(OpCodes.Ret);

            //create the FirstName Setter
            MethodBuilder lastNamePropertySetter = typeBuilder.DefineMethod("set_FirstName", MethodAttributes.Public | MethodAttributes.SpecialName |
                                                                                             MethodAttributes.HideBySig, null, new Type[] { typeof(System.String) });
            ILGenerator lastNamePropertySetterIL = lastNamePropertySetter.GetILGenerator();
            lastNamePropertySetterIL.Emit(OpCodes.Ldarg_0);
            lastNamePropertySetterIL.Emit(OpCodes.Ldarg_1);
            lastNamePropertySetterIL.Emit(OpCodes.Stfld, lastNameField);
            lastNamePropertySetterIL.Emit(OpCodes.Ret);

            //assign getter and setter
            lastNameProperty.SetGetMethod(lastNamePropertyGetter);
            lastNameProperty.SetSetMethod(lastNamePropertySetter);

            return typeBuilder.CreateType();
        }

        private void DoSmth()
        {
            dynamic obj = new ExpandoObject();
            obj.Tariffs = new [] {1, 2, 3};
            obj.Values = new[] {"One", "Two", "Three"};

        }
    }
}