using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Project.Web
{
    public class UnityDependencyResolver : IDependencyResolver, IDisposable
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if ((serviceType.IsClass && !serviceType.IsAbstract) || _container.IsRegistered(serviceType))
                return _container.Resolve(serviceType);
            return null;
        }


        public IEnumerable<object> GetServices(Type serviceType)
        {
            if ((serviceType.IsClass && !serviceType.IsAbstract) || _container.IsRegistered(serviceType))
                return _container.ResolveAll(serviceType);
            return new List<object>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}