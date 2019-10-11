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

    public class ItemsControllerTest
    {
        
        [Fact]
        public async Task Index_ReturnsActionResult()
        {
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

        [Fact]
        public void Create_ReturnsActionResult()
        {
            //Arrange
            var mock = new Mock<IItemsRepository>();
            var controller = new ItemsController(mock.Object);
            //Act
            ActionResult result = controller.Create(It.IsAny<ItemType>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public async Task Create_User_Admin_Returns_Item_Approved_AndActionResult()
        {
            var httpContextMock = new Mock<HttpContextBase>();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
            httpContextMock.Setup(m => m.User).Returns(mockPrincipal.Object);

            //Mock a file download
            var request = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            httpContextMock.SetupGet(x => x.Request.Files[It.IsAny<string>()]).Returns(request.Object);

            //Arrange
            var repository = new Mock<IItemsRepository>();
            var controller = new ItemsController(repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };
            controller.ModelState.AddModelError("title","title is required");
            Item item = new Item { ID=1,  UserID="john123", };

            var result =  await controller.Create(item);
            Assert.True(item.Approved);
            var viewResult = Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public async Task Create_User_Not_Admin_Returns_Item_Not_Approved_And_ReturnsRedirectToActionResult()
        {
            //var username = "FakeUserName";
            //var identity = new GenericIdentity(username,"");

            var httpContextMock = new Mock<HttpContextBase>();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(false);
            httpContextMock.Setup(m => m.User).Returns(mockPrincipal.Object);
            
            //Mock a file download
            var request = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            httpContextMock.SetupGet(x => x.Request.Files[It.IsAny<string>()]).Returns(request.Object);
            
            //Arrange
            var repository = new Mock<IItemsRepository>();
            var controller = new ItemsController(repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };
            Item item = new Item { ID = 1, UserID = "john123", };

            var result = await controller.Create(item);
            
            var viewResult = Assert.IsType<RedirectToRouteResult>(result);
            Assert.False(item.Approved);
        }


    }
}
