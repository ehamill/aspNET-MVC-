using aspNETfirstProject.Controllers;
using aspNETfirstProject.Models;
using aspNETfirstProject.Repository;
using aspNETfirstProject.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace XUnitTestProject3
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
        public async Task Testing_ReturnsActionResult()
        {
            
            var mock = new Mock<IItemsRepository>();
            mock.Setup(x => x.getTitle(It.IsAny<string>())).Returns("Device Title");
            

            var controller = new ItemsController(mock.Object);

            var result =  controller.Testing();
            Assert.NotNull(result);
            // var test = result as ViewResult;
            //var viewResult = Xunit.Assert.IsType<ItemsViewModel>(result);
            ////var model = Assert.re
            //Assert.Same(listOfItem, result);
            //Assert.Equal(id, actual.id);
            //Assert.Equal("Index", viewResult.);
        }


        [Fact]
        public async Task Index_ReturnsActionResult()
        {
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

            var controller = new ItemsController(mock.Object);

            var result = await controller.Index("Devices");
            var test = result as ViewResult;
            var viewResult = Xunit.Assert.IsType<ItemsViewModel>(result);
            ////var model = Assert.re
            //Assert.Same(listOfItem, result);
            //Assert.Equal(id, actual.id);
            //Assert.Equal("Index", viewResult.);
        }



    }
}
