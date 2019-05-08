#if NET46
namespace LightInject.Interception.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    using Xunit;

    [Collection("Interception")]
    public class DynamicMethodBuilderVerificationTests : DynamicMethodBuilderTests
    {
        protected override IMethodBuilder GetMethodBuilder()
        {
            var dynamicMethodBuilder = new DynamicMethodBuilder();
            var field = typeof(DynamicMethodBuilder).GetField(
                "methodSkeletonFactory", BindingFlags.Instance | BindingFlags.NonPublic);

            var path = Path.Combine(Path.GetDirectoryName(new Uri(typeof(DynamicMethodBuilderVerificationTests).Assembly.CodeBase).LocalPath), "DynamicMethodAssembly.dll");

            Func<IDynamicMethodSkeleton> methodSkeletonFactory = () => new InterceptionMethodBuilderMethodSkeleton(path);
            field.SetValue(dynamicMethodBuilder, methodSkeletonFactory);
            return dynamicMethodBuilder;
        }
    }
}
#endif