using System;
using Xunit;

namespace Routables.Tests
{
    public class RoutingServiceTest
    {
        private IRoutingService _routing;
        public RoutingServiceTest(IRoutingService routing) {
            this._routing = routing;
        }

        [Fact]
        public void GetRouter_WhenNoneExists_ReturnsCorrectRouter() {
            var intRouter = _routing.GetRouter<int>();

            Assert.NotNull(intRouter);
            Assert.IsType<Router<int>>(intRouter);
            Assert.Single(_routing.Routers);
        }

        [Fact]
        public void GetRouter_WhenSomeExists_ReturnsCorrectRouter() {
            var intRouter = _routing.GetRouter<int>();
            var stringRouter = _routing.GetRouter<string>();

            Assert.NotNull(intRouter);
            Assert.NotNull(stringRouter);
            Assert.IsType<Router<int>>(intRouter);
            Assert.IsType<Router<string>>(stringRouter);
            Assert.Equal(2, _routing.Routers.Count);
        }

        [Fact]
        public void GetRouter_WhenSameType_ReturnsSameRouterInstance() {
            var intRouter1 = _routing.GetRouter<int>();
            var intRouter2 = _routing.GetRouter<int>();

            Assert.NotNull(intRouter1);
            Assert.NotNull(intRouter2);
            Assert.IsType<Router<int>>(intRouter1);
            Assert.IsType<Router<int>>(intRouter2);
            Assert.Same(intRouter1, intRouter2);
            Assert.Single(_routing.Routers);
        }

        [Fact]
        public void Route_WithRouterRoutes_ReturnsCorrectRoute() {
            var intRouter1 = _routing.GetRouter<int>();
            intRouter1.AddRoute("/", 1);
            intRouter1.AddRoute("/a", 2);

            Assert.Equal(1, _routing.Route<int>("/"));
            Assert.Equal(2, _routing.Route<int>("/a"));
        }

        [Fact]
        public void AddRouteGeneric_WithNewRouter_AddsRoute() {
            _routing.AddRoute<int>("/", 1);
            _routing.AddRoute<int>("/a", 2);

            Assert.Equal(1, _routing.Route<int>("/"));
            Assert.Equal(2, _routing.Route<int>("/a"));
        }

        [Fact]
        public void AddRoute_WithNewRouter_AddsRoute() {
            _routing.AddRoute("/", 1);
            _routing.AddRoute("/a", 2);

            Assert.Equal(1, _routing.Route<int>("/"));
            Assert.Equal(2, _routing.Route<int>("/a"));
        }
    }
}
