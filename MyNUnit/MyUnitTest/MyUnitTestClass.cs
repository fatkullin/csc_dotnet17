using System;
using System.Threading;
using MyNUnitAttributes;
// ReSharper disable UnusedMember.Global

namespace MyUnitTest
{
    internal class MyUnitTestClass
    {
        [BeforeClass]
        public static void BeforeClass1()
        {
        }

        [BeforeClass]
        public static void BeforeClass2()
        {
        }

        [AfterClass]
        public static void AfterClassPrintMessage()
        {
            Console.WriteLine("Ensure: ignored: 1; succeeded: 2; failed: 3");
        }

        [Before]
        public static void BeforeAnyTest()
        {
        }

        [After]
        public static void AfterAnyTest()
        {
        }

        [Test]
        public static void TestMethod()
        {
            Console.WriteLine("Do something good");
            var rnd = new Random();
            Thread.Sleep(rnd.Next() % 1000);
        }

        [Test]
        [Ignore("Obsolete")]
        public static void ObsoleteMethod()
        {
            Console.WriteLine("Do something wrong");
            const double zero = 0;
            const double a = 5 / zero;
            Console.WriteLine(a);
        }

        [Test]
        [Expected(typeof(NullReferenceException))]
        public static void NrxMethod()
        {
            Console.WriteLine("Throw exception");
            throw new NullReferenceException("=)");
        }

        [Test]
        [Expected(typeof(NullReferenceException))]
        public static void ArgXUnexpectedMethod()
        {
            Console.WriteLine("Throw exception");
            throw new ArgumentException("=)");
        }

        [Test]
        public static void UnexpectedMethod()
        {
            Console.WriteLine("Throw exception");
            throw new ArgumentException("=)");
        }

        [Test]
        [Expected(typeof(NullReferenceException))]
        public static void MethodWithoutExpectedException()
        {
            Console.WriteLine("Do something good");
            var rnd = new Random();
            Thread.Sleep(rnd.Next() % 1000);
        }
    }
}
