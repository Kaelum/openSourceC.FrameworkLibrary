using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Web;

namespace openSourceC.FrameworkLibrary.Web.Util
{
	/// <summary>
	///		Summary description for ProviderUtil.
	/// </summary>
	public static class ProviderUtil
	{
		/// <summary></summary>
		public const int Infinite = 0x7fffffff;


		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="providerName"></param>
		public static void CheckUnrecognizedAttributes(NameValueCollection config, string providerName)
		{
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ConfigurationErrorsException(SR.GetString("Unexpected_provider_attribute", new object[] { key, providerName }));
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetAndRemoveBooleanAttribute(NameValueCollection config, string attrib, string providerName, ref bool val)
		{
			GetBooleanAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetAndRemoveNonZeroPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			GetNonZeroPositiveOrInfiniteAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetAndRemovePositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			GetPositiveAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetAndRemovePositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			GetPositiveOrInfiniteAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetAndRemoveRequiredNonEmptyStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			GetRequiredNonEmptyStringAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetAndRemoveStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			val = config.Get(attrib);
			config.Remove(attrib);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetBooleanAttribute(NameValueCollection config, string attrib, string providerName, ref bool val)
		{
			string str = config.Get(attrib);
			if (str != null)
			{
				if (str == "true")
				{
					val = true;
				}
				else
				{
					if (str != "false")
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_attribute", new object[] { attrib, providerName, str }));
					}
					val = false;
				}
			}
		}

		private static void GetNonEmptyStringAttributeInternal(NameValueCollection config, string attrib, string providerName, ref string val, bool required)
		{
			string str = config.Get(attrib);
			if (((str == null) && required) || (str.Length == 0))
			{
				throw new ConfigurationErrorsException(SR.GetString("Provider_missing_attribute", new object[] { attrib, providerName }));
			}
			val = str;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetNonZeroPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string str = config.Get(attrib);
			if (str != null)
			{
				int num;
				if (str == "Infinite")
				{
					num = 0x7fffffff;
				}
				else
				{
					try
					{
						num = Convert.ToInt32(str, CultureInfo.InvariantCulture);
					}
					catch (Exception exception)
					{
						if (((exception is ArgumentException) || (exception is FormatException)) || (exception is OverflowException))
						{
							throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_non_zero_positive_attributes", new object[] { attrib, providerName }));
						}
						throw;
					}
					if (num <= 0)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_non_zero_positive_attributes", new object[] { attrib, providerName }));
					}
				}
				val = num;
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetPositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string str = config.Get(attrib);
			if (str != null)
			{
				int num;
				try
				{
					num = Convert.ToInt32(str, CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					if (((exception is ArgumentException) || (exception is FormatException)) || (exception is OverflowException))
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
					}
					throw;
				}
				if (num < 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
				}
				val = num;
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string str = config.Get(attrib);
			if (str != null)
			{
				int num;
				if (str == "Infinite")
				{
					num = 0x7fffffff;
				}
				else
				{
					try
					{
						num = Convert.ToInt32(str, CultureInfo.InvariantCulture);
					}
					catch (Exception exception)
					{
						if (((exception is ArgumentException) || (exception is FormatException)) || (exception is OverflowException))
						{
							throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
						}
						throw;
					}
					if (num < 0)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
					}
				}
				val = num;
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="config"></param>
		/// <param name="attrib"></param>
		/// <param name="providerName"></param>
		/// <param name="val"></param>
		public static void GetRequiredNonEmptyStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			GetNonEmptyStringAttributeInternal(config, attrib, providerName, ref val, true);
		}
	}
}
