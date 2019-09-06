using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using aspNETfirstProject.Models;
using aspNETfirstProject.Controllers;
using aspNETfirstProject.Repository;
using Moq;
using System.Web.Mvc;
using aspNETfirstProject.ViewModels;

namespace aspNETfirstProject_2019.test
{
    public class ItemsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsActionResult() {
            string DeviceType = "Devices";//APs, Phones, Switches

            //"ID,ItemType,Title,Description,ImagePath,DocumentPath,Approved,UserID,Created_at,Updated_at
            //int id = 1;
            //ItemType itemType = ItemType.device;

            //var Item = new Item { ID= id, ItemType = itemType,Title = "Device title", Description = "Device Desc",
            //Approved = false, UserID = "123abc"};

            var mock = new Mock<IItemsRepository>();
            IList<Item> listOfItem = new List<Item>();
            //mock.Setup(x => x.getTitle(It.IsAny<string>()).Returns("asdfasfj");
            mock.Setup(x => x.getTitle("asdfa")).Returns("Device Title");
            mock.Setup(x => x.GetItems("Devices")).ReturnsAsync(listOfItem);

            var controller = new ItemsController(mock.Object);

            var result = await controller.Index(DeviceType) as ViewResult;

            //var viewResult = Assert.IsType<ItemsViewModel>(result);
            ////var model = Assert.re
            //Assert.Same(listOfItem, result);
            //Assert.Equal(id, actual.id);
            //Assert.Equal("Index", viewResult.);
       }

    }
}
