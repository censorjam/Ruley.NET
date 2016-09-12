using System;
using System.Collections.Generic;
using System.Linq;
using Ruley.NET;
using TinyIoC;

namespace Ruley.NET
{
    public static class IoC
    {
        private static TinyIoCContainer _container = new TinyIoCContainer();
        private static List<Type> KnownTypes = new List<Type>();

        public static void RegisterStage<T>() where T : Stage
        {
            KnownTypes.Add(typeof(T));
            _container.Register<T>();
        }

        public static Stage ResolveStage(string name)
        {
            var type = KnownTypes.FirstOrDefault(t => t.Name.Replace("Stage", "").ToLower() == name.ToLower());

            if (type == null)
            {
                throw new Exception($"Unable to locate Stage {name}");
            }

            var filter = (Stage)_container.Resolve(type);
            return (Stage)_container.Resolve(type);
        }

        public static void Register<T>(T instance) where T:class
        {
            _container.Register<T>(instance);
        }

        public static T Resolve<T>() where T:class
        {
            return _container.Resolve<T>();
        }

        public static void Reset()
        {
            _container.Dispose();
            _container = new TinyIoCContainer();
        }
    }
}