using System;
using System.Data;
using EventManagementSystem.DAL;
using EventManagementSystem.Entity;
using EventManagementSystem.Security;

namespace EventManagementSystem.BAL
{
    public class UserBAL
    {
        UserDAL dal = new UserDAL();

        public bool UserExists(string email)
        {
            return dal.UserExists(email);
        }

        public DataTable Login(string email, string password)
        {
            DataTable dt = dal.Login(email);

            if (dt.Rows.Count == 0)
                return null;

            string dbPassword = dt.Rows[0]["Password"].ToString();

            if (!PasswordHasher.Verify(password, dbPassword))
                return null;

            DataRow row = dt.Rows[0];
            var user = new UserEntity
            {
                UserID = HasColumn(row, "UserID") && row["UserID"] != DBNull.Value ? Convert.ToInt32(row["UserID"]) : 0,
                Name = HasColumn(row, "Name") ? Convert.ToString(row["Name"]) : string.Empty,
                Email = HasColumn(row, "Email") ? Convert.ToString(row["Email"]) : string.Empty,
                Password = HasColumn(row, "Password") ? Convert.ToString(row["Password"]) : string.Empty,
                Role = HasColumn(row, "Role") ? Convert.ToString(row["Role"]) : "User",
                FailedAttempts = HasColumn(row, "FailedAttempts") && row["FailedAttempts"] != DBNull.Value ? Convert.ToInt32(row["FailedAttempts"]) : 0,
                IsLocked = HasColumn(row, "IsLocked") && row["IsLocked"] != DBNull.Value && Convert.ToBoolean(row["IsLocked"]),
                ResetToken = HasColumn(row, "ResetToken") ? Convert.ToString(row["ResetToken"]) : string.Empty,
                TokenExpiry = HasColumn(row, "TokenExpiry") && row["TokenExpiry"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["TokenExpiry"]) : null
            };

            return dt;
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName);
        }
    }
}