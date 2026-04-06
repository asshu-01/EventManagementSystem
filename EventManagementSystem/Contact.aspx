<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="Contact.aspx.cs" Inherits="EventManagementSystem.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="container" style="max-width: 600px; margin-top: 3rem; margin-bottom: 4rem;">
    <div class="card" style="border: none; border-radius: 12px; box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.1); overflow: hidden; background-color: #ffffff;">

        <div style="background: linear-gradient(135deg, #0056b3 0%, #007bff 100%); color: #ffffff; padding: 2.5rem 2rem; text-align: center;">
            <h2 style="margin: 0; font-weight: 700; color: #ffffff;">📩 Contact Us</h2>
            <p style="margin: 10px 0 0 0; color: #e2e8f0; font-size: 1.1rem;">We'd love to hear from you!</p>
        </div>

        <div style="padding: 2.5rem; background-color: #ffffff;">

            <!-- NAME -->
            <div style="margin-bottom: 1.5rem;">
                <label style="font-weight: 600; color: #343a40; display: block; margin-bottom: 0.5rem;">Your Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control form-control-lg" MaxLength="100" placeholder="e.g. John Doe" oninput="this.value=this.value.replace(/[^A-Za-z ]/g,'');"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    runat="server"
                    ControlToValidate="txtName"
                    ErrorMessage="Name required"
                    CssClass="text-danger mt-1 d-block" />
                <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="txtName"
                    ErrorMessage="Name must be 3-100 letters/spaces only"
                    ValidationExpression="^[A-Za-z ]{3,100}$"
                    CssClass="text-danger mt-1 d-block" />
            </div>

            <!-- EMAIL -->
            <div style="margin-bottom: 1.5rem;">
                <label style="font-weight: 600; color: #343a40; display: block; margin-bottom: 0.5rem;">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-lg" TextMode="Email" MaxLength="200" placeholder="e.g. john@example.com" oninput="this.value=this.value.replace(/\s/g,'');"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Email required"
                    CssClass="text-danger mt-1 d-block" />
                <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Enter a valid email"
                    ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$"
                    CssClass="text-danger mt-1 d-block" />
            </div>

            <!-- MESSAGE -->
            <div style="margin-bottom: 2rem;">
                <label style="font-weight: 600; color: #343a40; display: block; margin-bottom: 0.5rem;">Your Message</label>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control form-control-lg" TextMode="MultiLine" Rows="5" MaxLength="1000" placeholder="How can we help you?"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    runat="server"
                    ControlToValidate="txtMessage"
                    ErrorMessage="Message required"
                    CssClass="text-danger mt-1 d-block" />
                <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="txtMessage"
                    ErrorMessage="Message must be at least 10 characters"
                    ValidationExpression="^[\s\S]{10,1000}$"
                    CssClass="text-danger mt-1 d-block" />
            </div>

            <!-- BUTTON -->
            <asp:Button ID="btnSend" runat="server"
                Text="Send Message"
                OnClick="btnSend_Click"
                CssClass="btn btn-primary btn-lg w-100" style="padding: 12px; font-weight: 600; border-radius: 8px;" />

            <div class="mt-3 text-center">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
            </div>

        </div>
    </div>
</div>

<script>
    // Auto-hide success message after 5 seconds to keep the UI clean
    window.onload = function () {
        var lblMsg = document.getElementById('<%= lblMsg.ClientID %>');
        if (lblMsg && lblMsg.innerHTML.trim() !== '' && lblMsg.className.indexOf('text-success') > -1) {
            setTimeout(function () {
                lblMsg.style.transition = "opacity 0.5s ease";
                lblMsg.style.opacity = "0";
                setTimeout(function () { lblMsg.style.display = "none"; }, 500);
            }, 5000);
        }
    };
</script>

</asp:Content>