using System;
using System.IO;
using System.Reflection;
using MyNUnitAttributes;

namespace MyNUnit
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("empty path: args");
                return;
            }

            var path = args[0];
            var dlls = Directory.EnumerateFiles(path, "*.dll");

            foreach (var dllPath in dlls)
            {
                RunTestsFromAssembly(dllPath);
            }
            
            Console.WriteLine("Completed. Press any key...");
            Console.ReadKey();
        }

        private static void RunTestsFromAssembly(string path)
        {
            if (path.EndsWith("MyNUnitAttributes.dll"))
            {
                // explicitly loaded non-signed assembly of MyNUnitAttributes 
                // leads to problems with 'switch' in TestClassLoader 
                // (comparison of attribute types will break)
                // sorry for this kludge
                return;
            }

            var assembly = Assembly.LoadFrom(path);

            var testMethodFilter = new OnlyStaticMethodsFilter();
            var testClassLoader = new TestClassLoader(testMethodFilter);
            var testClasses = testClassLoader.LoadFromAssembly(assembly);

            foreach (var testClass in testClasses)
            {
                PrintTestClassResults(TestClassRunner.Run(testClass));
            }
        }

        private static void PrintTestClassResults(TestClassRunResult result)
        {
            Console.WriteLine("Test Class: {0};\n\tSucceeded Tests: {1};\n\tFailed Tests: {2};\n\tIgnored Tests: {3}.",
                result.TestClassName, result.SucceededTestCount, result.FailedTestCount, result.IgnoredTestCount);
        }
    }
}
