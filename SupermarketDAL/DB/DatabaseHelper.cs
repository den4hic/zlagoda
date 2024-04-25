using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SupermarketDAL.Entities;
using System.Reflection.PortableExecutable;
using System.IO;
using System.Reflection.Emit;

namespace SupermarketDAL.DB
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        private T ExecuteQuery<T>(string sql, Func<SQLiteDataReader, T> readerFunc, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using SQLiteCommand command = new SQLiteCommand(sql, connection);
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return readerFunc(reader);
                }
            }

            return default;
        }

        private int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }


        public Employee GetEmployeeById(string idEmployee)
        {
            string sql = "SELECT * FROM Employee WHERE id_employee = @IdEmployee";
            return ExecuteQuery(sql, reader => new Employee
            {
                IdEmployee = reader["id_employee"].ToString(),
                EmplSurname = reader["empl_surname"].ToString(),
                EmplName = reader["empl_name"].ToString(),
                EmplPatronymic = reader["empl_patronymic"].ToString(),
                EmplRole = reader["empl_role"].ToString(),
                Salary = Convert.ToDecimal(reader["salary"]),
                DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                DateOfStart = Convert.ToDateTime(reader["date_of_start"]),
                PhoneNumber = reader["phone_number"].ToString(),
                City = reader["city"].ToString(),
                Street = reader["street"].ToString(),
                ZipCode = reader["zip_code"].ToString()
            }, new SQLiteParameter("@IdEmployee", idEmployee));
        }

        public Product GetProductById(int idProduct)
        {
            string sql = "SELECT * FROM Product WHERE id_product = @IdProduct";
            return ExecuteQuery(sql, reader => new Product
            {
                IdProduct = reader.GetInt32(0),
                CategoryNumber = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Producer = reader.GetString(3),
                Characteristics = reader.GetString(4)
            }, new SQLiteParameter("@IdProduct", idProduct));
        }

        public Category GetCategoryById(int categoryNumber)
        {
            string sql = "SELECT * FROM Category WHERE category_number = @CategoryNumber";
            return ExecuteQuery(sql, reader => new Category
            {
                CategoryNumber = reader.GetInt32(0),
                CategoryName = reader.GetString(1)
            }, new SQLiteParameter("@CategoryNumber", categoryNumber));
        }

        public Check GetCheckById(string checkNumber)
        {
            string sql = "SELECT * FROM \"Check\" WHERE check_number = @CheckNumber";
            return ExecuteQuery(sql, reader => new Check
            {
                CheckNumber = reader["check_number"].ToString(),
                IdEmployee = reader["id_employee"].ToString(),
                CardNumber = reader["card_number"].ToString(),
                PrintDate = Convert.ToDateTime(reader["print_date"]),
                SumTotal = Convert.ToDecimal(reader["sum_total"]),
                Vat = Convert.ToDecimal(reader["vat"])
            }, new SQLiteParameter("@CheckNumber", checkNumber));
        }

        public CustomerCard GetCustomerCardByNumber(string cardNumber)
        {
            string sql = "SELECT * FROM Costumer_Card WHERE card_number = @CardNumber";
            return ExecuteQuery(sql, reader => new CustomerCard
            {
                CardNumber = reader.GetString(0),
                CustSurname = reader.GetString(1),
                CustName = reader.GetString(2),
                CustPatronymic = reader.GetString(3),
                PhoneNumber = reader.GetString(4),
                City = reader.GetString(5),
                Street = reader.GetString(6),
                Index = reader.GetString(7),
                Percentage = reader.GetInt32(8)
            }, new SQLiteParameter("@CardNumber", cardNumber));
        }

        public StoreProduct GetStoreProductByUPC(string upc)
        {
            string sql = "SELECT * FROM Store_Product WHERE UPC = @UPC";
            return ExecuteQuery(sql, reader => new StoreProduct
            {
                UPC = reader.GetValue(0) as string,
                UPC_prom = reader.GetValue(1) as string,
                IdProduct = reader.GetInt32(2),
                SellingPrice = reader.GetDecimal(3),
                ProductsNumber = reader.GetInt32(4),
                PromotionalProduct = reader.GetInt32(5) != 0
            }, new SQLiteParameter("@UPC", upc));
        }

        public Sale GetSaleByUPCAndCheckNumber(string upc, string checkNumber)
        {
            string sql = "SELECT * FROM Sale WHERE UPC = @UPC AND check_number = @CheckNumber";
            return ExecuteQuery(sql, reader => new Sale
            {
                UPC = reader.GetString(0),
                CheckNumber = reader.GetString(1),
                ProductNumber = reader.GetInt32(2),
                SellingPrice = reader.GetDecimal(3)
            }, new SQLiteParameter("@UPC", upc), new SQLiteParameter("@CheckNumber", checkNumber));
        }

        public void UpdateEmployee(Employee employee)
        {
            string sql = "UPDATE Employee SET empl_surname = @EmplSurname, empl_name = @EmplName, empl_patronymic = @EmplPatronymic, empl_role = @EmplRole, salary = @Salary, date_of_birth = @DateOfBirth, date_of_start = @DateOfStart, phone_number = @PhoneNumber, city = @City, street = @Street, zip_code = @ZipCode WHERE id_employee = @IdEmployee";
            ExecuteNonQuery(sql,
                new SQLiteParameter("@EmplSurname", employee.EmplSurname),
                new SQLiteParameter("@EmplName", employee.EmplName),
                new SQLiteParameter("@EmplPatronymic", employee.EmplPatronymic),
                new SQLiteParameter("@EmplRole", employee.EmplRole),
                new SQLiteParameter("@Salary", employee.Salary),
                new SQLiteParameter("@DateOfBirth", employee.DateOfBirth),
                new SQLiteParameter("@DateOfStart", employee.DateOfStart),
                new SQLiteParameter("@PhoneNumber", employee.PhoneNumber),
                new SQLiteParameter("@City", employee.City),
                new SQLiteParameter("@Street", employee.Street),
                new SQLiteParameter("@ZipCode", employee.ZipCode),
                new SQLiteParameter("@IdEmployee", employee.IdEmployee));
        }

        public void UpdateCategory(int categoryNumber, string categoryName)
        {
            string sql = "UPDATE Category SET category_name = @CategoryName WHERE category_number = @CategoryNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@CategoryName", categoryName), new SQLiteParameter("@CategoryNumber", categoryNumber));
        }

        public void UpdateCheck(string checkNumber, string idEmployee, string cardNumber, string printDate, decimal sumTotal, decimal vat)
        {
            string sql = "UPDATE Check SET id_employee = @IdEmployee, card_number = @CardNumber, print_date = @PrintDate, sum_total = @SumTotal, vat = @Vat WHERE check_number = @CheckNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@IdEmployee", idEmployee), new SQLiteParameter("@CardNumber", cardNumber), new SQLiteParameter("@PrintDate", printDate), new SQLiteParameter("@SumTotal", sumTotal), new SQLiteParameter("@Vat", vat), new SQLiteParameter("@CheckNumber", checkNumber));
        }

        public void UpdateCustomerCard(string cardNumber, string custSurname, string custName, string custPatronymic, string phoneNumber, string city, string street, string index, int percentage)
        {
            string sql = "UPDATE Costumer_Card SET cust_surname = @CustSurname, cust_name = @CustName, cust_patronymic = @CustPatronymic, phone_number = @PhoneNumber, city = @City, street = @Street, \"index\" = @Index, percentage = @Percentage WHERE card_number = @CardNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@CustSurname", custSurname), new SQLiteParameter("@CustName", custName), new SQLiteParameter("@CustPatronymic", custPatronymic), new SQLiteParameter("@PhoneNumber", phoneNumber), new SQLiteParameter("@City", city), new SQLiteParameter("@Street", street), new SQLiteParameter("@Index", index), new SQLiteParameter("@Percentage", percentage), new SQLiteParameter("@CardNumber", cardNumber));
        }

        public void UpdateProduct(int idProduct, int categoryNumber, string productName, string producer, string characteristics)
        {
            string sql = "UPDATE Product SET category_number = @CategoryNumber, product_name = @ProductName, producer = @Producer, characteristics = @Characteristics WHERE id_product = @IdProduct";
            ExecuteNonQuery(sql, new SQLiteParameter("@CategoryNumber", categoryNumber), new SQLiteParameter("@ProductName", productName), new SQLiteParameter("@Producer", producer), new SQLiteParameter("@Characteristics", characteristics), new SQLiteParameter("@IdProduct", idProduct));
        }

        public void UpdateStoreProduct(string upc, string upcProm, int idProduct, decimal sellingPrice, int productsNumber, bool promotionalProduct)
        {
            string sql = "UPDATE Store_Product SET UPC_prom = @UPCProm, id_product = @IdProduct, selling_price = @SellingPrice, products_number = @ProductsNumber, promotional_product = @PromotionalProduct WHERE UPC = @UPC";
            ExecuteNonQuery(sql, new SQLiteParameter("@UPCProm", upcProm), new SQLiteParameter("@IdProduct", idProduct), new SQLiteParameter("@SellingPrice", sellingPrice), new SQLiteParameter("@ProductsNumber", productsNumber), new SQLiteParameter("@PromotionalProduct", promotionalProduct), new SQLiteParameter("@UPC", upc));
        }

        public void UpdateSale(string upc, string checkNumber, int productNumber, decimal sellingPrice)
        {
            string sql = "UPDATE Sale SET product_number = @ProductNumber, selling_price = @SellingPrice WHERE UPC = @UPC AND check_number = @CheckNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@ProductNumber", productNumber), new SQLiteParameter("@SellingPrice", sellingPrice), new SQLiteParameter("@UPC", upc), new SQLiteParameter("@CheckNumber", checkNumber));
        }

        public void DeleteCategory(int categoryNumber)
        {
            string sql = "DELETE FROM Category WHERE category_number = @CategoryNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@CategoryNumber", categoryNumber));
        }

        public void DeleteCheck(string checkNumber)
        {
            string sql = "DELETE FROM Check WHERE check_number = @CheckNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@CheckNumber", checkNumber));
        }

        public void DeleteCustomerCard(string cardNumber)
        {
            string sql = "DELETE FROM Costumer_Card WHERE card_number = @CardNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@CardNumber", cardNumber));
        }

        public void DeleteProduct(int idProduct)
        {
            string sql = "DELETE FROM Product WHERE id_product = @IdProduct";
            ExecuteNonQuery(sql, new SQLiteParameter("@IdProduct", idProduct));
        }

        public void DeleteStoreProduct(string upc)
        {
            string sql = "DELETE FROM Store_Product WHERE UPC = @UPC";
            ExecuteNonQuery(sql, new SQLiteParameter("@UPC", upc));
        }

        public void DeleteSale(string upc, string checkNumber)
        {
            string sql = "DELETE FROM Sale WHERE UPC = @UPC AND check_number = @CheckNumber";
            ExecuteNonQuery(sql, new SQLiteParameter("@UPC", upc), new SQLiteParameter("@CheckNumber", checkNumber));
        }

		public Employee GetEmployeeByUsernameAndPassword(string username, string hashedPassword)
		{
			throw new NotImplementedException();
		}

        public List<Employee> GetEmployeesList()
        {
            string sql = "SELECT * FROM Employee";
            List<Employee> employees = new List<Employee>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                IdEmployee = reader["id_employee"].ToString(),
                                EmplSurname = reader["empl_surname"].ToString(),
                                EmplName = reader["empl_name"].ToString(),
                                EmplPatronymic = reader["empl_patronymic"].ToString(),
                                EmplRole = reader["empl_role"].ToString(),
                                Salary = Convert.ToDecimal(reader["salary"]),
                                DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                DateOfStart = Convert.ToDateTime(reader["date_of_start"]),
                                PhoneNumber = reader["phone_number"].ToString(),
                                City = reader["city"].ToString(),
                                Street = reader["street"].ToString(),
                                ZipCode = reader["zip_code"].ToString()
                            };
                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }

        public List<Product> GetProductsList()
        {
            string sql = "SELECT * FROM Product";
            List<Product> products = new List<Product>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                IdProduct = reader.GetInt32(0),
                                CategoryNumber = reader.GetInt32(1),
                                ProductName = reader.GetString(2),
                                Producer = reader.GetString(3),
                                Characteristics = reader.GetString(4)
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public List<Category> GetCategoriesList()
        {
            string sql = "SELECT * FROM Category";
            List<Category> categories = new List<Category>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                CategoryNumber = reader.GetInt32(0),
                                CategoryName = reader.GetString(1)
                            };
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

        public List<Check> GetChecksList()
        {
            string sql = "SELECT * FROM [Check]";
            List<Check> checks = new List<Check>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Check check = new Check
                            {
                                CheckNumber = reader["check_number"].ToString(),
                                IdEmployee = reader["id_employee"].ToString(),
                                CardNumber = reader["card_number"].ToString(),
                                PrintDate = Convert.ToDateTime(reader["print_date"]),
                                SumTotal = Convert.ToDecimal(reader["sum_total"]),
                                Vat = Convert.ToDecimal(reader["vat"])
                            };
                            checks.Add(check);
                        }
                    }
                }
            }

            return checks;
        }

        public List<CustomerCard> GetCustomerCardsList()
        {
            string sql = "SELECT * FROM Costumer_Card";
            List<CustomerCard> costumerCards = new List<CustomerCard>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerCard card = new CustomerCard
                            {
                                CardNumber = reader["card_number"].ToString(),
                                CustSurname = reader["cust_surname"].ToString(),
                                CustName = reader["cust_name"].ToString(),
                                CustPatronymic = reader["cust_patronymic"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString(),
                                City = reader["city"].ToString(),
                                Street = reader["street"].ToString(),
                                Index = reader["index"].ToString(),
                                Percentage = Convert.ToInt32(reader["percentage"])
                            };
                            costumerCards.Add(card);
                        }
                    }
                }
            }

            return costumerCards;
        }

        public List<StoreProduct> GetStoreProductsList()
        {
            string sql = "SELECT * FROM Store_Product";
            List<StoreProduct> storeProducts = new List<StoreProduct>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StoreProduct storeProduct = new StoreProduct
                            {
                                UPC = reader.GetValue(0) as string,
                                UPC_prom = reader.GetValue(1) as string,
                                IdProduct = reader.GetInt32(2),
                                SellingPrice = reader.GetDecimal(3),
                                ProductsNumber = reader.GetInt32(4),
                                PromotionalProduct = reader.GetInt32(5) != 0
                            };
                            storeProducts.Add(storeProduct);
                        }
                    }
                }
            }

            return storeProducts;
        }

        public List<Sale> GetSalesList()
        {
            string sql = "SELECT * FROM Sale";
            List<Sale> sales = new List<Sale>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sale sale = new Sale
                            {
                                UPC = reader.GetString(0),
                                CheckNumber = reader.GetString(1),
                                ProductNumber = reader.GetInt32(2),
                                SellingPrice = reader.GetDecimal(3)
                            };
                            sales.Add(sale);
                        }
                    }
                }
            }

            return sales;
        }

		public List<Product> GetProductsListByCategory(string selectedCategory)
		{
			throw new NotImplementedException();
		}
	}
}
