using Madspild.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madspild.DataAccess
{
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
                SqlCommand cmd = new ("Select Id, PersonName, Email, AccessName, HomePhone, WorkPhone, Address, Zipcode, City From Users Join AccessRight On Users.AccessRight = AccessRight.Id Join Zipcodes On Users.Zipcode = Zipcodes.Code Where Name LIKE @Name AND Email LIKE @Email AND HomePhone LIKE @HPhone AND WorkPhone LIKE WPhone AND Address LIKE @Address AND Zipcode LIKE @Code", connection);
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
                OnChanged(DbOperation.SELECT, DbModeltype.User);

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
    }
}
