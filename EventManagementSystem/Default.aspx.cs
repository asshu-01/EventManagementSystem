using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventManagementSystem.BAL;
using EventManagementSystem.Entity;

namespace EventManagementSystem
{
    public partial class _Default : Page
    {
        private readonly EventBAL eventBal = new EventBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    List<EventEntity> events = eventBal.GetEvents();
                    int totalEvents = events.Count;

                    List<EventEntity> upcomingEvents = events
                        .Where(evt => evt.EventDate.Date >= DateTime.Today)
                        .OrderBy(evt => evt.EventDate)
                        .Take(3)
                        .ToList();

                    if (upcomingEvents.Count == 0)
                    {
                        upcomingEvents = events
                            .OrderBy(evt => evt.EventDate)
                            .Take(3)
                            .ToList();
                    }

                    rptUpcomingEvents.DataSource = upcomingEvents;
                    rptUpcomingEvents.DataBind();
                    pnlNoEvents.Visible = upcomingEvents.Count == 0;

                    Page.Title = "Home Page - " + totalEvents + " Events";
                }
                catch
                {
                    rptUpcomingEvents.DataSource = null;
                    rptUpcomingEvents.DataBind();
                    pnlNoEvents.Visible = true;
                    Page.Title = "Home Page";
                }
            }
        }

        protected string GetPreviewImageClass(int index)
        {
            if (index % 3 == 0) return "image-one";
            if (index % 3 == 1) return "image-two";
            return "image-three";
        }

        protected string GetPreviewImageCssClass(object imagePathObj, int index)
        {
            string resolvedUrl;
            return TryGetResolvedPreviewImageUrl(imagePathObj, out resolvedUrl) ? string.Empty : GetPreviewImageClass(index);
        }

        protected bool HasPreviewImage(object imagePathObj)
        {
            string resolvedUrl;
            return TryGetResolvedPreviewImageUrl(imagePathObj, out resolvedUrl);
        }

        protected string GetPreviewImageUrl(object imagePathObj)
        {
            string resolvedUrl;
            return TryGetResolvedPreviewImageUrl(imagePathObj, out resolvedUrl) ? resolvedUrl : string.Empty;
        }

        private bool TryGetResolvedPreviewImageUrl(object imagePathObj, out string resolvedUrl)
        {
            resolvedUrl = string.Empty;

            string imagePath = Convert.ToString(imagePathObj);
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return false;
            }

            imagePath = imagePath.Trim().Replace("\\", "/");

            if (imagePath.IndexOf(":/", StringComparison.OrdinalIgnoreCase) > 0)
            {
                string appRoot = Server.MapPath("~/").Replace("\\", "/");
                if (!appRoot.EndsWith("/"))
                {
                    appRoot += "/";
                }

                if (!imagePath.StartsWith(appRoot, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                imagePath = "~/" + imagePath.Substring(appRoot.Length).TrimStart('/');
            }
            else
            {
                if (imagePath.StartsWith("/"))
                {
                    imagePath = "~" + imagePath;
                }
                else if (!imagePath.StartsWith("~/", StringComparison.Ordinal))
                {
                    imagePath = "~/" + imagePath.TrimStart('/');
                }
            }

            string physicalPath = Server.MapPath(imagePath);
            if (!File.Exists(physicalPath))
            {
                return false;
            }

            resolvedUrl = ResolveClientUrl(imagePath).Replace("'", "%27");
            return true;
        }
    }
}