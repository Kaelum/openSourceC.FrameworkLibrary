using System;
using System.Configuration;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactoryProviderAccessor&lt;TDataFactoryProviderInterface&gt;.
	/// </summary>
	/// <typeparam name="TDataFactoryProviderInterface"></typeparam>
	public abstract class DbFactoryProviderAccessor<TDataFactoryProviderInterface> : IDisposable
		where TDataFactoryProviderInterface : class
	{
		private ProviderSettingsCollection _providerSettings;
		private readonly string _providerName;

		private DbFactoryProvider<TDataFactoryProviderInterface> _factoryProvider;
		private object _factoryProviderLock = new object();

		private bool _disposed = false;


		#region Contructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProviderAccessor&lt;TDataFactoryProviderInterface&gt;"/> class.
		/// </summary>
		/// <param name="providerSettings">The <see cref="ProviderSettingsCollection"/> object.</param>
		/// <param name="providerName">The name of the provider to create a data factory instance of.</param>
		public DbFactoryProviderAccessor(ProviderSettingsCollection providerSettings, string providerName)
		{
			_providerSettings = providerSettings;
			_providerName = providerName;
		}

		#endregion

		#region Destructor

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~DbFactoryProviderAccessor()
		{
			Dispose(false);
		}

		#endregion

		#region IDisposable Implmentation

		/// <summary>
		///		Dispose of 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose of managed resources.
				if (disposing)
				{
					DisposeManagedResources();
				}

				DisposeUnmanagedResources();

				_disposed = true;
			}
		}

		#endregion

		#region Protected Properties

		/// <summary>
		///		Gets the <typeparamref name="TDataFactoryProviderInterface"/> data factory proxy interface of the current
		///		<see cref="DbFactoryProvider&lt;TDataFactoryProviderInterface&gt;"/> instance.
		/// </summary>
		protected TDataFactoryProviderInterface DataFactory
		{
			get
			{
				if (_factoryProvider == null)
				{
					lock (_factoryProviderLock)
					{
						if (_factoryProvider == null)
						{
							_factoryProvider = DbFactoryProvider<TDataFactoryProviderInterface>.CreateInstance(_providerSettings, _providerName);
						}
					}
				}

				return _factoryProvider as TDataFactoryProviderInterface;
			}
		}

		#endregion

		#region Protected Abstract Methods

		/// <summary>
		///		Disposes of managed resources.
		/// </summary>
		protected abstract void DisposeManagedResources();

		/// <summary>
		///		Disposes of unmanaged resources.
		/// </summary>
		protected abstract void DisposeUnmanagedResources();

		#endregion
	}

	/// <summary>
	///		Summary description for DbFactoryProviderAccessor&lt;TRequestContext, TDataFactoryProviderInterface&gt;.
	/// </summary>
	/// <typeparam name="TUserRequestContext"></typeparam>
	/// <typeparam name="TDataFactoryProviderInterface"></typeparam>
	public abstract class DbFactoryProviderAccessor<TUserRequestContext, TDataFactoryProviderInterface> : IDisposable
		where TUserRequestContext : struct
		where TDataFactoryProviderInterface : class
	{
		private ProviderSettingsCollection _providerSettings;
		private readonly string _providerName;

		private DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface> _factoryProvider;
		private object _factoryProviderLock = new object();

		private bool _disposed = false;


		#region Contructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProviderAccessor&lt;TUserRequestContext, TDataFactoryProviderInterface&gt;"/> class.
		/// </summary>
		/// <param name="providerSettings">The <see cref="ProviderSettingsCollection"/> object.</param>
		/// <param name="providerName">The name of the provider to create a data factory instance of.</param>
		/// <param name="userRequestContext">The current <typeparamref name="TUserRequestContext"/> object.</param>
		public DbFactoryProviderAccessor(ProviderSettingsCollection providerSettings, string providerName, TUserRequestContext userRequestContext)
		{
			_providerSettings = providerSettings;
			_providerName = providerName;
			UserRequestContext = userRequestContext;
		}

		#endregion

		#region Destructor

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~DbFactoryProviderAccessor()
		{
			Dispose(false);
		}

		#endregion

		#region IDisposable Implmentation

		/// <summary>
		///		Dispose of 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose of managed resources.
				if (disposing)
				{
					DisposeManagedResources();
				}

				DisposeUnmanagedResources();

				_disposed = true;
			}
		}

		#endregion

		#region Protected Properties

		/// <summary>
		///		Gets the <typeparamref name="TDataFactoryProviderInterface"/> data factory interface of the current
		///		<see cref="DbFactoryProvider&lt;TUserRequestContext&gt;"/> instance.
		/// </summary>
		protected TDataFactoryProviderInterface DataFactory
		{
			get
			{
				if (_factoryProvider == null)
				{
					lock (_factoryProviderLock)
					{
						if (_factoryProvider == null)
						{
							_factoryProvider = DbFactoryProvider<TUserRequestContext, TDataFactoryProviderInterface>.CreateInstance(_providerSettings, _providerName, UserRequestContext);
						}
					}
				}

				return _factoryProvider as TDataFactoryProviderInterface;
			}
		}

		/// <summary>Gets the current <see cref="T:TUserRequestContext"/> object.</summary>
		protected TUserRequestContext UserRequestContext { get; private set; }

		#endregion

		#region Protected Abstract Methods

		/// <summary>
		///		Disposes of managed resources.
		/// </summary>
		protected abstract void DisposeManagedResources();

		/// <summary>
		///		Disposes of unmanaged resources.
		/// </summary>
		protected abstract void DisposeUnmanagedResources();

		#endregion
	}
}
