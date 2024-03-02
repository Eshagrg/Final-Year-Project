using Site.DataAccess.DBConn;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Site.DataAccess.Repository
{
    public class UserAuth_Repository : IUserAuth
    {
        private readonly ConnectionStrings _connection;

        public UserAuth_Repository(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }

        public string CheckEmailExist(string email)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Email", email);
                    string output = conn.ExecuteScalar<string>("USP_CheckEmailExist", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string CheckLogin(Login_VM obj)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Email", obj.Email);
                    param.Add("@Password", obj.Password);
                    string output = conn.ExecuteScalar<string>("USP_CheckLogin", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string DisableUserDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    string output = conn.ExecuteScalar<string>("USP_DisableUserDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string DisableMemberDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    string output = conn.ExecuteScalar<string>("USP_DisableMemberDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Portal_User GetUserData(Login_VM obj)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Email", obj.Email);
                    param.Add("@Password", obj.Password);
                    Portal_User output = conn.QueryFirstOrDefault<Portal_User>("USP_GetUserDetail", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Portal_User GetUserDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    Portal_User output = conn.QueryFirstOrDefault<Portal_User>("USP_GetUserDetailById", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Portal_User GetMemberDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    Portal_User output = conn.QueryFirstOrDefault<Portal_User>("USP_GetMemberDetailById", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Portal_User> GetUserList()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    IEnumerable<Portal_User> output = conn.Query<Portal_User>("USP_GetUserList", commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public IEnumerable<Portal_User> GetMemberList()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    IEnumerable<Portal_User> output = conn.Query<Portal_User>("USP_GetmemberList", commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SaveStaffData(AddStaff obj)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@FullName", obj.FullName);
                    param.Add("@Email", obj.Email);
                    param.Add("@Password", obj.Password);
                    param.Add("@PhoneNo", obj.PhoneNo);
                    param.Add("@RoleId", 2);
                    param.Add("@UploadFile", obj.UploadImage);
                    string output = conn.ExecuteScalar<string>("USP_SaveStaffDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SaveMemberData(AddMember_VM obj)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@FullName", obj.FullName);
                    param.Add("@Email", obj.Email);
                    param.Add("@PhoneNo", obj.PhoneNo);
                    param.Add("@RoleId", 3);
                    string output = conn.ExecuteScalar<string>("USP_SaveMemberDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string SaveUserData(Save_PortalUser obj)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@FullName", obj.FullName);
                    param.Add("@Email", obj.Email);
                    param.Add("@Password", obj.Password);
                    param.Add("@PhoneNo", obj.PhoneNo);
                    param.Add("@RoleId", obj.Role);
                    string output = conn.ExecuteScalar<string>("USP_SaveUserDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string VerifyUserDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    string output = conn.ExecuteScalar<string>("USP_VerifyUserDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string VerifyMemberDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    string output = conn.ExecuteScalar<string>("USP_VerifyMemberDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Portal_User> GetStaffList()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    IEnumerable<Portal_User> output = conn.Query<Portal_User>("USP_GetStaffList", commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public Portal_User GetStaffDetailsById(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    Portal_User output = conn.QueryFirstOrDefault<Portal_User>("USP_GetStaffDetailById", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateStaffDetail(Portal_User obj)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", obj.Id);
                    param.Add("@FullName", obj.FullName);
                    param.Add("@Email", obj.Email);
                    param.Add("@PhoneNo", obj.PhoneNo);
                    string output = conn.ExecuteScalar<string>("USP_UpdateStaffDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetUserHashedPassword(int userid)
        {
            using (var conn = new SqlConnection(_connection.DbConnection))
            {
                conn.Open();

                string query = "SELECT User_Password FROM Portal_Users WHERE Id = @Id";
                return conn.ExecuteScalar<string>(query, new { Id = userid });
            }
        }

        public bool UpdateUserPassword(int userid, string newHashedPassword)
        {
            using (var connection = new SqlConnection(_connection.DbConnection))
            {
                connection.Open();

                var parameters = new
                {
                    ProfileId = userid,
                    NewHashedPassword = newHashedPassword
                };

                // Call the stored procedure using Dapper's Execute method
                int rowsAffected = connection.Execute("dbo.USP_UpdateUserPassword", parameters, commandType: CommandType.StoredProcedure);

                // Check if the update was successful
                return rowsAffected > 0;
            }
        }

    }
}
