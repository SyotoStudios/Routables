using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Routables
{
    public class RoutingService : IRoutingService
    {
        private List<IRouter> _routers = new List<IRouter>();
        public ReadOnlyCollection<IRouter> Routers => _routers.AsReadOnly();

        public Router<T> GetRouter<T>() {
            // Check if a Router<T> already exists and return it.
            foreach(var r in _routers) {
                if (r is Router<T>) {
                    return r as Router<T>;
                }
            }

            // Otherwise, create a Router<T> and return it.
            Router<T> router = new Router<T>();
            _routers.Add(router);
            return router;
        }

        public T Route<T>(string path) {
            var router = GetRouter<T>();
            return router.Route(path);
        }

        public void AddRoute<T>(string path, T resource) {
            var router = GetRouter<T>();
            router.AddRoute(path, resource);
        }
    }
}
