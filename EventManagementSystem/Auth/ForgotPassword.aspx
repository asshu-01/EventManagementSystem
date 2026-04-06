<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="EventManagementSystem.Auth.ForgotPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link href="/CSS/style.css" rel="stylesheet" />
</head>
<body class="auth-page">
    <form id="form1" runat="server">
        <div class="auth-shell">
        <div class="auth-card" style="max-width:450px;">
            <div class="auth-links" style="margin-bottom:10px;">
                <a href="/Auth/Login.aspx">Login</a>
                <a href="/Auth/Register.aspx">Register</a>
            </div>

            <h2 class="auth-title">Forgot Password</h2>
            <p class="auth-subtitle">Enter your registered email to receive an OTP.</p>

            <asp:TextBox ID="txtEmail" runat="server" CssClass="auth-input" TextMode="Email" MaxLength="200" placeholder="Your Email Address"></asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvEmail"
                runat="server"
                ControlToValidate="txtEmail"
                ErrorMessage="Email is required"
                CssClass="field-error"
                Display="Dynamic">
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator
                ID="revEmail"
                runat="server"
                ControlToValidate="txtEmail"
                ErrorMessage="Enter a valid email"
                ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$"
                CssClass="field-error"
                Display="Dynamic">
            </asp:RegularExpressionValidator>

            <asp:Button
                ID="btnSend"
                runat="server"
                Text="Send OTP"
                CssClass="saas-btn saas-btn-primary auth-btn"
                OnClick="btnSend_Click" />

            <asp:Label ID="lblMsg" runat="server" CssClass="saas-subtext"></asp:Label>
        </div>
        </div>
    </form>
</body>
</html>
