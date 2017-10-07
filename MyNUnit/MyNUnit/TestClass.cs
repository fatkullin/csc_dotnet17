using System.Collections.Generic;
using System.Reflection;

namespace MyNUnit
{
    internal class TestClass : ITestClass
    {
        public TestClass(string name)
        {
            Name = name;
        }

        public IEnumerable<MethodInfo> BeforeClassMethods => BeforeClassMethodList;
        public IEnumerable<MethodInfo> BeforeMethods => BeforeMethodList;
        public IEnumerable<MethodInfo> AfterTestMethods => AfterMethodList;
        public IEnumerable<MethodInfo> AfterClassMethods => AfterClassMethodList;
        public IEnumerable<TestMethod> TestMethods => TestMethodList;
        public string Name { get; }

        public readonly List<TestMethod> TestMethodList = new List<TestMethod>();
        public readonly List<MethodInfo> BeforeMethodList = new List<MethodInfo>();
        public readonly List<MethodInfo> AfterMethodList = new List<MethodInfo>();
        public readonly List<MethodInfo> BeforeClassMethodList = new List<MethodInfo>();
        public readonly List<MethodInfo> AfterClassMethodList = new List<MethodInfo>();

        public bool Empty()
        {
            return TestMethodList.Count == 0;
        }

    }
}