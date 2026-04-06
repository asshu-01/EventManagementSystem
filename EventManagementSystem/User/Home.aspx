<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="EventManagementSystem.User.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Event Management System</title>
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/CSS/style.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<!-- NAVBAR -->
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

    <!-- HEADER -->
    <div class="home-header saas-page-card">
        <h2 class="saas-heading">Welcome, <asp:Label ID="lblUser" runat="server"></asp:Label></h2>
    </div>

    <div class="section-card saas-page-card">
        <h3 class="saas-heading">Available Events</h3>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- SEARCH -->
            <div class="search-panel">
                <asp:TextBox ID="txtSearch" runat="server"
                    CssClass="input-search"
                    AutoPostBack="true"
                    OnTextChanged="Search_Click"
                    placeholder="Search event..." />

                <asp:DropDownList ID="ddlFilter" runat="server"
                    CssClass="input-filter"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="Search_Click">
                    <asp:ListItem Value="all">All</asp:ListItem>
                    <asp:ListItem Value="open">Available</asp:ListItem>
                    <asp:ListItem Value="low">Low Seats</asp:ListItem>
                </asp:DropDownList>
            </div>

            <asp:Label ID="lblMsg" runat="server" CssClass="saas-subtext"></asp:Label>

            <h4 class="saas-subtext">Total Events: <asp:Label ID="lblTotalEvents" runat="server"></asp:Label></h4>

            <!-- GRID -->
            <asp:GridView ID="gvEvents"
                runat="server"
                CssClass="events-grid"
                AutoGenerateColumns="false"
                DataKeyNames="EventID"

                AllowPaging="true"
                PageSize="10"
                OnPageIndexChanging="gvEvents_PageIndexChanging"

                OnRowCommand="gvEvents_RowCommand"
                OnRowDataBound="gvEvents_RowDataBound">

                <Columns>
                    <asp:BoundField DataField="EventID" HeaderText="ID" />
                    <asp:BoundField DataField="EventName" HeaderText="Event" />
                    <asp:BoundField DataField="EventDate" HeaderText="Date" />
                    <asp:BoundField DataField="Venue" HeaderText="Venue" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="AvailableSeats" HeaderText="Seats" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" CssClass="status-badge" Text='<%# Eval("EventStatus") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnDetails" runat="server"
                                CssClass="btn-details"
                                Text="View Details"
                                CommandName="Details"
                                CommandArgument='<%# Eval("EventID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</div>
</form>
</body>
</html>
