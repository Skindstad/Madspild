using Madspild.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madspild.DataAccess
{
    // Lavet af Jakob
    public class UserRepository : Repository, IEnumerable<User>
    {
        private List<User> list = [];

        public IEnumerator<User> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string name, string email, string hPhone, string wPhone, string address, string code)
        {
            try
            {
                SqlCommand cmd = new ("Select Id, PersonName, Email, AccessName, HomePhone, WorkPhone, Address, Zipcode, City From Users Join AccessRight On Users.AccessRight = AccessRight.Id Join Zipcodes On Users.Zipcode = Zipcodes.Code Where PersonName LIKE @Name AND Email LIKE @Email AND HomePhone LIKE @HPhone AND WorkPhone LIKE WPhone AND Address LIKE @Address AND Zipcode LIKE @Code", connection);
                cmd.Parameters.Add(CreateParam("@Name", name + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Email", email + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@HPhone", hPhone + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@WPhone", wPhone + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Address", address + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Code", code + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new User(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), "", reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Users);

            }
            catch (Exception ex)
            {
                throw new DbException("Error in User repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public List<string> Login(string email, string password)
        {
            List<string> user = [];
            try
            {
                SqlCommand cmd = new("Select Users.Id, PersonName, Email, AccessName, HomePhone, WorkPhone, Address, Zipcode, City From Users Join AccessRight On Users.AccessRight = AccessRight.Id Join Zipcodes On Users.Zipcode = Zipcodes.Code Where Email = @Email AND Password = @Password", connection);
                SqlParameter param = new SqlParameter("@Email", SqlDbType.NVarChar);
                SqlParameter param1 = new SqlParameter("@Password", SqlDbType.NVarChar);
                param.Value = email;
                param1.Value = password;
                cmd.Parameters.Add(param);
                cmd.Parameters.Add(param1);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                user.Clear();
                if (reader.Read())
                {
                    user.Add(reader[0].ToString()); user.Add(reader[1].ToString()); user.Add(reader[2].ToString()); user.Add(reader[3].ToString()); user.Add(reader[4].ToString()); user.Add(reader[5].ToString()); user.Add(reader[6].ToString()); user.Add(reader[7].ToString()); user.Add(reader[8].ToString());
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new DbException("Error in User repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return null;
        }

        public void Add(User user)
        {
            string error = "";
            if (user.IsValid)
            {
                try
                {
                    string accessId = AccessRepository.GetId(user.AccessRight);
                    SqlCommand command = new SqlCommand("INSERT INTO Users ( Email, Password, AccessRight, PersonName, HomePhone, WorkPhone, Address, Zipcode) VALUES ( @Email, @Password, @AccessRight, @PersonName, @HomePhone, @WorkPhone, @Address, @Zipcode)", connection);
                    command.Parameters.Add(CreateParam("@Email", user.Email, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Password", user.Password, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@AccessRight", accessId, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@PersonName", user.Name, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@HomePhone", user.HomePhone, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@WorkPhone", user.WorkPhone, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Address", user.Address, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Zipcode", user.Zipcode, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        user.City = ZipcodeRepository.GetCity(user.Zipcode);
                        list.Add(user);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Users);
                        return;
                    }
                    error = string.Format("Id could not be inserted in the database");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
            else error = "Illegal value for Id";
            throw new DbException("Error in User repositiory: " + error);
        }
        public void Update(User user)
        {
            string error = "";
            if (user.IsValid)
            {
                try
                {
                    string id = GetId(user.Email);
                    string accessId = AccessRepository.GetId(user.AccessRight);
                    SqlCommand command = new SqlCommand("UPDATE Users SET Email = @Email, AccessRight = @Access, PersonName = @Name, HomePhone = @HPhone, WorkPhone = @WPhone, Address = @Address, Zipcode = @code WHERE Id = @Id", connection);
                    command.Parameters.Add(CreateParam("@Email", user.Email, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Access", accessId, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Name", user.Name, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@HPhone", user.HomePhone, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@WPhone", user.WorkPhone, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Address", user.Address, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Zipcode", user.Zipcode, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        for (int i = 0; i < list.Count; ++i)
                            if (list[i].Id.Equals(id))
                            {
                                list[i].City = ZipcodeRepository.GetCity(user.Zipcode);
                                break;
                            }
                        OnChanged(DbOperation.UPDATE, DbModeltype.Users);
                        return;
                    }
                    error = string.Format("User {0} could not be update", user.Email);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
            else error = "Illegal value for user";
            throw new DbException("Error in user repositiory: " + error);
        }

        public void Remove(string email)
        {
            string error = "";
            try
            {
                string id = GetId(email);
                SqlCommand command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                connection.Open();
                command.ExecuteNonQuery();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new User(id, "", email, "", "", "", "", "", "", ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Users);
                    return;
                }
                error = string.Format("User {0} could not be deleted", email);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in User repositiory: " + error);
        }
        public static string GetId(string Email)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Id FROM Users WHERE Email = @Email", connection);
                SqlParameter param = new SqlParameter("@Email", SqlDbType.NVarChar);
                param.Value = Email;
                command.Parameters.Add(param);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) return reader[0].ToString();
            }
            catch
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return "";
        }
    }

}
