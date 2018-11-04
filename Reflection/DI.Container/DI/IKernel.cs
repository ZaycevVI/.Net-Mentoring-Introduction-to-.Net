using System;

namespace DI.Container.DI
{
    public interface IKernel
    {
        /// <summary>
        /// Register types.
        /// </summary>
        /// <typeparam name="TParent">Parent type.</typeparam>
        /// <typeparam name="TChild">Child type.</typeparam>
        void Bind<TParent, TChild>() where TChild : TParent;

        /// <summary>
        /// Register types by key.
        /// </summary>
        /// <typeparam name="TParent">Parent type.</typeparam>
        /// <typeparam name="TChild">Child type.</typeparam>
        /// <param name="key">string key to find needed type.</param>
        void Bind<TParent, TChild>(string key) where TChild : TParent;

        /// <summary>
        /// Get instance of neccesery type.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <returns>Instance of type T</returns>
        T Get<T>();

        /// <summary>
        /// Get instance of neccesery type by key.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="key">String key to find needed type.</param>
        /// <returns>Instance of type T</returns>
        T Get<T>(string key);

        /// <summary>
        /// Bind all interfaces and their iplemetations for defined assembly.
        /// If interface has 2 or more implementations - take first like in Castle.Windsor.
        /// </summary>
        /// <param name="type">Assembly of this type will be used.</param>
        void AddAssembly(Type type);
    }
}