using System;

namespace IziHardGames.Apps.Abstractions.Lib
{
    /// <summary>
    /// <see cref="System.IServiceProvider"/>
    /// </summary>
    public interface IIziServiceProvider : IServiceProvider
    {
        object? GetService(Type type);
        object? GetService(Guid guid);
        T GetServiceAs<T>();
        T? GetServiceAs<T>(Guid guid);

        object GetSingleton();
        T? GetSingletonAs<T>();
    }
}
