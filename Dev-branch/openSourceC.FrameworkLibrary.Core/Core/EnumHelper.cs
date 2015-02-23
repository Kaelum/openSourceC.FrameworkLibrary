using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for EnumHelper.
	/// </summary>
	public static class EnumHelper
	{
		/// <summary>
		///		Converts the serialized value of one or more enumerated constants to an equivalent
		///		enumerated object.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to which to convert <i>serializedValue</i>.</typeparam>
		/// <param name="serializedValue">A string containing the name to convert.</param>
		/// <returns>
		/// </returns>
		public static TEnum ParseEnumMember<TEnum>(string serializedValue)
			where TEnum : struct
		{
			return ParseEnumMember<TEnum>(serializedValue, false);
		}

		/// <summary>
		///		Converts the serialized value of one or more enumerated constants to an equivalent
		///		enumerated object.  A parameter specifies whether the operation is case-sensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to which to convert <i>serializedValue</i>.</typeparam>
		/// <param name="serializedValue">A string containing the name to convert.</param>
		/// <param name="ignoreCase"><b>true</b> to ignore case; <b>false</b> to regard case.</param>
		/// <returns>
		/// </returns>
		public static TEnum ParseEnumMember<TEnum>(string serializedValue, bool ignoreCase)
			where TEnum : struct
		{
			TEnum result;

			if (TryParseEnumMember<TEnum>(serializedValue, ignoreCase, out result))
			{
				return result;
			}

			throw new ArgumentException("Value is not one of the defined constants for the enumeration.", "serializedValue");
		}

		/// <summary>
		///		Converts the serialized value of one or more enumerated constants to an equivalent
		///		enumerated object.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to which to convert <i>serializedValue</i>.</typeparam>
		/// <param name="serializedValue">A string containing the name to convert.</param>
		/// <returns>
		/// </returns>
		public static TEnum? ParseNullableEnumMember<TEnum>(string serializedValue)
			where TEnum : struct
		{
			return ParseNullableEnumMember<TEnum>(serializedValue, false);
		}

		/// <summary>
		///		Converts the serialized value of one or more enumerated constants to an equivalent
		///		enumerated object.  A parameter specifies whether the operation is case-sensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to which to convert <i>serializedValue</i>.</typeparam>
		/// <param name="serializedValue">A string containing the name to convert.</param>
		/// <param name="ignoreCase"><b>true</b> to ignore case; <b>false</b> to regard case.</param>
		/// <returns>
		/// </returns>
		public static TEnum? ParseNullableEnumMember<TEnum>(string serializedValue, bool ignoreCase)
			where TEnum : struct
		{
			TEnum result;

			if (TryParseEnumMember<TEnum>(serializedValue, ignoreCase, out result))
			{
				return result;
			}

			return null;
		}

		/// <summary>
		///		Converts the serialized value of one or more enumerated constants to an equivalent
		///		enumerated object. The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to which to convert <i>serializedValue</i>.</typeparam>
		/// <param name="serializedValue">A string containing the name to convert.</param>
		/// <param name="result">When this method returns, contains an object of type <i>TEnum</i>
		///		whose value is represented by <i>serializedValue</i>. This parameter is passed
		///		uninitialized.</param>
		/// <returns>
		///		<b>true</b> if the value parameter was converted successfully; otherwise, <b>false</b>.
		///	</returns>
		public static bool TryParseEnumMember<TEnum>(string serializedValue, out TEnum result)
			where TEnum : struct
		{
			return TryParseEnumMember<TEnum>(serializedValue, false, out result);
		}

		/// <summary>
		///		Converts the serialized value of one or more enumerated constants to an equivalent
		///		enumerated object.  A parameter specifies whether the operation is case-sensitive.
		///		The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to which to convert <i>serializedValue</i>.</typeparam>
		/// <param name="serializedValue">A string containing the name to convert.</param>
		/// <param name="ignoreCase"><b>true</b> to ignore case; <b>false</b> to regard case.</param>
		/// <param name="result">When this method returns, contains an object of type <i>TEnum</i>
		///		whose value is represented by <i>serializedValue</i>. This parameter is passed
		///		uninitialized.</param>
		/// <returns>
		///		<b>true</b> if the value parameter was converted successfully; otherwise, <b>false</b>.
		///	</returns>
		public static bool TryParseEnumMember<TEnum>(string serializedValue, bool ignoreCase, out TEnum result)
			where TEnum : struct
		{
			StringComparison comparisonType = (ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
			FieldInfo[] fieldArray = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);

			foreach (FieldInfo field in fieldArray)
			{
				if (
					field.Name.Equals(serializedValue, comparisonType)
					|| ((EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), false)).Any(t => t.Value.Equals(serializedValue, comparisonType))
					|| ((XmlEnumAttribute[])field.GetCustomAttributes(typeof(XmlEnumAttribute), false)).Any(t => t.Name.Equals(serializedValue, comparisonType))
				)
				{
					result = (TEnum)field.GetRawConstantValue();
					return true;
				}
			}

			result = default(TEnum);
			return typeof(TEnum).IsSubclassOf(typeof(Nullable<>));
		}

		/// <summary>
		///		Converts type to an equivalent enumerated object using the
		///		<see cref="T:RelatedTypeAttribute"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to convert <i>type</i> to.</typeparam>
		/// <param name="type">The type to convert.</param>
		/// <returns>
		/// </returns>
		public static TEnum ParseRelatedType<TEnum>(Type type)
			where TEnum : struct
		{
			TEnum result;

			if (TryParseRelatedType<TEnum>(type, out result))
			{
				return result;
			}

			throw new ArgumentException("Type is not one of the defined constants for the enumeration.", "type");
		}

		/// <summary>
		///		Converts type to an equivalent enumerated object using the
		///		<see cref="T:RelatedTypeAttribute"/>.  A parameter specifies whether the operation
		///		is case-sensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to convert <i>type</i> to.</typeparam>
		/// <param name="type">The type to convert.</param>
		/// <returns>
		/// </returns>
		public static TEnum? ParseNullableRelatedType<TEnum>(Type type)
			where TEnum : struct
		{
			TEnum result;

			if (TryParseRelatedType<TEnum>(type, out result))
			{
				return result;
			}

			return null;
		}

		/// <summary>
		///		Converts type to an equivalent enumerated object using the
		///		<see cref="T:RelatedTypeAttribute"/>.  A parameter specifies whether the operation
		///		is case-sensitive.  The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type to convert <i>type</i> to.</typeparam>
		/// <param name="type">The type to convert.</param>
		/// <param name="result">When this method returns, contains an object of type <i>TEnum</i>
		///		whose value is represented by <i>serializedValue</i>. This parameter is passed
		///		uninitialized.</param>
		/// <returns>
		///		<b>true</b> if the value parameter was converted successfully; otherwise, <b>false</b>.
		///	</returns>
		public static bool TryParseRelatedType<TEnum>(Type type, out TEnum result)
			where TEnum : struct
		{
			FieldInfo[] fieldArray = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);

			foreach (FieldInfo field in fieldArray)
			{
				if (((RelatedTypeAttribute[])field.GetCustomAttributes(typeof(RelatedTypeAttribute), false)).Any(t => t.Type.Equals(type)))
				{
					result = (TEnum)field.GetRawConstantValue();
					return true;
				}
			}

			result = default(TEnum);
			return typeof(TEnum).IsSubclassOf(typeof(Nullable<>));
		}
	}
}
