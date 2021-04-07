using Microsoft.Extensions.DependencyInjection;
using System;
using Spectre.Console.Cli;


namespace SpCli
{
    public class TypeRegistrar : ITypeRegistrar
    {
        readonly IServiceCollection _serviceCollection;


        public TypeRegistrar(IServiceCollection serviceCollection) =>
            (_serviceCollection) = (serviceCollection)
        ;


        public ITypeResolver Build() => new TypeResolver(_serviceCollection.BuildServiceProvider());


        public void Register(Type service, Type implementation) =>
            _serviceCollection.AddSingleton(service, implementation)
        ;

        public void RegisterInstance(Type service, object implementation) =>
            _serviceCollection.AddSingleton(service, implementation)
        ;

        public void RegisterLazy(Type service, Func<object> factory)
        {
            if(factory is null)
                throw new ArgumentNullException(nameof(factory));


            _serviceCollection.AddSingleton(service, factory);
        }
    }
}
