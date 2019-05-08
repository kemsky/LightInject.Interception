#if NET46
namespace LightInject.Interception.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using AssemblyName = System.Reflection.AssemblyName;

    public class VerifiableTypeBuilderFactory : ITypeBuilderFactory
    {
        public TypeBuilder CreateTypeBuilder(Type targetType, Type[] additionalInterfaces)
        {
            ModuleBuilder moduleBuilder = GetModuleBuilder();
            const TypeAttributes typeAttributes = TypeAttributes.Public | TypeAttributes.Class;
            var typeName = targetType.Name + "Proxy";
            if (targetType.IsInterface)
            {
                Type[] interfaceTypes = new[] { targetType }.Concat(additionalInterfaces).ToArray();
                return moduleBuilder.DefineType(typeName, typeAttributes, null, interfaceTypes);
            }

            return moduleBuilder.DefineType(typeName, typeAttributes, targetType, additionalInterfaces);
        }

        public Type CreateType(TypeBuilder typeBuilder)
        {


            Type proxyType = typeBuilder.CreateType();
            ((AssemblyBuilder)typeBuilder.Assembly).Save("ProxyAssembly.dll");
            AssemblyAssert.IsValidAssembly("ProxyAssembly.dll");
            return proxyType;
        }

        private static ModuleBuilder GetModuleBuilder()
        {
            AssemblyBuilder assemblyBuilder = GetAssemblyBuilder();
            return assemblyBuilder.DefineDynamicModule("ProxyAssembly", "ProxyAssembly.dll");
        }

        private static AssemblyBuilder GetAssemblyBuilder()
        {
            var assemblyPath = Path.Combine(Path.GetDirectoryName(new Uri(typeof(VerifiableTypeBuilderFactory).Assembly.CodeBase).LocalPath), "ProxyAssembly.dll");
            var assemblybuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName("ProxyAssembly"), AssemblyBuilderAccess.RunAndSave, Path.GetDirectoryName(assemblyPath));
            return assemblybuilder;
        }
    }


}
#endif