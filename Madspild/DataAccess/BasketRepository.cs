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
    public class BasketRepository : Repository, IEnumerable<Basket>
    {
        private List<Basket> list = [];

        public IEnumerator<Basket> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string personEmail, string productName, string amount)
        {
            try
            {
                SqlCommand command = new("Select Basket.Id, Email, ProductName, Basket.Amount, Basket.Price, BasketDato, BoughtDato From Basket Join Users On Users.Id = Basket.PersonId Join Goods On Goods.Id = Basket.ProductId Where Email LIKE @Email AND ProductName LIKE @Name AND Basket.Amount LIKE @Amount AND Bought = 'false'", connection);
                command.Parameters.Add(CreateParam("@Name", productName + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Email", personEmail + "%", SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Amount", amount + "%", SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Basket(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Basket);

            }
            catch (Exception ex)
            {
                throw new DbException("Error in Basket repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Bought(string personEmail)
        {
            try
            {
                SqlCommand command = new("Select Basket.Id, Email, ProductName, Basket.Amount, Basket.Price, BasketDato, BoughtDato From Basket Join Users On Users.Id = Basket.PersonId Join Goods On Goods.Id = Basket.ProductId Where Email = @Email AND Bought = 'true'", connection);
                command.Parameters.Add(CreateParam("@Email", personEmail, SqlDbType.NVarChar));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                list.Clear();
                while (reader.Read()) list.Add(new Basket(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString()));
                OnChanged(DbOperation.SELECT, DbModeltype.Basket);

            }
            catch (Exception ex)
            {
                throw new DbException("Error in Basket repositiory: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        public void Add(string email, string productName, string amount)
        {
            Add(new Basket("", email, productName, amount, "", "", ""));
        }


        public void Add(Basket basket)
        {
            string error = "";
            if (basket.IsValid)
            {
                try
                {
                    string personId = UserRepository.GetId(basket.PersonEmail);
                    string productId = GoodsRepository.GetId(basket.ProductName);
                    string limit = GoodsRepository.GetLimit(productId);
                    if (int.Parse(basket.Amount) > int.Parse(limit))
                    {
                        error = string.Format("Amount is Higher end the limit");
                    }
                    else
                    {
                        string price = GoodsRepository.GetPrice(productId);
                        float sumPrice = float.Parse(price) * int.Parse(basket.Amount); 
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                        SqlCommand command = new SqlCommand("INSERT INTO Basket (PersonId, ProductId, Price, Amount, BasketDato) VALUES (@PersonId, @ProductId, @Price, @Amount, @BasketDato)", connection);
                        command.Parameters.Add(CreateParam("@PersonId", personId, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@ProductId", productId, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@Price", sumPrice, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@Amount", basket.Amount, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@BasketDato", date, SqlDbType.NVarChar));
                        connection.Open();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            basket.BacketDato = date;
                            list.Add(basket);
                            list.Sort();
                            OnChanged(DbOperation.INSERT, DbModeltype.Basket);
                            return;
                        }
                        error = string.Format("Id could not be inserted in the database");
                    }
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
            throw new DbException("Error in Basket repositiory: " + error);
        }

        public void Update(string email, string productName, string amount, string basketDate)
        {
            Update(new Basket("", email, productName, amount, "", basketDate, ""));
        }

        public void Update(Basket basket)
        {
            string error = "";
            if (basket.IsValid)
            {
                try
                {
                    string personId = UserRepository.GetId(basket.PersonEmail);
                    string productId = GoodsRepository.GetId(basket.ProductName);
                    string limit = GoodsRepository.GetLimit(productId);
                    if (int.Parse(basket.Amount) > int.Parse(limit))
                    {
                        error = string.Format("Amount is Higher end the limit");
                    }
                    else
                    {
                        string price = GoodsRepository.GetPrice(productId);
                        float sumPrice = float.Parse(price) * int.Parse(basket.Amount);
                        string id = GetId(basket.BacketDato, personId, productId);
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                        SqlCommand command = new SqlCommand("UPDATE Basket SET Amount = @Amount, Price = @Price, BacketDato = @dato WHERE Id = @Id", connection);
                        command.Parameters.Add(CreateParam("@Amount", basket.Amount, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@Price", sumPrice, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@Dato", date, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                        connection.Open();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            OnChanged(DbOperation.UPDATE, DbModeltype.Basket);
                            return;
                        }
                        error = string.Format("Basket {0} could not be update", basket.BacketDato);
                    }
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
            else error = "Illegal value for Basket";
            throw new DbException("Error in Basket repositiory: " + error);
        }


        public void Buy(string email)
        {
            string error = "";
            if (email.Length > 0)
            {
                try
                {
                        string personId = UserRepository.GetId(email);
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                        SqlCommand command = new SqlCommand("UPDATE Basket SET Bought = 'true', BoughtDato = @Dato WHERE PersonId = @Id AND Bought = 'false'", connection);;
                        command.Parameters.Add(CreateParam("@Dato", date, SqlDbType.NVarChar));
                        command.Parameters.Add(CreateParam("@Id", personId, SqlDbType.NVarChar));
                        connection.Open();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            OnChanged(DbOperation.UPDATE, DbModeltype.Basket);
                            return;
                        }
                        error = string.Format("Basket {0} could not be update", email);
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
            else error = "Illegal value for email";
            throw new DbException("Error in Basket repositiory: " + error);
        }

        public void Remove(string email, string productName, string basketDate)
        {
            Remove(new Basket("", email, productName, "" , "", basketDate, ""));
        }
        public void Remove(Basket basket)
        {
            string error = "";
            try
            {
                string personId = UserRepository.GetId(basket.PersonEmail);
                string productId = GoodsRepository.GetId(basket.ProductName);
                string id = GetId(basket.BacketDato, personId, productId);
                SqlCommand command = new SqlCommand("DELETE FROM Basket WHERE Id = @Id", connection);
                command.Parameters.Add(CreateParam("@Id", id, SqlDbType.NVarChar));
                connection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    command.ExecuteNonQuery();
                    list.Remove(new Basket(id, "", "", "", "", "", ""));
                    OnChanged(DbOperation.DELETE, DbModeltype.Users);
                    return;
                }
                error = string.Format("Basket {0} could not be deleted", basket.BacketDato);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            throw new DbException("Error in Basket repositiory: " + error);
        }
        public static string GetId(string dato, string personId, string productId)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT Id FROM PersonGoods WHERE BasketDato = @Basket AND PersonID = @PersonID AND ProductID = @ProductId", connection);
                SqlParameter param = new SqlParameter("@Basket", SqlDbType.NVarChar);
                SqlParameter param1 = new SqlParameter("@PersonID", SqlDbType.NVarChar);
                SqlParameter param2 = new SqlParameter("@ProductID", SqlDbType.NVarChar);
                param.Value = dato;
                param1.Value = personId;
                param2.Value = productId;
                command.Parameters.Add(param);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
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
