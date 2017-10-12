using System;
using System.Collections.Generic;

namespace Option
{
    public class Option<T>
    {
        public static Option<T> Some(T value)
        {
            return new Option<T>(value);
        }

        public static Option<T> None()
        {
            return new Option<T>();
        }

        public bool IsSome()
        {
            return _hasValue;
        }

        public bool IsNone()
        {
            return !IsSome();
        }

        public T Value()
        {
            if (IsNone())
            {
                throw new InvalidCastException("Getting value for None object");
            }
            return _value;
        }

        public Option<TRes> Map<TRes>(Func<T, TRes> f)
        {
            return IsNone() ? Option<TRes>.None() : Option<TRes>.Some(f(Value()));
        }

        public override bool Equals(object other)
        {
            if (other is Option<T> option)
            {
                return _hasValue == option._hasValue
                       && EqualityComparer<T>.Default.Equals(_value, option._value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_hasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(_value);
            }
        }

        public override string ToString()
        {
            return IsNone() ? "None" : Value().ToString();
        }

        private Option(T value)
        {
            _value = value;
            _hasValue = true;
        }

        private Option()
        {
        }

        private readonly bool _hasValue;
        private readonly T _value;
    }

    public static class Extentions
    {
        public static Option<T> Flatten<T>(this Option<Option<T>> option)
        {
            return option.IsNone() ? Option<T>.None() : option.Value();
        }
    }
}
