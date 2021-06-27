using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Projekt_StudieTips.Controllers;

namespace Projekt_StudieTips.Test.Unit
{
    class HomeControllerTest
    {
        private HomeController _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new HomeController();
        }

        [Test]
        public void Index_ReturnsView()
        {
            // Arrange

            // Act
            var result = _uut.Index();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void UnauthorizedAccess_ReturnsView()
        {
            // Arrange

            // Act
            var result = _uut.UnauthorizedAccess();
            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

    }
}
