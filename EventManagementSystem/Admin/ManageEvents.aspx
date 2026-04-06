<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="false" OnLoad="Page_Load" CodeBehind="ManageEvents.aspx.cs" Inherits="EventManagementSystem.Admin.ManageEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="saas-page-card admin-page-shell">

    <h2 class="saas-heading">Manage Events</h2>

    <asp:Label ID="lblCount" runat="server" CssClass="saas-subtext"></asp:Label>

    <div class="form-grid">

        <!-- NAME -->
        <div class="form-group">
            <label>Event Name</label>
            <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtName" runat="server"
                ErrorMessage="Required" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- DATE -->
        <div class="form-group">
            <label>Date</label>
            <asp:TextBox ID="txtDate" runat="server" TextMode="Date"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtDate" runat="server"
                ErrorMessage="Required" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- VENUE -->
        <div class="form-group">
            <label>Venue</label>
            <asp:TextBox ID="txtVenue" runat="server" MaxLength="100"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtVenue" runat="server"
                ErrorMessage="Required" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- PHONE -->
        <div class="form-group">
            <label>Phone Number</label>
            <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" oninput="this.value=this.value.replace(/\D/g,'');"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPhone" runat="server"
                ErrorMessage="Required" CssClass="field-error" ValidationGroup="AddEvent" />
            <asp:RegularExpressionValidator ControlToValidate="txtPhone" runat="server"
                ValidationExpression="^\d{10}$" ErrorMessage="Phone must be 10 digits" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- PRICE -->
        <div class="form-group">
            <label>Price</label>
            <asp:TextBox ID="txtPrice" runat="server" TextMode="Number" oninput="this.value=this.value.replace(/[^0-9.]/g,'');"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPrice" runat="server"
                ErrorMessage="Required" CssClass="field-error" ValidationGroup="AddEvent" />
            <asp:RegularExpressionValidator ControlToValidate="txtPrice" runat="server"
                ValidationExpression="^\d+(\.\d{1,2})?$" ErrorMessage="Invalid price" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- SEATS -->
        <div class="form-group">
            <label>Seats</label>
            <asp:TextBox ID="txtSeats" runat="server" TextMode="Number" oninput="this.value=this.value.replace(/\D/g,'');"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtSeats" runat="server"
                ErrorMessage="Required" CssClass="field-error" ValidationGroup="AddEvent" />
            <asp:RegularExpressionValidator ControlToValidate="txtSeats" runat="server"
                ValidationExpression="^\d+$" ErrorMessage="Seats must be digits only" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- DESCRIPTION -->
        <div class="form-group">
            <label>Description</label>
            <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>
        </div>

        <!-- MODE -->
        <div class="form-group">
            <label>Mode</label>
            <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                <asp:ListItem Value="">--Select--</asp:ListItem>
                <asp:ListItem>Offline</asp:ListItem>
                <asp:ListItem>Online</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlMode" InitialValue="" runat="server"
                ErrorMessage="Select mode" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- LINK -->
        <asp:Panel ID="pnlMeetingLink" runat="server" CssClass="form-group">
            <label>Meeting Link</label>
            <asp:TextBox ID="txtLink" runat="server"></asp:TextBox>
        </asp:Panel>

        <!-- TYPE -->
        <div class="form-group">
            <label>Event Type</label>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem Value="">--Select--</asp:ListItem>
                <asp:ListItem>Seminar</asp:ListItem>
                <asp:ListItem>Workshop</asp:ListItem>
                <asp:ListItem>Hackathon</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlType" InitialValue="" runat="server"
                ErrorMessage="Select event type" CssClass="field-error" ValidationGroup="AddEvent" />
        </div>

        <!-- PRIZE -->
        <div class="form-group">
            <label>Prize Pool</label>
            <asp:TextBox ID="txtPrize" runat="server" oninput="this.value=this.value.replace(/[^0-9.]/g,'');"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>Event Banner</label>
            <asp:FileUpload ID="fuBanner" runat="server" CssClass="auth-input" accept=".jpg,.jpeg,.png,.webp" />
            <asp:Label ID="lblBannerHint" runat="server" CssClass="saas-subtext" Text="Optional. JPG, PNG, WEBP (max 2MB)"></asp:Label>
        </div>

    </div>

    <br />

    <asp:Button ID="btnAdd" runat="server" Text="Add Event"
        CssClass="btn-primary" OnClick="btnAdd_Click" ValidationGroup="AddEvent" />
    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel"
        CssClass="btn-secondary" OnClick="btnCancelEdit_Click" CausesValidation="false" Visible="false" />

    <asp:Label ID="lblMsg" runat="server" CssClass="saas-subtext"></asp:Label>

    <hr />


    <div class="search-row">

    <!-- 🔍 SEARCH -->
    <asp:TextBox ID="txtSearch" runat="server"
        CssClass="filter-search-input"
        placeholder="Search event name"></asp:TextBox>

    <!-- 🎯 FILTER -->
    <asp:DropDownList ID="ddlFilter" runat="server" CssClass="filter-search-select">
        <asp:ListItem Value="all">All</asp:ListItem>
        <asp:ListItem Value="upcoming">Upcoming</asp:ListItem>
        <asp:ListItem Value="full">Full</asp:ListItem>
        <asp:ListItem Value="online">Online</asp:ListItem>
        <asp:ListItem Value="offline">Offline</asp:ListItem>
    </asp:DropDownList>

    <asp:Button ID="btnSearch" runat="server"
        Text="Search"
        CssClass="btn-primary filter-search-btn"
        CausesValidation="false"
        OnClick="btnSearch_Click" />

