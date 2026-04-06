<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="EventManagementSystem.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        .about-header {
            background: linear-gradient(135deg, #0056b3 0%, #007bff 100%);
            color: #ffffff;
            padding: 3rem 1rem;
            border-radius: 8px;
            margin-bottom: 2.5rem;
            margin-top: 1.5rem;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }
        .about-header h1 {
            font-weight: 700;
            color: #ffffff;
            margin-bottom: 0.5rem;
        }
        .about-header .lead {
            color: #e2e8f0;
            font-size: 1.25rem;
        }
        .feature-box {
            background-color: #f8f9fa;
            border: 1px solid #e9ecef;
            border-radius: 8px;
            padding: 2rem;
            height: 100%;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        }
        .feature-list {
            list-style: none;
            padding-left: 0;
            margin-bottom: 0;
        }
        .feature-list li {
            padding: 12px 0;
            border-bottom: 1px solid #dee2e6;
            font-size: 1.1em;
            color: #495057;
        }
        .feature-list li:last-child {
            border-bottom: none;
            padding-bottom: 0;
        }
        .feature-icon {
            color: #28a745;
            margin-right: 12px;
            font-weight: bold;
        }
        .content-section p {
            font-size: 1.1rem;
            line-height: 1.7;
            color: #495057;
        }
        .content-section h3 {
            color: #212529;
            font-weight: 600;
            margin-top: 1.5rem;
            margin-bottom: 1rem;
        }
        .content-section h3:first-child {
            margin-top: 0;
        }
    </style>

    <div class="container">
        
        <!-- Header Banner -->
        <div class="row">
            <div class="col-md-12 text-center about-header">
                <h1>About Event Management System</h1>
                <p class="lead">Simplifying event discovery, booking, and administration for everyone.</p>
            </div>
        </div>

        <!-- Main Content -->
        <div class="row" style="margin-bottom: 3rem;">
            
            <!-- Information Column -->
            <div class="col-md-6 content-section" style="margin-bottom: 2rem;">
                <div style="padding-right: 15px;">
                    <h3>Our Mission</h3>
                    <p>
                        Event Management System is built to streamline the entire lifecycle of an event. 
                        Users can easily explore upcoming events, reserve seats, and manage their bookings securely online.
                        Meanwhile, administrators are equipped with a powerful dashboard to publish details, track availability, and manage attendances.
                    </p>
                    
                    <h3>Purpose & Architecture</h3>
                    <p>
                        The goal of this project is to demonstrate a robust real-world web application workflow. 
                        It encompasses secure authentication, role-based access controls, interactive booking management, 
                        and streamlined database-driven CRUD operations.
                    </p>
                </div>
            </div>

            <!-- Features Column -->
            <div class="col-md-6" style="margin-bottom: 2rem;">
                <div class="feature-box">
                    <h3 style="color: #212529; font-weight: 600; margin-top: 0; margin-bottom: 1.5rem;">Key Features</h3>
                    <ul class="feature-list">
                        <li><span class="feature-icon">✓</span> User registration and secure login</li>
                        <li><span class="feature-icon">✓</span> Intuitive browsing and event search</li>
                        <li><span class="feature-icon">✓</span> Online & offline event support</li>
                        <li><span class="feature-icon">✓</span> Real-time seat availability tracking</li>
                        <li><span class="feature-icon">✓</span> Seamless booking and cancellation workflows</li>
                        <li><span class="feature-icon">✓</span> Centralized admin dashboard</li>
                    </ul>
                </div>
            </div>
            
        </div>
    </div>

</asp:Content>