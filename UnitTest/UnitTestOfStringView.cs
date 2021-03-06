﻿// Copyright (c) egmkang wang. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace UnitTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class UnitTestOfStringView
    {
        [TestMethod]
        public void Test_ToString()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.ToString(), "1234567890");

            Assert.AreEqual(view.Substring(1, 2).ToString(), "23");
        }

        [TestMethod]
        public void Test_Equals()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view, "1234567890");
            Assert.AreEqual(view.Substring(0, 2), "12");
            Assert.AreEqual(new StringView("123123", 0, 3), new StringView("234123123", 3, 3));
            Assert.AreEqual(StringView.Empty, StringView.Empty);
        }


        static StringView[] splitExpectedArray = new StringView[]
            {
                new StringView("1"),
                new StringView("2"),
                new StringView("3"),
                new StringView("4"),
                new StringView("5"),
                new StringView("6"),
                new StringView("7"),
                new StringView("8"),
                new StringView("9"),
                new StringView("0"),
            };

        [TestMethod]
        public void Test_SplitByChar()
        {
            StringView view = new StringView("1,2,3,4,5,6,7,8,9,0");
            var array = view.Split(',');
            Assert.AreEqual(array.Length, 10);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(array[i], splitExpectedArray[i]);
            }

            var result = Enumerable.SequenceEqual(StringView.Empty.Split('_'), new StringView[1] { StringView.Empty });
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void Test_SplitByCharArray()
        {
            StringView view = new StringView("1,2|3@4!5,6#7@8!9,0");
            var array = view.Split(',', '|', '@', '!', '#');
            Assert.AreEqual(array.Length, 10);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(array[i], splitExpectedArray[i]);
            }
        }

        [TestMethod]
        public void Test_SplitByString()
        {
            StringView view = new StringView("1,2,3,4,5,6,7,8,9,0");
            var array = view.Split(",");
            Assert.AreEqual(array.Length, 10);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(array[i], splitExpectedArray[i]);
            }
        }

        [TestMethod]
        public void Test_SplitByBlank()
        {
            StringView view = new StringView("1\t2\n3 4 5 6 7 8 9 0");
            var array = view.Split((string[])null);
            Assert.AreEqual(array.Length, 10);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(array[i], splitExpectedArray[i]);
            }
        }

        [TestMethod]
        public void Test_SplitByStrings()
        {
            StringView view = new StringView("1,2|3@4!5,6#7@8!9,0");
            var array = view.Split(",", "|", "@", "!", "#");
            Assert.AreEqual(array.Length, 10);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(array[i], splitExpectedArray[i]);
            }
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
                Assert.IsTrue(e is ArgumentOutOfRangeException ? true : false);
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
            Assert.AreEqual(view.IndexOfAny(new char[2] { '1', '2' }), 0);
            Assert.AreEqual(view.IndexOfAny(new char[2] { '2', '8' }), 1);
            Assert.AreEqual(view.IndexOfAny(new char[2] { '9', '0' }), 8);
            Assert.AreEqual(view.IndexOfAny(new char[1] { 'A' }), -1);
            Assert.AreEqual(view.IndexOfAny(new char[3] { 'A', 'v', 'c' }), -1);
            Assert.AreEqual(view.IndexOfAny(new char[10] { '0', '2', '3', '4', '5', '6', '8', '7', '9', '1' }), 0);
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
        public void Test_LastIndexOf_Char()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.LastIndexOf('0'), view.Length - 1);
            Assert.AreEqual(view.LastIndexOf('9'), view.Length - 2);
            Assert.AreEqual(view.LastIndexOf('1'), 0);
            Assert.AreEqual(view.LastIndexOf('-'), -1);
        }

        [TestMethod]
        public void Test_LastIndexOf_Chars()
        {
            StringView view = new StringView("1234567890");

            Assert.AreEqual(view.LastIndexOfAny('0', '9'), view.Length - 1);
            Assert.AreEqual(view.LastIndexOfAny('9', '+'), view.Length - 2);
            Assert.AreEqual(view.LastIndexOfAny('-', '1'), 0);
            Assert.AreEqual(view.LastIndexOfAny('-', '+'), -1);
        }

        [TestMethod]
        public void Test_LastIndexOf_String()
        {
            StringView view = new StringView("1234567890");

            Assert.AreEqual(view.LastIndexOf("90"), view.Length - 2);
            Assert.AreEqual(view.LastIndexOf("80"), -1);
            Assert.AreEqual(view.LastIndexOf("12345678901212"), -1);
            Assert.AreEqual(view.LastIndexOf("12"), 0);
            Assert.AreEqual(view.LastIndexOf("23"), 1);
            Assert.AreEqual(view.LastIndexOf("1234567890"), 0);
        }

        [TestMethod]
        public void Test_StarstWith()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.StartsWith(String.Empty), true);
            Assert.AreEqual(view.StartsWith("12"), true);
            Assert.AreEqual(view.StartsWith("12345678901"), false);
            Assert.AreEqual(view.StartsWith("1234567890"), true);
        }

        [TestMethod]
        public void Test_EndsWith()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.EndsWith('0'), true);
            Assert.AreEqual(view.EndsWith("1"), false);
            Assert.AreEqual(view.EndsWith("12345678901"), false);
            Assert.AreEqual(view.EndsWith("1234567890"), true);
            Assert.AreEqual(view.EndsWith("90"), true);
        }

        [TestMethod]
        public void Test_SubString()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.Substring(0), view);
            Assert.AreNotEqual(view.Substring(1), view);
        }

        [TestMethod]
        public void Test_IsAscii()
        {
            StringView view = new StringView("1234567890");
            Assert.AreEqual(view.IsAscii(), true);
            Assert.AreEqual(new StringView("123456789012345678901234567890123456789012345678901234567890").IsAscii(), true);
            Assert.AreEqual(new StringView("1234567890哈哈").IsAscii(), false);
            Assert.AreEqual(new StringView("12哈哈34567890").IsAscii(), false);
            Assert.AreEqual(new StringView("12哈哈3456789012哈哈3456789012哈哈34567890").IsAscii(), false);
        }

        [TestMethod]
        public void Test_ToCharArray()
        {
            StringView view = new StringView("1234567890");
            var ret = view.ToCharArray();
            var expected = view.Original.ToCharArray();
            Assert.AreEqual(Enumerable.SequenceEqual(ret, expected), true);
            Assert.AreEqual(Enumerable.SequenceEqual(StringView.Empty.ToCharArray(), String.Empty.ToCharArray()), true);
            Assert.AreEqual(Enumerable.SequenceEqual(view.ToCharArray(1, 5), view.Original.ToCharArray(1, 5)), true);
        }

        [TestMethod]
        public void Test_Concat()
        {
            Assert.AreEqual(StringView.Concat("12", "23", "34", "45"), "12233445");
            Assert.AreEqual(StringView.Concat("1"), "1");
            Assert.AreEqual(StringView.Concat("12"), "12");
            Assert.AreEqual(StringView.Concat("123"), "123");
            Assert.AreEqual(StringView.Concat("1234"), "1234");
            Assert.AreEqual(StringView.Concat("12345"), "12345");
            Assert.AreEqual(StringView.Concat("123456"), "123456");
            Assert.AreEqual(StringView.Concat("1234567"), "1234567");
            Assert.AreEqual(StringView.Concat("12345678"), "12345678");
            Assert.AreEqual(StringView.Concat("1234567890~!@#$%^&*()_+QWERTYUIOP{}][poiuytrewqasdfghjkl;:LKJHGFDSAZXCVBNM<>?/.,mnbvcxz",
                "1234567890~!@#$%^&*()_+QWERTYUIOP{}][poiuytrewqasdfghjkl;:LKJHGFDSAZXCVBNM<>?/.,mnbvcxz"),
                "1234567890~!@#$%^&*()_+QWERTYUIOP{}][poiuytrewqasdfghjkl;:LKJHGFDSAZXCVBNM<>?/.,mnbvcxz" +
                "1234567890~!@#$%^&*()_+QWERTYUIOP{}][poiuytrewqasdfghjkl;:LKJHGFDSAZXCVBNM<>?/.,mnbvcxz"
                );
            Assert.AreEqual(StringView.Concat(new StringView("12"), new StringView("23"), new StringView("34"), new StringView("45")), "12233445");
        }

        [TestMethod]
        public void Test_Join()
        {
            Assert.AreEqual(StringView.Join("|", "1", "2", "3", "4"), "1|2|3|4");
            Assert.AreEqual(StringView.Join(null, "1", "2", "3", "4"), "1234");
            Assert.AreEqual(StringView.Join("|", new StringView[] { "1", "2", "3", "4" }), "1|2|3|4");
        }

        [TestMethod]
        public void Test_StringViewToInt()
        {
            Assert.AreEqual(new StringView("123456789").ToInt32(), 123456789);
            Assert.AreEqual(new StringView("-123456789").ToInt32(), -123456789);
            Assert.AreEqual(new StringView("-123456789121212").ToInt64(), -123456789121212);
        }

        [TestMethod]
        public void Test_StringBuilderAppend()
        {
            var builder = new System.Text.StringBuilder();
            builder.Append(new StringView("123456789"));
            Assert.AreEqual(builder.ToString(), "123456789");
        }

        [TestMethod]
        public void Test_HashCode()
        {
            Assert.AreEqual(new StringView("123456789").GetHashCode(),
                new StringView("0123456789", 1, 9).GetHashCode());
        }
    }
}
