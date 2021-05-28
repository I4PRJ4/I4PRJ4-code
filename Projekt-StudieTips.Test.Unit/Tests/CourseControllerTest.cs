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
using Microsoft.AspNetCore.Mvc;
using Projekt_StudieTips.Models.ViewModels;

namespace Projekt_StudieTips.Test.Unit
{
    [TestFixture]
    public class CourseControllerTest
    {

        private CoursesController _uut;
        private ICourseRepository _courseRepo;
        private IDegreeRepository _degreeRepo;

        [SetUp]
        public void Setup()
        {
            _courseRepo = Substitute.For<ICourseRepository>();
            _degreeRepo = Substitute.For<IDegreeRepository>();
            _uut = new CoursesController(_courseRepo, _degreeRepo);
           
        }

        [Test]
        public void Index_ReturnsView_WithListOfCourses()
        {
            //Arrange
            var courses = new List<Course>(
               new[] {
                    new Course() {CourseId = 0, DegreeId = 0},
                    new Course() {CourseId = 1, DegreeId = 0}
               });
            _courseRepo.FindCourses(0).Returns(courses);

            //Act
            var result = _uut.Index(0);

            //Assert
            Assert.IsAssignableFrom<List<Course>>((result as ViewResult).ViewData.Model);
            Assert.AreEqual(courses, (result as ViewResult).ViewData.Model);
        }

        [Test]
        public void Index_ReturnsView_WithNull()
        {
            //Arrange
            

            //Act
            var result = _uut.Index(null);

            //Assert
            Assert.IsNull((result as ViewResult).ViewData.Model);
        }

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
            
            Assert.AreEqual("Index", goToResult?.ActionName);
            Assert.AreEqual("Edit", editResult?.ActionName);
            Assert.AreEqual("Delete", deleteResult?.ActionName);
        }


        [Test]
        public async Task Create_WithNullParameter_CallOnRepository_ReturnsANotFoundResult()
        {
            //Arrange

            //Act
            var result = await _uut.Create(DegreeId: null);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_WithValidDegree_ReturnsRedirectToIndex_AndCallsRepo()
        {
            //Arrange
            Course course = new()
            {
                Tips = new List<Tip>(),
                CourseId = 0,
                CourseName = "TestCourse"
            };

            //Act
            var result = await _uut.Create(course) as RedirectToActionResult;
            //Assert
            await _courseRepo.Received().AddCourse(course);
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", result?.ActionName);

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
        public async Task Edit_WithIdMismatch_Returns_NotFound_RedirectToIndex()
        {
            //Arrange
            var course = new Course() { CourseId = 0 };
            //Act
            var result = await _uut.Edit(2, course);
            //Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }


        [Test]
        public async Task Edit_WithIdMatch_ReturnsRedirectToIndex_AndUpdatesViaRepo()
        {
            //Arrange
            var course = new Course() { CourseId = 0 };

            //Act
            var result = await _uut.Edit(0, course);
            //Assert

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult)?.ActionName);
            await _courseRepo.Received().UpdateCourse(course);
        }


        [Test]
        public async Task DeleteConfirmed_WithValidId_ReturnsRedirectToIndex()
        {
            //Arrange
            var course = new Course() { CourseId = 0, DegreeId = 0 };
            _courseRepo.GetCourse(0).Returns(course);
            //Act
            var result = await _uut.DeleteConfirmed(0);
            //Assert
            await _courseRepo.Received().RemoveCourse(course);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult)?.ActionName);

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

    
    }
}