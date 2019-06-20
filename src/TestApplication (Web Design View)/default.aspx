<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebControlDebugApplication._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title></title>
	</head>
	<body>
		<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<div>
			<osc:Captcha ID="CaptchaControl1" runat="server" CaptchaBackgroundNoise="High" CaptchaFontWarping="Medium" CaptchaLength="6"></osc:Captcha>
			&nbsp;
			<asp:ValidationSummary ID="ValidationSummary1" runat="server" />
			<p>
			</p>
			(note that ViewState and SessionState are both <strong>disabled</strong>)<br />
			<br />
			<asp:Button ID="Button1" runat="server" Text="Submit" />
		</div>
		</form>
	</body>
</html>
