using aspNETfirstProject.Models;
//using System;
//using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Threading.Tasks;
using aspNETfirstProject.Repository;
using System.Collections.Generic;
using aspNETfirstProject.Controllers;
using System.Web.Mvc;
using aspNETfirstProject.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using System.Web.Mvc;

namespace XUnitTestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
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
            string DeviceType = "Devices";//APs, Phones, Switches

            //"ID,ItemType,Title,Description,ImagePath,DocumentPath,Approved,UserID,Created_at,Updated_at
            int id = 1;
            ItemType itemType = ItemType.device;

            var items = new Item { ID= id, ItemType = itemType,Title = "Device title", Description = "Device Desc",
            Approved = false, UserID = "123abc"};

            var mock = new Mock<IItemsRepository>();
            var expected = new List<Item>(){
                new Item {
                    ID= id, ItemType = itemType,Title = "Device title", Description = "Device Desc",
                    Approved = false, UserID = "123abc"
                },
                new Item {
                    ID= id+1, ItemType = itemType,Title = "Device title2", Description = "Device Desc2",
                    Approved = true, UserID = "123abc2"
                }
            };
            //IList<Item> listOfItem = new List<Item>(items);
            //mock.Setup(x => x.getTitle(It.IsAny<string>()).Returns("asdfasfj");
            mock.Setup(x => x.getTitle(It.IsAny<string>())).Returns("Device Title");
            mock.Setup(x => x.GetItems(It.IsAny<string>())).ReturnsAsync(expected);

            var controller = new ItemsController(mock.Object);

            var result = await controller.Index(DeviceType);
            ViewResult test = result as ViewResult;
            var viewResult = Assert.IsType<ItemsViewModel>(result);
            ////var model = Assert.re
            //Assert.Same(listOfItem, result);
            //Assert.Equal(id, actual.id);
            //Assert.Equal("Index", viewResult.);
        }


    }
}
