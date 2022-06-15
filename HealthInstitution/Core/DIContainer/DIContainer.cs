using HealthInstitution.Core.RestRequests.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.DIContainer
{
    public static class DIContainer
    {
        private static List<ServiceDescriptor> s_serviceDescriptors;

        public static void UpdateDescriptor(List<ServiceDescriptor> serviceDescriptors)
        {
            s_serviceDescriptors = serviceDescriptors;
        }

        public static object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRestRequestDoctorRepository))
            {
                int x = 0;
            }
            var descriptor = s_serviceDescriptors
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

        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
