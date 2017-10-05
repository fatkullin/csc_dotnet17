using System;

namespace MyNUnitAttributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class BeforeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AfterAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class BeforeClassAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AfterClassAttribute : Attribute
    {
    }

    public sealed class ExpectedAttribute : Attribute
    {
        public ExpectedAttribute(Type exceptionType)
        {
            ExceptionType = exceptionType;
        }

        public Type ExceptionType { get; private set; }
    }

    public sealed class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; private set; }
    }
}
