<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventDetails.aspx.cs" Inherits="EventManagementSystem.User.EventDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Event Management System</title>
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

            <div class="home-header">
                <h2>Event Details</h2>
                <p class="muted">Review event information before booking.</p>
            </div>

            <div class="section-card">
                <div class="details-grid">
                    <div class="detail-row"><span>Event</span><asp:Label ID="lblEventName" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Date</span><asp:Label ID="lblEventDate" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Venue</span><asp:Label ID="lblVenue" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Price</span><span>₹<asp:Label ID="lblPrice" runat="server"></asp:Label></span></div>
                    <div class="detail-row"><span>Description</span><asp:Label ID="lblDescription" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Mode</span><asp:Label ID="lblMode" runat="server"></asp:Label></div>
                    <div id="meetingRow" runat="server" class="detail-row">
                        <span>Meeting Link</span>
                        <asp:HyperLink ID="lnkMeeting" runat="server" Target="_blank">Join Event</asp:HyperLink>
                    </div>
                    <div class="detail-row"><span>Type</span><asp:Label ID="lblType" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Prize</span><asp:Label ID="lblPrize" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Available Seats</span><asp:Label ID="lblAvailable" runat="server"></asp:Label></div>
                    <div class="detail-row"><span>Status</span><asp:Label ID="lblStatus" runat="server" CssClass="status-pill"></asp:Label></div>
                </div>

                <asp:Label ID="lblWarning" runat="server" CssClass="warning msg-block"></asp:Label>
                <asp:Label ID="lblMsg" runat="server" CssClass="msg-block"></asp:Label>

                <div class="action-field" style="margin-top: 10px;">
                    <asp:Button ID="btnBookNow" runat="server" Text="Proceed to Booking" CssClass="btn-primary" OnClick="btnBookNow_Click" />
                </div>
            </div>

            <div class="footer">© 2026 Event Management System | MCA Project</div>
        </div>
    </form>
</body>
</html>