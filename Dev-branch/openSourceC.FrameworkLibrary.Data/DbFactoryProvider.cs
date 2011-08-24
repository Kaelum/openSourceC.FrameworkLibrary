using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;

using openSourceC.FrameworkLibrary.Common;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactoryProvider.
	/// </summary>
	public abstract class DbFactoryProvider : DbFactoryProviderBase
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProvider"/> class.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySection"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.  If null, or not set,
		///		the <see cref="P:FactorySettings"/>, <see cref="P:ApplicationName"/>, and
		///		<see cref="P:ConnectionStringName"/> properties will not be usable until the
		///		<see cref="P:FactoryName"/> property is set.</param>
		protected DbFactoryProvider(DbFactorySection configurationSection, string settingName = null)
			: base(configurationSection, settingName) { }

		#endregion

		#region CreateInstance

		/// <summary>
		///		Creates an instance of <see cref="DbFactoryProvider"/> defined by the specified
		///		<see cref="DbFactorySection"/> and provider name.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactoryProvider"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <returns>
		///		A <see cref="DbFactoryProvider"/> instance.
		/// </returns>
		public static new DbFactoryProvider CreateInstance(DbFactorySection configurationSection, string providerName)
		{
			CreateInstanceReturn ro = DbFactoryProviderBase.CreateInstance(configurationSection, providerName);

			DbFactoryProvider instance = ro.Instance as DbFactoryProvider;

			if (instance == null)
			{
				throw new OscErrorException(string.Format("{0} is not a {1}.", ro.Settings.Name, typeof(DbFactoryProvider).ToString()));
			}

			instance.Initialize(ro.Settings.Name, ro.Settings.Parameters);

			return instance;
		}

		#endregion
	}

	/// <summary>
	///		Summary description for DbFactoryProvider&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext">The <typeparamref name="TRequestContext"/> type.</typeparam>
	public abstract class DbFactoryProvider<TRequestContext> : DbFactoryProviderBase
		where TRequestContext : struct
	{
		/// <summary>Gets the current <see cref="T:TRequestContext"/> object.</summary>
		public TRequestContext RequestContext { get; private set; }


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProvider&lt;TRequestContext&gt;"/> class. 
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySection"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.  If null, or not set,
		///		the <see cref="P:FactorySettings"/>, <see cref="P:ApplicationName"/>, and
		///		<see cref="P:ConnectionStringName"/> properties will not be usable until the
		///		<see cref="P:FactoryName"/> property is set.</param>
		protected DbFactoryProvider(DbFactorySection configurationSection, string settingName = null)
			: base(configurationSection, settingName) { }

		#endregion

		#region CreateInstance

		/// <summary>
		///		Creates an instance of <see cref="DbFactoryProvider"/> defined by the specified
		///		<see cref="DbFactorySection"/> and provider name.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactoryProvider"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="requestContext">The <typeparamref name="TRequestContext"/> object.</param>
		/// <returns>
		///		A <see cref="DbFactoryProvider&lt;TRequestContext&gt;"/> instance.
		/// </returns>
		public static DbFactoryProvider<TRequestContext> CreateInstance(DbFactorySection configurationSection, string providerName, TRequestContext requestContext)
		{
			CreateInstanceReturn ro = DbFactoryProviderBase.CreateInstance(configurationSection, providerName);

			DbFactoryProvider<TRequestContext> instance = ro.Instance as DbFactoryProvider<TRequestContext>;

			if (instance == null)
			{
				throw new OscErrorException(string.Format("{0} is not a {1}.", ro.Settings.Name, typeof(DbFactoryProvider<TRequestContext>).ToString()));
			}

			instance.RequestContext = requestContext;
			instance.Initialize(ro.Settings.Name, ro.Settings.Parameters);

			return instance;
		}

		#endregion
	}

	/// <summary>
	///		Summary description for DbFactoryProviderBase.
	/// </summary>
	public abstract class DbFactoryProviderBase : ProviderBase
	{
		/// <summary></summary>
		protected struct CreateInstanceReturn
		{
			/// <summary></summary>
			public ProviderSettings Settings;
			/// <summary></summary>
			public ProviderBase Instance;
		}


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProviderBase"/> class.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySection"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.  If null, or not set,
		///		the <see cref="P:FactorySettings"/>, <see cref="P:ApplicationName"/>, and
		///		<see cref="P:ConnectionStringName"/> properties will not be usable until the
		///		<see cref="FactoryName"/> property is set.</param>
		protected DbFactoryProviderBase(DbFactorySection configurationSection, string settingName = null)
		{
			if (configurationSection == null)
			{
				throw new ArgumentNullException("configurationSection");
			}

			if (configurationSection.Settings == null)
			{
				throw new OscErrorException("settings element not found.");
			}

			Configuration = configurationSection;

			if (settingName != null)
			{
				FactoryName = settingName;

				if (FactorySettings == null)
				{
					throw new OscErrorException(string.Format("{0} not found.", FactoryName));
				}

				Debug.WriteLine(string.Format("DataFactorySettings: Name: {0}, ConnectionStringName: {1}", FactorySettings.Name, FactorySettings.ConnectionStringName));

				if (FactorySettings.ElementInformation != null && FactorySettings.ElementInformation.Properties != null)
				{
					foreach (PropertyInformation pi in FactorySettings.ElementInformation.Properties)
					{
						Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value));
					}
				}
			}
		}

		#endregion

		#region Initialize

		/// <summary>
		///		Initializes the factory with the property values specified in application's
		///		configuration file. This method is not intended to be used directly from your code.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="config"></param>
		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
		}

		#endregion

		#region CreateInstance

		/// <summary>
		///		Creates a <see cref="ProviderBase"/> based instance defined by the specified
		///		provider within the <see cref="DbFactorySection"/> object.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySection"/>  object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <returns>
		///		A <see cref="CreateInstanceReturn"/> object.
		/// </returns>
		protected static CreateInstanceReturn CreateInstance(DbFactorySection configurationSection, string providerName)
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
				throw new OscErrorException("providers element not found.");
			}

			CreateInstanceReturn ro = new CreateInstanceReturn();
			ro.Settings = configurationSection.Providers[providerName];

			if (ro.Settings == null)
			{
				throw new OscErrorException(string.Format("{0} not found.", providerName));
			}

			Debug.WriteLine(string.Format("Provider: Name: {0}, Type: {1}", ro.Settings.Name, ro.Settings.Type));

			if (ro.Settings.ElementInformation != null && ro.Settings.ElementInformation.Properties != null)
			{
				foreach (PropertyInformation pi in ro.Settings.ElementInformation.Properties)
				{
					try { Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value)); }
					catch { }
				}
			}

			Type type = Type.GetType(ro.Settings.Type, true);

			ro.Instance = Activator.CreateInstance(type) as ProviderBase;

			if (ro.Instance == null)
			{
				throw new OscErrorException(string.Format("{0} does not derive from {1}.", ro.Settings.Name, typeof(ProviderBase).ToString()));
			}

			return ro;
		}

		#endregion

		#region Public Properties

		/// <summary>Gets the <see cref="DbFactorySection"/> object.</summary>
		public DbFactorySection Configuration { get; private set; }

		#endregion

		#region Protected Properties

		/// <summary>Gets the application name.</summary>
		protected string ApplicationName { get { return FactorySettings.ApplicationName; } }

		/// <summary>Gets the connection string name.</summary>
		protected string ConnectionStringName { get { return FactorySettings.ConnectionStringName; } }

		/// <summary>Gets or sets the factory name.</summary>
		protected string FactoryName { get; set; }

		/// <summary>Gets the <see cref="DbFactorySettings"/> object.</summary>
		protected DbFactorySettings FactorySettings { get { return Configuration.Settings[FactoryName]; } }

		#endregion
	}
}
