using EventManagementSystem.DAL;
using EventManagementSystem.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace EventManagementSystem.Admin
{
    public partial class ManageEvents : System.Web.UI.Page
    {
        private int? EditingEventId
        {
            get
            {
                object value = ViewState["EditingEventId"];
                return value == null ? (int?)null : Convert.ToInt32(value);
            }
            set
            {
                ViewState["EditingEventId"] = value;
            }
        }

        private string EditingImagePath
        {
            get
            {
                object value = ViewState["EditingImagePath"];
                return value == null ? string.Empty : Convert.ToString(value);
            }
            set
            {
                ViewState["EditingImagePath"] = value ?? string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes["enctype"] = "multipart/form-data";

            // 🔒 SECURITY
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (Session["UserRole"] == null ||
                !Session["UserRole"].ToString().Equals("Admin"))
            {
                Response.Redirect("~/User/Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                SetMeetingLinkVisibility();
                LoadEvents(); // default load
            }
        }

        // 🔥 UPDATED LOAD (SEARCH + FILTER)
        void LoadEvents(string search = "", string filter = "all")
        {
            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetEvents", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    string escapedSearch = (search ?? string.Empty).Trim().Replace("'", "''");
                    string rowFilter = "1=1";

                    if (!string.IsNullOrWhiteSpace(escapedSearch))
                    {
                        rowFilter += " AND Convert(EventName, 'System.String') LIKE '%" + escapedSearch + "%'";
                    }

                    if (filter == "upcoming")
                        rowFilter += " AND EventDate >= #" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "#";
                    else if (filter == "full")
                        rowFilter += " AND AvailableSeats = 0";
                    else if (filter == "online")
                        rowFilter += " AND EventMode = 'Online'";
                    else if (filter == "offline")
                        rowFilter += " AND EventMode = 'Offline'";

                    DataView view = dt.DefaultView;
                    view.RowFilter = rowFilter;
                    view.Sort = "EventDate ASC";
                    DataTable filtered = view.ToTable();

                    if (!filtered.Columns.Contains("ContactPhone"))
                        filtered.Columns.Add("ContactPhone", typeof(string));
                    if (!filtered.Columns.Contains("ImagePath"))
                        filtered.Columns.Add("ImagePath", typeof(string));

                    foreach (DataRow row in filtered.Rows)
                    {
                        if (row["ContactPhone"] == DBNull.Value)
                            row["ContactPhone"] = string.Empty;

                        if (row["ImagePath"] == DBNull.Value)
                            row["ImagePath"] = string.Empty;

                        string mode = Convert.ToString(row["EventMode"]);
                        if (string.Equals(mode, "Offline", StringComparison.OrdinalIgnoreCase))
                            row["MeetingLink"] = "Not applicable";
                    }

                    lblCount.Text = "Total Events: " + filtered.Rows.Count;
                    gvEvents.DataSource = filtered;
                    gvEvents.DataBind();
                }
            }
        }

        // 🔥 SEARCH BUTTON
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            LoadEvents(txtSearch.Text, ddlFilter.SelectedValue);
        }

        // 🔥 ADD EVENT
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblMsg.CssClass = "error";
            lblMsg.Text = string.Empty;

            if (!Page.IsValid)
            {
                lblMsg.Text = "Please fill all required fields correctly.";
                return;
            }

            DateTime selectedDate;

            if (!DateTime.TryParse(txtDate.Text, out selectedDate))
            {
                lblMsg.Text = "Invalid date";
                return;
            }

            if (selectedDate.Date <= DateTime.Today)
            {
                lblMsg.Text = "Date must be future (no past allowed)";
                return;
            }

            if (ddlMode.SelectedValue == "Online" && string.IsNullOrWhiteSpace(txtLink.Text))
            {
                lblMsg.Text = "Meeting link required for online event.";
                return;
            }

            decimal price;
            int seats;
            decimal prizePool = 0m;
            string phone = Regex.Replace(txtPhone.Text ?? string.Empty, "[^0-9]", string.Empty).Trim();
            txtPhone.Text = phone;

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                lblMsg.Text = "Phone number must be exactly 10 digits.";
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out price))
            {
                lblMsg.Text = "Invalid price.";
                return;
            }

            if (!int.TryParse(txtSeats.Text.Trim(), out seats) || seats <= 0)
            {
                lblMsg.Text = "Seats must be a positive number.";
                return;
            }

            if (ddlType.SelectedValue == "Hackathon" && !decimal.TryParse(txtPrize.Text.Trim(), out prizePool))
            {
                lblMsg.Text = "Prize pool is required for hackathon.";
                return;
            }

            try
            {
                bool isUpdate = EditingEventId.HasValue;

                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(string.Empty, con))
                {
                    con.Open();
                    string bannerPath = EditingImagePath;
                    if (fuBanner.HasFile)
                    {
                        bannerPath = SaveBannerImage();
                        if (string.IsNullOrWhiteSpace(bannerPath))
                        {
                            return;
                        }
                    }

                    if (isUpdate)
                    {
                        cmd.CommandText = "sp_UpdateEvent";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EventID", EditingEventId.Value);
                    }
                    else
                    {
                        cmd.CommandText = "sp_AddEvent";
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    cmd.Parameters.Add("@EventName", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
                    cmd.Parameters.Add("@EventDate", SqlDbType.DateTime).Value = selectedDate;
                    cmd.Parameters.Add("@Venue", SqlDbType.NVarChar, 100).Value = txtVenue.Text.Trim();
                    SqlParameter priceParam = cmd.Parameters.Add("@Price", SqlDbType.Decimal);
                    priceParam.Precision = 10;
                    priceParam.Scale = 2;
                    priceParam.Value = price;
                    cmd.Parameters.Add("@MaxSeats", SqlDbType.Int).Value = seats;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = txtDesc.Text.Trim();
                    cmd.Parameters.Add("@EventMode", SqlDbType.NVarChar, 10).Value = ddlMode.SelectedValue;
                    cmd.Parameters.Add("@MeetingLink", SqlDbType.NVarChar, 300).Value = ddlMode.SelectedValue == "Online"
                        ? (object)txtLink.Text.Trim()
                        : DBNull.Value;
                    cmd.Parameters.Add("@EventType", SqlDbType.NVarChar, 50).Value = ddlType.SelectedValue;
                    cmd.Parameters.Add("@ContactPhone", SqlDbType.NVarChar, 20).Value = phone;
                    SqlParameter prizeParam = cmd.Parameters.Add("@PrizePool", SqlDbType.Decimal);
                    prizeParam.Precision = 10;
                    prizeParam.Scale = 2;
                    prizeParam.Value = ddlType.SelectedValue == "Hackathon" ? (object)prizePool : DBNull.Value;
                    cmd.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 300).Value = string.IsNullOrWhiteSpace(bannerPath)
                        ? (object)DBNull.Value
                        : bannerPath;

                    cmd.ExecuteNonQuery();

                    if (isUpdate)
                    {
                        NotifyUsersEventUpdated(con, EditingEventId.Value, txtName.Text.Trim(), selectedDate,
                            txtVenue.Text.Trim(), ddlMode.SelectedValue, txtLink.Text.Trim(), price);
                    }
                }

                lblMsg.CssClass = "success";
                lblMsg.Text = isUpdate ? "Event updated successfully!" : "Event added successfully!";
                ResetEditMode();
                ClearFields();
                LoadEvents(); // reload
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Unable to add event. " + ex.Message;
            }
        }

        protected void gvEvents_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "EditEvent", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            int eventId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out eventId))
            {
                return;
            }

            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetEventById", con))
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EventID", eventId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return;
                    }

                    EditingEventId = eventId;
                    txtName.Text = Convert.ToString(reader["EventName"]);
                    txtDate.Text = Convert.ToDateTime(reader["EventDate"]).ToString("yyyy-MM-dd");
                    txtVenue.Text = Convert.ToString(reader["Venue"]);
                    txtPrice.Text = Convert.ToString(reader["Price"]);
                    txtSeats.Text = Convert.ToString(reader["MaxSeats"]);
                    txtDesc.Text = Convert.ToString(reader["Description"]);
                    txtPhone.Text = Convert.ToString(reader["ContactPhone"]);
                    ddlMode.SelectedValue = Convert.ToString(reader["EventMode"]);
                    txtLink.Text = Convert.ToString(reader["MeetingLink"]);
                    ddlType.SelectedValue = Convert.ToString(reader["EventType"]);
                    txtPrize.Text = reader["PrizePool"] == DBNull.Value ? string.Empty : Convert.ToString(reader["PrizePool"]);
                    EditingImagePath = Convert.ToString(reader["ImagePath"]);
                }
            }

            SetMeetingLinkVisibility();
            btnAdd.Text = "Update Event";
            btnCancelEdit.Visible = true;
            lblMsg.CssClass = "success";
            lblMsg.Text = "Edit mode enabled. Update details and click Update Event.";
        }

        // 🔥 DELETE EVENT
        protected void gvEvents_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int eventId = Convert.ToInt32(gvEvents.DataKeys[e.RowIndex].Value);

            try
            {
                using (SqlConnection con = DBHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_DeleteEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.CssClass = "success";
                lblMsg.Text = "Event deleted successfully.";
                LoadEvents(txtSearch.Text, ddlFilter.SelectedValue);
            }
            catch
            {
                lblMsg.CssClass = "error";
                lblMsg.Text = "Unable to delete event.";
            }
        }

        // 🔥 MODE CHANGE
        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMeetingLinkVisibility();
        }

        private void SetMeetingLinkVisibility()
        {
            bool isOnline = ddlMode.SelectedValue == "Online";
            txtLink.Enabled = isOnline;
            pnlMeetingLink.Visible = isOnline;
            if (!isOnline)
            {
                txtLink.Text = string.Empty;
            }
        }

        // 🔥 CLEAR FORM
        void ClearFields()
        {
            txtName.Text = "";
            txtDate.Text = "";
            txtVenue.Text = "";
            txtPrice.Text = "";
            txtSeats.Text = "";
            txtDesc.Text = "";
            txtPhone.Text = "";
            txtLink.Text = "";
            txtPrize.Text = "";
            EditingImagePath = string.Empty;
            ddlMode.SelectedValue = "";
            ddlType.SelectedValue = "";
            SetMeetingLinkVisibility();
        }

        private void ResetEditMode()
        {
            EditingEventId = null;
            EditingImagePath = string.Empty;
            btnAdd.Text = "Add Event";
            btnCancelEdit.Visible = false;
        }

        protected bool HasImage(object imagePath)
        {
            return !string.IsNullOrWhiteSpace(Convert.ToString(imagePath));
        }

        protected string ResolveImageUrl(object imagePath)
        {
            string path = Convert.ToString(imagePath);
            return string.IsNullOrWhiteSpace(path) ? string.Empty : ResolveUrl(path);
        }

        private string SaveBannerImage()
        {
            string extension = Path.GetExtension(fuBanner.FileName ?? string.Empty).ToLowerInvariant();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".webp")
            {
                lblMsg.Text = "Only JPG, PNG, or WEBP images are allowed.";
                return string.Empty;
            }

            if (fuBanner.PostedFile != null && fuBanner.PostedFile.ContentLength > (2 * 1024 * 1024))
            {
                lblMsg.Text = "Banner image size must be 2MB or less.";
                return string.Empty;
            }

            string folderVirtualPath = "~/Uploads/EventBanners/";
            string folderPhysicalPath = Server.MapPath(folderVirtualPath);
            if (!Directory.Exists(folderPhysicalPath))
            {
                Directory.CreateDirectory(folderPhysicalPath);
            }

            string fileName = "event_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;
            string fullPath = Path.Combine(folderPhysicalPath, fileName);
            fuBanner.SaveAs(fullPath);

            return folderVirtualPath + fileName;
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ResetEditMode();
            ClearFields();
            lblMsg.CssClass = "success";
            lblMsg.Text = "Update cancelled.";
        }

        protected void gvEvents_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvEvents.PageIndex = e.NewPageIndex;

            // maintain search + filter
            LoadEvents(txtSearch.Text, ddlFilter.SelectedValue);
        }

        private void NotifyUsersEventUpdated(SqlConnection con, int eventId, string eventName, DateTime eventDate,
            string venue, string eventMode, string meetingLink, decimal price)
        {
            using (SqlCommand notifyCmd = new SqlCommand("sp_GetEventNotificationUsers", con))
            {
                notifyCmd.CommandType = CommandType.StoredProcedure;
                notifyCmd.Parameters.AddWithValue("@EventID", eventId);

                using (SqlDataReader reader = notifyCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string userName = Convert.ToString(reader["UserName"]);
                        string email = Convert.ToString(reader["Email"]);

                        try
                        {
                            NotificationHelper.SendEventUpdatedEmail(email, userName, eventName, eventDate,
                                venue, eventMode, meetingLink, price);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}