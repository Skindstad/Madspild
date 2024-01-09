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
    internal class GoodsRepository : Repository, IEnumerable<Goods>
    {
        private List<Goods> list = [];

        public IEnumerator<Goods> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string name, string price, string amount, string limit, string path)
        {
            Add(new Goods("", name, price, amount, limit, path));
        }

        public void Search(string name, string price)
        {
            try
            {
                // Join Category On Goods.Category = Category.Id  Where ProductName LIKE @Name AND Price LIKE @Price AND Category LIKE @Category
                SqlCommand cmd = new("Select Id, ProductName, Price, Amount, AmountLimit, PicturePath From Goods Where ProductName LIKE @Name AND Price LIKE @Price", connection);
                cmd.Parameters.Add(CreateParam("@Name", name + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Price", price + "%", SqlDbType.NVarChar));
               // cmd.Parameters.Add(CreateParam("@Category", category + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Goods(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Goods);

            }
            catch (Exception ex)
            {
                throw new DbException("Error in Goods repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(Goods product)
        {
            string error = "";
            if (product.IsValid)
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Goods (ProductName, Price, Amount, AmountLimit, PicturePath) VALUES ( @Name, @Price, @Amount, @Limit, @Path)", connection);
                    command.Parameters.Add(CreateParam("@Name", product.Name, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Price", product.Price, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Amount", product.Amount, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Limit", product.AmountLimit, SqlDbType.NVarChar));
                    //command.Parameters.Add(CreateParam("@Category", product.Category, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Path", product.Path, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        list.Add(product);
                        list.Sort();
                        OnChanged(DbOperation.INSERT, DbModeltype.Goods);
                        return;
                    }
                    error = string.Format("Product could not be inserted in the database");
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
            else error = "Illegal value for Product";
            throw new DbException("Error in Goods repositiory: " + error);
        }

        public void Update(string name, string price, string amount, string limit, string path)
        {
            Update(new Goods("", name, price, amount, limit, path));
        }
        public void Update(Goods product)
        {
            string error = "";
            if (product.IsValid)
            {
                try
                {
                    string id = GetId(product.Name);
                    SqlCommand command = new SqlCommand("UPDATE Users SET ProductName = @Name, Price = @Price, Amount = @Amount, AmountLimit = @Limit, PicturePath = @Path WHERE Id = @Id", connection);
                    command.Parameters.Add(CreateParam("@Name", product.Name, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Price", product.Price, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Amount", product.Amount, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Limit", product.AmountLimit, SqlDbType.NVarChar));
                    //command.Parameters.Add(CreateParam("@Category", product.Category, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Path", product.Path, SqlDbType.NVarChar));
                    command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        OnChanged(DbOperation.UPDATE, DbModeltype.Goods);
                        return;
                    }
                    error = string.Format("User {0} could not be update", product.Name);
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
            else error = "Illegal value for product";
            throw new DbException("Error in Goods repositiory: " + error);
        }

        public void Remove(string name)
        {
            string error = "";
            try
            {
                string id = GetId(name);
                SqlCommand command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                connection.Open();
                command.ExecuteNonQuery();
                if (command.ExecuteNonQuery() == 1)
                {
                    list.Remove(new Goods(id, name, "", "", "", ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Users);
                    return;
                }
                error = string.Format("Product {0} could not be deleted", name);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Product repositiory: " + error);
        }
        public static string GetId(string name)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Id FROM Goods WHERE ProductName = @Name", connection);
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
        public static string GetLimit(string id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT AmountLimit FROM Goods WHERE Id = @Id", connection);
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

        public static string GetPrice(string id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Price FROM Goods WHERE Id = @Id", connection);
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
    }
}
