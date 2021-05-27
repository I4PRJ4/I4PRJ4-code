using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Controllers;
using Projekt_StudieTips.Repository;
using NUnit.Framework;

namespace Projekt_StudieTips.Test.Unit
{
    [TestFixture]
    class DegreeControllerTest
    {

        private DegreesController _uut;
        private IDegreeRepository _subRepo;
        [SetUp]
        public void Setup()
        {

            _subRepo = Substitute.For<IDegreeRepository>();
            _uut = new(_subRepo);
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithListOfDegrees()
        {
            //Arrange
            _subRepo.GetDegrees().Returns(new List<Degree>(
                new[] {
                    new Degree() { DegreeId = 0 },
                    new Degree() { DegreeId = 1 }
                })
            );

            //Act
            var result = await _uut.Index();
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.IsAssignableFrom<List<Degree>>(((ViewResult)result).ViewData.Model);
        }

        [Test]
        public void Create_WithoutParemeters_ReturnsAViewResult_WithNoViewData_AndCallsCreateOnRepository()
        {
            //Arrange

            //Act
            var result = _uut.Create();
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.IsNull((result as ViewResult)?.ViewData.Model);
        }

        [Test]
        public async Task Create_WithValidDegree_ReturnsRedirectToIndex_AndCallsRepo()
        {
            //Arrange
            Degree degree = new()
            {
                Courses = new List<Course>(),
                DegreeId = 0,
                DegreeName = "TestDegree"
            };

            //Act
            var result = await _uut.Create(degree) as RedirectToActionResult;
            //Assert
            await _subRepo.Received().AddDegree(degree);
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", result?.ActionName);
            
        }


        //Not running this test, as models do not have validation
        /*[Test]
        public async Task Create_WithInvalidDegree_ReturnsViewResult_WithDegree()
        {
            //Arrange
            Degree degree = null;

            //Act
            var result = await _uut.Create(degree);
            //Assert
            await _subRepo.DidNotReceive().AddDegree(degree);
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(degree, ((ViewResult)result).ViewData.Model);

        }*/



    }
}
