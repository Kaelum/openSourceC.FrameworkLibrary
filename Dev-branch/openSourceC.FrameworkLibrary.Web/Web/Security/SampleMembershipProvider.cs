using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;

namespace openSourceC.FrameworkLibrary.Web.Security
{
	///// <summary>
	/////		Summary description for SampleMembershipProvider.
	///// </summary>
	//[Serializable]
	//public class SampleMembershipProvider : MembershipProvider
	//{
	//    //private int _schemaVersionCheck;
	//    private ProxyMembershipProviderSettings _proxyMembershipProviderSettings;

	//    private MembershipProvider _proxiedMembershipProvider;


	//    #region Initialize

	//    /// <summary>
	//    ///		Initializes the eCommerce membership provider with the property values specified in
	//    ///		the ASP.NET application's configuration file. This method is not intended to be used
	//    ///		directly from your code.
	//    /// </summary>
	//    /// <param name="name">The name of the <see cref="T:SampleMembershipProvider" /> instance to
	//    ///		initialize.</param>
	//    /// <param name="config">A <see cref="T:NameValueCollection"/> that contains the names and
	//    ///		values of configuration options for the membership provider.</param>
	//    /// <exception cref="T:System.Web.HttpException">
	//    ///		The current trust level is less than Low.
	//    /// </exception>
	//    /// <exception cref="T:System.ArgumentNullException">
	//    ///		config is null.
	//    /// </exception>
	//    /// <exception cref="T:System.InvalidOperationException">
	//    ///		The provider has already been initialized prior to the current call to the
	//    ///		<see cref="M:SampleMembershipProvider.Initialize(String,NameValueCollection)" />
	//    ///		method.
	//    /// </exception>
	//    /// <exception cref="T:System.Configuration.Provider.ProviderException">
	//    ///		The enablePasswordRetrieval, enablePasswordReset, requiresQuestionAndAnswer, or
	//    ///		requiresUniqueEmail attribute is set to a value other than a Boolean.
	//    ///		- or -
	//    ///		The maxInvalidPasswordAttempts or the passwordAttemptWindow attribute is set to a
	//    ///		value other than a positive integer.- or -The minRequiredPasswordLength attribute
	//    ///		is set to a value other than a positive integer, or the value is greater than 128.
	//    ///		- or -
	//    ///		The minRequiredNonalphanumericCharacters attribute is set to a value other than
	//    ///		zero or a positive integer, or the value is greater than 128.
	//    ///		- or -
	//    ///		The value for the passwordStrengthRegularExpression attribute is not a valid
	//    ///		regular expression.
	//    ///		- or -
	//    ///		The applicationName attribute is set to a value that is greater than 256 characters.
	//    ///		- or -
	//    ///		The passwordFormat attribute specified in the application configuration file is an
	//    ///		invalid <see cref="T:MembershipPasswordFormat" /> enumeration.
	//    ///		- or -
	//    ///		The passwordFormat attribute is set to <see cref="F:MembershipPasswordFormat.Hashed" />
	//    ///		and the enablePasswordRetrieval attribute is set to true in the application configuration.
	//    ///		- or -
	//    ///		The passwordFormat attribute is set to Encrypted and the machineKey configuration element
	//    ///		specifies AutoGenerate for the decryptionKey attribute.
	//    ///		- or -
	//    ///		The connectionStringName attribute is empty or does not exist in the application configuration.
	//    ///		- or -
	//    ///		The value of the connection string for the connectionStringName attribute value is empty, or
	//    ///		the specified connectionStringName does not exist in the application configuration file.
	//    ///		- or -
	//    ///		The value for the commandTimeout attribute is set to a value other than zero or a positive
	//    ///		integer.
	//    ///		- or -
	//    ///		The application configuration file for this <see cref="T:SampleMembershipProvider" />
	//    ///		instance contains an unrecognized attribute.
	//    ///	</exception>
	//    public override void Initialize(string name, NameValueCollection config)
	//    {
	//        if (config == null)
	//        {
	//            throw new ArgumentNullException("config");
	//        }

	//        if (string.IsNullOrEmpty(name))
	//        {
	//            name = "SampleMembershipProvider";
	//        }

	//        if (string.IsNullOrEmpty(config["description"]))
	//        {
	//            config.Remove("description");
	//            config.Add("description", SR.GetString(SR.MembershipProxyProvider_description));
	//        }

	//        // Initialize the abstract base class.
	//        base.Initialize(name, config);

	//        //_schemaVersionCheck = 0;

	//        _proxyMembershipProviderSettings = new ProxyMembershipProviderSettings(config);

	//        //foreach (object module in HttpContext.Current.ApplicationInstance.Modules)
	//        //{
	//        //    if (module is EcHttpModule)
	//        //    {
	//        //        _ecHttpModule = (EcHttpModule)module;
	//        //        _secondaryAuthenticationModule = _ecHttpModule.DefaultSecondaryAuthenticationModule;
	//        //        break;
	//        //    }
	//        //}
	//    }

	//    #endregion

	//    #region Properties

	//    public override string ApplicationName
	//    {
	//        get { throw new NotImplementedException(); }
	//        set { throw new NotImplementedException(); }
	//    }

	//    public override bool EnablePasswordReset
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override bool EnablePasswordRetrieval
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override int MaxInvalidPasswordAttempts
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override int MinRequiredNonAlphanumericCharacters
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override int MinRequiredPasswordLength
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override int PasswordAttemptWindow
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override MembershipPasswordFormat PasswordFormat
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override string PasswordStrengthRegularExpression
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override bool RequiresQuestionAndAnswer
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    public override bool RequiresUniqueEmail
	//    {
	//        get { throw new NotImplementedException(); }
	//    }

	//    #endregion

	//    #region Methods

	//    public override bool ChangePassword(string username, string oldPassword, string newPassword)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override bool DeleteUser(string username, bool deleteAllRelatedData)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override int GetNumberOfUsersOnline()
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override string GetPassword(string username, string answer)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override MembershipUser GetUser(string username, bool userIsOnline)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override string GetUserNameByEmail(string email)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override string ResetPassword(string username, string answer)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override bool UnlockUser(string userName)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override void UpdateUser(MembershipUser user)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public override bool ValidateUser(string username, string password)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    #endregion
	//}
}
