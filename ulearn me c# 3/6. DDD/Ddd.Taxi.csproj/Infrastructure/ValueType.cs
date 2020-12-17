using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public class ValueType<T>
	{
		static IEnumerable<string> propertyNames;
		static ValueType()
		{
			propertyNames = GetPropertyNames(typeof(T));
		}

		static IEnumerable<string> GetPropertyNames(Type type)
		{
			return type
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Select(x => x.Name);
		}

		public bool Equals(T obj)
		{
			return Equals((object)obj);
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var objType = obj.GetType();
			var objPropertyNamesSet = GetPropertyNames(objType).ToHashSet();

			foreach (var property in propertyNames)
			{
				if (!objPropertyNamesSet.Contains(property))
				{
					return false;
				}

				var thisPropertyVal = typeof(T).GetProperty(property)?.GetValue(this, null);
				var objPropertyVal = objType.GetProperty(property)?.GetValue(obj, null);

				if (thisPropertyVal == null && objPropertyVal != null)
				{
					return false;
				}
				
				if (thisPropertyVal != null && !thisPropertyVal.Equals(objPropertyVal))
				{
					return false;
				}
			}
			
			return true;
		}

		public override int GetHashCode()
		{
			var sum = 0;
			var index = 1;
			var random = new Random(21323);
			
			foreach (var propertyName in propertyNames)
			{
				var propertyValue = typeof(T).GetProperty(propertyName)?.GetValue(this, null);

				sum ^= unchecked((propertyValue?.GetHashCode() ?? 0) * random.Next());
				index++;
			}

			return sum;
		}

		public override string ToString()
		{
			var propsStrArray = propertyNames
				.OrderBy(x => x, StringComparer.InvariantCulture)
				.Select(
				property => $"{property}: {typeof(T).GetProperty(property)?.GetValue(this, null)}");
			
			return $"{typeof(T).Name}({string.Join("; ", propsStrArray)})";
		}
	}
}