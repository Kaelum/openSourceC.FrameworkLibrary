using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;

using openSourceC.FrameworkLibrary.Configuration;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactoryProvider.
	/// </summary>
	/// <typeparam name="TDataFactoryProviderInterface">The <typeparamref name="TDataFactoryProviderInterface"/> type.</typeparam>
	public abstract class DbFactoryProvider<TDataFactoryProviderInterface> : DbFactoryProviderBase<TDataFactoryProviderInterface>
		where TDataFactoryProviderInterface : class
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProvider&lt;TDataFactoryProviderInterface&gt;"/> class.
		/// </summary>
		/// <param name="settingSettingsCollection">The <see cref="SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.  If null, or not set,
		///		the <see cref="P:FactorySettings"/>, <see cref="P:ApplicationName"/>, and
		///		<see cref="P:ConnectionStringName"/> properties will not be usable until the
		///		<see cref="P:FactoryName"/> property is set.</param>
		protected DbFactoryProvider(SettingSettingsCollection settingSettingsCollection, string settingName = null)
			: base(settingSettingsCollection, settingName) { }

		#endregion

		#region CreateInstance

		/// <summary>
		///		Creates an instance of <see cref="DbFactoryProvider&lt;TDataFactoryProviderInterface&gt;"/> defined by the specified
		///		<see cref="ProviderSettingsCollection"/> and provider name.
		/// </summary>
		/// <param name="providerSettings">The <see cref="ProviderSettingsCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <returns>
		///		A <see cref="DbFactoryProvider&lt;TDataFactoryProviderInterface&gt;"/> instance.
		/// </returns>
		public static new DbFactoryProvider<TDataFactoryProviderInterface> CreateInstance(ProviderSettingsCollection providerSettings, string providerName)
		{
			CreateInstanceReturn ro = DbFactoryProviderBase<TDataFactoryProviderInterface>.CreateInstance(providerSettings, providerName);

			DbFactoryProvider<TDataFactoryProviderInterface> instance = ro.Instance as DbFactoryProvider<TDataFactoryProviderInterface>;

			if (instance == null)
			{
				throw new OscErrorException(string.Format("{0} is not a {1}.", ro.Settings.Name, typeof(DbFactoryProvider<TDataFactoryProviderInterface>).ToString()));
			}

			instance.Initialize(ro.Settings.Name, ro.Settings.Parameters);

			return instance;
		}

		#endregion
	}

	/// <summary>
	///		Summary description for DbFactoryProvider&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TUserRequestContext">The <typeparamref name="TUserRequestContext"/> type.</typeparam>
	/// <typeparam name="TDataFactoryProviderInterface">The <typeparamref name="TDataFactoryProviderInterface"/> type.</typeparam>
	public abstract class DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface> : DbFactoryProviderBase<TDataFactoryProviderInterface>
		where TUserRequestContext : struct
		where TDataFactoryProviderInterface : class
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProvider&lt;TUserRequestContext&gt;"/> class. 
		/// </summary>
		/// <param name="settingSettingsCollection">The <see cref="SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.  If null, or not set,
		///		the <see cref="P:FactorySettings"/>, <see cref="P:ApplicationName"/>, and
		///		<see cref="P:ConnectionStringName"/> properties will not be usable until the
		///		<see cref="P:FactoryName"/> property is set.</param>
		protected DbFactoryProvider(SettingSettingsCollection settingSettingsCollection, string settingName = null)
			: base(settingSettingsCollection, settingName) { }

		#endregion

		#region CreateInstance

		/// <summary>
		///		Creates an instance of <see cref="DbFactoryProvider&lt;TDataFactoryProviderInterface&gt;"/> defined by the specified
		///		<see cref="ProviderSettingsCollection"/> and provider name.
		/// </summary>
		/// <param name="providerSettings">The <see cref="ProviderSettingsCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="userRequestContext">The <typeparamref name="TUserRequestContext"/> object.</param>
		/// <returns>
		///		A <see cref="DbFactoryProvider&lt;TDataFactoryProviderInterface, TUserRequestContext&gt;"/> instance.
		/// </returns>
		public static DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface> CreateInstance(ProviderSettingsCollection providerSettings, string providerName, TUserRequestContext userRequestContext)
		{
			CreateInstanceReturn ro = DbFactoryProviderBase<TDataFactoryProviderInterface>.CreateInstance(providerSettings, providerName);

			DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface> instance = ro.Instance as DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface>;

			if (instance == null)
			{
				throw new OscErrorException(string.Format("{0} is not a {1}.", ro.Settings.Name, typeof(DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface>).ToString()));
			}

			instance.UserRequestContext = userRequestContext;
			instance.Initialize(ro.Settings.Name, ro.Settings.Parameters);

			return instance;
		}

		#endregion

		#region Protected Properties

		/// <summary>Gets the current <see cref="T:TUserRequestContext"/> object.</summary>
		protected TUserRequestContext UserRequestContext { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for DbFactoryProviderBase.
	/// </summary>
	/// <typeparam name="TDataFactoryProviderInterface">The <typeparamref name="TDataFactoryProviderInterface"/> type.</typeparam>
	public abstract class DbFactoryProviderBase<TDataFactoryProviderInterface> : ProviderBase
		where TDataFactoryProviderInterface : class
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
		///		Initializes a new instance of the <see cref="DbFactoryProviderBase&lt;TDataFactoryProviderInterface&gt;"/> class.
		/// </summary>
		/// <param name="settingSettingsCollection">The <see cref="ProviderSettingsCollection"/> object.</param>
		/// <param name="settingName">The name of data factory setting to use.  If null, or not set,
		///		the <see cref="P:FactorySettings"/>, <see cref="P:ApplicationName"/>, and
		///		<see cref="P:ConnectionStringName"/> properties will not be usable until the
		///		<see cref="FactoryName"/> property is set.</param>
		protected DbFactoryProviderBase(SettingSettingsCollection settingSettingsCollection, string settingName = null)
		{
			if (settingSettingsCollection == null)
			{
				throw new ArgumentNullException("configurationSection");
			}

			SettingSettingsCollection = settingSettingsCollection;

			if (settingName != null)
			{
				FactoryName = settingName;

				if (SettingSettings == null)
				{
					throw new OscErrorException(string.Format("{0} not found.", FactoryName));
				}

				Debug.WriteLine(string.Format("DataFactorySettings: Name: {0}, ConnectionStringName: {1}", SettingSettings.Name, SettingSettings.ConnectionStringName));

				if (SettingSettings.ElementInformation != null && SettingSettings.ElementInformation.Properties != null)
				{
					foreach (PropertyInformation pi in SettingSettings.ElementInformation.Properties)
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
		///		provider within the <see cref="ProviderSettingsCollection"/> object.
		/// </summary>
		/// <param name="providerSettings">The <see cref="ProviderSettingsCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <returns>
		///		A <see cref="CreateInstanceReturn"/> object.
		/// </returns>
		protected static CreateInstanceReturn CreateInstance(ProviderSettingsCollection providerSettings, string providerName)
		{
			if (providerSettings == null)
			{
				throw new ArgumentNullException("providerSettings");
			}

			if (providerName == null)
			{
				throw new ArgumentNullException("providerName");
			}

			CreateInstanceReturn ro = new CreateInstanceReturn();
			ro.Settings = providerSettings[providerName];

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
			object instance = Activator.CreateInstance(type);

			if (!(instance is TDataFactoryProviderInterface))
			{
				throw new OscErrorException(string.Format("{0} does not derive from {1}.", ro.Settings.Name, typeof(TDataFactoryProviderInterface).ToString()));
			}

			if (!(instance is ProviderBase))
			{
				throw new OscErrorException(string.Format("{0} does not derive from {1}.", ro.Settings.Name, typeof(ProviderBase).ToString()));
			}

			ro.Instance = (ProviderBase)instance;

			return ro;
		}

		#endregion

		#region Public Properties

		/// <summary>Gets the <see cref="SettingSettingsCollection"/> object.</summary>
		public SettingSettingsCollection SettingSettingsCollection { get; private set; }

		#endregion

		#region Protected Properties

		/// <summary>Gets the application name.</summary>
		protected string ApplicationName { get { return SettingSettings.ApplicationName; } }

		/// <summary>Gets the connection string name.</summary>
		protected string ConnectionStringName { get { return SettingSettings.ConnectionStringName; } }

		/// <summary>Gets or sets the factory name.</summary>
		protected string FactoryName { get; set; }

		/// <summary>Gets the <see cref="SettingSettings"/> object.</summary>
		protected SettingSettings SettingSettings { get { return SettingSettingsCollection[FactoryName]; } }

		#endregion
	}
}
