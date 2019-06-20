using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace openSourceC.FrameworkLibrary.Web.Http
{
	/// <summary>
	///		Summary description for XmlCommentDocumentationProvider.
	/// </summary>
	public class XmlCommentDocumentationProvider : IDocumentationProvider
	{
		private const string _typeExpression = "/doc/members/member[@name='T:{0}']";
		private const string _methodExpression = "/doc/members/member[@name='M:{0}']";
		private const string _propertyExpression = "/doc/members/member[@name='P:{0}']";
		private const string _fieldExpression = "/doc/members/member[@name='F:{0}']";
		private const string _parameterExpression = "param[@name='{0}']";

		private XPathNavigator _documentNavigator;


		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="documentPath"></param>
		public XmlCommentDocumentationProvider(string documentPath)
		{
			XPathDocument xpathDocument = new XPathDocument(documentPath);

			_documentNavigator = xpathDocument.CreateNavigator();
		}

		#endregion

		#region IDocumentationProvider

		/// <summary>
		///		Gets the documentation based on <see cref="T:HttpActionDescriptor" />.
		///	</summary>
		/// <param name="actionDescriptor">The action descriptor.</param>
		/// <returns>
		///		The documentation for the controller action.
		///	</returns>
		public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
		{
			XPathNavigator methodNode = GetMethodNode(actionDescriptor);

			return GetNodeValue(methodNode, "summary", "No Documentation Found.");
		}

		/// <summary>
		///		Gets the documentation based on <see cref="T:HttpControllerDescriptor" />.
		/// </summary>
		/// <param name="controllerDescriptor">The controller descriptor.</param>
		/// <returns>
		///		The documentation for the controller.
		///	</returns>
		public virtual string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
		{
			XPathNavigator typeNode = GetTypeNode(controllerDescriptor.ControllerType);

			return GetNodeValue(typeNode, "summary", "No Documentation Found.");
		}

		/// <summary>
		///		Gets the documentation based on <see cref="T:HttpParameterDescriptor" />.
		///	</summary>
		/// <returns>The documentation for the controller.</returns>
		/// <param name="parameterDescriptor">
		///		The parameter descriptor.
		///	</param>
		public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
		{
			ReflectedHttpParameterDescriptor reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;

			if (reflectedParameterDescriptor != null)
			{
				XPathNavigator methodNode = GetMethodNode(reflectedParameterDescriptor.ActionDescriptor);

				if (methodNode != null)
				{
					string selectExpression = string.Format(_parameterExpression, reflectedParameterDescriptor.ParameterInfo.Name);

					XPathNavigator parameterNode = methodNode.SelectSingleNode(selectExpression);

					if (parameterNode != null)
					{
						return parameterNode.Value.Trim();
					}
				}
			}

			return "No Documentation Found.";
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="actionDescriptor"></param>
		/// <returns></returns>
		public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
		{
			XPathNavigator methodNode = GetMethodNode(actionDescriptor);

			return GetNodeValue(methodNode, "returns", "No Documentation Found.");
		}

		#endregion

		#region Public Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		public string GetDocumentation(MemberInfo member)
		{
			string memberName = string.Format("{0}.{1}", GetTypeName(member.DeclaringType), member.Name);
			string expression = (member.MemberType == MemberTypes.Field ? _fieldExpression : _propertyExpression);
			string selectExpression = string.Format(expression, memberName);

			XPathNavigator propertyNode = _documentNavigator.SelectSingleNode(selectExpression);

			return GetNodeValue(propertyNode, "summary");
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public string GetDocumentation(Type type)
		{
			XPathNavigator typeNode = GetTypeNode(type);

			return GetNodeValue(typeNode, "summary");
		}

		#endregion

		#region Private Methods

		private string GetMethodName(MethodInfo method)
		{
			string name = string.Format("{0}.{1}", GetTypeName(method.DeclaringType), method.Name);

			ParameterInfo[] parameters = method.GetParameters();

			if (parameters.Length != 0)
			{
				string[] parameterTypeNames = parameters.Select(param => GetTypeName(param.ParameterType)).ToArray();

				name += string.Format("({0})", string.Join(", ", parameterTypeNames));
			}

			return name;
		}

		private XPathNavigator GetMethodNode(HttpActionDescriptor actionDescriptor)
		{
			ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;

			if (reflectedActionDescriptor == null) { return null; }

			string selectExpression = string.Format(_methodExpression, GetMethodName(reflectedActionDescriptor.MethodInfo));

			return _documentNavigator.SelectSingleNode(selectExpression);
		}

		private string GetNodeValue(XPathNavigator xpathNavigator, string nodeName, string defaultValue = null)
		{
			if (xpathNavigator == null) { return defaultValue; }

			XPathNavigator node = xpathNavigator.SelectSingleNode(nodeName);

			return (node == null ? defaultValue : node.Value.Trim());
		}

		private string GetTypeName(Type type)
		{
			string name = type.FullName;

			if (type.IsGenericType)
			{
				// Format the generic type name to something like: Generic{System.Int32,System.String}
				Type genericType = type.GetGenericTypeDefinition();
				Type[] genericArguments = type.GetGenericArguments();
				string genericTypeName = genericType.FullName;


				// Trim the generic parameter counts from the name
				genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
				string[] argumentTypeNames = genericArguments.Select(t => GetTypeName(t)).ToArray();
				name = string.Format("{0}{{{1}}}", genericTypeName, string.Join(",", argumentTypeNames));
			}

			if (type.IsNested)
			{
				// Changing the nested type name from OuterType+InnerType to OuterType.InnerType to match the XML documentation syntax.
				name = name.Replace("+", ".");
			}

			return name;
		}

		private XPathNavigator GetTypeNode(Type type)
		{
			string selectExpression = string.Format(_typeExpression, GetTypeName(type));

			return _documentNavigator.SelectSingleNode(selectExpression);
		}

		#endregion
	}
}