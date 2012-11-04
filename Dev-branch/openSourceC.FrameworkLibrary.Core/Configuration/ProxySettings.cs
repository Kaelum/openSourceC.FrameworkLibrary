using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace openSourceC.FrameworkLibrary.Configuration
{
	/// <summary>
	///		Represents a single, named connection string in the connection strings configuration file section.
	///	</summary>
	public sealed class ProxySettings : ConfigurationElement
	{
		private NameValueCollection _parameters;


		#region Attributes

		/// <summary>
		///		Gets the name of the proxy configured by this class.
		///	</summary>
		/// <value>The name of the proxy.</value>
		/// <remarks>
		///		This value of the <b>Name</b> property is the same as the value for the name
		///		attribute that appears in the configuration section for the proxy that is
		///		configured by the <see cref="ProxySettings"/> class.
		///	</remarks>
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)base["name"]; }
		}

		/// <summary>
		///		Gets a collection of user-defined parameters.
		///	</summary>
		/// <value>A <see cref="NameValueCollection"/> of parameters for the proxy.</value>
		/// <remarks>
		///		Use the <b>Parameters</b> property to access the <see cref="NameValueCollection"/>
		///		parameters for this <see cref="ProxySettings"/> object.
		///	</remarks>
		public NameValueCollection Parameters
		{
			get
			{
				if (_parameters == null)
				{
					lock (this)
					{
						if (_parameters == null)
						{
							_parameters = new NameValueCollection(StringComparer.Ordinal);

							foreach (ConfigurationProperty property in Properties)
							{
								_parameters.Add(property.Name, (string)base[property]);
							}
						}
					}
				}

				return _parameters;
			}
		}

		/// <summary>
		///		Gets the type of the proxy configured by this class.
		///	</summary>
		/// <value>The fully qualified namespace and class name for the type of proxy configured by this <see cref="ProxySettings"/> instance.</value>
		/// <remarks>
		///		The <b>Type</b> property contains the fully qualified namespace and class name for
		///		the proxy configured by the <see cref="ProxySettings"/> class.
		///	</remarks>
		[ConfigurationProperty("type", Options = ConfigurationPropertyOptions.IsTypeStringTransformationRequired, IsRequired = true)]
		public string Type
		{
			get { return (string)base["type"]; }
		}

		#endregion

		#region Override Methods

		/// <summary>
		///		Gets a value indicating whether an unknown attribute is encountered during deserialization.
		/// </summary>
		/// <param name="name">The name of the unrecognized attribute.</param>
		/// <param name="value">The value of the unrecognized attribute.</param>
		/// <returns>
		///		<b>true</b> when an unknown attribute is encountered while deserializing;
		///		otherwise, <b>false</b>.
		/// </returns>
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			ConfigurationProperty property = new ConfigurationProperty(name, typeof(string));
			base[property] = value;

			return true;
		}

		/// <summary>
		///		Gets a value indicating whether an unknown element is encountered during
		///		deserialization.
		/// </summary>
		/// <param name="elementName">The name of the unknown subelement.</param>
		/// <param name="reader">The <see cref="XmlReader"/> being used for deserialization</param>
		/// <returns>
		///		<b>true</b> when an unknown element is encountered while deserializing; otherwise,
		///		<b>false</b>.
		/// </returns>
		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			return false;
		}

		#endregion
	}
}
