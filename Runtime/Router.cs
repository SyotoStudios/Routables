using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Routables
{
    public delegate void RouteAddedHandler<T>(string path, T resource);
    public delegate void RouteRequestHandler<T>(T resource);

    public class Router<T>: IRouter
    {
        public Route<T> _rootRoute = new Route<T>("");

        public event RouteAddedHandler<T> OnRouteAdded;
        public event RouteRequestHandler<T> OnRouteRequested;

        public void AddRoute(string path, T resource) {
            if (!IsPath(path)) {
                throw new ArgumentException("Invalid path: " + path);
            }

            if (IsRoot(path)) {
                _rootRoute.SetResource(resource);
                OnRouteAdded?.Invoke(path, resource);
                return;
            }

            path = path.Trim('/').ToLower();
            string[] pathSegments = path.Split('/');

            RecursiveAdd(pathSegments, resource, _rootRoute);
            OnRouteAdded?.Invoke(path, resource);
        }

        private void RecursiveAdd(string[] segments, T resource, Route<T> route) {
            if (segments.Length == 0) return;

            if(!route.HasRoute(segments[0])) {
                var r = new Route<T>(segments[0]);
                route.AddRoute(r);
                if (segments.Length == 1) {
                    r.SetResource(resource);
                } else {
                    RecursiveAdd(segments.Skip(1).ToArray(), resource, r);
                }
            } else {
                var r = route.GetRoute(segments[0]);
                if (segments.Length == 1) {
                    r.SetResource(resource);
                } else {
                    RecursiveAdd(segments.Skip(1).ToArray(), resource, r);
                }
            }
        }

        private (T, bool) RecursiveRoute(string[] segments, Route<T> route) {
            if (segments.Length == 0) return (default(T), false);

            Route<T> nextRoute = route;
            foreach(var r in route.Routes) {
                if (r.Name.Equals(segments[0])) {
                    nextRoute = r;
                    if (segments.Length == 1) {
                        return (nextRoute.Resource, nextRoute.HasResource);
                    }
                    break;
                }
            }

            return RecursiveRoute(segments.Skip(1).ToArray(), nextRoute);
        }

        public T Route(string path) {
            if (!IsPath(path)) {
                throw new ArgumentException("Invalid path: " + path);
            }

            if (IsRoot(path)) {
                if (_rootRoute.HasResource) {
                    OnRouteRequested?.Invoke(_rootRoute.Resource);
                    return _rootRoute.Resource;
                } else {
                    throw new ArgumentException("A resource does not exist at the path: " + path);
                }
            }

            path = path.TrimEnd('/').ToLower();
            string[] pathSegments = path.Split('/');
            
            var (res, hasResource) = RecursiveRoute(pathSegments, _rootRoute);
            if (hasResource) {
                OnRouteRequested?.Invoke(res);
                return res;
            } else {
                throw new ArgumentException("A resource does not exist at the path: " + path);
            }
        }

        private bool IsPath(string path) {
            if (string.IsNullOrWhiteSpace(path)) return false;
            if (!path.StartsWith("/")) return false;
            if (path.Contains("//")) return false;
            return true;
        }

        private bool IsRoot(string path) {
            return path.Equals("/");
        }
    }
}
