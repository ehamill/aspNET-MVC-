using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspNETfirstProject.Controllers;
using aspNETfirstProject.Models;
using aspNETfirstProject.Repository;
using aspNETfirstProject.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        //[Fact]
        //public void ChangesCustomerName()
        //{
        //    Item cust = new Item();
        //    cust.ID = 2;
        //    Assert.Equal(2, cust.ID);
        //}
        
        [Fact]
        public async Task Index_ReturnsActionResult()
        {
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

            //var controller = new ItemsController(mock.Object);

            //var result = await controller.Index(DeviceType); //as ViewResult;

            //var viewResult = Xunit.Assert.IsType<ItemsViewModel>(result);
            ////var model = Assert.re
            //Assert.Same(listOfItem, result);
            //Assert.Equal(id, actual.id);
            //Assert.Equal("Index", viewResult.);
        }


    }
}
