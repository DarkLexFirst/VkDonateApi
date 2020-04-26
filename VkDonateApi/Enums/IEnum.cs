using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VkDonateApi
{
	public abstract class IEnum<T> : IEqualityComparer<IEnum<T>>, IEquatable<IEnum<T>> where T : IEnum<T>, new()
	{
		private string _value;

		protected static T CreateObject(string value) 
		{
			return new T { _value = value };
		}

		public override string ToString()
		{
			return _value;
		}

		public bool Equals(IEnum<T> x, IEnum<T> y)
		{
			return x == y;
		}

		public int GetHashCode(IEnum<T> obj)
		{
			return obj._value.GetHashCode();
		}

		public bool Equals(IEnum<T> other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return this == obj as IEnum<T>;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static bool operator ==(IEnum<T> left, IEnum<T> right)
		{
			if (ReferenceEquals(objA: right, objB: left))
			{
				return true;
			}

			if (left is null || right is null)
				return false;

			return left._value == right._value;
		}

		public static bool operator !=(IEnum<T> left, IEnum<T> right)
		{
			return !(left == right);
		}
	}
}
