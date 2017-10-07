using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyNUnitAttributes;

namespace MyNUnit
{
    internal class TestClassLoader
    {
        public TestClassLoader(ITestMethodValidator testMethodFilter)
        {
            _testMethodFilter = testMethodFilter;
        }

        public IEnumerable<ITestClass> LoadFromAssembly(Assembly assembly)
        {
            return assembly.DefinedTypes.Where(type => type.IsClass)
                .Select(CreateTestClass)
                .Where(testClass => !testClass.Empty());
        }

        [Flags]
        private enum UsedAttributes
        {
            NoAttibutes = 0x0,
            TestMethod = 0x1,
            ExpectedException = 0x2,
            Ignore = 0x4,
            BeforeMethod = 0x8,
            AfterMethod = 0x10,
            BeforeClass = 0x20,
            AfterClass = 0x40
        }

        private struct ParsedMethod
        {
            public UsedAttributes Attributes;
            public Type ExpectedExceptionType;
            public string IgnoreReason;
            public MethodInfo Method;
        }

        private TestClass CreateTestClass(Type classType)
        {
            if (!classType.IsClass)
            {
                throw new ArgumentException(nameof(classType));
            }

            var testClass = new TestClass(classType.Name);

            var validParsedMethods = classType.GetTypeInfo().DeclaredMembers
                .Where(member => member is MethodInfo)
                .Select(method => ParseAttributes(method as MethodInfo))
                .Where(parsed => parsed.Attributes != UsedAttributes.NoAttibutes)
                .Where(parsed => ValidParsedMethod(classType, parsed));

            foreach (var parsedMethod in validParsedMethods)
            {
                AddParsedMethodToTestClass(testClass, parsedMethod);
            }

            return testClass;
        }

        private bool ValidParsedMethod(Type classType, ParsedMethod parsedMethod)
        {
            if (_testMethodFilter.Valid(parsedMethod.Method, out var reason))
            {
                return true;
            }

            Console.WriteLine("Invalid method declaration. Method {0}, Class {1}. Reason: {2}",
                parsedMethod.Method.Name, classType.Name, reason);

            return false;
        }

        private static void AddParsedMethodToTestClass(TestClass testClass, ParsedMethod parsedMethod)
        {
            var attr = parsedMethod.Attributes;

            if (attr.HasFlag(UsedAttributes.TestMethod))
            {
                testClass.TestMethodList.Add(CreateTestMethod(parsedMethod));
            }

            if (attr.HasFlag(UsedAttributes.BeforeMethod))
            {
                testClass.BeforeMethodList.Add(parsedMethod.Method);
            }

            if (attr.HasFlag(UsedAttributes.AfterMethod))
            {
                testClass.AfterMethodList.Add(parsedMethod.Method);
            }

            if (attr.HasFlag(UsedAttributes.BeforeClass))
            {
                testClass.BeforeClassMethodList.Add(parsedMethod.Method);
            }

            if (attr.HasFlag(UsedAttributes.AfterClass))
            {
                testClass.AfterClassMethodList.Add(parsedMethod.Method);
            }
        }

        private static TestMethod CreateTestMethod(ParsedMethod parsedMethod)
        {
            return new TestMethod
            {
                Method = parsedMethod.Method,
                ExpectedExceptionType = parsedMethod.ExpectedExceptionType,
                IgnoreReason = parsedMethod.IgnoreReason
            };
        }

        private static ParsedMethod ParseAttributes(MethodInfo methodInfo)
        {
            var result = new ParsedMethod {Method = methodInfo};

            foreach (var attribute in methodInfo.GetCustomAttributes(true))
            {
                switch (attribute)
                {
                    case TestAttribute _:
                        result.Attributes |= UsedAttributes.TestMethod;
                        break;
                    case ExpectedAttribute expectedAttribute:
                        result.Attributes |= UsedAttributes.ExpectedException;
                        result.ExpectedExceptionType = expectedAttribute.ExceptionType;
                        break;
                    case IgnoreAttribute ignoreAttribute:
                        result.Attributes |= UsedAttributes.Ignore;
                        result.IgnoreReason = ignoreAttribute.Reason;
                        break;
                    case BeforeAttribute _:
                        result.Attributes |= UsedAttributes.BeforeMethod;
                        break;
                    case AfterAttribute _:
                        result.Attributes |= UsedAttributes.AfterMethod;
                        break;
                    case BeforeClassAttribute _:
                        result.Attributes |= UsedAttributes.BeforeClass;
                        break;
                    case AfterClassAttribute _:
                        result.Attributes |= UsedAttributes.AfterClass;
                        break;
                }
            }

            return result;
        }

        private readonly ITestMethodValidator _testMethodFilter;
    }
}