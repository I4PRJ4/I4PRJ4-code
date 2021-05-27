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

        [Test]
        public void Redirect_WithPlaintextRoutes_ReturnsRedirectsToCorrectPaths()
        {

            //Arrange
            const string goTo = "Gå til";
            const string edit = "Rediger";
            const string delete = "Slet";

            //Act
            var goToResult = _uut.Redirect(null, goTo) as RedirectToActionResult;
            var editResult = _uut.Redirect(null, edit) as RedirectToActionResult;
            var deleteResult = _uut.Redirect(null, delete) as RedirectToActionResult;
            //Assert
            ;
            Assert.AreEqual("Index", goToResult?.ActionName);
            Assert.AreEqual("Edit", editResult?.ActionName);
            Assert.AreEqual("Delete", deleteResult?.ActionName);
        }

        [Test]
        public async Task Edit_WithNoSpecifiedId_Returns_NotFound()
        {
            //Arrange

            //Act
            var result = await _uut.Edit(id: null);
            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]

        public async Task Edit_WithIdMismatch_Returns_NotFound()
        {
            //Arrange
            var degree = new Degree() { DegreeId = 0 };
            //Act
            var result = await _uut.Edit(2, degree);
            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_WithIdMatch_ReturnsRedirectToIndex_AndUpdatesViaRepo()
        {
            //Arrange
            var degree = new Degree() { DegreeId = 0 };

            //Act
            var result = await _uut.Edit(0, degree);
            //Assert

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult)?.ActionName);
            await _subRepo.Received().UpdateDegree(degree);
        }

        [Test]
        public async Task Delete_WithNoId_ReturnsNotFound()
        {
            //Arrange

            //Act
            var result = await _uut.Delete(id: null);
            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsViewWithDegree()
        {
            //Arrange
            var degree = new Degree() {DegreeId = 0};
            _subRepo.FindDegree(0).Returns(degree);
            //Act
            var result = await _uut.Delete(0);
            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _subRepo.Received().FindDegree(0);
            Assert.AreEqual(degree, (result as ViewResult)?.ViewData.Model);
        }

        [Test]
        public async Task DeleteConfirmed_WithValidId_ReturnsRedirectToIndex()
        {
            //Arrange
            var degree = new Degree() { DegreeId = 0 };
            _subRepo.FindDegree(0).Returns(degree);
            //Act
            var result = await _uut.DeleteConfirmed(0);
            //Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            _subRepo.Received().FindDegree(0);
            _subRepo.Received().RemoveDegree(degree);
            Assert.AreEqual("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Test]
        public async Task DeleteConfirmed_WithInvalidId_ReturnsRedirectToIndex()
        {
            //Arrange
            //Act
            var result = await _uut.DeleteConfirmed(-5);
            //Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult)?.ActionName);
        }


    }
}
