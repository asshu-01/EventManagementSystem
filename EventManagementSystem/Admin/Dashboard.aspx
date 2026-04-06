<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="Dashboard.aspx.cs" Inherits="EventManagementSystem.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="saas-page-card admin-page-shell">
        <h2 class="saas-heading">Admin Dashboard</h2>
        <p class="saas-subtext">Welcome, <asp:Label ID="lblUserName" runat="server"></asp:Label></p>
        <asp:Label ID="lblRole" runat="server" CssClass="saas-subtext"></asp:Label>

        <div class="saas-stats-grid">
            <div class="saas-stat-card">
                <div class="saas-stat-label">Total Users</div>
                <asp:Label ID="lblUsers" runat="server" CssClass="saas-stat-value"></asp:Label>
            </div>

            <div class="saas-stat-card">
                <div class="saas-stat-label">Total Events</div>
                <asp:Label ID="lblEvents" runat="server" CssClass="saas-stat-value"></asp:Label>
            </div>

            <div class="saas-stat-card">
                <div class="saas-stat-label">Total Bookings</div>
                <asp:Label ID="lblBookings" runat="server" CssClass="saas-stat-value"></asp:Label>
            </div>

            <div class="saas-stat-card">
                <div class="saas-stat-label">Total Tickets Sold</div>
                <asp:Label ID="lblTickets" runat="server" CssClass="saas-stat-value"></asp:Label>
            </div>
        </div>
    </div>

</asp:Content>
