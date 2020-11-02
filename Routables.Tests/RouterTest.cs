using System;
using Xunit;

namespace Routables.Tests
{
    public class RouterTest
    {
        private IRoutingService _router;

        public RouterTest(IRoutingService router){
            this._router = router;
        }

        [Fact]
        public void GetResources_WithNewRouter_ReturnsNoResources() {
            var intRouter = _router.GetRouter<int>();
        }

        [Fact]
        public void GetResources_WithUsedRouter_ReturnsSomeResources() {
            var intRouter = _router.GetRouter<int>();
            intRouter.AddRoute("/", 1);
            intRouter.AddRoute("/a", 2);
        }

        [Fact]
        public void AddRoute_WithOneRoute_ReturnsOneRoute() {
            var intRouter = _router.GetRouter<int>();
            intRouter.AddRoute("/", 1);
        }

        [Fact]
        public void Route_WithOnlyRootRoute_ThrowsArgumentException() {
            var intRouter = _router.GetRouter<int>();
            Assert.Throws<ArgumentException>(() => intRouter.Route("/"));
        }

        [Fact]
        public void Route_WithValidPath_ReturnsCorrectResource() {
            var intRouter = _router.GetRouter<int>();
            intRouter.AddRoute("/", 1);
            intRouter.AddRoute("/a", 2);
            Assert.Equal(1, intRouter.Route("/"));
            Assert.Equal(2, intRouter.Route("/a"));
        }

        [Fact]
        public void AddRoute_WithDuplicatePath_OverwritesResource() {
            var intRouter = _router.GetRouter<int>();
            intRouter.AddRoute("/", 1);
            intRouter.AddRoute("/", 2);
            Assert.Equal(2, intRouter.Route("/"));
        }

        [Fact]
        public void Route_WithRootResource_ReturnsCorrectResource() {
            _router.AddRoute("/", 1);
            _router.AddRoute("/", "a");

            Assert.Equal(1, _router.Route<int>("/"));
            Assert.Equal("a", _router.Route<string>("/"));
        }
    }
}
