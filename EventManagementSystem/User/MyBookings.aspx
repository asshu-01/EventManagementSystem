<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBookings.aspx.cs" Inherits="EventManagementSystem.User.MyBookings" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Bookings</title>
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/CSS/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-sm navbar-dark bg-dark home-top-nav">
            <div class="container-fluid">
                <a class="navbar-brand" href="/User/Home.aspx">Event Management System</a>
                <ul class="navbar-nav me-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="/User/Home.aspx">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/User/MyBookings.aspx">My Bookings</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Contact.aspx">Contact Us</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/About.aspx">About</a>
                    </li>
                </ul>
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <asp:LinkButton ID="btnLogout" runat="server" CssClass="nav-link" CausesValidation="false" OnClick="btnLogout_Click">Logout</asp:LinkButton>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="container home-page">

            <div class="home-header saas-page-card">
                <h2 class="saas-heading">My Bookings</h2>
                <asp:Label ID="lblMsg" runat="server" CssClass="msg-block saas-subtext"></asp:Label>
            </div>

            <div class="section-card saas-page-card">
                <div class="summary-grid">
                    <div class="metric-card">
                        <div class="metric-label">Total Bookings</div>
                        <asp:Label ID="lblTotalBookings" runat="server" CssClass="metric-value"></asp:Label>
                    </div>
                    <div class="metric-card">
                        <div class="metric-label">Total Spent</div>
                        <asp:Label ID="lblTotalAmount" runat="server" CssClass="metric-value"></asp:Label>
                    </div>
                </div>

                <asp:GridView ID="gvBookings"
                    runat="server"
                    AutoGenerateColumns="false"
                    AllowPaging="true"
                    PageSize="10"
                    CssClass="events-grid"
                    EmptyDataText="You have no bookings yet."
                    OnPageIndexChanging="gvBookings_PageIndexChanging"
                    OnRowCommand="gvBookings_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="EventID" HeaderText="Event ID" />
                        <asp:BoundField DataField="EventName" HeaderText="Event" />
                        <asp:BoundField DataField="EventDate" HeaderText="Date" />
                        <asp:BoundField DataField="Venue" HeaderText="Venue" />
                        <asp:BoundField DataField="SeatsBooked" HeaderText="Seats" />
                        <asp:BoundField DataField="BookingStatus" HeaderText="Status" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button
                                    Text="Cancel"
                                    CommandName="CancelBooking"
                                    CommandArgument='<%# Eval("BookingID") %>'
                                    CssClass="btn-danger"
                                    OnClientClick="return confirm('Cancel this booking?');"
                                    Visible='<%# Eval("BookingStatus") == null || Eval("BookingStatus").ToString().ToLower() != "cancelled" %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>