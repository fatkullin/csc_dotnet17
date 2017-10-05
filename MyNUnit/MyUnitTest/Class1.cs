using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyNUnitAttributes;

namespace MyUnitTest
{
    internal class MyUnitTest
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
        public static void AfterClassX()
        {
        }

        [AfterClass]
        public static void AfterClassY()
        {
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
            double zero = 0;
            double a = 5 / zero;
        }

        [Test]
        [Expected(typeof(NullReferenceException))]
        public static void NRXMethod()
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
