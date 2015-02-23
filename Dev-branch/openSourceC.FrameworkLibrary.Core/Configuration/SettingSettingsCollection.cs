using System;
using System.Configuration;

namespace openSourceC.FrameworkLibrary.Configuration
{
	/// <summary>
	///		Contains a collection of <see cref="T:SettingSettings"/> objects.
	///	</summary>
	[ConfigurationCollection(typeof(SettingSettings))]
	public sealed class SettingSettingsCollection : ConfigurationElementCollection
	{
		#region Constructors

		/// <summary>
		///		Creates a new instance of a <see cref="T:SettingSettingsCollection"/> class.
		///	</summary>
		public SettingSettingsCollection() : base(StringComparer.OrdinalIgnoreCase) { }

		#endregion

		#region Index Accessors

		/// <summary>
		///		Gets or sets a value at the specified index in the
		///		<see cref="SettingSettingsCollection"/> collection.
		///	</summary>
		/// <param name="index">The index of the <see cref="SettingSettings"/> to return.</param>
		/// <value>The specified <see cref="SettingSettings"/>.</value>
		/// <remarks>
		///		Use the <see cref="P:this"/> property to get or set a specified
		///		<see cref="SettingSettings"/> object contained within this
		///		<see cref="SettingSettingsCollection"/> class
		/// </remarks>
		public SettingSettings this[int index]
		{
			get { return (SettingSettings)base.BaseGet(index); }

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
		///		Gets an item from the collection.
		///	</summary>
		/// <param name="key">A string reference to the <see cref="SettingSettings"/> object within
		///		the collection.</param>
		/// <value>A <see cref="SettingSettings"/> object contained in the collection.</value>
		public new SettingSettings this[string key]
		{
			get { return (SettingSettings)base.BaseGet(key); }
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
			return new SettingSettings();
		}

		/// <summary>
		///		Gets the element key for a specified configuration element when overridden in a
		///		derived class.
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement"/> to return the key for.</param>
		/// <returns>
		///		An <see cref="object"/> that acts as the key for the specified
		///		<see cref="ConfigurationElement"/>.
		/// </returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SettingSettings)element).Name;
		}

		#endregion
	}
}
