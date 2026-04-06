<%@ Page Language="C#" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="Login.aspx.cs" Inherits="EventManagementSystem.Auth.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="<%= ResolveUrl("~/CSS/style.css") %>" rel="stylesheet" />
    <script type="text/javascript">
        function showAdminLogin() {
            var userPanel = document.getElementById('user-login-panel');
            var adminPanel = document.getElementById('admin-login-panel');
            var title = document.getElementById('pageTitle');
            if (userPanel) userPanel.style.display = 'none';
            if (adminPanel) adminPanel.style.display = 'block';
            if (title) title.innerText = 'Admin Login';
            return false;
        }

        function showUserLogin() {
            var userPanel = document.getElementById('user-login-panel');
            var adminPanel = document.getElementById('admin-login-panel');
            var title = document.getElementById('pageTitle');
            if (adminPanel) adminPanel.style.display = 'none';
            if (userPanel) userPanel.style.display = 'block';
            if (title) title.innerText = 'Login';
            return false;
        }
    </script>
</head>
<body class="auth-page">
<form id="form1" runat="server">
    <div class="auth-shell">
    <div class="auth-card">
        <h2 id="pageTitle" class="auth-title">Login</h2>
        <p class="auth-subtitle">Welcome back. Access your event workspace.</p>
        <div class="form-box" id="user-login-panel">
            <span class="field-label">Email</span>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="auth-input" MaxLength="200"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvLoginEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="field-error" Display="Dynamic" ValidationGroup="UserLogin" />
            <asp:RegularExpressionValidator ID="revLoginEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Enter a valid email" CssClass="field-error" Display="Dynamic" ValidationGroup="UserLogin" ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$" />
            <span class="field-label">Password</span>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="auth-input" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvLoginPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" CssClass="field-error" Display="Dynamic" ValidationGroup="UserLogin" />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="saas-btn saas-btn-primary auth-btn" OnClick="btnLogin_Click" UseSubmitBehavior="false" ValidationGroup="UserLogin" />
            <div class="auth-links">
                <a href="<%= ResolveUrl("~/Auth/Register.aspx") %>">New User? Register here</a>
                <a href="<%= ResolveUrl("~/Auth/ForgotPassword.aspx") %>">Forgot Password?</a>
                <a href="#" onclick="return showAdminLogin();">Go to Admin Login</a>
            </div>
        </div>

        <div class="form-box" id="admin-login-panel" style="display:none;">
            <span class="field-label">Admin ID</span>
            <asp:TextBox ID="txtAdminId" runat="server" CssClass="auth-input" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAdminId" runat="server" ControlToValidate="txtAdminId" ErrorMessage="Admin ID is required" CssClass="field-error" Display="Dynamic" ValidationGroup="AdminLogin" />
            <span class="field-label">Special Key</span>
            <asp:TextBox ID="txtSpecialKey" runat="server" TextMode="Password" CssClass="auth-input" MaxLength="100"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvSpecialKey" runat="server" ControlToValidate="txtSpecialKey" ErrorMessage="Special Key is required" CssClass="field-error" Display="Dynamic" ValidationGroup="AdminLogin" />
            <asp:Button ID="btnAdminLogin" runat="server" Text="Admin Login" CssClass="saas-btn saas-btn-primary auth-btn" OnClick="btnAdminLogin_Click" UseSubmitBehavior="false" ValidationGroup="AdminLogin" />
            <div class="auth-links">
                <a href="#" onclick="return showUserLogin();">Back to User Login</a>
            </div>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="field-error"></asp:Label>
    </div>
    </div>
</form>
</body>
</html>