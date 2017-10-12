using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Option;

namespace OptionTests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void SomeTest_ValueType()
        {
            var someValue = Option<int>.Some(10);
            const int expected = 10;
            Assert.AreEqual(someValue.Value(), expected);
        }

        [TestMethod]
        public void SomeTest_ReferenceType()
        {
            const int firstElement = -1;
            var someValue = Option<ArrayList>.Some(new ArrayList { firstElement });
            Assert.AreEqual(someValue.Value()[0], firstElement);
        }

        [TestMethod]
        public void NoneTest_ValueType()
        {
            var someValue = Option<int>.None();
            Assert.IsTrue(someValue.IsNone());
        }

        [TestMethod]
        public void NoneTest_ReferenceType()
        {
            var someValue = Option<ArrayList>.None();
            Assert.IsTrue(someValue.IsNone());
        }

        [TestMethod]
        public void IsSomeTest()
        {
            var someValue = Option<ArrayList>.Some(new ArrayList { 0 });
            Assert.IsTrue(someValue.IsSome());
            Assert.IsFalse(someValue.IsNone());
        }

        [TestMethod]
        public void IsNoneTest()
        {
            var someValue = Option<double>.None();
            Assert.IsTrue(someValue.IsNone());
            Assert.IsFalse(someValue.IsSome());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValueTest()
        {
            var someValue = Option<double>.None();
            var value = someValue.Value();
            Debug.WriteLine(value);
        }

        [TestMethod]
        public void MapTest_Some()
        {
            var two = Option<int>.Some(2);
            var expected = Option<int>.Some(4);
            Assert.AreEqual(two.Map(x => x * 2), expected);
        }

        [TestMethod]
        public void MapTest_None()
        {
            var none = Option<int>.None();
            var expected = Option<int>.None();
            Assert.AreEqual(none.Map(x => x * 2), expected);
        }

        [TestMethod]
        public void FlattenTest_NoneOptionOfOption()
        {
            var none = Option<Option<int>>.None();
            var expected = Option<int>.None();
            Assert.AreEqual(none.Flatten(), expected);
        }

        [TestMethod]
        public void FlattenTest_SomeOptionOfNoneOption()
        {
            var none = Option<Option<int>>.Some(Option<int>.None());
            var expected = Option<int>.None();
            Assert.AreEqual(none.Flatten(), expected);
        }

        [TestMethod]
        public void FlattenTest_SomeOptionOfSomeOption()
        {
            var none = Option<Option<int>>.Some(Option<int>.Some(5));
            var expected = Option<int>.Some(5);
            Assert.AreEqual(none.Flatten(), expected);
        }
    }
}