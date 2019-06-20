using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace openSourceC.FrameworkLibrary.Web.Security
{
	///// <summary>
	/////		Summary description for SampleMembershipProviderSettings.
	///// </summary>
	//[Serializable]
	//public class SampleMembershipProviderSettings
	//{
	//    /// <summary>This is the default maximum number of hours that a regular password may be
	//    ///		used before it expires.</summary>
	//    public const int DefaultMaximumPasswordAge = 90 * 24;

	//    /// <summary>This is the default maximum number of hours that a temporary password may be
	//    ///		used before it expires.</summary>
	//    public const int DefaultMaximumTemporaryPasswordAge = 48;

	//    /// <summary>This is the default size for generated passwords.</summary>
	//    public const int DefaultGeneratedPasswordSize = 14;

	//    private string _applicationName;
	//    private string _connectionStringName;
	//    private int _commandTimeout;
	//    private bool _enablePasswordReset;
	//    private bool _enablePasswordRetrieval;
	//    private int _maxInvalidPasswordAttempts;
	//    private int _maxPasswordAge;
	//    private int _maxTempPasswordAge;
	//    private int _minRequiredNonAlphanumericCharacters;
	//    private int _minRequiredPasswordLength;
	//    private int _passwordAttemptWindow;
	//    private MembershipPasswordFormat _passwordFormat;
	//    private string _passwordStrengthRegularExpression;
	//    private bool _requiresQuestionAndAnswer;
	//    private bool _requiresUniqueEmail;


	//    #region Constructors

	//    /// <summary>
	//    ///		Class constructor.
	//    /// </summary>
	//    /// <param name="parameters">The collection of user-defined parameters for the provider.</param>
	//    public SampleMembershipProviderSettings(NameValueCollection parameters)
	//    {
	//        string tempString;


	//        if (parameters == null)
	//        {
	//            throw new ArgumentNullException("parameters");
	//        }

	//        parameters = new NameValueCollection(parameters);

	//        _commandTimeout = SecUtility.GetIntValue(parameters, "commandTimeout", 30, true, 0);

	//        _enablePasswordRetrieval = SecUtility.GetBooleanValue(parameters, "enablePasswordRetrieval", false);
	//        _enablePasswordReset = SecUtility.GetBooleanValue(parameters, "enablePasswordReset", true);
	//        _requiresQuestionAndAnswer = SecUtility.GetBooleanValue(parameters, "requiresQuestionAndAnswer", true);
	//        _requiresUniqueEmail = SecUtility.GetBooleanValue(parameters, "requiresUniqueEmail", true);
	//        _maxInvalidPasswordAttempts = SecUtility.GetIntValue(parameters, "maxInvalidPasswordAttempts", 5, false, 0);
	//        _passwordAttemptWindow = SecUtility.GetIntValue(parameters, "passwordAttemptWindow", 10, false, 0);
	//        _minRequiredPasswordLength = SecUtility.GetIntValue(parameters, "minRequiredPasswordLength", 6, false, 128);
	//        _minRequiredNonAlphanumericCharacters = SecUtility.GetIntValue(parameters, "minRequiredNonalphanumericCharacters", 0, true, 128);

	//        if (!int.TryParse(ConfigurationManager.AppSettings["passwordAge"], out _maxPasswordAge))
	//        {
	//            _maxPasswordAge = DefaultMaximumPasswordAge;
	//        }

	//        if (!int.TryParse(ConfigurationManager.AppSettings["tempPasswordAge"], out _maxTempPasswordAge))
	//        {
	//            _maxTempPasswordAge = DefaultMaximumTemporaryPasswordAge;
	//        }

	//        _passwordStrengthRegularExpression = parameters["passwordStrengthRegularExpression"];

	//        if (_passwordStrengthRegularExpression != null)
	//        {
	//            _passwordStrengthRegularExpression = _passwordStrengthRegularExpression.Trim();

	//            if (_passwordStrengthRegularExpression.Length != 0)
	//            {
	//                try
	//                {
	//                    Regex regex = new Regex(_passwordStrengthRegularExpression);
	//                }
	//                catch (ArgumentException ex)
	//                {
	//                    throw new ProviderException(ex.Message, ex);
	//                }
	//            }
	//        }
	//        else
	//        {
	//            _passwordStrengthRegularExpression = string.Empty;
	//        }

	//        if (_minRequiredNonAlphanumericCharacters > _minRequiredPasswordLength)
	//        {
	//            throw new HttpException(SR.GetString(SR.MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength));
	//        }

	//        _applicationName = parameters["applicationName"];

	//        if (string.IsNullOrEmpty(_applicationName))
	//        {
	//            _applicationName = SecUtility.GetDefaultAppName();
	//        }

	//        if (_applicationName.Length > 256)
	//        {
	//            throw new ProviderException(SR.GetString(SR.Provider_application_name_too_long));
	//        }

	//        tempString = parameters["passwordFormat"];

	//        if (tempString == null)
	//        {
	//            tempString = "Hashed";
	//        }

	//        switch (tempString)
	//        {
	//            case "Clear":
	//                _passwordFormat = MembershipPasswordFormat.Clear;
	//                break;

	//            case "Encrypted":
	//                _passwordFormat = MembershipPasswordFormat.Encrypted;
	//                break;

	//            case "Hashed":
	//                _passwordFormat = MembershipPasswordFormat.Hashed;
	//                break;

	//            default:
	//                throw new ProviderException(SR.GetString(SR.Provider_bad_password_format));
	//        }

	//        if (_passwordFormat == MembershipPasswordFormat.Hashed && _enablePasswordRetrieval)
	//        {
	//            throw new ProviderException(SR.GetString(SR.Provider_can_not_retrieve_hashed_password));
	//        }

	//        //if (_passwordFormat == MembershipPasswordFormat.Encrypted && MachineKeySection.IsDecryptionKeyAutogenerated)
	//        //    throw new ProviderException(SR.GetString(SR.Can_not_use_encrypted_passwords_with_autogen_keys));

	//        _connectionStringName = parameters["connectionStringName"];

	//        if (_connectionStringName == null || _connectionStringName.Length < 1)
	//        {
	//            throw new ProviderException(SR.GetString(SR.Connection_name_not_specified));
	//        }

	//        string connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

	//        if (connectionString == null || connectionString.Length < 1)
	//        {
	//            throw new ProviderException(SR.GetString(SR.Connection_string_not_found, _connectionStringName));
	//        }

	//        parameters.Remove("connectionStringName");
	//        parameters.Remove("commandTimeout");
	//        parameters.Remove("enablePasswordRetrieval");
	//        parameters.Remove("enablePasswordReset");
	//        parameters.Remove("requiresQuestionAndAnswer");
	//        parameters.Remove("applicationName");
	//        parameters.Remove("requiresUniqueEmail");
	//        parameters.Remove("maxInvalidPasswordAttempts");
	//        parameters.Remove("passwordAttemptWindow");
	//        parameters.Remove("passwordFormat");
	//        parameters.Remove("name");
	//        parameters.Remove("minRequiredPasswordLength");
	//        parameters.Remove("minRequiredNonalphanumericCharacters");
	//        parameters.Remove("passwordStrengthRegularExpression");
	//        parameters.Remove("tempPasswordAge");
	//        parameters.Remove("passwordAge");

	//        if (parameters.Count > 0)
	//        {
	//            string attribUnrecognized = parameters.GetKey(0);

	//            if (!string.IsNullOrEmpty(attribUnrecognized))
	//            {
	//                throw new ProviderException(SR.GetString(SR.Provider_unrecognized_attribute, attribUnrecognized));
	//            }
	//        }
	//    }

	//    #endregion

	//    #region Public Properties

	//    /// <summary>
	//    ///		Gets or sets the name of the application to store and retrieve membership information for.
	//    /// </summary>
	//    /// <value>
	//    ///		The name of the application to store and retrieve membership information for. The
	//    ///		default is the <see cref="P:System.Web.HttpRequest.ApplicationPath" /> property
	//    ///		value for the current <see cref="P:System.Web.HttpContext.Request" />.
	//    ///	</value>
	//    public string ApplicationName
	//    {
	//        get { return _applicationName; }

	//        set
	//        {
	//            if (string.IsNullOrEmpty(value))
	//            {
	//                throw new ArgumentNullException("value");
	//            }

	//            if (value.Length > 256)
	//            {
	//                throw new ProviderException(SR.GetString(SR.Provider_application_name_too_long));
	//            }

	//            _applicationName = value;
	//        }
	//    }

	//    /// <summary>
	//    ///		Gets the number of seconds before a command that is issued to the membership data
	//    ///		source times out.
	//    /// </summary>
	//    /// <value>
	//    ///		The number of seconds before a command that is issued to the membership data source
	//    ///		times out.
	//    ///	</value>
	//    public int CommandTimeout
	//    {
	//        get { return _commandTimeout; }
	//    }

	//    /// <summary>
	//    ///		Gets the name of the connection string used.
	//    /// </summary>
	//    /// <value>
	//    ///		The name of the connection string.
	//    ///	</value>
	//    public string ConnectionStringName
	//    {
	//        get { return _connectionStringName; }
	//    }

	//    /// <summary>
	//    ///		Gets a value indicating whether the membership provider is configured to allow
	//    ///		users to reset their passwords.
	//    /// </summary>
	//    /// <value>
	//    ///		true if the membership provider supports password reset; otherwise, false. The
	//    ///		default is true.
	//    ///	</value>
	//    public bool EnablePasswordReset
	//    {
	//        get { return _enablePasswordReset; }
	//    }

	//    /// <summary>
	//    ///		Gets a value indicating whether the membership provider is configured to allow
	//    ///		users to retrieve their passwords.
	//    /// </summary>
	//    /// <value>
	//    ///		true if the membership provider supports password retrieval; otherwise, false. The
	//    ///		default is false.
	//    ///	</value>
	//    public bool EnablePasswordRetrieval
	//    {
	//        get { return _enablePasswordRetrieval; }
	//    }

	//    /// <summary>
	//    ///		Gets the number of invalid password or password-answer attempts allowed before the
	//    ///		membership user is locked out.
	//    /// </summary>
	//    /// <value>
	//    ///		The number of invalid password or password-answer attempts allowed before the
	//    ///		membership user is locked out.
	//    ///	</value>
	//    public int MaxInvalidPasswordAttempts
	//    {
	//        get { return _maxInvalidPasswordAttempts; }
	//    }

	//    /// <summary>
	//    ///		Gets the number of hours that a password may be used before it expires.
	//    /// </summary>
	//    /// <value>
	//    ///		The number of hours that a password may be used before it expires.
	//    ///	</value>
	//    public int MaxPasswordAge
	//    {
	//        get { return _maxPasswordAge; }
	//    }

	//    /// <summary>
	//    ///		Gets the number of hours that a temporary password may be used before it expires.
	//    /// </summary>
	//    /// <value>
	//    ///		The number of hours that a temporary password may be used before it expires.
	//    ///	</value>
	//    public int MaxTemporaryPasswordAge
	//    {
	//        get { return _maxTempPasswordAge; }
	//    }

	//    /// <summary>
	//    ///		Gets the minimum number of special characters that must be present in a valid
	//    ///		password.
	//    /// </summary>
	//    /// <value>
	//    ///		The minimum number of special characters that must be present in a valid password.
	//    ///	</value>
	//    public int MinRequiredNonAlphanumericCharacters
	//    {
	//        get { return _minRequiredNonAlphanumericCharacters; }
	//    }

	//    /// <summary>
	//    ///		Gets the minimum length required for a password.
	//    /// </summary>
	//    /// <value>
	//    ///		The minimum length required for a password.
	//    ///	</value>
	//    public int MinRequiredPasswordLength
	//    {
	//        get { return _minRequiredPasswordLength; }
	//    }

	//    /// <summary>
	//    ///		Gets the number of minutes in which a maximum number of invalid password or
	//    ///		password-answer attempts are allowed before the membership user is locked out.
	//    /// </summary>
	//    /// <value>
	//    ///		The number of minutes in which a maximum number of invalid password or
	//    ///		password-answer attempts are allowed before the membership user is locked out.
	//    ///	</value>
	//    public int PasswordAttemptWindow
	//    {
	//        get { return _passwordAttemptWindow; }
	//    }

	//    /// <summary>
	//    ///		Gets a value indicating the format for storing passwords in the membership database.
	//    /// </summary>
	//    /// <value>
	//    ///		One of the <see cref="T:System.Web.Security.MembershipPasswordFormat" /> values,
	//    ///		indicating the format for storing passwords in the SQL Server database.
	//    ///	</value>
	//    public MembershipPasswordFormat PasswordFormat
	//    {
	//        get { return _passwordFormat; }
	//    }

	//    /// <summary>
	//    ///		Gets the regular expression used to evaluate a password.
	//    /// </summary>
	//    /// <value>
	//    ///		A regular expression used to evaluate a password.
	//    ///	</value>
	//    public string PasswordStrengthRegularExpression
	//    {
	//        get { return _passwordStrengthRegularExpression; }
	//    }

	//    /// <summary>
	//    ///		Gets a value indicating whether the membership provider is configured to require
	//    ///		the user to answer a password question for password reset and retrieval.
	//    /// </summary>
	//    /// <value>
	//    ///		true if a password answer is required for password reset and retrieval; otherwise,
	//    ///		false. The default is true.
	//    ///	</value>
	//    public bool RequiresQuestionAndAnswer
	//    {
	//        get { return _requiresQuestionAndAnswer; }
	//    }

	//    /// <summary>
	//    ///		Gets a value indicating whether the membership provider is configured to require a
	//    ///		unique e-mail address for each user name.
	//    /// </summary>
	//    /// <value>
	//    ///		true if the membership provider requires a unique e-mail address; otherwise, false.
	//    ///		The default is false.
	//    ///	</value>
	//    public bool RequiresUniqueEmail
	//    {
	//        get { return _requiresUniqueEmail; }
	//    }

	//    #endregion
	//}
}
