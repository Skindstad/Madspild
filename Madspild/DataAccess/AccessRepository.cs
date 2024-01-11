using Madspild.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madspild.DataAccess
{
    // Lavet af Jakob
    public class AccessRepository : Repository, IEnumerable<Access>
    {
        private List<Access> list = [];

        public IEnumerator<Access> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string name)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT Id, AccessName FROM Access WHERE AccessName LIKE @Name", connection);
                command.Parameters.Add(CreateParam("@Name", name + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Access(reader[0].ToString(), reader[1].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Access);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in Access repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(string name)
        {
            string error = "";
            name = name.Trim();
            Access access = new Access("", name);
            if (access.IsValid)
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Access (AccessName) VALUES (@Name)", connection);
                    command.Parameters.Add(CreateParam("@Name", name, SqlDbType.NVarChar));

                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Add(access);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Access);
                        return;
                    }
                    error = string.Format("{0} could not be inserted in the database", name);
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
            else error = "Illegal value for Name";
            throw new DbException("Error in Access repositiory: " + error);
        }

        public void Update(string name)
        {
            string error = "";
            name = name.Trim();
            if (name.Length > 0)
            {
                try
                {
                    string id = GetId(name);
                    SqlCommand command = new SqlCommand("UPDATE Access SET AccessName = @Name WHERE Id = @Id", connection);
                    command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Name", name, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        for (int i = 0; i < list.Count; ++i)
                            if (list[i].Id.Equals(id))
                            {
                                list[i].Name = name;
                                break;
                            }
                        OnChanged(DbOperation.UPDATE, DbModeltype.Access);
                        return;
                    }
                    error = string.Format("Access {0} could not be updated", id);
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
            else error = "Illegal value for name";
            throw new DbException("Error in Access repositiory: " + error);
        }

        public void Remove(string name)
        {
            string error = "";
            try
            {
                string id = GetId(name);
                SqlCommand command = new SqlCommand("DELETE FROM Access WHERE Id = @Id", connection);
                command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                connection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Access(id, ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Access);
                    return;
                }
                error = string.Format("Access{0} could not be deleted", id);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Access repositiory: " + error);
        }

        public static string GetName(string id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT AccessName FROM Access WHERE Id = @Id", connection);
                SqlParameter param = new SqlParameter("@Id", SqlDbType.NVarChar);
                param.Value = id;
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

        public static string GetId(string name)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Id FROM Access WHERE AccessName = @Name", connection);
                SqlParameter param = new SqlParameter("@Name", SqlDbType.NVarChar);
                param.Value = name;
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
