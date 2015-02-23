using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;

namespace openSourceC.FrameworkLibrary.Web.Http
{
	/// <summary>
	///		Summary description for BasicHttpMembershipMessageHandler.
	/// </summary>
	public class BasicHttpMembershipMessageHandler : DelegatingHandler
	{
		private const string _basicAuthResponseHeaderValue = "Basic";


		#region Override Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			IPrincipal principal = Thread.CurrentPrincipal;
			bool basicAuthorizationHeaderFound = false;

			if (principal == null
				|| principal.Identity == null
				|| !principal.Identity.IsAuthenticated
			)
			{
				AuthenticationHeaderValue authenticationHeader = request.Headers.Authorization;

				if (authenticationHeader != null)
				{
					basicAuthorizationHeaderFound = (
						_basicAuthResponseHeaderValue.Equals(authenticationHeader.Scheme, StringComparison.InvariantCultureIgnoreCase)
						&& !string.IsNullOrWhiteSpace(authenticationHeader.Parameter)
					);

					if (basicAuthorizationHeaderFound)
					{
						string[] parameterSplit = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeader.Parameter)).Split(":".ToCharArray(), 2);

						if (parameterSplit.Length == 2)
						{
							principal = Authenticate(parameterSplit[0], parameterSplit[1]);

							if (principal != null)
							{
								Thread.CurrentPrincipal = principal;
							}
						}
					}
				}
			}

			return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(
				task =>
				{
					HttpResponseMessage response = task.Result;

					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						response.Headers.Add("WWW-Authenticate", string.Format("Basic {0}", request.RequestUri.DnsSafeHost));
					}

					return response;
				}
			);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		///		Implement to include authentication logic and create IPrincipal
		/// </summary>
		protected virtual IPrincipal Authenticate(string user, string password)
		{
			if (!Membership.ValidateUser(user, password))
			{
				return null;
			}

			string[] roles = System.Web.Security.Roles.Provider.GetRolesForUser(user);

			return new GenericPrincipal(new GenericIdentity(user, "Basic Membership"), roles);
		}

		#endregion
	}
}
