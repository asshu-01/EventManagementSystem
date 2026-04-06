<%@ Page Language="C#" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="ResetPassword.aspx.cs" Inherits="EventManagementSystem.Auth.ResetPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="<%= ResolveUrl("~/CSS/style.css") %>" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #0f172a 0%, #1e3a8a 100%) !important;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0;
            padding: 2rem 0;
        }

        form { width: 100%; }

        .container {
            max-width: 450px;
            background: #ffffff;
            padding: 2.5rem;
            border-radius: 12px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.5);
            margin: 0 auto !important;
            text-align: center;
        }

        h2 {
            font-weight: 700;
            color: #1e293b;
            margin-bottom: 1rem;
        }

        p {
            color: #64748b;
            margin-bottom: 2rem;
            font-size: 0.95rem;
        }

        .input-box {
            width: 100%;
            padding: 12px 15px;
            border: 1px solid #cbd5e1;
            border-radius: 8px;
            margin-bottom: 0.5rem;
            box-sizing: border-box;
            transition: border-color 0.2s;
        }
        .input-box:focus {
            outline: none;
            border-color: #3b82f6;
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.2);
        }

        .btn-primary {
            width: 100%;
            background-color: #2563eb;
            color: #ffffff;
            border: none;
            padding: 12px;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.2s;
            margin-top: 1rem;
            margin-bottom: 1.5rem;
        }
        .btn-primary:hover {
            background-color: #1d4ed8;
        }

        .error { color:#ef4444; font-size: 0.85rem; display: block; margin-bottom: 1rem; text-align: left; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container">
        <h2>🔑 Reset Password</h2>
        <p>Enter the OTP sent to your email and set a new password.</p>
        <asp:Label ID="lblStatus" runat="server" style="font-weight: 600; font-size: 0.95rem; display: block; margin-bottom: 1rem;"></asp:Label>
        <asp:Panel ID="pnlReset" runat="server">
            <asp:TextBox ID="txtOtp" runat="server" CssClass="input-box" MaxLength="6" placeholder="Enter 6-digit OTP"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtOtp" ErrorMessage="OTP required" CssClass="error" runat="server" Display="Dynamic" />
            <asp:RegularExpressionValidator ControlToValidate="txtOtp" ErrorMessage="OTP must be 6 digits" CssClass="error" ValidationExpression="^\d{6}$" runat="server" Display="Dynamic" />

            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-box" placeholder="New Password"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ErrorMessage="Password required" CssClass="error" runat="server" Display="Dynamic" />
            <asp:RegularExpressionValidator ControlToValidate="txtPassword" ErrorMessage="Min 8 chars, 1 Upper, 1 Lower, 1 Number, 1 Symbol" CssClass="error" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).{8,}$" runat="server" Display="Dynamic" />

            <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="input-box" placeholder="Confirm Password"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtConfirm" ErrorMessage="Confirm password required" CssClass="error" runat="server" Display="Dynamic" />
            <asp:CompareValidator ControlToValidate="txtConfirm" ControlToCompare="txtPassword"
                ErrorMessage="Passwords do not match"
                CssClass="error"
                runat="server" />

            <br /><br />

            <asp:Button ID="btnReset" runat="server"
                Text="Reset Password"
                CssClass="btn-primary"
                OnClick="btnReset_Click" />

        </asp:Panel>

        <br />
        <asp:Label ID="lblMsg" runat="server"></asp:Label>

    </div>
</div>

</form>
</body>
</html>