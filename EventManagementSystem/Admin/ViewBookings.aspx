<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="ViewBookings.aspx.cs" Inherits="EventManagementSystem.Admin.ViewBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="saas-page-card admin-page-shell">

    <h2 class="saas-heading">View Bookings</h2>

    <asp:Label ID="lblMsg" runat="server" CssClass="saas-subtext"></asp:Label>
    <div class="booking-filter-row">

        <asp:TextBox ID="txtSearch" runat="server" CssClass="booking-filter-input" placeholder="Search by user or event"></asp:TextBox>

        <asp:DropDownList ID="ddlFilter" runat="server" CssClass="booking-filter-select">
            <asp:ListItem Value="all">All</asp:ListItem>
            <asp:ListItem Value="today">Today</asp:ListItem>
            <asp:ListItem Value="recent">Last 7 Days</asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="btnSearch" runat="server" Text="Search"
            CssClass="btn-primary booking-filter-btn" OnClick="btnSearch_Click" />

    </div>

    <div class="grid-wrap">
        <asp:GridView ID="gvBookings" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="BookingID"
            AllowPaging="true"
            PageSize="10"
            PagerSettings-Mode="NextPreviousFirstLast"
            PagerSettings-FirstPageText="First"
            PagerSettings-PreviousPageText="Previous"
            PagerSettings-NextPageText="Next"
            PagerSettings-LastPageText="Last"
            OnPageIndexChanging="gvBookings_PageIndexChanging"
            OnRowCommand="gvBookings_RowCommand"
            OnRowDataBound="gvBookings_RowDataBound"
            CssClass="grid"
            PagerStyle-HorizontalAlign="Center"
            PagerStyle-CssClass="grid-pager">

        <Columns>
            <asp:BoundField DataField="BookingID" HeaderText="ID" />
            <asp:BoundField DataField="UserName" HeaderText="User" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="EventName" HeaderText="Event" />
            <asp:BoundField DataField="SeatsBooked" HeaderText="Seats" />
            <asp:BoundField DataField="BookingDate" HeaderText="Date" />
            <asp:BoundField DataField="BookingStatus" HeaderText="Status" />

            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="False" ItemStyle-Width="130px" HeaderStyle-Width="130px">
                <ItemTemplate>
                    <span class="booking-actions">
                        <asp:LinkButton ID="btnAccept" runat="server"
                            CommandName="AcceptBooking"
                            CommandArgument='<%# Eval("BookingID") %>'
                            Text="Accept"
                            OnClientClick="return confirm('Accept this booking?');">
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnCancel" runat="server"
                            CommandName="CancelBooking"
                            CommandArgument='<%# Eval("BookingID") %>'
                            Text="Cancel"
                            OnClientClick="return confirm('Cancel this booking?');">
                        </asp:LinkButton>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

        </asp:GridView>
    </div>

</div>

</asp:Content>