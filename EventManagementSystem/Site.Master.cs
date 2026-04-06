using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace EventManagementSystem
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            Response.AppendHeader("Pragma", "no-cache");

            string path = Request.Url.AbsolutePath.ToLowerInvariant();

            if (Session["UserEmail"] != null)
            {
                string role = Session["Role"] == null ? string.Empty : Session["Role"].ToString();
                lblWelcome.Visible = false;
                lblWelcome.Text = string.Empty;
                mainContainer.Attributes["class"] = "container mt-4";
                lnkHome.Attributes["class"] = "nav-link";
                lnkMyBookings.Attributes["class"] = "nav-link";
                lnkContact.Attributes["class"] = "nav-link";
                lnkAdminDashboard.Attributes["class"] = "nav-link";
                lnkManageEvents.Attributes["class"] = "nav-link";
                lnkViewBookings.Attributes["class"] = "nav-link";
                btnLogout.CssClass = "nav-link";
                btnLogout.Text = "Logout";
                btnLogout.PostBackUrl = string.Empty;
                btnLogout.OnClientClick = "return confirm('Are you sure you want to logout?');";

                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    lnkBrand.HRef = ResolveUrl("~/Admin/ManageEvents.aspx");
                    liHome.Visible = false;
                    liMyBookings.Visible = false;
                    liContact.Visible = false;
                    liAdminDashboard.Visible = true;
                    liManageEvents.Visible = true;
                    liViewBookings.Visible = true;

                    if (path.Contains("/admin/dashboard")) lnkAdminDashboard.Attributes["class"] = "nav-link active";
                    else if (path.Contains("/admin/manageevents")) lnkManageEvents.Attributes["class"] = "nav-link active";
                    else if (path.Contains("/admin/viewbookings")) lnkViewBookings.Attributes["class"] = "nav-link active";
                    else if (path.Contains("/contact")) lnkContact.Attributes["class"] = "nav-link active";
                }
                else
                {
                    lnkBrand.HRef = ResolveUrl("~/User/Home.aspx");
                    lnkHome.InnerText = "Home";
                    lnkHome.HRef = ResolveUrl("~/User/Home.aspx");
                    lnkMyBookings.InnerText = "My Bookings";
                    lnkMyBookings.HRef = ResolveUrl("~/User/MyBookings.aspx");
                    lnkContact.InnerText = "Contact Us";
                    btnLogout.Visible = true;
                    liHome.Visible = true;
                    liMyBookings.Visible = true;
                    liContact.Visible = true;
                    liAdminDashboard.Visible = false;
                    liManageEvents.Visible = false;
                    liViewBookings.Visible = false;

                    if (path.Contains("/user/home")) lnkHome.Attributes["class"] = "nav-link active";
                    else if (path.Contains("/user/mybookings")) lnkMyBookings.Attributes["class"] = "nav-link active";
                    else if (path.Contains("/contact")) lnkContact.Attributes["class"] = "nav-link active";

                }
            }
            else
            {
                bool isPublicPage = path.EndsWith("/default.aspx") ||
                                    path.EndsWith("/default") ||
                                    path.EndsWith("/about.aspx") ||
                                    path.EndsWith("/about") ||
                                    path.EndsWith("/contact.aspx") ||
                                    path.EndsWith("/contact") ||
                                    path.EndsWith("/");

                lnkBrand.HRef = ResolveUrl("~/Default.aspx");
                liHome.Visible = true;
                liMyBookings.Visible = true;
                liContact.Visible = true;
                liAdminDashboard.Visible = false;
                liManageEvents.Visible = false;
                liViewBookings.Visible = false;

                lnkHome.InnerText = "Home";
                lnkHome.HRef = ResolveUrl("~/Default.aspx#home");
                lnkMyBookings.InnerText = "Events";
                lnkMyBookings.HRef = ResolveUrl("~/Default.aspx#events");
                lnkContact.InnerText = "Contact";
                lnkContact.HRef = ResolveUrl("~/Default.aspx#contact");

                lblWelcome.Visible = true;
                lblWelcome.Text = "Login";
                lblWelcome.CssClass = "nav-link nav-login-btn";

                btnLogout.Visible = true;
                btnLogout.Text = "Register";
                btnLogout.CssClass = "nav-link nav-register-btn";
                btnLogout.PostBackUrl = ResolveUrl("~/Auth/Register.aspx");
                btnLogout.OnClientClick = string.Empty;
                mainContainer.Attributes["class"] = path.EndsWith("/default.aspx") || path.EndsWith("/default") || path.EndsWith("/")
                    ? "landing-main-container"
                    : "container mt-4";

                if (!isPublicPage &&
                    !path.Contains("/auth/login") &&
                    !path.Contains("/auth/register") &&
                    !path.Contains("/auth/forgotpassword") &&
                    !path.Contains("/auth/resetpassword"))
                {
                    Response.Redirect("~/Auth/Login.aspx");
                }
            }
        }

        // 🔓 LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("~/Auth/Register.aspx");
                return;
            }

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpCookie cookie = new HttpCookie("ASP.NET_SessionId", string.Empty)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Path = "/"
                };
                Response.Cookies.Add(cookie);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            Response.AppendHeader("Pragma", "no-cache");

            Response.Redirect("~/Default.aspx");
        }
    }

}
