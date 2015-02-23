using System;
using System.Configuration;

namespace openSourceC.FrameworkLibrary.Configuration
{
	/// <summary>
	///		Summary description for DataFactorySection.
	/// </summary>
	public class DataFactorySection : DbFactorySectionBase
	{
		private static DataFactorySection _configSection = null;
		private static bool? _isRunningUnderIIS = null;
		private static object _configSectionLock = new object();


		#region GetInstance

		/// <summary>
		///		Gets the singleton instance of <see cref="DataFactorySection"/>.
		/// </summary>
		/// <param name="isRunningUnderIIS"><b>true</b> if the application is running under IIS
		///		application (Web.config); otherwise, <b>false</b> (App.config).</param>
		///	<returns>
		///		A <see cref="T:DataFactorySection"/> object.
		///	</returns>
		public static DataFactorySection GetInstance(bool isRunningUnderIIS)
		{
			if (_configSection == null)
			{
				lock (_configSectionLock)
				{
					if (_configSection == null)
					{
						_configSection = ConfigurationSectionManager.GetConfigurationSection<DataFactorySection>(isRunningUnderIIS);
						_isRunningUnderIIS = isRunningUnderIIS;

						if (_configSection == null)
						{
							throw new ConfigurationErrorsException(string.Format("Configuration section for type=\"{0}\" not found.", typeof(DataFactorySection).FullName));
						}
					}
				}
			}
			else if (_isRunningUnderIIS != isRunningUnderIIS)
			{
				throw new ConfigurationErrorsException("The isRunningUnderIIS parameter does not match the value used to create the instance.");
			}

			return _configSection;
		}

		#endregion
	}
}
