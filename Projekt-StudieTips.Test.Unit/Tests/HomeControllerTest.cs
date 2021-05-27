using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Controllers;
using Projekt_StudieTips.Repository;
using NUnit.Framework;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Projekt_StudieTips.Data;

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
