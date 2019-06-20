﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Threading;

using openSourceC.FrameworkLibrary.Configuration;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for AbstractProviderBase&lt;TSettingsElement&gt;.
	/// </summary>
	/// <typeparam name="TSettingsElement">The settings element type.</typeparam>
	[Serializable]
	public abstract class AbstractProviderBase<TSettingsElement> : RemotingProviderBase
		where TSettingsElement : ProviderElement, new()
	{
		[NonSerialized]
		private string _appDomainName;
		[NonSerialized]
		private OscLog _log;
		[NonSerialized]
		private string[] _parentNames;
		[NonSerialized]
		private TSettingsElement _settings;
		[NonSerialized]
		private string _nameSuffix;


		#region Constructors

		/// <summary>
		///		Creates an instance of <see cref="AbstractProviderBase&lt;TSettingsElement&gt;"/>.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="parentNames">The names of the parent configuration elements.</param>
		/// <param name="settings">The <typeparamref name="TSettingsElement"/> object.</param>
		/// <param name="nameSuffix">The name suffix used, or <b>null</b> if not used.</param>
		protected AbstractProviderBase(OscLog log, string[] parentNames, TSettingsElement settings, string nameSuffix)
			: base(settings.Parameters["description"])
		{
			if (log == null)
			{
				throw new ArgumentNullException("log");
			}

			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			_log = log;
			_parentNames = parentNames;
			_settings = settings;
			_nameSuffix = nameSuffix;
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///		Dispose(bool disposing) executes in two distinct scenarios.  If disposing equals
		///		<b>true</b>, <see cref="M:Dispose()"/> has been called directly or indirectly
		///		by a user's code.  Managed and unmanaged resources can be disposed.  If disposing
		///		equals <b>false</b>, <see cref="M:Dispose()"/> has been called by the runtime from
		///		inside the finalizer and you should not reference other objects.  Only unmanaged
		///		resources can be disposed.
		/// </summary>
		/// <param name="disposing"><b>true</b> when <see cref="M:Dispose()"/> has been called
		///		directly or indirectly by a user's code.  <b>false</b> when <see cref="M:Dispose()"/>
		///		has been called by the runtime from inside the finalizer and you should not
		///		reference other objects.</param>
		protected override void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!Disposed)
			{
				// If disposing equals true, dispose of managed resources.
				if (disposing)
				{
					// Dispose of managed resources.
				}

				// Dispose of unmanaged resources.
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Initialize

		/// <summary>
		///		Initializes the provider.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			Debug.WriteLine(string.Format("Provider: {0}", SettingsElement.Type));

			if (SettingsElement.ElementInformation != null && SettingsElement.ElementInformation.Properties != null)
			{
				foreach (PropertyInformation pi in SettingsElement.ElementInformation.Properties)
				{
					try { Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value)); }
					catch { }
				}
			}
		}

		#endregion

		#region CreateInstance (static)

		/// <summary>
		///		Creates a provider instance that implements <typeparamref name="TInterface"/>.
		/// </summary>
		/// <typeparam name="TInterface">The interface type.</typeparam>
		/// <param name="appDomain">The <see cref="T:AppDomain"/> instantiate the provider is, or
		///		<b>null</b> to use the current <see cref="T:AppDomain"/>.</param>
		/// <param name="settings">The <typeparamref name="TSettingsElement"/> object.</param>
		/// <param name="args">The arguments to pass to the constructor. This array of arguments
		///		must match in number, order, and type the parameters of the constructor to invoke.
		///		If the default constructor is preferred, <paramref name="args"/> must be an empty
		///		array or null.</param>
		/// <returns>
		///		An instance that implements <typeparamref name="TInterface"/>.
		/// </returns>
		public static TInterface CreateInstance<TInterface>(
			AppDomain appDomain,
			TSettingsElement settings,
			params object[] args
		)
			where TInterface : class
		{
			try
			{
				if (settings == null)
				{
					throw new ArgumentNullException("settings");
				}

				Debug.WriteLine(string.Format("Provider: {0}", settings.Type));

				if (settings.ElementInformation != null && settings.ElementInformation.Properties != null)
				{
					foreach (PropertyInformation pi in settings.ElementInformation.Properties)
					{
						try { Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value)); }
						catch { }
					}
				}

				if (appDomain == null)
				{
					appDomain = AppDomain.CurrentDomain;
				}

				Match match = RegexHelper.ParseType.Match(settings.Type);

				if (!match.Success)
				{
					throw new InvalidOperationException(string.Format("Invalid type name: {0}", settings.Type));
				}

				string typeFullName = match.Groups["type"].Value;
				string assemblyFullName = match.Groups["assembly"].Value;

				ObjectHandle objHandle = appDomain.CreateInstance(
					assemblyFullName,
					typeFullName,
					false,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.OptionalParamBinding,
					null,
					args,
					null,
					null
				);

				object instance = objHandle.Unwrap();

				//if (RemotingServices.IsTransparentProxy(instance))
				//{
				//	Debug.WriteLine("The unwrapped object is a proxy.");
				//}
				//else
				//{
				//	Debug.WriteLine("The unwrapped object is not a proxy!");
				//}

				TInterface provider = instance as TInterface;

				if (provider == null)
				{
					throw new OscErrorException(string.Format("{0} does not derive from {1}.", settings.Type, typeof(TInterface)));
				}

				if (provider is AbstractProviderBase<TSettingsElement>)
				{
					AbstractProviderBase<TSettingsElement> thisInstance = provider as AbstractProviderBase<TSettingsElement>;
					thisInstance.AppDomainName = appDomain.FriendlyName;
					thisInstance.Initialize();
				}

				return provider;
			}
			catch (Exception ex)
			{
				string exceptionMessage = string.Format("Unable to create an instance of {0} from the '{1}' provider.", typeof(TInterface), settings.Type);

				throw new OscErrorException(exceptionMessage, ex);
			}
		}

		#endregion

		#region Protected Properties

		/// <summary>Gets the <see cref="P:AppDomain.Name"/> used.</summary>
		protected string AppDomainName
		{
			get { return _appDomainName; }
			private set { _appDomainName = value; }
		}

		/// <summary>Gets the <see cref="T:OscLog"/> object.</summary>
		protected OscLog Log { get { return _log; } }

		/// <summary>Gets the name suffix.</summary>
		protected string NameSuffix { get { return _nameSuffix; } }

		/// <summary>Gets the names of the parent configuration elements.</summary>
		protected string[] ParentNames { get { return _parentNames; } }

		/// <summary>Gets the <typeparamref name="TSettingsElement"/> object.</summary>
		protected TSettingsElement SettingsElement { get { return _settings; } }

		#endregion

		#region Protected Methods

		/// <summary>
		///		Extends the <see cref="P:ParentNames"/> property with the specified names.
		/// </summary>
		/// <param name="args">The names to appended to <see cref="P:ParentNames"/>.</param>
		/// <returns>
		///		The extended list of parent element names.
		/// </returns>
		protected string[] ExtendParentNames(params string[] args)
		{
			int parentNamesCount = (_parentNames == null ? 0 : _parentNames.Length);
			string[] newParentNames = new string[parentNamesCount + args.Length];

			if (parentNamesCount != 0) { _parentNames.CopyTo(newParentNames, 0); }
			if (args.Length != 0) { args.CopyTo(newParentNames, (_parentNames == null ? 0 : _parentNames.Length)); }

			return newParentNames;
		}

		#endregion
	}
}