</div>

    <!-- GRID -->
    <div class="table-wrap">
    <asp:GridView ID="gvEvents" runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="EventID"
    AllowPaging="true"
    PageSize="10"
    PagerSettings-Mode="NextPreviousFirstLast"
    PagerSettings-FirstPageText="First"
    PagerSettings-PreviousPageText="Previous"
    PagerSettings-NextPageText="Next"
    PagerSettings-LastPageText="Last"
    OnPageIndexChanging="gvEvents_PageIndexChanging"
    OnRowCommand="gvEvents_RowCommand"
    OnRowDeleting="gvEvents_RowDeleting"
        CssClass="grid"
    PagerStyle-HorizontalAlign="Center"
    PagerStyle-CssClass="grid-pager">

        <Columns>
            <asp:BoundField DataField="EventID" HeaderText="ID" />
            <asp:BoundField DataField="EventName" HeaderText="Name" />
            <asp:BoundField DataField="EventDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" />
            <asp:BoundField DataField="Venue" HeaderText="Venue" />
            <asp:BoundField DataField="Price" HeaderText="Price" />
            <asp:BoundField DataField="MaxSeats" HeaderText="Seats" />
            <asp:BoundField DataField="ContactPhone" HeaderText="Phone" />
            <asp:TemplateField HeaderText="Banner">
                <ItemTemplate>
                    <asp:Image ID="imgBanner" runat="server"
                        ImageUrl='<%# ResolveImageUrl(Eval("ImagePath")) %>'
                        Visible='<%# HasImage(Eval("ImagePath")) %>'
                        CssClass="event-thumb" />
                    <asp:Label ID="lblNoBanner" runat="server" Text="No image"
                        Visible='<%# !HasImage(Eval("ImagePath")) %>' CssClass="saas-subtext" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="EventMode" HeaderText="Mode" />
            <asp:BoundField DataField="MeetingLink" HeaderText="Meeting Link" />
            <asp:BoundField DataField="Description" HeaderText="Description" />

            <asp:TemplateField>
                <ItemTemplate>
                    <div class="grid-actions">
                        <asp:LinkButton runat="server" CommandName="EditEvent" Text="Update" CausesValidation="false"
                            CommandArgument='<%# Eval("EventID") %>'
                            OnClientClick="return confirm('Are you sure you want to update this event?');" />
                        <asp:LinkButton runat="server" CommandName="Delete"
                            Text="Delete" CausesValidation="false"
                            OnClientClick="return confirm('Delete this event?');" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>
    </div>

</div>

<script>
    // 🔥 BLOCK PAST DATES
    window.onload = function () {
        let today = new Date().toISOString().split("T")[0];
        document.getElementById('<%= txtDate.ClientID %>').setAttribute("min", today);
    };
</script>

</asp:Content>