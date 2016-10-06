﻿using System;
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
		/// <param name="providerName">The name of data factory provider to use.</param>
		protected DbFactoryBase(DbFactorySectionBase configurationSection, string providerName)
		{
			if (configurationSection == null)
			{
				throw new ArgumentNullException("configurationSection");
			}

			if (providerName == null)
			{
				throw new ArgumentNullException("providerName");
			}

			if (configurationSection.Providers == null)
			{
				throw new OscErrorException("provider element not found.");
			}

			DbProviderElement provider = configurationSection.Providers[providerName];

			if (provider == null)
			{
				throw new OscErrorException(string.Format("Provider {0} not found.", providerName));
			}

			Debug.WriteLine(string.Format("DbFactory Provider: Name: {0}, ConnectionStringName: {1}", provider.Name, provider.ConnectionStringName));

			if (provider.ElementInformation != null && provider.ElementInformation.Properties != null)
			{
				foreach (PropertyInformation pi in provider.ElementInformation.Properties)
				{
					Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value));
				}
			}

			ConnectionStringName = provider.ConnectionStringName;
			ApplicationName = provider.ApplicationName;
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
