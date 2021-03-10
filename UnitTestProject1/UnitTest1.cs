using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ProverkaSrav()
        {
            var page = new MainWindow();
            Assert.IsTrue(page.Sravnenie("Book","Книга"));
            Assert.IsTrue(page.Sravnenie("Shop","магазин"));
            Assert.IsTrue(page.Sravnenie("shop", "Магазин"));
            Assert.IsTrue(page.Sravnenie("shop", "магазин"));
            Assert.IsTrue(page.Sravnenie("Pig", "Свинья"));
            Assert.IsTrue(page.Sravnenie("User", "Пользователь"));
            Assert.IsFalse(page.Sravnenie("", ""));
            Assert.IsFalse(page.Sravnenie("132", "3113"));
            Assert.IsFalse(page.Sravnenie("Пользователь", "User"));
        }
    }
}
