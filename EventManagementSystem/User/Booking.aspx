<%@ Page Language="C#" 
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true" 
    CodeBehind="Booking.aspx.cs" 
    Inherits="EventManagementSystem.User.Booking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="booking-card saas-page-card" style="max-width:460px; margin:auto;">

    <h2 class="saas-heading">Book Event</h2>

    <div class="form-group">
        <label>Number of Seats</label>
        <asp:TextBox ID="txtSeats" runat="server" CssClass="auth-input" TextMode="Number"></asp:TextBox>

        <asp:RequiredFieldValidator 
            ID="rfvSeats" 
            runat="server"
            ControlToValidate="txtSeats"
            ErrorMessage="Seats required"
            CssClass="field-error" />

        <asp:RangeValidator 
            ID="rvSeats"
            runat="server"
            ControlToValidate="txtSeats"
            MinimumValue="1"
            MaximumValue="100"
            Type="Integer"
            ErrorMessage="Enter valid seats"
            CssClass="field-error" />
    </div>

    <asp:Button ID="btnBook" runat="server"
        Text="Confirm Booking"
        CssClass="saas-btn saas-btn-primary auth-btn"
        OnClick="btnBook_Click" />

    <asp:Label ID="lblMsg" runat="server" CssClass="saas-subtext"></asp:Label>

</div>

</asp:Content>