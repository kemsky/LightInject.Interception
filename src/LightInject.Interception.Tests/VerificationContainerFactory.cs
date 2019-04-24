#if NET46
namespace LightInject.Interception.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    internal static class VerificationContainerFactory
    {
        internal static IServiceContainer CreateContainerForAssemblyVerification()
        {
            var path = Path.Combine(Path.GetDirectoryName(new Uri(typeof(VerificationContainerFactory).Assembly.CodeBase).LocalPath), "DynamicAssembly.dll");            
            if (File.Exists(path))
            {
                File.Delete(path);
            }


            var container = new ServiceContainer();

            Func<Type, Type[], IMethodSkeleton> methodSkeletonFactory =
                (returnType, parameterTypes) => new MethodBuilderMethodSkeleton(returnType, parameterTypes, path);

            var factoryField = typeof(ServiceContainer).GetField("methodSkeletonFactory", BindingFlags.Instance | BindingFlags.NonPublic);

            factoryField.SetValue(container, methodSkeletonFactory);

            //var serviceContainer = new ServiceContainer((returnType, parameterTypes) => new MethodBuilderMethodSkeleton(returnType, parameterTypes, path));
            return container;
        }
    }
}
#endif