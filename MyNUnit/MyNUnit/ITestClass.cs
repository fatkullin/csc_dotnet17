using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyNUnit
{
    internal interface ITestClass
    {
        IEnumerable<MethodInfo> BeforeClassMethods { get; }
        IEnumerable<MethodInfo> BeforeMethods { get; }
        IEnumerable<MethodInfo> AfterTestMethods { get; }
        IEnumerable<MethodInfo> AfterClassMethods { get; }
        IEnumerable<TestMethod> TestMethods { get; }

        string Name { get; }
    }

    internal class TestMethod
    {
        public MethodInfo Method { get; set; }
        public Type ExpectedExceptionType { get; set; }
        public string IgnoreReason { get; set; }
    }
}