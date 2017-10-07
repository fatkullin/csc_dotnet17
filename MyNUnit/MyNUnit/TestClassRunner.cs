using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace MyNUnit
{
    internal static class TestClassRunner
    {
        public static TestClassRunResult Run(ITestClass testClass)
        {
            var result = new TestClassRunResult()
            {
                TestClassName = testClass.Name
            };

            try
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("Start Test Class: {0}\n", testClass.Name);
                Console.WriteLine("Running before class methods...");
                RunMethods(testClass.BeforeClassMethods);

                Console.WriteLine("\nRunning Tests...");
                RunTests(testClass, ref result);

                Console.WriteLine("\nRunning after class methods...");
                RunMethods(testClass.AfterClassMethods);
                Console.WriteLine("Test Class: {0} finished", testClass.Name);
                Console.WriteLine("=============================================");
            }
            catch (TargetInvocationException e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        private static void RunTests(ITestClass testClass, ref TestClassRunResult testsResult)
        {
            foreach (var testMethod in testClass.TestMethods)
            {
                Console.WriteLine("\n-------------------------------------------");

                if (testMethod.IgnoreReason == null)
                {
                    var result = RunTest(testClass, testMethod);
                    if (result.TestGreen)
                    {
                        testsResult.SucceededTestCount += 1;
                    }
                    else
                    {
                        testsResult.FailedTestCount += 1;
                    }
                }
                else
                {
                    Console.WriteLine("IGNORED: Test '{0}', reason: {1}", testMethod.Method.Name,
                        testMethod.IgnoreReason);
                    testsResult.IgnoredTestCount += 1;
                }

                Console.WriteLine("-------------------------------------------");
            }
        }

        private static TestResult RunTest(ITestClass testClass, TestMethod testMethod)
        {
            Console.WriteLine("Running {0}", testMethod.Method.Name);

            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("Prepare test");
            RunMethods(testClass.BeforeMethods);

            var testResult = RunTestMethod(testMethod);
            WriteTestResult(testResult);

            Console.WriteLine("Clear test");
            RunMethods(testClass.AfterTestMethods);

            sw.Stop();

            Console.WriteLine("{0} Test {1}, time: {2} ms", testResult.TestGreen ? "Succeeded" : "FAILED",
                testMethod.Method.Name, sw.ElapsedMilliseconds);

            return testResult;
        }

        private static void WriteTestResult(TestResult testResult)
        {
            if (testResult.TestGreen)
            {
                return;
            }

            if (testResult.ExpectedExceptionType != null)
            {
                Console.WriteLine("Expected exception: {0}\nReceivedException: {1}",
                    testResult.ExpectedExceptionType.Name,
                    testResult.ThrownException);
            }
            else
            {
                Console.WriteLine("Exception: {0}", testResult.ThrownException);
            }
        }

        private struct TestResult
        {
            public bool TestGreen;
            public Exception ThrownException;
            public Type ExpectedExceptionType;
        }

        private static TestResult RunTestMethod(TestMethod testMethod)
        {
            Exception thrownException = null;
            try
            {
                testMethod.Method.Invoke(null, null);
            }
            catch (TargetInvocationException  e)
            {
                thrownException = e.InnerException;
            }

            if (testMethod.ExpectedExceptionType != thrownException?.GetType())
            {
                return new TestResult()
                {
                    TestGreen = false,
                    ThrownException = thrownException,
                    ExpectedExceptionType = testMethod.ExpectedExceptionType
                };
            }

            return new TestResult() {TestGreen = true};
        }

        private static void RunMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                Console.WriteLine("Running {0}", method.Name);
                method.Invoke(null, null);
            }
        }
    }

    internal struct TestClassRunResult
    {
        public string TestClassName;
        public uint SucceededTestCount;
        public uint FailedTestCount;
        public uint IgnoredTestCount;
    }
}