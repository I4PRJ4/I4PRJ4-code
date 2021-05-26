using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Controllers;
using Projekt_StudieTips.Repository;
using NUnit.Framework;

namespace Projekt_StudieTips.Test.Unit
{
    [TestFixture]
    public class CourseControllerTest
    {

        private CoursesController _uut;
        
        
        [SetUp]
        public void Setup()
        {
            _uut = new CoursesController();
        }

        [Test]
        public void TestCourseCreate()
        {
           
        }
    }
}