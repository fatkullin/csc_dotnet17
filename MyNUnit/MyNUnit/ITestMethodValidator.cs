using System.Reflection;

namespace MyNUnit
{
    internal interface ITestMethodValidator
    {
        bool Valid(MethodInfo method, out string reason);
    }
}