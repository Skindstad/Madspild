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
    // Taget fra ContaktsEditor
    public class ZipcodeRepository : Repository, IEnumerable<Zipcode>
    {
        private List<Zipcode> list = new List<Zipcode>();

        public IEnumerator<Zipcode> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string code, string city)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Zipcodes WHERE Code LIKE @Code AND City LIKE @City", connection);
                command.Parameters.Add(CreateParam("@Code", code + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@City", city + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Zipcode(reader[0].ToString(), reader[1].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Zipcodes);
            }
            catch (Exception ex)
            {
                throw new DbException("Error in Zipcode repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(string code, string city)
        {
            string error = "";
            city = city.Trim();
            Zipcode zipcode = new Zipcode(code, city);
            if (zipcode.IsValid)
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Zipcodes (code, city) VALUES (@Code, @City)", connection);
                    command.Parameters.Add(CreateParam("@Code", code, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@City", city, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Add(zipcode);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Zipcodes);
                        return;
                    }
                    error = string.Format("{0} {1} could not be inserted in the database", code, city);
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
            else error = "Illegal value for code or city";
            throw new DbException("Error in Zipcode repositiory: " + error);
        }

        public void Update(string code, string city)
        {
            string error = "";
            city = city.Trim();
            if (city.Length > 0)
            {
                try
                {
                    SqlCommand command = new SqlCommand("UPDATE Zipcodes SET City = @City WHERE code = @Code", connection);
                    command.Parameters.Add(CreateParam("@Code", code, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@City", city, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        for (int i = 0; i < list.Count; ++i)
                            if (list[i].Code.Equals(code))
                            {
                                list[i].City = city;
                                break;
                            }
                        OnChanged(DbOperation.UPDATE, DbModeltype.Zipcodes);
                        return;
                    }
                    error = string.Format("Zipcode {0} could not be updated", code);
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
            else error = "Illegal value for city";
            throw new DbException("Error in Zipcode repositiory: " + error);
        }

        public void Remove(string code)
        {
            string error = "";
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM Zipcodes WHERE Code = @Code", connection);
                command.Parameters.Add(CreateParam("@Code", code, SqlDbType.NVarChar));
                connection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Zipcode(code, ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Zipcodes);
                    return;
                }
                error = string.Format("Zipcode {0} could not be deleted", code);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Zipcode repositiory: " + error);
        }

        public static string GetCity(string code)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT City FROM Zipcodes WHERE Code = @Code", connection);
                SqlParameter param = new SqlParameter("@Code", SqlDbType.NVarChar);
                param.Value = code;
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

        public static string GetCode(string city)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Code FROM Zipcodes WHERE City = @City", connection);
                SqlParameter param = new SqlParameter("@City", SqlDbType.NVarChar);
                param.Value = city;
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
