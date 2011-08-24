using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace openSourceC.FrameworkLibrary.Web.Security
{
	/// <summary>
	///		Summary description for ProxyMembershipProviderSettings.
	/// </summary>
	[Serializable]
	public class ProxyMembershipProviderSettings
	{
		private string _proxyFor;


		#region Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		/// <param name="parameters">The collection of user-defined parameters for the provider.</param>
		public ProxyMembershipProviderSettings(NameValueCollection parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}

			parameters = new NameValueCollection(parameters);

			_proxyFor = parameters["proxyFor"];

			if (string.IsNullOrEmpty(_proxyFor))
			{
				throw new ProviderException(SR.GetString("Proxy_for_provider_not_specified"));
			}

			parameters.Remove("proxyFor");

			if (parameters.Count > 0)
			{
				string attribUnrecognized = parameters.GetKey(0);

				if (!string.IsNullOrEmpty(attribUnrecognized))
				{
					throw new ProviderException(string.Format(SR.Provider_unrecognized_attribute, attribUnrecognized));
				}
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		///		Gets the friendly name of the provider that is being proxied.
		/// </summary>
		/// <value>
		///		The friendly name of the proxied provider.
		///	</value>
		public string ProxyFor
		{
			get { return _proxyFor; }
		}

		#endregion
	}
}
