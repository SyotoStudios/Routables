using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Routables
{
    public class Route<T>
    {
        public string Name;
        public List<Route<T>> Routes;
        public T Resource;
        public bool HasResource = false;

        public Route(string name) {
            this.Name = name.ToLower();
            this.Routes = new List<Route<T>>();
        }

        public Route(string name, T resource) {
            this.Name = name.ToLower();
            this.Routes = new List<Route<T>>();
            this.Resource = resource;
            this.HasResource = true;
        }

        public void SetResource(T resource) {
            this.Resource = resource;
            this.HasResource = true;
        }

        public void AddRoute(Route<T> route) {
            Routes.Add(route);
        }

        public Route<T> GetRoute(string name) {
            name = name.ToLower();
            foreach(var route in Routes) {
                if (route.Name.Equals(name)) {
                    return route;
                }
            }

            return null;
        }

        public bool HasRoute(string name) {
            return GetRoute(name) != null;
        }
    }
}
