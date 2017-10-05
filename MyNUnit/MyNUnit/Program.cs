using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using MyNUnitAttributes;


namespace MyNUnit
{
    class TestMethod
    {
        public MethodInfo Method { get; set; }
        public Type ExpectedExceptionType { get; set;}
        public string IgnoreReason { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("empty path: args");
            }

            var path = args[0];
            var assembly = Assembly.LoadFrom(path);


            foreach (var type in assembly.DefinedTypes)
            {
                if (!type.IsClass)
                    continue;

                var testMethods = new List<TestMethod>();
                var beforeMethods = new List<MethodInfo>();
                var afterMethods = new List<MethodInfo>();
                var beforeClassMethods = new List<MethodInfo>();
                var afterClassMethods = new List<MethodInfo>();

                foreach (var declaredMember in type.GetTypeInfo().DeclaredMembers)
                {
                    if (!(declaredMember is MethodInfo))
                        continue;

                    var method = declaredMember as MethodInfo;
                    Type expectedExceptionType = null;
                    string ignoreReason = null;

                    bool testMethod = false;
                    bool beforeMethod = false;
                    bool afterMethod = false;
                    bool beforeClassMethod = false;
                    bool afterClassMethod = false;

                    foreach (var attribute in method.GetCustomAttributes(true))
                    {
                        if (attribute is TestAttribute)
                        {
                            if (SimpleStaticMethod(method, type))
                            {
                                testMethod = true;
                            }
                        }

                        if (attribute is ExpectedAttribute)
                        {
                            expectedExceptionType = (attribute as ExpectedAttribute).ExceptionType;
                        }

                        if (attribute is IgnoreAttribute)
                        {
                            ignoreReason = (attribute as IgnoreAttribute).Reason;
                        }

                        if (attribute is BeforeAttribute)
                        {
                            if (SimpleStaticMethod(method, type))
                            {
                                beforeMethod = true;
                            }
                        }

                        if (attribute is AfterAttribute)
                        {
                            if (SimpleStaticMethod(method, type))
                            {
                                afterMethod = true;
                            }
                        }

                        if (attribute is BeforeClassAttribute)
                        {
                            if (SimpleStaticMethod(method, type))
                            {
                                beforeClassMethod = true;
                            }
                        }


                        if (attribute is AfterClassAttribute)
                        {
                            if (SimpleStaticMethod(method, type))
                            {
                                afterClassMethod = true;
                            }
                        }

                    }

                    if (testMethod)
                    {
                        var tm = new TestMethod
                        {
                            Method = method,
                            ExpectedExceptionType = expectedExceptionType,
                            IgnoreReason = ignoreReason
                        };
                        testMethods.Add(tm);
                    }

                    if (beforeMethod)
                    {
                        beforeMethods.Add(method);
                    }

                    if (afterMethod)
                    {
                        afterMethods.Add(method);
                    }

                    if (beforeClassMethod)
                    {
                        beforeClassMethods.Add(method);
                    }

                    if (afterClassMethod)
                    {
                        afterClassMethods.Add(method);
                    }
                }

                Console.WriteLine("Running before class methods");
                foreach (var method in beforeClassMethods)
                {
                    Console.WriteLine("\tRunning {0}", method.Name);
                    method.Invoke(null, null);
                }

                Console.WriteLine("Running test methods");
                foreach (var testMethod in testMethods)
                {
                    Console.WriteLine("\t-------------------------------------------");

                    if (testMethod.IgnoreReason != null)
                    {
                        Console.WriteLine("\tIGNORED: Test '{0}', reason: {1}", testMethod.Method.Name, testMethod.IgnoreReason);
                        continue;
                    }

                    Console.WriteLine("\tRunning {0}", testMethod.Method.Name);

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    if (beforeMethods.Count != 0)
                    {
                        Console.WriteLine("\tPrepare environment");

                        foreach (var beforeMethod in beforeMethods)
                        {
                            beforeMethod.Invoke(null, null);
                        }
                    }

                    bool exceptionThrown = false;
                    bool testGreen = true;
                    try
                    {
                        testMethod.Method.Invoke(null, null);
                    }
                    catch (Exception e)
                    {
                        sw.Stop();
                        if (e.GetType() != testMethod.ExpectedExceptionType)
                        {
                            Console.WriteLine("\t\tUnexpected exception: {0} {1}", e.GetType().Name, e);
                            testGreen = false;
                        }
                        exceptionThrown = true;
                    }
                    sw.Stop();

                    if (!exceptionThrown && testMethod.ExpectedExceptionType != null)
                    {
                        Console.WriteLine("\t\tExpected exception missed: {0}", testMethod.ExpectedExceptionType.Name);
                        testGreen = false;
                    }

                    Console.WriteLine("\t{0} Test {1}, time: {2} ms", testGreen ? "Succeeded" : "FAILED", 
                        testMethod.Method.Name, sw.ElapsedMilliseconds);

                    if (afterClassMethods.Count != 0)
                    {
                        Console.WriteLine("\tClear environment");

                        foreach (var afterMethod in afterMethods)
                        {
                            afterMethod.Invoke(null, null);
                        }
                    }

                }

                Console.WriteLine("\n\nRunning after class methods");
                foreach (var method in afterClassMethods)
                {
                    Console.WriteLine("\tRunning {0}", method.Name);
                    method.Invoke(null, null);
                }
            }


        }

        private static bool SimpleStaticMethod(MethodInfo method, Type type)
        {
            if (!method.IsStatic)
            {
                Console.WriteLine("Only static methods can be Testable: {0} {1}", type.Name, method.Name);
                return false;
            }

            if (method.GetParameters().Length > 0)
            {
                Console.WriteLine("Only methods without parameters can be Testable: {0} {1}", type.Name, method.Name);
                return false;
            }
            return true;
        }
    }

}
