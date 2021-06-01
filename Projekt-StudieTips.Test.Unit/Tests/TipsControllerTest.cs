using NUnit.Framework;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Controllers;
using Projekt_StudieTips.Repository;
using NSubstitute;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PagedList;

namespace Projekt_StudieTips.Test.Unit
{
    class TipsControllerTest
    {

        private TipsController _uut;
        private ITipRepository _TipRepo;
        private UserManager<IdentityUser> _user;
        [SetUp]
        public void Setup()
        {
            _TipRepo = Substitute.For<ITipRepository>();
            _uut = new(_TipRepo, _user);
        }

        [Test]
        public async Task Index_Null_ID_ReturnsRedirect()
        {
            //Arrange
            
            //Act
            var result = await _uut.Index(null,"date_desc",1);
            //Assert
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task Index_NoTips_ReturnsViewWithNoTips()
        {
            //Arrange
            List<Tip> tips = new();
            Course c = new();
            c.CourseId = 1;
            c.CourseName = "TestString";
            
            _TipRepo.GetTips(1,"date_desc").Returns(tips);
            _TipRepo.GetCourse(1).Returns(c);

            //Act
            var result = await _uut.Index(1, "date_desc", 1);
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            await _TipRepo.Received().GetTips(1, "date_desc");
            await _TipRepo.Received().GetCourse(1);
            Assert.That((int)((ViewResult)result).ViewData["CourseId"] == c.CourseId);
            Assert.That((string)((ViewResult)result).ViewData["CourseName"] == c.CourseName);
            Assert.That((string)((ViewResult)result).ViewData["NoTip"] == "har desværre ingen tips..");
        }

        [Test]
        public async Task Index_TipsExist_ReturnsViewWithTips()
        {
            //Arrange
            Course c = new();
            c.CourseId = 1;
            c.CourseName = "TestString";
            var TipMock = Substitute.For<Tip>();
            TipMock.TipId = 1;
            TipMock.CourseId = 1;
            TipMock.Course = c;
            TipMock.IsVerified = true;
            List<Tip> tips = new();
            tips.Add(TipMock);
            _TipRepo.GetTips(1, "date_desc").Returns(tips);

            //Act
            var result = await _uut.Index(1, "date_desc", 1);
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            await _TipRepo.Received().GetTips(1, "date_desc");
            Assert.That((int)((ViewResult)result).ViewData["CourseId"] == TipMock.CourseId);
            Assert.That((string)((ViewResult)result).ViewData["CourseName"] == TipMock.Course.CourseName);
            Assert.IsAssignableFrom<PagedList<Tip>>(((ViewResult)result).ViewData.Model);
        }

        [Test]
        public void SearchTip_Null_Term_ReturnsRedirect()
        {
            //Arrange

            SearchDto search = new();
            search.SearchTerm = null;

            //Act
            var result = _uut.SearchTip("date_desc",1, search);
            //Assert
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task SearchTip_NoTips_ReturnsViewWithNoTips()
        {
            //Arrange
            SearchDto search = new();
            search.SearchTerm = "term";

            List<Tip> tips = new();
            Course c = new();
            c.CourseId = 1;
            c.CourseName = "TestString";

            _TipRepo.GetTipsWithinSearchTerm(search).Returns(tips);
            _TipRepo.GetCourse(1).Returns(c);

            //Act
            var result = _uut.SearchTip("date_desc", 1, search);
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            await _TipRepo.Received().GetTipsWithinSearchTerm(search);
            Assert.That((int)((ViewResult)result).ViewData["CourseId"] == 0);
            Assert.That((string)((ViewResult)result).ViewData["CourseName"] == "Vi kunne ikke finde nogle tips som indeholdte søgsordene.");
        }

        [Test]
        public async Task SearchTip_TipsExist_ReturnsViewWithTips()
        {
            //Arrange
            SearchDto search = new();
            search.SearchTerm = "term";
            Course c = new();
            c.CourseId = 1;
            c.CourseName = "TestString";
            var TipMock = Substitute.For<Tip>();
            TipMock.TipId = 1;
            TipMock.CourseId = 1;
            TipMock.Course = c;
            TipMock.IsVerified = true;
            List<Tip> tips = new();
            tips.Add(TipMock);
            _TipRepo.GetTipsWithinSearchTerm(search).Returns(tips);

            //Act
            var result = _uut.SearchTip("date_desc", 1, search);
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            await _TipRepo.Received().GetTipsWithinSearchTerm(search);
            Assert.That((int)((ViewResult)result).ViewData["CourseId"] == TipMock.CourseId);
            Assert.That((string)((ViewResult)result).ViewData["CourseName"] == TipMock.Course.CourseName);
            Assert.IsAssignableFrom<PagedList<Tip>>(((ViewResult)result).ViewData.Model);
        }

        [Test]
        public void Create_Null_Id_ReturnsNotFound()
        {
            //Arrange

            //Act
            var result = _uut.Create(value:null);
            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void Create_ReturnsAViewResult()
        {
            //Arrange

            //Act
            var result = _uut.Create(1);
            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.That((int)((ViewResult)result).ViewData["CourseId"] == 1);
        }

        [Test]
        public async Task Create_WithBoundTip_CreatesTip_ReturnsAViewResult()
        {
            //Arrange

            var TipMock = Substitute.For<Tip>();


            //Act
            var result = await _uut.Create(TipMock);
            //Assert
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            await _TipRepo.Received().AddTip(TipMock);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task Edit_Null_Id_ReturnsNotFound()
        {
            //Arrange

            //Act
            var result = await _uut.Edit(null);
            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task Edit_NoTip_ReturnsNotFound()
        {
            //Arrange

            //Act
            var result = await _uut.Edit(1);
            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task Edit_BoundTip_WrongID_ReturnsNotFound()
        {
            //Arrange

            var TipMock = Substitute.For<Tip>();

            TipMock.TipId = 2;

            //Act
            var result = await _uut.Edit(1,TipMock);
            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task Edit_BoundTip_CallsUpdateTip_ReturnsToIndex()
        {
            //Arrange

            var TipMock = Substitute.For<Tip>();

            TipMock.TipId = 1;

            //Act
            var result = await _uut.Edit(1, TipMock);
            //Assert
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
            await _TipRepo.Received().UpdateTip(TipMock);

        }

        [Test]
        public async Task DeleteConfirmed_CallsUpdate_Tip_ReturnsToIndex()
        {
            //Arrange
            //Act
            var result = await _uut.DeleteConfirmed(1);
            //Assert
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
            await _TipRepo.Received().DeleteTip(1);

        }
    }
}
