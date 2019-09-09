using System;
using aspNETfirstProject.Models;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Security.Principal;
using aspNETfirstProject.Repository;
using System.Web;
using System.Collections.Generic;
using aspNETfirstProject.Controllers;
using System.Web.Mvc;
using aspNETfirstProject.ViewModels;

namespace aspNetMvcProjectTesting
{

    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
        }

        [Fact]
        public void ChangesCustomerName()
        {
            Item cust = new Item();
            cust.ID = 2;
            Assert.Equal(2, cust.ID);
        }

        [Fact]
        public async Task Index_ReturnsActionResult()
        {

            var username = "FakeUserName";
            var identity = new GenericIdentity(username,"");

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            
            var mockHttpContext = new Mock<HttpContextBase>();
            //to add context base. add ref c/windos/asembly/gac_23/.../system.web.dll
            mockHttpContext.Setup(m => m.User).Returns(mockPrincipal.Object);


            //var username = "FakeUserName";
            //var mocks = new MockRepository(MockBehavior.Default);
            //Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            //mockPrincipal.SetupGet(p => p.Identity.).Returns(username);
            //mockPrincipal.Setup(p => p.IsInRole("admin")).Returns(true);

            // create mock controller context



            //var context = new Mock<HttpContext>();
            //var mockIdentity = new Mock<IIdentity>();
            //context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            //mockIdentity.Setup(x => x.Name).Returns("test_name");

            string DeviceType = "Devices";//APs, Phones, Switches

            //"ID,ItemType,Title,Description,ImagePath,DocumentPath,Approved,UserID,Created_at,Updated_at
            int id = 1;
            ItemType itemType = ItemType.device;

            //var Item = new Item { ID= id, ItemType = itemType,Title = "Device title", Description = "Device Desc",
            //Approved = false, UserID = "123abc"};

            var mock = new Mock<IItemsRepository>();
            IList<Item> listOfItem = new List<Item>();
            listOfItem.Add(new Item
            {
                ID = id,
                ItemType = itemType,
                Title = "Device title",
                Description = "Device Desc",
                Approved = false,
                UserID = "123abc"
            });
            //mock.Setup(x => x.getTitle(It.IsAny<string>()).Returns("asdfasfj");
            mock.Setup(x => x.getTitle("asdfa")).Returns("Device Title");
            mock.Setup(x => x.GetItems("Devices")).ReturnsAsync(listOfItem);


            var controller = new ItemsController(mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            ActionResult result = await controller.Index("Devices");

            //var model = Xunit.Assert.NotNull(result);
            //var test = result as ViewResult;
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            ItemsViewModel i = viewResult.Model as ItemsViewModel;
            //viewResult.
            ////var model = Assert.re
            //Assert.Same(listOfItem, result);
            //Xunit.Assert.Equal(, actual.id);
            Assert.Equal(1, i.Items.Count);
        }


    }
}
