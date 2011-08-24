<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SampleWeb._default" %>

<%@ Import Namespace="openSourceC.FrameworkLibrary.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title></title>
	</head>
	<body>
		<form id="form1" runat="server">
			<asp:ScriptManager ID="ScriptManager1" runat="server" />
			<table border="0" cellspacing="0" cellpadding="0" style="height: 100%; width: 800px; border-collapse: collapse; border: 0px solid cyan;">
				<tr valign="top" style="height: 50px; margin: 0; padding: 0;">
					<td style="margin: 0; padding: 0;"><osc:ContentBlock ID="TestContentBlock" SkinID="Skin1" runat="server">
						<HeaderTemplate><asp:Label SkinID="MainTitle" runat="server" /></HeaderTemplate>
						<ContentTemplate><asp:TextBox ID="TextBox1" runat="server" /> test me</ContentTemplate>
					</osc:ContentBlock></td>
				</tr>
				<tr valign="top" style="height: 50px; margin: 0; padding: 0;">
					<td style="margin: 0; padding: 0;"><osc:ContentBlock ID="Test2ContentBlock" SkinID="Skin1" ForeColor="Blue" CollapseMode="Expanded" runat="server" BorderWidth="1px" BorderStyle="Solid" BorderColor="Cyan" BackColor="Red">
						<HeaderTemplate>HEADER</HeaderTemplate>
						<ContentTemplate><asp:TextBox ID="TextBox3" runat="server" /> test me</ContentTemplate>
					</osc:ContentBlock></td>
				</tr>
				<tr>
					<td>
						<asp:UpdatePanel ID="TestUpdatePanel" runat="server">
							<ContentTemplate>
								Update panel content.<br />
								IsEIN("26-3609569") =
								<%= StringValidate.IsEIN("26-3609569") %><br />
								<input type="text" style="" /><br />
								<asp:Label ID="Label2" runat="server" AssociatedControlID="TextBox2" Text="Label" AccessKey="A"></asp:Label>
								<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
			</table>
			<div>
				<osc:Captcha ID="CaptchaControl1" runat="server" CaptchaBackgroundNoise="High" CaptchaFontWarping="Medium" CaptchaLength="6"></osc:Captcha>
				&nbsp;
				<asp:ValidationSummary ID="ValidationSummary1" runat="server" />
				<p></p>
				(note that ViewState and SessionState are both <strong>disabled</strong>)<br />
				<br />
				<asp:Button ID="Button1" runat="server" Text="Submit" />
			</div>
		</form>
	</body>
</html>
