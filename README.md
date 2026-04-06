# Event Management System

Role-based ASP.NET Web Forms project for event listing, booking, and admin management.

## What is currently in this repository

### Framework & stack
- `.NET Framework 4.7.2`
- `ASP.NET Web Forms`
- `C#`
- `SQL Server` (`System.Data.SqlClient`)
- `Bootstrap 5`, `jQuery`

### Main modules
- `Auth/` → `Login`, `Register`, `ForgotPassword`, `ResetPassword`
- `User/` → `Home`, `EventDetails`, `Booking`, `MyBookings`
- `Admin/` → `Dashboard`, `ManageEvents`, `ViewBookings`
- `BAL/`, `DAL/`, `Entity/`
- `Security/PasswordHasher.cs`
- `Utilities/NotificationHelper.cs`
- `Site.Master` + `Site.Master.cs`

### Database-related files present
- `EventManagementSystem/Database/EventManagementDB_Master.sql` (database setup script)

### Database runtime location
- The actual running database is in your **SQL Server instance** (not inside the project folder).
- Project file only stores the SQL script; execution/data live in SQL Server.

## Features currently implemented

### Public
- Landing page with upcoming events
- About page
- Contact page

### User
- Register and login
- Browse/search/filter events
- Book event seats
- View and cancel own bookings
- Booking summary

### Admin
- Admin login using `AdminId` + `AdminSpecialKey` from `Web.config`
- Dashboard stats
- Add/update/delete events
- Review bookings (accept/cancel)

## Stored procedures currently used by code

- `sp_UserExists`
- `sp_UserLogin`
- `sp_RegisterUser`
- `sp_SetResetOtp`
- `sp_ResetPasswordWithOtp`
- `sp_GetEvents`
- `sp_GetEventById`
- `sp_AddEvent`
- `sp_UpdateEvent`
- `sp_DeleteEvent`
- `sp_GetEventNotificationUsers`
- `sp_BookEvent`
- `sp_GetUserBookings`
- `sp_GetUserBookingSummary`
- `sp_UserCancelBooking`
- `sp_GetAdminBookings`
- `sp_GetBookingNotificationDetails`
- `sp_AdminAcceptBooking`
- `sp_AdminCancelBooking`
- `sp_SaveContactMessage`
- `sp_AdminStats`

## Local run steps (current project)

1. Open project in Visual Studio.
2. Ensure SQL database and required stored procedures exist.
3. Update `EventManagementSystem/Web.config`:
   - `connectionStrings:DBCS`
   - `appSettings:Email`
   - `appSettings:Password`
   - `appSettings:ContactToEmail`
   - `appSettings:AdminId`
   - `appSettings:AdminSpecialKey`
4. Build and run.

## Notes

- The project builds successfully in the current workspace.
- This repository is prepared for backend implementation, not final production deployment.
