using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace openSourceC.FrameworkLibrary.Extensions
{
	/// <summary>
	///		Summary description for EnumExtensions.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		///		Gets value of the <see cref="T:ActionNameAttribute"/> for the specified enumerator
		///		member.
		/// </summary>
		/// <param name="enumerator">The enumerator value.</param>
		/// <returns>
		///		The value of the <see cref="T:ActionNameAttribute"/> for the specified enumerator
		///		member if it exists; otherwise, the member name.
		/// </returns>
		public static string GetActionName(this Enum enumerator)
		{
			ActionNameAttribute attribute = enumerator.GetType().GetField(enumerator.ToString()).GetCustomAttributes(typeof(ActionNameAttribute), true).SingleOrDefault() as ActionNameAttribute;

			return (attribute == null ? enumerator.ToString() : attribute.ActionName);
		}

		/// <summary>
		///		Gets value of the <see cref="T:DescriptionAttribute"/> for the specified enumerator
		///		member.
		/// </summary>
		/// <param name="enumerator">The enumerator value.</param>
		/// <returns>
		///		The value of the <see cref="T:DescriptionAttribute"/> for the specified enumerator
		///		member if it exists; otherwise, the lowercase value of the member name.
		/// </returns>
		public static string GetDescription(this Enum enumerator)
		{
			DescriptionAttribute attribute = enumerator.GetType().GetField(enumerator.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).SingleOrDefault() as DescriptionAttribute;

			return (attribute == null ? enumerator.ToString().ToLowerInvariant() : attribute.Description);
		}

		/// <summary>
		///		Gets value of the <see cref="T:EnumMemberAttribute"/> for the specified enumerator
		///		member.
		/// </summary>
		/// <param name="enumerator">The enumerator value.</param>
		/// <returns>
		///		The value of the <see cref="T:EnumMemberAttribute"/> for the specified enumerator
		///		member if it exists; otherwise, the lowercase value of the member name.
		/// </returns>
		public static string GetEnumMember(this Enum enumerator)
		{
			EnumMemberAttribute attribute = enumerator.GetType().GetField(enumerator.ToString()).GetCustomAttributes(typeof(EnumMemberAttribute), true).SingleOrDefault() as EnumMemberAttribute;

			return (attribute == null ? enumerator.ToString().ToLowerInvariant() : attribute.Value);
		}

		/// <summary>
		///		Gets value of the <see cref="T:RelatedTypeAttribute"/> for the specified enumerator
		///		member.
		/// </summary>
		/// <param name="enumerator">The enumerator value.</param>
		/// <returns>
		///		The value of the <see cref="T:RelatedTypeAttribute"/> for the specified enumerator
		///		member if it exists; otherwise, null.
		/// </returns>
		public static Type GetRelatedType(this Enum enumerator)
		{
			RelatedTypeAttribute attribute = enumerator.GetType().GetField(enumerator.ToString()).GetCustomAttributes(typeof(RelatedTypeAttribute), true).SingleOrDefault() as RelatedTypeAttribute;

			return (attribute == null ? null : attribute.Type);
		}

		/// <summary>
		///		Gets value of the <see cref="T:XmlEnumAttribute"/> for the specified enumerator
		///		member.
		/// </summary>
		/// <param name="enumerator">The enumerator value.</param>
		/// <returns>
		///		The value of the <see cref="T:XmlEnumAttribute"/> for the specified enumerator
		///		member if it exists; otherwise, the lowercase value of the member name.
		/// </returns>
		public static string GetXmlEnum(this Enum enumerator)
		{
			XmlEnumAttribute attribute = enumerator.GetType().GetField(enumerator.ToString()).GetCustomAttributes(typeof(XmlEnumAttribute), true).SingleOrDefault() as XmlEnumAttribute;

			return (attribute == null ? enumerator.ToString().ToLowerInvariant() : attribute.Name);
		}
	}
}
