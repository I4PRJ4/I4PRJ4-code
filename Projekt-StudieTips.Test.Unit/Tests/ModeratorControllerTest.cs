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
using PagedList;

namespace Projekt_StudieTips.Test.Unit
{
    class ModeratorControllerTest
    {

        private ModeratorController _uut;
        private ITipRepository _TipRepo;

        [SetUp]
        public void Setup()
        {
            _TipRepo = Substitute.For<ITipRepository>();
            _uut = new ModeratorController(_TipRepo);
        }

        [Test]
        public async Task IndexCall_TipsExist_ReturnsView()
        {
            // Arrange
            var tip = new Tip
            {
                TipId = 1
            };
            List<Tip> tips = new();
            tips.Add(tip);

            _TipRepo.GetUnmoderatedTips().Returns(tips);

            // Act
            var result = await _uut.Index("date_desc", 1);

            // Assert
            
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.IsAssignableFrom<PagedList<Tip>>(((ViewResult)result).ViewData.Model);
        }

        [Test]
        public async Task IndexCall_NoTipsExist_ReturnsViewWithViewBag()
        {
            // Arrange
            List<Tip> tips = new();

            _TipRepo.GetUnmoderatedTips().Returns(tips);

            // Act
            var result = await _uut.Index("date_desc", 1);

            // Assert

            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.IsAssignableFrom<PagedList<Tip>>(((ViewResult)result).ViewData.Model);
            string ViewBagData = "Der er ingen tips at verificere.";
            Assert.That(((ViewResult)result).ViewData["CourseName"] == ViewBagData);
        }

        [Test]
        public async Task VerifyTip_Null_id_ReturnsNotFound()
        {
            // Arrange

            // Act

            var result = await _uut.VerifyTip(null);

            // Assert

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task VerifyTip_NoTip_ReturnsNotFound()
        {
            // Arrange


            // Act

            var result = await _uut.VerifyTip(1);

            // Assert

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task VerifyTip_TipExists_ReturnsViewandVerifiesTip()
        {
            // Arrange
            var TipMock = Substitute.For<Tip>();
            TipMock.TipId = 1;
            TipMock.IsVerified = false;
            _TipRepo.GetTip(1).Returns(TipMock);

            // Act

            var result = await _uut.VerifyTip(1);

            // Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
            Assert.That(TipMock.IsVerified == true);
            
            _TipRepo.Received().GetTip(1);
            _TipRepo.Received().SaveChanges();
        }

        [Test]
        public async Task DenyTip_ReturnsViewandDeletesTip()
        {
            // Arrange

            // Act

            var result = await _uut.DenyTip(1);

            // Assert

            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
            
            _TipRepo.Received().DeleteTip(1);
        }
    }
}
