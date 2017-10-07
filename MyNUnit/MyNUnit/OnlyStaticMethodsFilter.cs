using System.Reflection;

namespace MyNUnit
{
    internal class OnlyStaticMethodsFilter : ITestMethodValidator
    {
        public bool Valid(MethodInfo method, out string reason)
        {
            if (!method.IsStatic)
            {
                reason = "Only static methods can be Testable.";
                return false;
            }

            if (method.GetParameters().Length > 0)
            {
                reason = "Only methods without parameters can be Testable";
                return false;
            }

            reason = "";
            return true;
        }
    }
}