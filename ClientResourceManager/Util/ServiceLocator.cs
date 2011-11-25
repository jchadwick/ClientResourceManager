using System;
using System.Collections.Generic;

namespace ClientResourceManager.Util
{
    // Poor developer's Service Locator
    public class ServiceLocator
    {
        private static readonly IDictionary<Type, object> Factories = new Dictionary<Type, object>();

        public static T Resolve<T>()
        {
            var targetType = typeof (T);

            if(Factories.ContainsKey(targetType) == false)
                throw new ApplicationException("No service registered for type " + targetType.FullName);

            var factory = (Func<T>) Factories[targetType];

            return factory();
        }

        public static void Register<T>(Func<T> factory)
        {
            Factories.Add(typeof(T), factory);
        }
    }
}
