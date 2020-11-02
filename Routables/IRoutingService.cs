using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Routables
{
    public interface IRoutingService
    {
        ReadOnlyCollection<IRouter> Routers {get;}

        Router<T> GetRouter<T>();

        T Route<T>(string path);

        void AddRoute<T>(string path, T resource);
    }
}
