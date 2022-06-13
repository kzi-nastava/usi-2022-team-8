using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.DIContainer
{
    public class DIContainer
    {
        private List<ServiceDescriptor> _serviceDescriptors;

        public DIContainer(List<ServiceDescriptor> serviceDescriptors)
        {
            this._serviceDescriptors = serviceDescriptors;
        }

        public object GetService(Type serviceType)
        {
            var descriptor = _serviceDescriptors
                .SingleOrDefault(x => x.ServiceType == serviceType);

            if (descriptor == null)
            {
                throw new ArgumentException($"Service of type {serviceType.Name} is not registered");
            }

            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }

            var actualType = descriptor.ImplementationType ?? descriptor.ServiceType;

            if (actualType.IsAbstract || actualType.IsInterface)
            {
                throw new Exception("Cannot instantiate abstract classes or interfaces");
            }

            var constructorInfo = actualType.GetConstructors().First();

            var parameters = constructorInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();

            var implementation = Activator.CreateInstance(actualType, parameters);

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.Implementation = implementation;
            }

            return implementation;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
