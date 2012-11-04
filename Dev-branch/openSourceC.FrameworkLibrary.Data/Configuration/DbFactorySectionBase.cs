using System;
using System.Configuration;

namespace openSourceC.FrameworkLibrary.Configuration
{
	/// <summary>
	///		Summary description for DbFactorySectionBase.
	/// </summary>
	public class DbFactorySectionBase : ConfigurationSection
	{
		#region Elements

		/// <summary></summary>
		[ConfigurationProperty("providers", IsRequired = true)]
		[ConfigurationCollection(typeof(ProviderSettingsCollection))]
		public ProviderSettingsCollection Providers
		{
			get { return (ProviderSettingsCollection)base["providers"]; }
		}

		/// <summary></summary>
		[ConfigurationProperty("settings", IsRequired = false)]
		[ConfigurationCollection(typeof(SettingSettingsCollection))]
		public SettingSettingsCollection Settings
		{
			get { return (SettingSettingsCollection)base["settings"]; }
		}

		#endregion
	}
}
