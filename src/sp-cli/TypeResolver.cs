using Microsoft.Extensions.DependencyInjection;
using System;
using Spectre.Console.Cli;


namespace SpCli
{
    public class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;


        public TypeResolver(IServiceProvider provider) => (_provider) = (provider);


        public void Dispose()
        {
            if(_provider is IDisposable disposable)
                disposable.Dispose();
        }

        public object? Resolve(Type? type)
        {
            if(type is not null)
                return _provider.GetRequiredService(type);


            return null;
        }
    }
}
