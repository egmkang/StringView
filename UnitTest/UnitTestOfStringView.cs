using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t1;

namespace UnitTest
{
    [TestClass]
    public class UnitTestOfStringView
    {
        [TestMethod]
        public void Test_ToString()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.ToString(), "1234567890");
        }

        [TestMethod]
        public void Test_Index()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view[0], '1');
            Assert.AreEqual(view[1], '2');
            Assert.AreEqual(view[2], '3');
            try
            {
                char a = view[1000];
                a = '1';
                Assert.Fail("No Exception Throw");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.IndexOf("OutOfBound") >= 0);
            }
        }

        [TestMethod]
        public void Test_IndexOf_Char()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.IndexOf('1'), 0);
            Assert.AreEqual(view.IndexOf('2'), 1);
            Assert.AreEqual(view.IndexOf('3'), 2);
            Assert.AreEqual(view.IndexOf('z'), -1);
        }

        [TestMethod]
        public void Test_IndexOf_CharArray()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.IndexOf(new char[2] { '1', '2' }), 0);
            Assert.AreEqual(view.IndexOf(new char[2] { '2', '8' }), 1);
            Assert.AreEqual(view.IndexOf(new char[2] { '9', '0' }), 8);
            Assert.AreEqual(view.IndexOf(new char[1] { 'A' }), -1);
            Assert.AreEqual(view.IndexOf(new char[3] { 'A', 'v', 'c' }), -1);
            Assert.AreEqual(view.IndexOf(new char[10] { '0', '2', '3', '4', '5', '6', '8', '7', '9', '1' }), 0);
        }

        [TestMethod]
        public void Test_IndexOf_String()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.IndexOf("12"), 0);
            Assert.AreEqual(view.IndexOf("90"), 8);
            Assert.AreEqual(view.IndexOf("901"), -1);
        }

        [TestMethod]
        public void Test_SubString()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.Substring(0), view);
            Assert.AreNotEqual(view.Substring(1), view);
        }
    }
}
