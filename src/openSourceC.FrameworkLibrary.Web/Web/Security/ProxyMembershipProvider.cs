using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

using openSourceC.FrameworkLibrary.Web;

namespace openSourceC.FrameworkLibrary.Web.Security
{
	/// <summary>
	///		Summary description for ProxyMembershipProvider.
	/// </summary>
	public abstract class ProxyMembershipProvider : MembershipProvider
	{
		//private int _schemaVersionCheck;
		private ProxyMembershipProviderSettings _proxyMembershipProviderSettings;

		private MembershipProvider _proxiedMembershipProvider;


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="T:ProxyMembershipProvider"/> class.
		///	</summary>
		protected ProxyMembershipProvider() { }

		#endregion

		#region Initialize

		/// <summary>
		///		Initializes the eCommerce membership provider with the property values specified in
		///		the ASP.NET application's configuration file. This method is not intended to be used
		///		directly from your code.
		/// </summary>
		/// <param name="name">The name of the <see cref="T:ProxyMembershipProvider" /> instance to
		///		initialize.</param>
		/// <param name="config">A <see cref="T:NameValueCollection"/> that contains the names and
		///		values of configuration options for the membership provider.</param>
		/// <exception cref="T:System.Web.HttpException">
		///		The current trust level is less than Low.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///		config is null.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///		The provider has already been initialized prior to the current call to the
		///		<see cref="M:ProxyMembershipProvider.Initialize(String,NameValueCollection)" />
		///		method.
		/// </exception>
		/// <exception cref="T:System.Configuration.Provider.ProviderException">
		///		The enablePasswordRetrieval, enablePasswordReset, requiresQuestionAndAnswer, or
		///		requiresUniqueEmail attribute is set to a value other than a Boolean.
		///		- or -
		///		The maxInvalidPasswordAttempts or the passwordAttemptWindow attribute is set to a
		///		value other than a positive integer.- or -The minRequiredPasswordLength attribute
		///		is set to a value other than a positive integer, or the value is greater than 128.
		///		- or -
		///		The minRequiredNonalphanumericCharacters attribute is set to a value other than
		///		zero or a positive integer, or the value is greater than 128.
		///		- or -
		///		The value for the passwordStrengthRegularExpression attribute is not a valid
		///		regular expression.
		///		- or -
		///		The applicationName attribute is set to a value that is greater than 256 characters.
		///		- or -
		///		The passwordFormat attribute specified in the application configuration file is an
		///		invalid <see cref="T:MembershipPasswordFormat" /> enumeration.
		///		- or -
		///		The passwordFormat attribute is set to <see cref="F:MembershipPasswordFormat.Hashed" />
		///		and the enablePasswordRetrieval attribute is set to true in the application configuration.
		///		- or -
		///		The passwordFormat attribute is set to Encrypted and the machineKey configuration element
		///		specifies AutoGenerate for the decryptionKey attribute.
		///		- or -
		///		The connectionStringName attribute is empty or does not exist in the application configuration.
		///		- or -
		///		The value of the connection string for the connectionStringName attribute value is empty, or
		///		the specified connectionStringName does not exist in the application configuration file.
		///		- or -
		///		The value for the commandTimeout attribute is set to a value other than zero or a positive
		///		integer.
		///		- or -
		///		The application configuration file for this <see cref="T:ProxyMembershipProvider" />
		///		instance contains an unrecognized attribute.
		///	</exception>
		public override void Initialize(string name, NameValueCollection config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}

