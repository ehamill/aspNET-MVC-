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
            
            var mockHttpContext = new Mock<HttpContextBase>(); //to add contextbase. add ref c/windos/asembly/gac_23/.../system.web.dll
            mockHttpContext.Setup(m => m.User).Returns(mockPrincipal.Object);
            
            int id = 1;
            ItemType itemType = ItemType.device;
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
            var mock = new Mock<IItemsRepository>();
            mock.Setup(x => x.getTitle(It.IsAny<ItemType>())).Returns("Device Title");
            mock.Setup(x => x.GetItems(It.IsAny<ItemType>())).ReturnsAsync(listOfItem);
            
            var controller = new ItemsController(mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            ActionResult result = await controller.Index(It.IsAny<ItemType>());

            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            ItemsViewModel i = viewResult.Model as ItemsViewModel;
            Assert.Equal(1, i.Items.Count);
        }


    }
}
