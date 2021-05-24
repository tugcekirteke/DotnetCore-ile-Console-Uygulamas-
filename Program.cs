using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace dotnetCore
{       
    public interface IProductDal
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> Find(string productName);
        int Count();
        int Create(Product p);
        int Update(Product p);
        int Delete(int productId);
    }

    public class MySQLProductDal : IProductDal
    {
        private MySqlConnection GetMySqlConnection()
        {
            string connectionString = @"server=localhost;port=3306;database=northwind;user=root;password=mysql1234;";
            return new MySqlConnection(connectionString);         
        }
        public int Create(Product p)
        {
            throw new NotImplementedException();
        }

        public int Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = null;

            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    
                    string sql = "select * from products";

                    MySqlCommand command = new MySqlCommand(sql,connection);

                    MySqlDataReader reader = command.ExecuteReader();
                    products = new List<Product>();

                    while(reader.Read())
                    {
                        products.Add(
                            new Product
                            {
                                ProductId=int.Parse(reader["id"].ToString()),
                                Name = reader["product_name"].ToString(),
                                Price = double.Parse(reader["list_price"]?.ToString())
                            }
                        );
                    }
                    reader.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return products;  
        }

        public Product GetProductById(int id)
        {
            Product product = null;

            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    
                    string sql = "select * from products where id=@productid";

                    MySqlCommand command = new MySqlCommand(sql,connection);
                    command.Parameters.Add("@productid", MySqlDbType.Int32).Value = id;

                    MySqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    if (reader.HasRows)
                    {                    
                        product = new Product()
                        {                       
                            ProductId=int.Parse(reader["id"].ToString()),
                            Name = reader["product_name"].ToString(),
                            Price = double.Parse(reader["list_price"]?.ToString())
                        };
                    }
                 
                    reader.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return product;  
        }

        public int Update(Product p)
        {
            throw new NotImplementedException();
        }

        public List<Product> Find(string productName)
        {
            List<Product> products = null;

            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    
                    string sql = "select * from products where product_name LIKE @productName";

                    MySqlCommand command = new MySqlCommand(sql,connection);
                    command.Parameters.Add("@productName", MySqlDbType.String).Value = "%"+ productName+"%";

                    MySqlDataReader reader = command.ExecuteReader();

                    products = new List<Product>();

                    while(reader.Read())
                    {
                        products.Add(
                            new Product
                            {
                                ProductId=int.Parse(reader["id"].ToString()),
                                Name = reader["product_name"].ToString(),
                                Price = double.Parse(reader["list_price"]?.ToString())
                            }
                        );
                    }                 
                 
                    reader.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return products;  
        }

        public int Count()
        {
            int count = 0; 

            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    
                    string sql = "select count(*) from products";

                    MySqlCommand command = new MySqlCommand(sql,connection);
                    object result = command.ExecuteScalar();
                    if (result!=null)
                    {
                        count = Convert.ToInt32(result); 
                    }
                 
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return count;  
        }
    }

    public class MsSQLProductDal : IProductDal
    {
        private SqlConnection GetMsSqlConnection()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=SSPI";
            return new SqlConnection(connectionString);         
        }
        public int Create(Product p)
        {
            int result = 0;

            using(var connection = GetMsSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "insert into products (ProductName,UnitPrice,Discontinued) VALUES (@productname,@unitprice,@discontinued)";
                    SqlCommand command = new SqlCommand(sql,connection);

                    command.Parameters.AddWithValue("@productname", p.Name);
                    command.Parameters.AddWithValue("@unitprice", p.Price);
                    command.Parameters.AddWithValue("@discontinued", 1);
                    
                    result = command.ExecuteNonQuery();

                    Console.WriteLine($"{result} adet kayıt eklendi");

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            
            return result;
        }

        public int Delete(int productId)
        {
            int result = 0;

            using(var connection = GetMsSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "delete from products where ProductId=@productid";
                    SqlCommand command = new SqlCommand(sql,connection);

                    command.Parameters.AddWithValue("@productid", productId);
                    
                    result = command.ExecuteNonQuery();

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            
            return result;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = null;

            using(var connection = GetMsSqlConnection())
            {
                try
                {
                    connection.Open();
                    
                    string sql = "select * from products";

                    SqlCommand command = new SqlCommand(sql,connection);

                    SqlDataReader reader = command.ExecuteReader();
                    products = new List<Product>();

                    while(reader.Read())
                    {
                        products.Add(
                            new Product
                            {
                                ProductId=int.Parse(reader["ProductID"].ToString()),
                                Name = reader["ProductName"].ToString(),
                                Price = double.Parse(reader["UnitPrice"]?.ToString())
                            }
                        );
                    }
                    reader.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return products;  
        }

        public Product GetProductById(int id)
        {
            Product product = null;

            using(var connection = GetMsSqlConnection())
            {
                try
                {
                    connection.Open();
                    
                    string sql = "select * from products where ProductId=@productid";

                    SqlCommand command = new SqlCommand(sql,connection);
                    command.Parameters.AddWithValue("@productid", id);

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    if (reader.HasRows)
                    {                    
                        product = new Product()
                        {                       
                            ProductId=int.Parse(reader["ProductId"].ToString()),
                            Name = reader["ProductName"].ToString(),
                            Price = double.Parse(reader["UnitPrice"]?.ToString())
                        };
                    }
                 
                    reader.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return product;  
        }

        public int Update(Product p)
        {
            int result = 0;

            using(var connection = GetMsSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "update products set ProductName=@productname, UnitPrice=@unitprice where ProductId=@productid";
                    SqlCommand command = new SqlCommand(sql,connection);

                    command.Parameters.AddWithValue("@productname", p.Name);
                    command.Parameters.AddWithValue("@unitprice", p.Price);
                    command.Parameters.AddWithValue("@productid", p.ProductId);
                    
                    result = command.ExecuteNonQuery();

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            
            return result;
        }

        public List<Product> Find(string productName)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }

    public class ProductManager : IProductDal
    {
        IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public int Count()
        {
            return _productDal.Count();
        }

        public int Create(Product p)
        {
           return _productDal.Create(p);
        }

        public int Delete(int productId)
        {
            return _productDal.Delete(productId);
        }

        public List<Product> Find(string productName)
        {
           return _productDal.Find(productName);
        }

        public List<Product> GetAllProducts()
        {
            return _productDal.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            return _productDal.GetProductById(id);
        }

        public int Update(Product p)
        {
            return _productDal.Update(p);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
       
           var productDal = new ProductManager(new MsSQLProductDal());

           int result = productDal.Delete(78);

           Console.WriteLine($"{result} adet kayıt silindi.");
        
        }
      
      
    }
}
