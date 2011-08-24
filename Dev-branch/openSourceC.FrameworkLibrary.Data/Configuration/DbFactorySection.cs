using System;
using System.Configuration;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactorySection.
	/// </summary>
	public class DbFactorySection : ConfigurationSection
	{
		#region Public Properties

		/// <summary></summary>
		[ConfigurationProperty("providers", IsRequired = true)]
		[ConfigurationCollection(typeof(ProviderSettingsCollection))]
		public ProviderSettingsCollection Providers
		{
			get { return (ProviderSettingsCollection)base["providers"]; }
		}

		/// <summary></summary>
		[ConfigurationProperty("settings", IsRequired = false)]
		[ConfigurationCollection(typeof(DbFactorySettingsCollection))]
		public DbFactorySettingsCollection Settings
		{
			get { return (DbFactorySettingsCollection)base["settings"]; }
		}

		#endregion
	}

	/// <summary>
	///		Contains a collection of <see cref="T:DbFactorySettings"/> objects.
	///	</summary>
	[ConfigurationCollection(typeof(DbFactorySettings))]
	public class DbFactorySettingsCollection : ConfigurationElementCollection
	{
		#region Contructors

		/// <summary>
		///		Creates a new instance of a <see cref="T:DataFactorySettingsCollection"/> class.
		///	</summary>
		public DbFactorySettingsCollection() : base(StringComparer.OrdinalIgnoreCase) { }

		#endregion

		#region Index Accessors

		/// <summary>
		///		Gets or sets the <see cref="DbFactorySettings"/> object at the specified index
		///		in the collection.
		///	</summary>
		/// <param name="index">The index of a <see cref="DbFactorySettings"/> object in the
		///		collection.</param>
		/// <returns>
		///		The <see cref="DbFactorySettings"/> object at the specified index.
		///	</returns>
		public DbFactorySettings this[int index]
		{
			get { return (DbFactorySettings)base.BaseGet(index); }

			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}

				this.BaseAdd(index, value);
			}
		}

		/// <summary>
		///		Gets or sets the <see cref="DbFactorySettings"/> object with the specified name
		///		in the collection.
		///	</summary>
		/// <param name="name">The name of a <see cref="DbFactorySettings"/> object in the
		///		collection.</param>
		/// <returns>
		///		The <see cref="DbFactorySettings"/> object with the specified name.
		///	</returns>
		public new DbFactorySettings this[string name]
		{
			get { return (DbFactorySettings)base.BaseGet(name); }
		}

		#endregion

		#region Override Methods

		/// <summary>
		///		Creates a new <see cref="ConfigurationElement"/>.
		/// </summary>
		/// <returns>
		///		A new <see cref="ConfigurationElement"/>.
		/// </returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new DbFactorySettings();
		}

		/// <summary>
		///		Gets the element key for a specified configuration element when overridden in a
		///		derived class.
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement"/> to return the key for.</param>
		/// <returns>
		///		An <see cref="object"/> that acts as the key for the specified <see cref="ConfigurationElement"/>.
		/// </returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DbFactorySettings)element).Name;
		}

		#endregion
	}

	/// <summary>
	///		Represents a single, named connection string in the connection strings configuration file section.
	///	</summary>
	public class DbFactorySettings : ConfigurationElement
	{
		#region Public Properties

		/// <summary>Gets the application name.</summary>
		[ConfigurationProperty("applicationName", IsRequired = false)]
		public string ApplicationName
		{
			get { return (string)base["applicationName"]; }
		}

		/// <summary>Gets the connection string name.</summary>
		[ConfigurationProperty("connectionStringName", IsRequired = false)]
		public string ConnectionStringName
		{
			get { return (string)base["connectionStringName"]; }
		}

		/// <summary>Gets the <see cref="DbFactorySettings"/> name.</summary>
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)base["name"]; }
		}

		/// <summary>Gets the collection of properties.</summary>
		public new ConfigurationPropertyCollection Properties
		{
			get { return base.Properties; }
		}

		#endregion
	}
}
