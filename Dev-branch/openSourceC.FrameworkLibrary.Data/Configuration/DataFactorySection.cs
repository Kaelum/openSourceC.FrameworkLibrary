using System;
using System.Configuration;

using openSourceC.FrameworkLibrary.Data;

namespace openSourceC.FrameworkLibrary.Configuration
{
	/// <summary>
	///		Summary description for DataFactorySection.
	/// </summary>
	public class DataFactorySection : DbFactorySectionBase
	{
		/// <summary>Gets the configuration file section name.</summary>
		public static string SectionName { get; private set; }

		private static DataFactorySection _configSection = null;
		private static object _configSectionLock = new object();


		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		static DataFactorySection() { SectionName = "openSourceC/dataFactory"; }

		#endregion

		#region Instance

		/// <summary>
		///		Gets the singleton instance of <see cref="DataFactorySection"/>.
		/// </summary>
		public static DataFactorySection Instance
		{
			get
			{
				if (_configSection == null)
				{
					lock (_configSectionLock)
					{
						if (_configSection == null)
						{
							_configSection = (DataFactorySection)ConfigurationManager.GetSection(SectionName);
						}
					}
				}

				return _configSection;
			}
		}

		#endregion
	}
}
