using System;
using Xunit;

namespace XUnitTestProject1
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }
}
