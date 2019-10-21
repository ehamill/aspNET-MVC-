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
        public async Task Index_Get_All_Items_ReturnsActionResult()
        {
            //Arrange
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

            //Act
            ActionResult result = await controller.Index(It.IsAny<ItemType>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            ItemsViewModel i = viewResult.Model as ItemsViewModel;
            Assert.Equal(1, i.Items.Count);
        }

        [Fact]
        public void Get_Create_Item_ReturnsActionResult()
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
        public async Task Post_Create_Item_User_Admin_Returns_Item_Approved_AndActionResult()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContextBase>();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
            httpContextMock.Setup(m => m.User).Returns(mockPrincipal.Object);

            //Mock a file download
            var request = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            httpContextMock.SetupGet(x => x.Request.Files[It.IsAny<string>()]).Returns(request.Object);
            Item item = new Item { ID=1,  UserID="john123", };
            
            var repository = new Mock<IItemsRepository>();

            var controller = new ItemsController(repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };
            controller.ModelState.AddModelError("title","title is required");
            
            //Act
            var result =  await controller.Create(item);
            //Assert
            Assert.True(item.Approved);
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Post_Create_Item_User_Not_Admin_Returns_Item_Not_Approved_And_ReturnsRedirectToActionResult()
        {
            //var username = "FakeUserName";
            //var identity = new GenericIdentity(username,"");
            //Arrange
            var httpContextMock = new Mock<HttpContextBase>();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(false);
            httpContextMock.Setup(m => m.User).Returns(mockPrincipal.Object);

            Item item = new Item { ID = 1, UserID = "john123", };

            //Mock a file download
            var request = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            httpContextMock.SetupGet(x => x.Request.Files[It.IsAny<string>()]).Returns(request.Object);
            
            var repository = new Mock<IItemsRepository>();
            repository.Setup(repo => repo.AddItem(item))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ItemsController(repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            //Act
            var result = await controller.Create(item);
            
            //Assert
            var viewResult = Assert.IsType<RedirectToRouteResult>(result);
            Assert.False(item.Approved);
            repository.Verify();
        }

        [Fact]
        public async Task Get_Edit_Null_Item_ReturnsHttpNotFound()
        {
            //Arrange
            var mock = new Mock<IItemsRepository>();
            Item item = null;
            mock.Setup(x => x.GetItem(It.IsAny<int>())).ReturnsAsync(item);
            var controller = new ItemsController(mock.Object);
            int id = 1;
            
            //Act
            ActionResult result = await controller.Edit(id);

            //Assert
            var viewResult = Assert.IsType<HttpNotFoundResult>(result);
            mock.Verify();
        }

        [Fact]
        public async Task Get_Edit_Item_ReturnsViewResult()
        {
            //Arrange
            var mock = new Mock<IItemsRepository>();
            Item item = new Item { ID = 1, UserID = "john123", Title = "New Title" };
            mock.Setup(x => x.GetItem(It.IsAny<int>())).ReturnsAsync(item);
            var controller = new ItemsController(mock.Object);
            
            //Act
            ActionResult result = await controller.Edit(item.ID);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Item>(viewResult.ViewData.Model);
            Assert.Equal(item.ID, model.ID);
            Assert.Equal(item.UserID, model.UserID);
            Assert.Equal(item.Title, model.Title);
        }

        [Fact]
        public async Task Post_Edit_Item_Fails_User_Admin_Returns_Item_Approved_AndActionResult()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContextBase>();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
            httpContextMock.Setup(m => m.User).Returns(mockPrincipal.Object);

            //Mock a file download
            var request = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            httpContextMock.SetupGet(x => x.Request.Files[It.IsAny<string>()]).Returns(request.Object);
            Item item = new Item { ID = 1, UserID = "john123", };

            var repository = new Mock<IItemsRepository>();

            var controller = new ItemsController(repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };
            controller.ModelState.AddModelError("title", "title is required");

            //Act
            var result = await controller.Edit(item,"","");
            //Assert
            Assert.True(item.Approved);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Item>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Post_Edit_Item_Success_User_Admin_Returns_Item_Not_Approved_AndRedirectResult()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContextBase>();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(false);
            httpContextMock.Setup(m => m.User).Returns(mockPrincipal.Object);

            //Mock a file download
            var request = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            httpContextMock.SetupGet(x => x.Request.Files[It.IsAny<string>()]).Returns(request.Object);
            Item item = new Item { ID = 1, UserID = "john123", ItemType = ItemType.ap };

            var repository = new Mock<IItemsRepository>();
            repository.Setup(repo => repo.UpdateItem(It.IsAny<Item>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ItemsController(repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };

            //Act
            var result = await controller.Edit(item, "", "");
            //Assert
            Assert.False(item.Approved);
            var RedirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
            Assert.Equal("Index", RedirectToRouteResult.RouteValues["action"]);
            Assert.Equal(ItemType.ap, RedirectToRouteResult.RouteValues["ItemType"]);
            repository.Verify(); //Only Verifies update was actually called.

        }



    }
}
