using System;
using System.Reflection;

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
            var assembly = Assembly.LoadFrom(path);

            var testMethodFilter = new OnlyStaticMethodsFilter();
            var testClassLoader = new TestClassLoader(testMethodFilter);
            var testClasses = testClassLoader.LoadFromAssembly(assembly);

            foreach (var testClass in testClasses)
            {
                PrintTestClassResults(TestClassRunner.Run(testClass));
            }

            Console.WriteLine("Completed. Press any key...");
            Console.ReadKey();
        }

        private static void PrintTestClassResults(TestClassRunResult result)
        {
            Console.WriteLine("Test Class: {0};\n\tSucceeded Tests: {1};\n\tFailed Tests: {2};\n\tIgnored Tests: {3}.",
                result.TestClassName, result.SucceededTestCount, result.FailedTestCount, result.IgnoredTestCount);
        }
    }
}
