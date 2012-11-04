using System;
using System.Configuration;
using System.Diagnostics;

using openSourceC.FrameworkLibrary.Configuration;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactoryBase.
	/// </summary>
	public abstract class DbFactoryBase
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryBase"/> class.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySectionBase"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.</param>
		protected DbFactoryBase(DbFactorySectionBase configurationSection, string settingName)
		{
			if (configurationSection == null)
			{
				throw new ArgumentNullException("configurationSection");
			}

			if (settingName == null)
			{
				throw new ArgumentNullException("settingName");
			}

			if (configurationSection.Settings == null)
			{
				throw new OscErrorException("settings element not found.");
			}

			SettingSettings settingSettings = configurationSection.Settings[settingName];

			if (settingSettings == null)
			{
				throw new OscErrorException(string.Format("{0} not found.", settingName));
			}

			Debug.WriteLine(string.Format("DataFactorySettings: Name: {0}, ConnectionStringName: {1}", settingSettings.Name, settingSettings.ConnectionStringName));

			if (settingSettings.ElementInformation != null && settingSettings.ElementInformation.Properties != null)
			{
				foreach (PropertyInformation pi in settingSettings.ElementInformation.Properties)
				{
					Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value));
				}
			}

			ConnectionStringName = settingSettings.ConnectionStringName;
			ApplicationName = settingSettings.ApplicationName;
		}

		#endregion

		#region Public Properties

		/// <summary>
		///		Gets the application name.
		/// </summary>
		protected string ApplicationName { get; private set; }

		/// <summary>
		///		Gets the connection string name.
		/// </summary>
		protected string ConnectionStringName { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for DataFactoryBase&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext">The <typeparamref name="TRequestContext"/> type.</typeparam>
	public abstract class DbFactoryBase<TRequestContext> : DbFactoryBase
		where TRequestContext : struct
	{
		/// <summary>Gets the current <see cref="T:TRequestContext"/> object.</summary>
		public TRequestContext RequestContext { get; private set; }


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryBase&lt;TRequestContext&gt;"/> class.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySectionBase"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.</param>
		/// <param name="requestContext">The <typeparamref name="TRequestContext"/> object.</param>
		protected DbFactoryBase(DbFactorySectionBase configurationSection, string settingName, TRequestContext requestContext)
			: base(configurationSection, settingName) { RequestContext = requestContext; }

		#endregion
	}
}