			if (string.IsNullOrEmpty(name))
			{
				name = "ProxyMembershipProvider";
			}

			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("MembershipProxyProvider_description"));
			}

			// Initialize the abstract base class.
			base.Initialize(name, config);

			_proxyMembershipProviderSettings = new ProxyMembershipProviderSettings(config);
		}

		private MembershipProvider ProxiedProvider
		{
			get
			{
				if (_proxiedMembershipProvider == null)
				{
					_proxiedMembershipProvider = Membership.Providers[_proxyMembershipProviderSettings.ProxyFor];

					if (_proxiedMembershipProvider == null)
					{
						throw new ProviderException(SR.GetString("Membership_provider_not_found", _proxyMembershipProviderSettings.ProxyFor));
					}

					_proxiedMembershipProvider.ValidatingPassword += new MembershipValidatePasswordEventHandler(ProxyMembershipProvider_ValidatingPassword);
				}

				return _proxiedMembershipProvider;
			}
		}

		private void ProxyMembershipProvider_ValidatingPassword(object sender, ValidatePasswordEventArgs e)
		{
			base.OnValidatingPassword(e);
		}

		#endregion

		#region Properties

		/// <summary>
		///		The name of the application using the custom membership provider.
		///	</summary>
		/// <returns>
		///		The name of the application using the custom membership provider.
		///	</returns>
		public override string ApplicationName
		{
			get { return ProxiedProvider.ApplicationName; }
			set { ProxiedProvider.ApplicationName = value; }
		}

		/// <summary>
		///		Indicates whether the membership provider is configured to allow users to reset
		///		their passwords.
		///	</summary>
		/// <returns>
		///		<b>true</b> if the membership provider supports password reset; otherwise,
		///		<b>false</b>. The default is <b>true</b>.
		///	</returns>
		public override bool EnablePasswordReset
		{
			get { return ProxiedProvider.EnablePasswordReset; }
		}

		/// <summary>
		///		Indicates whether the membership provider is configured to allow users to retrieve
		///		their passwords.
		///	</summary>
		/// <returns>
		///		<b>true</b> if the membership provider is configured to support password retrieval;
		///		otherwise, <b>false</b>. The default is <b>false</b>.
		///	</returns>
		public override bool EnablePasswordRetrieval
		{
			get { return ProxiedProvider.EnablePasswordRetrieval; }
		}

		/// <summary>
		///		Gets the number of invalid password or password-answer attempts allowed before the
		///		membership user is locked out.
		///	</summary>
		/// <returns>
		///		The number of invalid password or password-answer attempts allowed before the
		///		membership user is locked out.
		///	</returns>
		public override int MaxInvalidPasswordAttempts
		{
			get { return ProxiedProvider.MaxInvalidPasswordAttempts; }
		}

		/// <summary>
		///		Gets the minimum number of special characters that must be present in a valid
		///		password.
		/// </summary>
		/// <returns>
		///		The minimum number of special characters that must be present in a valid password.
		/// </returns>
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return ProxiedProvider.MinRequiredNonAlphanumericCharacters; }
		}

		/// <summary>
		///		Gets the minimum length required for a password.
		/// </summary>
		/// <returns>
		///		The minimum length required for a password.
		/// </returns>
		public override int MinRequiredPasswordLength
		{
			get { return ProxiedProvider.MinRequiredPasswordLength; }
		}

		/// <summary>
		///		Gets the number of minutes in which a maximum number of invalid password or
		///		password-answer attempts are allowed before the membership user is locked out.
		///	</summary>
		/// <returns>
		///		The number of minutes in which a maximum number of invalid password or
		///		password-answer attempts are allowed before the membership user is locked out.
		///	</returns>
		public override int PasswordAttemptWindow
		{
			get { return ProxiedProvider.PasswordAttemptWindow; }
		}

		/// <summary>
		///		Gets a value indicating the format for storing passwords in the membership data
		///		store.
		/// </summary>
		/// <returns>
		///		One of the <see cref="T:MembershipPasswordFormat"/> values indicating the format
		///		for storing passwords in the data store.
		/// </returns>
		public override MembershipPasswordFormat PasswordFormat
		{
			get { return ProxiedProvider.PasswordFormat; }
		}

		/// <summary>
		///		Gets the regular expression used to evaluate a password.
		/// </summary>
		/// <returns>
		///		A regular expression used to evaluate a password.
		/// </returns>
		public override string PasswordStrengthRegularExpression
		{
			get { return ProxiedProvider.PasswordStrengthRegularExpression; }
		}

		/// <summary>
		///		Gets a value indicating whether the membership provider is configured to require
		///		the user to answer a password question for password reset and retrieval.
		///	</summary>
		/// <returns>
		///		<b>true</b> if a password answer is required for password reset and retrieval;
		///		otherwise, <b>false</b>. The default is <b>true</b>.
		///	</returns>
		public override bool RequiresQuestionAndAnswer
		{
			get { return ProxiedProvider.RequiresQuestionAndAnswer; }
		}

		/// <summary>
		///		Gets a value indicating whether the membership provider is configured to require a
		///		unique e-mail address for each user name.
		///	</summary>
		/// <returns>
		///		<b>true</b> if the membership provider requires a unique e-mail address; otherwise,
		///		<b>false</b>. The default is <b>true</b>.
		///	</returns>
		public override bool RequiresUniqueEmail
		{
			get { return ProxiedProvider.RequiresUniqueEmail; }
		}

		#endregion

		#region Methods

		/// <summary>
		///		Processes a request to update the password for a membership user.
		/// </summary>
		/// <param name="username">The user to update the password for.</param>
		/// <param name="oldPassword">The current password for the specified user.</param>
		/// <param name="newPassword">The new password for the specified user.</param>
		/// <returns>
		///		<b>true</b> if the password was updated successfully; otherwise, <b>false</b>.
		/// </returns>
		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return ProxiedProvider.ChangePassword(username, oldPassword, newPassword);
		}

		/// <summary>
		///		Processes a request to update the password question and answer for a membership user.
		/// </summary>
		/// <param name="username">The user to change the password question and answer for.</param>
		/// <param name="password">The password for the specified user.</param>
		/// <param name="newPasswordQuestion">The new password question for the specified user.</param>
		/// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
		/// <returns>
		///		<b>true</b> if the password question and answer are updated successfully;
		///		otherwise, <b>false</b>.
		/// </returns>
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			return ProxiedProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
		}

		/// <summary>
		///		Adds a new membership user to the data source.
		/// </summary>
		/// <param name="username">The user name for the new user.</param>
		/// <param name="password">The password for the new user.</param>
		/// <param name="email">The e-mail address for the new user.</param>
		/// <param name="passwordQuestion">The password question for the new user.</param>
		/// <param name="passwordAnswer">The password answer for the new user</param>
		/// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
		/// <param name="providerUserKey">The unique identifier from the membership data source for
		///		the user.</param>
		/// <param name="status">A <see cref="T:MembershipCreateStatus"/> enumeration value
		///		indicating whether the user was created successfully.</param>
		/// <returns>
		///		A <see cref="T:MembershipUser"/> object populated with the information for the
		///		newly created user.
		///	</returns>
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			return ProxiedProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
		}

		/// <summary>
		///		Removes a user from the membership data source.
		/// </summary>
		/// <param name="username">The name of the user to delete.</param>
		/// <param name="deleteAllRelatedData"><b>true</b> to delete data related to the user from
		///		the database; <b>false</b> to leave data related to the user in the database.</param>
		/// <returns>
		///		<b>true</b> if the user was successfully deleted; otherwise, <b>false</b>.
		/// </returns>
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			return ProxiedProvider.DeleteUser(username, deleteAllRelatedData);
		}

		/// <summary>
		///		Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
		/// </summary>
		/// <param name="emailToMatch">The e-mail address to search for.</param>
		/// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">The total number of matched users.</param>
		/// <returns>
		///		A <see cref="T:MembershipUserCollection"/> collection that contains a page of
		///		pageSize <see cref="T:MembershipUser"/> objects beginning at the page specified by
		///		pageIndex.
		/// </returns>
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return ProxiedProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		///		Gets a collection of membership users where the user name contains the specified
		///		user name to match.
		/// </summary>
		/// <param name="usernameToMatch">The user name to search for.</param>
		/// <param name="pageIndex">The index of the page of results to return. pageIndex is
		///		zero-based.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">The total number of matched users.</param>
		/// <returns>
		///		A <see cref="T:MembershipUserCollection"/> collection that contains a page of
		///		pageSize <see cref="T:MembershipUser"/> objects beginning at the page specified by
		///		pageIndex.
		///	</returns>
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return ProxiedProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		///		Gets a collection of all the users in the data source in pages of data.
		/// </summary>
		/// <param name="pageIndex">The index of the page of results to return. pageIndex is
		///		zero-based.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">The total number of matched users.</param>
		/// <returns>
		///		A <see cref="T:MembershipUserCollection"/> collection that contains a page of
		///		pageSize <see cref="T:MembershipUser"/> objects beginning at the page specified by
		///		pageIndex.
		///	</returns>
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return ProxiedProvider.GetAllUsers(pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		///		Gets the number of users currently accessing the application.
		/// </summary>
		/// <returns>
		///		The number of users currently accessing the application.
		/// </returns>
		public override int GetNumberOfUsersOnline()
		{
			return ProxiedProvider.GetNumberOfUsersOnline();
		}

		/// <summary>
		///		Gets the password for the specified user name from the data source.
		/// </summary>
		/// <param name="username">The user to retrieve the password for.</param>
		/// <param name="answer">The password answer for the user.</param>
		/// <returns>
		///		The password for the specified user name.
		/// </returns>
		public override string GetPassword(string username, string answer)
		{
			return ProxiedProvider.GetPassword(username, answer);
		}

		/// <summary>
		///		Gets information from the data source for a user based on the unique identifier for
		///		the membership user. Provides an option to update the last-activity date/time stamp
		///		for the user.
		/// </summary>
		/// <param name="providerUserKey">The unique identifier for the membership user to get
		///		information for.</param>
		/// <param name="userIsOnline">true to update the last-activity date/time stamp for the
		///		user; false to return user information without updating the last-activity date/time
		///		stamp for the user.</param>
		/// <returns>
		///		A <see cref="T:MembershipUser"/> object populated with the specified user's
		///		information from the data source.
		/// </returns>
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			return ProxiedProvider.GetUser(providerUserKey, userIsOnline);
		}

		/// <summary>
		///		Gets information from the data source for a user. Provides an option to update the
		///		last-activity date/time stamp for the user.
		/// </summary>
		/// <param name="username">The name of the user to get information for.</param>
		/// <param name="userIsOnline">true to update the last-activity date/time stamp for the
		///		user; false to return user information without updating the last-activity date/time
		///		stamp for the user.</param>
		/// <returns>
		///		A <see cref="T:MembershipUser"></see> object populated with the specified user's
		///		information from the data source.
		/// </returns>
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			return ProxiedProvider.GetUser(username, userIsOnline);
		}

		/// <summary>
		///		Gets the user name associated with the specified e-mail address.
		/// </summary>
		/// <param name="email">The e-mail address to search for.</param>
		/// <returns>
		///		The user name associated with the specified e-mail address. If no match is found,
		///		return null.
		///	</returns>
		public override string GetUserNameByEmail(string email)
		{
			return ProxiedProvider.GetUserNameByEmail(email);
		}

		/// <summary>
		///		Resets a user's password to a new, automatically generated password.
		/// </summary>
		/// <param name="username">The user to reset the password for.</param>
		/// <param name="answer">The password answer for the specified user.</param>
		/// <returns>
		///		The new password for the specified user.
		/// </returns>
		public override string ResetPassword(string username, string answer)
		{
			return ProxiedProvider.ResetPassword(username, answer);
		}

		/// <summary>
		///		Clears a lock so that the membership user can be validated.
		/// </summary>
		/// <param name="userName">The membership user to clear the lock status for.</param>
		/// <returns>
		///		<b>true</b> if the membership user was successfully unlocked; otherwise, <b>false</b>.
		/// </returns>
		public override bool UnlockUser(string userName)
		{
			return ProxiedProvider.UnlockUser(userName);
		}

		/// <summary>
		///		Updates information about a user in the data source.
		/// </summary>
		/// <param name="user">A <see cref="T:MembershipUser"></see> object that represents the
		///		user to update and the updated information for the user.</param>
		public override void UpdateUser(MembershipUser user)
		{
			ProxiedProvider.UpdateUser(user);
		}

		/// <summary>Verifies that the specified user name and password exist in the data source.
		/// </summary>
		/// <param name="username">The name of the user to validate.</param>
		/// <param name="password">The password for the specified user.</param>
		/// <returns>
		///		<b>true</b> if the specified username and password are valid; otherwise, <b>false</b>.
		/// </returns>
		public override bool ValidateUser(string username, string password)
		{
			return ProxiedProvider.ValidateUser(username, password);
		}

		#endregion
	}
}
