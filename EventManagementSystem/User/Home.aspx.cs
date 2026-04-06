using EventManagementSystem.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EventManagementSystem.User
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            string userName = Convert.ToString(Session["UserName"]);
            if (string.IsNullOrWhiteSpace(userName))
                userName = Convert.ToString(Session["UserEmail"]);

            if (!string.IsNullOrWhiteSpace(userName) && userName.StartsWith("Welcome:", StringComparison.OrdinalIgnoreCase))
                userName = userName.Substring("Welcome:".Length).Trim();

            if (!string.IsNullOrWhiteSpace(userName) && userName.EndsWith("(User)", StringComparison.OrdinalIgnoreCase))
                userName = userName.Substring(0, userName.Length - "(User)".Length).Trim();

            lblUser.Text = userName;

            if (!IsPostBack)
            {
                LoadEvents();
            }
        }

        // 🔥 LOAD DEFAULT EVENTS
        void LoadEvents()
        {
            LoadFilteredEvents();
        }

        // 🔍 SEARCH + FILTER
        protected void Search_Click(object sender, EventArgs e)
        {
            gvEvents.PageIndex = 0; // reset page
            LoadFilteredEvents();
        }

        void LoadFilteredEvents()
        {
            using (SqlConnection con = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetEvents", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (!dt.Columns.Contains("EventStatus"))
                    {
                        dt.Columns.Add("EventStatus", typeof(string));
                    }

                    string searchText = (txtSearch.Text ?? string.Empty).Trim();
                    foreach (DataRow row in dt.Rows)
                    {
                        int availableSeats = row["AvailableSeats"] == DBNull.Value ? 0 : Convert.ToInt32(row["AvailableSeats"]);
                        DateTime eventDate = row["EventDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["EventDate"]);
                        row["EventStatus"] = availableSeats == 0 ? "FULL" : (eventDate < DateTime.Now ? "CLOSED" : "OPEN");
                    }

                    string filter = ddlFilter.SelectedValue;
                    string rowFilter = "1=1";
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        string escaped = searchText.Replace("'", "''");
                        rowFilter += " AND Convert(EventName, 'System.String') LIKE '%" + escaped + "%'";
                    }

                    if (filter == "open")
                    {
                        rowFilter += " AND AvailableSeats > 0 AND EventDate >= #" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "#";
                    }
                    else if (filter == "low")
                    {
                        rowFilter += " AND AvailableSeats <= 5 AND AvailableSeats > 0";
                    }

                    DataView view = dt.DefaultView;
                    view.RowFilter = rowFilter;
                    DataTable filtered = view.ToTable();

                    FormatDates(filtered);

                    gvEvents.DataSource = filtered;
                    gvEvents.DataBind();

                    lblTotalEvents.Text = filtered.Rows.Count.ToString();
                    lblMsg.Text = filtered.Rows.Count == 0 ? "No events found." : "";
                }
            }
        }

        // 📄 PAGINATION
        protected void gvEvents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEvents.PageIndex = e.NewPageIndex;
            LoadFilteredEvents();
        }

        // 🎨 STATUS COLOR
        protected void gvEvents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string status = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "EventStatus"));
            int availableSeats = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AvailableSeats"));
            var lblStatus = e.Row.FindControl("lblStatus") as Label;

            if (lblStatus == null) return;

            lblStatus.CssClass = "status-badge";

            if (status == "OPEN")
            {
                if (availableSeats <= 10)
                {
                    lblStatus.Text = $"{availableSeats} Left";
                    lblStatus.CssClass += " status-full";
                }
                else
                {
                    lblStatus.CssClass += " status-open";
                }
            }
            else if (status == "FULL")
            {
                lblStatus.CssClass += " status-full";
            }
            else
            {
                lblStatus.CssClass += " status-closed";
            }
        }

        // 🔗 NAVIGATION
        protected void gvEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Details")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/User/EventDetails.aspx?EventID=" + id);
            }
        }

        // 📅 DATE FORMAT
        void FormatDates(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                DateTime d;
                if (DateTime.TryParse(row["EventDate"].ToString(), out d))
                    row["EventDate"] = d.ToString("dd MMM yyyy");
            }
        }

        // 🔓 LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Auth/Login.aspx");
        }
    }
}