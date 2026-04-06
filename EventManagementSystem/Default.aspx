<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EventManagementSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="saas-landing">
        <section class="saas-hero" id="home">
            <div class="hero-shape hero-shape-1"></div>
            <div class="hero-shape hero-shape-2"></div>

            <div class="saas-container hero-grid">
                <div class="hero-content">
                    <p class="hero-tag">Trusted by teams for modern event operations</p>
                    <h1>Plan, Manage, and Experience Events Seamlessly</h1>
                    <p class="hero-subtitle">All-in-one platform to create, organize, and book events effortlessly.</p>

                    <div class="hero-actions">
                        <a href="<%= ResolveUrl("~/Auth/Register.aspx") %>" class="saas-btn saas-btn-primary">Get Started</a>
                        <a href="#events" class="saas-btn saas-btn-outline">Explore Events</a>
                    </div>
                </div>

                <div class="hero-preview">
                    <div class="preview-card glass-card">
                        <h3>Event Dashboard</h3>
                        <div class="preview-metrics">
                            <div>
                                <span>Total Events</span>
                                <strong>128</strong>
                            </div>
                            <div>
                                <span>Active Bookings</span>
                                <strong>2,640</strong>
                            </div>
                            <div>
                                <span>Revenue</span>
                                <strong>$48.5K</strong>
                            </div>
                        </div>
                        <ul class="preview-list">
                            <li><span>Corporate Summit 2026</span><em>Open</em></li>
                            <li><span>Design & Product Expo</span><em>Filling Fast</em></li>
                            <li><span>Startup Investor Meetup</span><em>Open</em></li>
                        </ul>
                    </div>
                </div>
            </div>
        </section>

        <section class="saas-section" id="features">
            <div class="saas-container">
                <div class="section-header">
                    <h2>Built for every event workflow</h2>
                    <p>Powerful features that help organizers, teams, and attendees stay aligned and efficient.</p>
                </div>

                <div class="features-grid">
                    <article class="feature-card glass-card">
                        <span class="feature-icon">&#128197;</span>
                        <h3>Smart Event Scheduling</h3>
                        <p>Create timelines, manage slots, and avoid conflicts with intelligent schedule handling.</p>
                    </article>

                    <article class="feature-card glass-card">
                        <span class="feature-icon">&#127903;</span>
                        <h3>Easy Ticket Booking</h3>
                        <p>Enable fast and secure bookings with a smooth user flow from browsing to confirmation.</p>
                    </article>

                    <article class="feature-card glass-card">
                        <span class="feature-icon">&#9881;</span>
                        <h3>Admin Control Panel</h3>
                        <p>Manage events, users, and bookings from a centralized dashboard built for productivity.</p>
                    </article>

                    <article class="feature-card glass-card">
                        <span class="feature-icon">&#128200;</span>
                        <h3>Real-time Insights</h3>
                        <p>Track attendance, booking trends, and performance with instant reporting.</p>
                    </article>
                </div>
            </div>
        </section>

        <section class="saas-section" id="events">
            <div class="saas-container">
                <div class="section-header">
                    <h2>Upcoming events preview</h2>
                    <p>Discover events your audience can book right away.</p>
                </div>

                <div class="events-grid-preview">
                    <asp:Repeater ID="rptUpcomingEvents" runat="server">
                        <ItemTemplate>
                            <article class="event-preview-card glass-card">
                                <div class='event-image <%# GetPreviewImageCssClass(Eval("ImagePath"), Container.ItemIndex) %>'>
                                    <asp:Image ID="imgPreviewBanner" runat="server"
                                        ImageUrl='<%# GetPreviewImageUrl(Eval("ImagePath")) %>'
                                        Visible='<%# HasPreviewImage(Eval("ImagePath")) %>'
                                        CssClass="event-banner-img" />
                                </div>
                                <div class="event-content">
                                    <h3><%# Server.HtmlEncode(Eval("EventName").ToString()) %></h3>
                                    <p><%# Convert.ToDateTime(Eval("EventDate")).ToString("MMMM dd, yyyy") %></p>
                                    <a href="<%= ResolveUrl("~/Auth/Login.aspx") %>" class="saas-btn saas-btn-primary">Book Now</a>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Panel ID="pnlNoEvents" runat="server" Visible="false">
                        <article class="event-preview-card glass-card">
                            <div class="event-image image-one"></div>
                            <div class="event-content">
                                <h3>No events available</h3>
                                <p>Please check back soon.</p>
                                <a href="<%= ResolveUrl("~/Auth/Login.aspx") %>" class="saas-btn saas-btn-primary">Book Now</a>
                            </div>
                        </article>
                    </asp:Panel>
                </div>
            </div>
        </section>

        <section class="saas-section" id="about">
            <div class="saas-container">
                <div class="section-header">
                    <h2>About Event Management System</h2>
                    <p>A reliable SaaS platform designed to help organizations deliver events with confidence.</p>
                </div>

                <div class="about-grid">
                    <article class="glass-card about-card">
                        <h3>Why teams choose us</h3>
                        <p>From planning to booking and analytics, every workflow is designed for speed, clarity, and control.</p>
                        <ul>
                            <li>Secure user and booking workflows</li>
                            <li>Admin-first controls with role-based access</li>
                            <li>Clear insights to optimize performance</li>
                        </ul>
                    </article>

                    <article class="glass-card about-card">
                        <h3>Built for growth</h3>
                        <p>Whether you are running internal events or public conferences, the platform scales with your operations.</p>
                        <div class="about-stats">
                            <div><strong>99.9%</strong><span>Availability</span></div>
                            <div><strong>24/7</strong><span>Support</span></div>
                            <div><strong>10k+</strong><span>Monthly Bookings</span></div>
                        </div>
                    </article>
                </div>
            </div>
        </section>

        <section class="saas-section" id="contact">
            <div class="saas-container">
                <div class="section-header">
                    <h2>Contact us</h2>
                    <p>Have questions or need help onboarding your team? We are here to help.</p>
                </div>

                <div class="contact-two-grid">
                    <article class="glass-card contact-card contact-details-card">
                        <h3>Contact Details</h3>
                        <p class="contact-intro">Reach our team for support, onboarding, or partnership requests.</p>
                        <div class="contact-lines">
                            <div><span>Email Support</span><strong>support@eventmanagementsystem.com</strong></div>
                            <div><span>Sales</span><strong>sales@eventmanagementsystem.com</strong></div>
                            <div><span>Phone</span><strong>+1 (800) 555-2048</strong></div>
                            <div><span>Working Hours</span><strong>Mon - Fri, 9:00 AM to 6:00 PM</strong></div>
                        </div>
                    </article>

                    <article class="glass-card contact-card contact-form-card">
                        <h3>Quick Inquiry Form</h3>
                        <p>Share your requirement and our team will contact you quickly.</p>
                        <div class="landing-form">
                            <input type="text" placeholder="Your name" />
                            <input type="email" placeholder="Work email" />
                            <textarea rows="4" placeholder="Tell us what you need"></textarea>
                            <button type="button" class="saas-btn saas-btn-primary">Submit Request</button>
                        </div>
                    </article>
                </div>
            </div>
        </section>

        <section class="saas-cta">
            <div class="saas-container cta-inner glass-card">
                <h2>Start managing your events today</h2>
                <a href="<%= ResolveUrl("~/Auth/Register.aspx") %>" class="saas-btn saas-btn-primary">Create Event</a>
            </div>
        </section>
    </main>

</asp:Content>
