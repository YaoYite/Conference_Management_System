using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Controllers.Tests
{
    [TestClass()]
    public class papersControllerTests
    {
        [TestMethod()]
        public void AssignTest()
        {
            papersController controller = new papersController();
            var test = controller.Assign("6", "2", "assign 2 reviewers to one paper") as ViewResult;
            Assert.AreEqual("AssignError", test.ViewName);
        }

        [TestMethod()]
        public void AssignTest1()
        {
            papersController controller = new papersController();
            var test = controller.Assign("6", "5", "assign more than 4 reviewers to one paper") as ViewResult;
            Assert.AreEqual("AssignError", test.ViewName);
        }

        [TestMethod()]
        public void AssignTest2()
        {
            papersController controller = new papersController();
            var test = controller.Assign("7", "4", "Normal assign") as ViewResult;
            Assert.AreEqual("SeeResult", test.ViewName);
        }

        [TestMethod()]
        public void CreateTest()
        {
            papersController controller = new papersController();
            var test = controller.Create("7", "4", "Normal assign") as ViewResult;
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest1()
        {
            papersController controller = new papersController();
            var test = controller.Create("123", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "3") as ViewResult;
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest2()
        {
            papersController controller = new papersController();
            var test = controller.Create("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "", "3") as ViewResult;
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest3()
        {
            papersController controller = new papersController();
            var test = controller.Create("123", "1234567890
                asdfghjkl;
                qwertyuiop
                zxcvbnm,./
                ", "3") as ViewResult;
            Assert.Fail();
        }

    }
}