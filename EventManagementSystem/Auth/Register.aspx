<%@ Page Language="C#" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="Register.aspx.cs" Inherits="EventManagementSystem.Auth.Register" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <link href="<%= ResolveUrl("~/CSS/style.css") %>" rel="stylesheet" />
    <script type="text/javascript">
        function toggleRegisterPassword() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            var confirmPwd = document.getElementById('<%= txtConfirmPassword.ClientID %>');
            var chk = document.getElementById('chkShowRegisterPassword');
            if (!pwd || !confirmPwd || !chk) return;

            var type = chk.checked ? 'text' : 'password';
            pwd.type = type;
            confirmPwd.type = type;
        }
    </script>
</head>
<body class="auth-page">
<form id="form1" runat="server">
    <div class="auth-shell">
    <div class="auth-card">
        <h2 class="auth-title">Register</h2>
        <p class="auth-subtitle">Create your account and start managing events.</p>
        <div class="form-box">
            <span class="field-label">Name</span>
            <div class="field-line">
                <asp:TextBox ID="txtName" runat="server" CssClass="auth-input" MaxLength="100" oninput="this.value=this.value.replace(/[^A-Za-z ]/g,'');"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" />
                <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ErrorMessage="Min 3 letters, letters/spaces only" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" OnServerValidate="cvName_ServerValidate" />
            </div>

            <span class="field-label">Email</span>
            <div class="field-line">
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="auth-input" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" />
                <asp:CustomValidator ID="cvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" OnServerValidate="cvEmail_ServerValidate" />
            </div>

            <span class="field-label">Phone</span>
            <div class="field-line">
                <asp:TextBox ID="txtPhone" runat="server" CssClass="auth-input" MaxLength="10" oninput="this.value=this.value.replace(/\D/g,'');"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone is required" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" />
                <asp:CustomValidator ID="cvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Enter 10 digits" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" OnServerValidate="cvPhone_ServerValidate" />
            </div>

            <span class="field-label">Password</span>
            <div class="field-line">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="auth-input" TextMode="Password" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" />
            </div>

            <span class="field-label">Confirm Password</span>
            <div class="field-line">
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="auth-input" TextMode="Password" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm required" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" />
                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Passwords mismatch" CssClass="field-error" Display="Dynamic" ValidationGroup="Reg" />
            </div>

            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="saas-btn saas-btn-primary auth-btn" OnClick="btnRegister_Click" ValidationGroup="Reg" />
            <a href="Login.aspx" class="login-link">Already have an account? Login here</a>
            <div style="text-align: center; margin-top: 1rem;">
                <asp:Label ID="lblMsg" runat="server" CssClass="saas-subtext"></asp:Label>
            </div>
        </div>
    </div>
    </div>
</form>
</body>
</html>
