using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DI.Container.Constants;

// ReSharper disable PossibleMultipleEnumeration

namespace DI.Container.DI
{
    public class StandardKernel : IKernel
    {
        /// <summary>
        /// Bool flag, determines wether stirng key is neccesery.
        /// </summary>
        private bool _useKey;

        /// <summary>
        /// String key to find registred object.
        /// </summary>
        private string _key;

        /// <summary>
        /// Dictionary contains bindings object to object.
        /// </summary>
        private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();

        /// <summary>
        /// Dictionary contains bindings object to object with string key.
        /// </summary>
        private readonly Dictionary<string, KeyValuePair<Type, Type>> _bindingsByKey =
            new Dictionary<string, KeyValuePair<Type, Type>>();

        /// <inheritdoc />
        /// <summary>
        /// Bind all interfaces and their iplemetations for defined assembly.
        /// If interface has 2 or more implementations - take first like in Castle.Windsor.
        /// </summary>
        /// <param name="type">Assembly of this type will be used.</param>
        public void AddAssembly(Type type)
        {
            var assembly = Assembly.GetAssembly(type);

            var exportedTypes = assembly.GetExportedTypes();

            var bindingList = exportedTypes.Where(e => e.IsInterface).Select(e => new
            {
                Inteface = e,
                Implementation = exportedTypes.FirstOrDefault(t => e.IsAssignableFrom(t) && t.IsClass)
            })
                .Where(t => t.Implementation != null);

            foreach (var item in bindingList)
                _bindings.Add(item.Inteface, item.Implementation);
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers types and put them to dictionary.
        /// </summary>
        /// <typeparam name="TParent">Parent type.</typeparam>
        /// <typeparam name="TChild">Child type. Instance to create when TParent requested.</typeparam>
        /// <exception cref="T:System.ArgumentException">Such key was already registred.</exception>
        public void Bind<TParent, TChild>()
            where TChild : TParent
        {
            var parentType = typeof(TParent);

            if (_bindings.ContainsKey(parentType))
                throw new ArgumentException(string.Format(Error.ParameterWasAlreadyRegistred, parentType));

            _bindings.Add(typeof(TParent), typeof(TChild));
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers types by string key and put them to dictionary.
        /// </summary>
        /// <typeparam name="TParent">Parent type.</typeparam>
        /// <typeparam name="TChild">Child type. Instance to create when TParent requested.</typeparam>
        /// <param name="key">Key to find registred types.</param>
        /// <exception cref="T:System.ArgumentException">Such key was already registred.</exception>
        public void Bind<TParent, TChild>(string key)
            where TChild : TParent
        {
            if (_bindingsByKey.ContainsKey(key))
                throw new ArgumentException(string.Format(Error.ParameterWasAlreadyRegistred, key));

            _bindingsByKey.Add(key, new KeyValuePair<Type, Type>(typeof(TParent), typeof(TChild)));
        }

        /// <inheritdoc />
        /// <summary>
        /// Get type T. Replace all registred types by value in dictionary.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <returns>Instance of type T.</returns>
        public T Get<T>()
        {
            var type = typeof(T);

            if (type.IsInterface && !_bindings.ContainsKey(type))
                throw new ArgumentException(string.Format(Error.NoRegisteredTypeForInterface, type));

            _useKey = false;
            return (T)Get(typeof(T));
        }

        /// <inheritdoc />
        /// <summary>
        /// Get type T by string key. Replace all registred types by value in dictionary.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="key">String key to find registred types.</param>
        /// <returns>Instance of type T.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// 1) Key is invalid.
        /// 2) Such key was already registred.
        /// 3) Can't cast needed instance to type T.
        /// </exception>
        public T Get<T>(string key)
        {
            if (!_bindingsByKey.ContainsKey(key) || key == null)
                throw new ArgumentException(string.Format(Error.KeyInvalidOrDoesntExist, key));

            if (!typeof(T).IsAssignableFrom(_bindingsByKey[key].Value))
                throw new ArgumentException(string.Format(Error.NoInheritanceBetweenTypes, typeof(T), _bindingsByKey[key].Value));

            _useKey = true;
            _key = key;
            return (T)Get(typeof(T));
        }

        /// <summary>
        /// Extract object using registration by key or by type-to-type.
        /// </summary>
        /// <param name="type">Type of instance to create.</param>
        /// <returns>Instance of type Type.</returns>
        private object Get(Type type)
        {
            // type of instance to create
            Type instance;

            if (_useKey)
                instance = _bindingsByKey[_key].Value;

            else
                instance = _bindings.ContainsKey(type) ? _bindings[type] : type;

            var obj = CreateInstance(instance);

            return obj;
        }

        /// <summary>
        /// Create instance with type of "type" variable.
        /// </summary>
        /// <param name="type">Type, needed to create instance.</param>
        /// <returns>Created instance.</returns>
        /// <exception cref="ArgumentException">
        /// No matching ctors was found.
        /// </exception>
        private object CreateInstance(Type type)
        {
            // get ctor with max amount of registred params or default
            var constructor = GetRequiredConstructor(type);

            if (constructor == null)
                throw new ArgumentException(string.Format(Error.NoMatchingCtorsForType, type));

            var parameters = CreateParametrs(constructor);
            var instance = Activator.CreateInstance(type, parameters);
            PropertyInjection(instance);

            return instance;
        }

        /// <summary>
        /// Create instance of property, which was matched by attribute [Inject].
        /// If there is no registred type for property, and this property isn't interface, 
        /// call ctor with max registred args or default.
        /// </summary>
        /// <param name="instance">Instance in which we find injected properties.</param>
        /// <exception cref="ArgumentException"> No matching type for needed interface.</exception>
        private void PropertyInjection(object instance)
        {
            // Get all properties with attribute [Inject]
            var properties = instance.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<InjectAttribute>() != null);

            var types = properties.Select(p => p.PropertyType).ToArray();

            var cyclingRef = types.FirstOrDefault(t => t.IsInstanceOfType(
                instance));

            if (cyclingRef != null)
                throw new ArgumentException(Error.CyclingReferenceDetected);

            var i = 0;

            foreach (var property in properties)
            {
                var propType = types[i];
                var type = _bindings.ContainsKey(propType) ? _bindings[propType] : propType;

                if (type.IsInterface)
                    throw new ArgumentException(string.Format(Error.NoRegisteredTypeForInterface, type));

                property.SetValue(instance, CreateInstance(type));
                i++;
            }
        }

        /// <summary>
        /// Create instances of all parametr of constructor.
        /// </summary>
        /// <param name="constructor">Constructor to analyze.</param>
        /// <returns>Array of objects, which were created.</returns>
        /// <exception cref="ArgumentException">
        /// Cycling reference was detected like:
        /// class Controler { Controller(IControlle c); }
        /// </exception>
        private object[] CreateParametrs(ConstructorInfo constructor)
        {
            var parameters = (from c in constructor.GetParameters()
                              select c.ParameterType).ToArray();

            // when ctor has argument like source instance
            var cyclingParam = parameters.FirstOrDefault(p => p.IsAssignableFrom(
                constructor.ReflectedType));

            if (cyclingParam != null)
                throw new ArgumentException(Error.CyclingReferenceDetected);

            return parameters.Select(parameter => CreateInstance(_bindings[parameter])).ToArray();
        }

        /// <summary>
        /// Find constructor in type, which has max amount of registred params.
        /// If such constructor does't exist - return default ctor.
        /// </summary>
        /// <param name="type">Type to scan and find ctor in.</param>
        /// <returns>Ctor with max amount of registred params or default; otherwise - null.</returns>
        /// <exception cref="TypeLoadException">
        /// There are several ctors with equal max amount of registred params.
        /// </exception>
        private ConstructorInfo GetRequiredConstructor(Type type)
        {
            // get sorted by descending sequence of needed ctors.
            var ctors = type.GetConstructors()
                .Where(c => c.GetParameters()
                .All((i) => _bindings.ContainsKey(i.ParameterType)))
                .OrderByDescending(c => c.GetParameters().Length);

            if (ctors.Count() <= 1) return ctors.FirstOrDefault();
            var arr = ctors.ToArray();

            // there are at least two ctors, with max amount of registred params
            if (arr[0].GetParameters().Length == arr[1].GetParameters().Length)
                throw new TypeLoadException(Error.MoreThanOneContructorsMatches);

            return ctors.First();
        }
    }
}