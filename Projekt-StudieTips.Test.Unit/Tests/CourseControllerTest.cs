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

namespace Projekt_StudieTips.Test.Unit
{
    [TestFixture]
    public class CourseControllerTest
    {

        private CoursesController _uut;
        
        
        [SetUp]
        public void Setup()
        {
            _uut = new CoursesController(Substitute.For<ICourseRepository>(), Substitute.For<IDegreeRepository>());
        }

        [Test]
        public void Index_ReturnsView(int id)
        {
            var result = _uut.Index(id);

            Assert.IsInstanceOf(typeof(ViewResult), result);
           
        }

        [Test]
        public void _IfCourseExists()
        {
            return; 
        }
    }
}