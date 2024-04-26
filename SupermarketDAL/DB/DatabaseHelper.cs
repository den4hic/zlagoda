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
using System.Text.RegularExpressions;
using Xceed.Wpf.AvalonDock.Themes;

namespace SupermarketDAL.DB
{
    public class DatabaseHelper
    {
        private string connectionString;
        private string databasePath;

        public DatabaseHelper(string databasePath)
        {
            this.databasePath = databasePath;
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        private IEnumerable<T> ExecuteQuery<T>(string sql, Func<SQLiteDataReader, T> readerFunc, params SQLiteParameter[] parameters)
        {
            List<T> results = new List<T>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(readerFunc(reader));
                        }
                    }
                }
            }
            return results;
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
            }, new SQLiteParameter("@IdEmployee", idEmployee)).FirstOrDefault();
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
            }, new SQLiteParameter("@IdProduct", idProduct)).FirstOrDefault();
        }

        public Category GetCategoryById(int categoryNumber)
        {
            string sql = "SELECT * FROM Category WHERE category_number = @CategoryNumber";
            return ExecuteQuery(sql, reader => new Category
            {
                CategoryNumber = reader.GetInt32(0),
                CategoryName = reader.GetString(1)
            }, new SQLiteParameter("@CategoryNumber", categoryNumber)).FirstOrDefault();
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
            }, new SQLiteParameter("@CheckNumber", checkNumber)).FirstOrDefault();
        }

        public CustomerCard GetCustomerCardByNumber(string cardNumber)
        {
            string sql = "SELECT * FROM Costumer_Card WHERE card_number = @CardNumber";
            return ExecuteQuery(sql, reader => new CustomerCard
            {
                CardNumber = reader.GetValue(0).ToString(),
                CustSurname = reader.GetValue(1).ToString(),
                CustName = reader.GetValue(2).ToString(),
                CustPatronymic = reader.GetValue(3).ToString(),
                PhoneNumber = reader.GetValue(4).ToString(),
                City = reader.GetValue(5).ToString(),
                Street = reader.GetValue(6).ToString(),
                Index = reader.GetValue(7).ToString(),
                Percentage = reader.GetInt32(8)
            }, new SQLiteParameter("@CardNumber", cardNumber)).FirstOrDefault();
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
            }, new SQLiteParameter("@UPC", upc)).FirstOrDefault();
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
            }, new SQLiteParameter("@UPC", upc), new SQLiteParameter("@CheckNumber", checkNumber)).FirstOrDefault();
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
            string sql = "UPDATE \"Check\" SET id_employee = @IdEmployee, card_number = @CardNumber, print_date = @PrintDate, sum_total = @SumTotal, vat = @Vat WHERE check_number = @CheckNumber";
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
            string sql = "DELETE FROM \"Check\" WHERE check_number = @CheckNumber";
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
            string sql = "SELECT e.* FROM Employee e JOIN User_Account ua ON e.id_employee = ua.id_employee WHERE ua.username = @username AND ua.hashed_password = @hashedPassword";
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
            }, new SQLiteParameter("@hashedPassword", hashedPassword), new SQLiteParameter("@username", username)).FirstOrDefault();
        }

        public List<Employee> GetEmployeesList()
        {
            string sql = "SELECT * FROM Employee";
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
            }).ToList();
        }


        public List<Product> GetProductsList()
        {
            string sql = "SELECT * FROM Product";
            return ExecuteQuery(sql, reader => new Product
            {
                IdProduct = reader.GetInt32(0),
                CategoryNumber = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Producer = reader.GetString(3),
                Characteristics = reader.GetString(4)
            }).ToList();
        }


        public List<Category> GetCategoriesList()
        {
            string sql = "SELECT * FROM Category";
            return ExecuteQuery(sql, reader => new Category
            {
                CategoryNumber = reader.GetInt32(0),
                CategoryName = reader.GetString(1)
            }).ToList();
        }

        public List<Check> GetChecksList()
        {
            string sql = "SELECT * FROM [Check]";
            return ExecuteQuery(sql, reader => new Check
            {
                CheckNumber = reader["check_number"].ToString(),
                IdEmployee = reader["id_employee"].ToString(),
                CardNumber = reader["card_number"].ToString(),
                PrintDate = Convert.ToDateTime(reader["print_date"]),
                SumTotal = Convert.ToDecimal(reader["sum_total"]),
                Vat = Convert.ToDecimal(reader["vat"])
            }).ToList();
        }


        public List<CustomerCard> GetCustomerCardsList()
        {
            string sql = "SELECT * FROM Costumer_Card";
            return ExecuteQuery(sql, reader => new CustomerCard
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
            }).ToList();
        }


        public List<StoreProduct> GetStoreProductsList()
        {
            string sql = "SELECT * FROM Store_Product";
            return ExecuteQuery(sql, reader => new StoreProduct
            {
                UPC = reader.GetValue(0) as string,
                UPC_prom = reader.GetValue(1) as string,
                IdProduct = reader.GetInt32(2),
                SellingPrice = reader.GetDecimal(3),
                ProductsNumber = reader.GetInt32(4),
                PromotionalProduct = reader.GetInt32(5) != 0
            }).ToList();
        }


        public List<Sale> GetSalesList()
        {
            string sql = "SELECT * FROM Sale";
            return ExecuteQuery(sql, reader => new Sale
            {
                UPC = reader.GetString(0),
                CheckNumber = reader.GetString(1),
                ProductNumber = reader.GetInt32(2),
                SellingPrice = reader.GetDecimal(3)
            }).ToList();
        }


        public List<Product> GetProductsListByCategoryID(int selectedCategory)
        {
            string sql = "SELECT * FROM Product WHERE category_number = @SelectedCategory";
            return ExecuteQuery(sql, reader => new Product
            {
                IdProduct = reader.GetInt32(0),
                CategoryNumber = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Producer = reader.GetString(3),
                Characteristics = reader.GetString(4)
            }, new SQLiteParameter("@SelectedCategory", selectedCategory)).ToList();
        }

        public List<GoodsInStock> GetGoodsListByCategoryID(int selectedCategory)
        {

            string sql = @"SELECT p.id_product, p.product_name, p.producer, p.characteristics, sp.UPC, sp.selling_price, sp.products_number, sp.promotional_product
                   FROM Product p
                   INNER JOIN Store_Product sp ON p.id_product = sp.id_product
                   WHERE p.category_number = @SelectedCategory";
            return ExecuteQuery(sql, reader => new GoodsInStock
            {
                IdProduct = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Producer = reader.GetString(2),
                Characteristics = reader.GetString(3),
                UPC = reader.GetString(4),
                SellingPrice = reader.GetDecimal(5),
                ProductsNumber = reader.GetInt32(6),
                PromotionalProduct = reader.GetBoolean(7)
            }, new SQLiteParameter("@SelectedCategory", selectedCategory)).ToList();
        }

        public void PutTestsData()
        {
            ExecuteNonQuery("INSERT INTO Category (category_name) VALUES ('Electronics');");
            ExecuteNonQuery("INSERT INTO Category (category_name) VALUES ('Food');");

            ExecuteNonQuery("INSERT INTO Costumer_Card (card_number, cust_surname, cust_name, phone_number, percentage) VALUES ('1234567890123', 'Smith', 'John', '123456789', 5);");
            ExecuteNonQuery("INSERT INTO Costumer_Card (card_number, cust_surname, cust_name, phone_number, city, street, \"index\", percentage) VALUES ('9876543210987', 'Doe', 'Jane', '987654321', 'New York', 'Broadway', '12345', 10);");

            ExecuteNonQuery("INSERT INTO Employee (id_employee, empl_surname, empl_name, empl_role, salary, date_of_birth, date_of_start, phone_number, city, street, zip_code) VALUES ('EMP001', 'Johnson', 'Michael', 'Manager', 5000.00, '1980-05-15', '2010-01-01', '111222333', 'Los Angeles', 'Main St', '54321');");
            ExecuteNonQuery("INSERT INTO Employee (id_employee, empl_surname, empl_name, empl_role, salary, date_of_birth, date_of_start, phone_number, city, street, zip_code) VALUES ('EMP002', 'Brown', 'Emily', 'Cashier', 3000.00, '1990-10-20', '2015-03-15', '444555666', 'Chicago', 'Oak St', '67890');");

            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Pizza', 'Pizza Hut', 'Large, Pepperoni');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (1, 'Smartphone', 'Samsung', '6.4\" Display, 128GB Storage');");

            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('123456789012', 1, 10.99, 50, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('987654321098', 2, 699.99, 100, 1);");

            ExecuteNonQuery("INSERT INTO Sale (UPC, check_number, product_number, selling_price) VALUES ('123456789012', 'CHECK001', 1, 10.99);");
            ExecuteNonQuery("INSERT INTO Sale (UPC, check_number, product_number, selling_price) VALUES ('987654321098', 'CHECK002', 2, 699.99);");

            ExecuteNonQuery("INSERT INTO \"Check\" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('CHECK001', 'EMP001', NULL, '2024-04-04', 50.00, 5.00);");
            ExecuteNonQuery("INSERT INTO \"Check\" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('CHECK003', 'EMP001', NULL, DATE('now'), 50.00, 5.00);");
            ExecuteNonQuery("INSERT INTO \"Check\" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('CHECK002', 'EMP002', '9876543210987', '2024-04-04', 699.99, 69.99);");

            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Burger', 'McDonalds', 'Big Mac with fries');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Salad', 'Subway', 'Vegetable salad with dressing');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (1, 'Laptop', 'Apple', 'MacBook Pro 13 with Touch Bar');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (1, 'Headphones', 'Sony', 'Noise-cancelling wireless headphones');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Sushi', 'Sushi Bar', 'Assorted sushi rolls');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Sandwich', 'Starbucks', 'Chicken and avocado sandwich');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (1, 'Smartwatch', 'Fitbit', 'Fitbit Versa 3 smartwatch');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Pasta', 'Italiano', 'Spaghetti carbonara');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (1, 'TV', 'Samsung', '55\" QLED 4K Smart TV');");
            ExecuteNonQuery("INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (2, 'Cake', 'Bakery', 'Chocolate fudge cake');");

            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('111111111111', 3, 1299.99, 20, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('222222222222', 4, 199.99, 30, 1);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('333333333333', 5, 19.99, 50, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('444444444444', 6, 7.99, 40, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('555555555555', 7, 249.99, 15, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('666666666666', 8, 49.99, 25, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('777777777777', 9, 899.99, 10, 1);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('888888888888', 10, 29.99, 60, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('999999999999', 11, 39.99, 20, 0);");
            ExecuteNonQuery("INSERT INTO Store_Product (UPC, id_product, selling_price, products_number, promotional_product) VALUES ('000000000000', 12, 29.99, 25, 0);");
            ExecuteNonQuery("INSERT INTO User_Account (username, hashed_password, id_employee) VALUES ('user123', '2b53b02df672eadf4fa638e008aae8af5623bb656c062c3201f56dd9f32ec990', 'EMP001');");
            ExecuteNonQuery("INSERT INTO User_Account (username, hashed_password, id_employee) VALUES ('employee456', '8687859fd2b7fd63eb634212a78586727ffd3bfa5158fc60b88b3a7a8c25ded5', 'EMP002');");

        }
        public void ResetDatabase()
        {
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }

            SQLiteConnection.CreateFile(databasePath);

            ExecuteNonQuery("PRAGMA foreign_keys = off;");
            ExecuteNonQuery("BEGIN TRANSACTION;");

            ExecuteNonQuery("CREATE TABLE Category (category_number INTEGER PRIMARY KEY AUTOINCREMENT, category_name TEXT (50));");
            ExecuteNonQuery("CREATE TABLE \"Check\" (check_number TEXT (10) PRIMARY KEY NOT NULL, id_employee TEXT (10) NOT NULL REFERENCES Employee (id_employee) ON DELETE NO ACTION ON UPDATE CASCADE, card_number TEXT (13) REFERENCES Costumer_Card (card_number) ON DELETE NO ACTION ON UPDATE CASCADE, print_date TEXT NOT NULL, sum_total NUMERIC (13, 4) NOT NULL, vat NUMERIC (13, 4) NOT NULL);");
            ExecuteNonQuery("CREATE TABLE Costumer_Card (card_number TEXT (13) NOT NULL PRIMARY KEY, cust_surname TEXT (50) NOT NULL, cust_name TEXT (50) NOT NULL, cust_patronymic TEXT (50), phone_number TEXT (13) NOT NULL, city TEXT (50), street TEXT (50), \"index\" TEXT (9), percentage INTEGER NOT NULL);");
            ExecuteNonQuery("CREATE TABLE Employee (id_employee TEXT (10) NOT NULL PRIMARY KEY, empl_surname TEXT (50) NOT NULL, empl_name TEXT (50) NOT NULL, empl_patronymic TEXT (50), empl_role TEXT (10) NOT NULL, salary NUMERIC (13, 4) NOT NULL, date_of_birth TEXT NOT NULL, date_of_start TEXT NOT NULL, phone_number TEXT (13) NOT NULL, city TEXT (50) NOT NULL, street TEXT (50) NOT NULL, zip_code TEXT (9) NOT NULL);");
            ExecuteNonQuery("CREATE TABLE Product (id_product INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, category_number INTEGER REFERENCES Category (category_number) ON DELETE NO ACTION ON UPDATE CASCADE NOT NULL, product_name TEXT (50) NOT NULL, producer TEXT (50) NOT NULL, characteristics TEXT (100) NOT NULL);");
            ExecuteNonQuery("CREATE TABLE Sale (UPC TEXT (12) NOT NULL REFERENCES Store_Product (UPC) ON DELETE NO ACTION ON UPDATE CASCADE, check_number TEXT (10) NOT NULL REFERENCES \"Check\" (check_number) ON DELETE CASCADE ON UPDATE CASCADE, product_number INTEGER NOT NULL, selling_price NUMERIC (13, 4) NOT NULL, PRIMARY KEY (UPC, check_number));");
            ExecuteNonQuery("CREATE TABLE Store_Product (UPC TEXT (12) NOT NULL PRIMARY KEY, UPC_prom TEXT (12) REFERENCES Store_Product (UPC) ON DELETE SET NULL ON UPDATE CASCADE, id_product INTEGER REFERENCES Product (id_product) ON DELETE NO ACTION ON UPDATE CASCADE NOT NULL, selling_price NUMERIC (13, 4) NOT NULL, products_number INTEGER NOT NULL, promotional_product BOOLEAN NOT NULL);");
            ExecuteNonQuery("CREATE TABLE User_Account (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, username TEXT UNIQUE NOT NULL, hashed_password TEXT NOT NULL, id_employee TEXT REFERENCES Employee (id_employee) NOT NULL UNIQUE);");

            ExecuteNonQuery("CREATE INDEX adress ON Employee (city, street, zip_code);");
            ExecuteNonQuery("CREATE INDEX cliend_adress ON Costumer_Card (city, street, \"index\");");
            ExecuteNonQuery("CREATE INDEX client_PIB ON Costumer_Card (cust_surname, cust_name, cust_patronymic);");
            ExecuteNonQuery("CREATE INDEX PIB ON Employee (empl_surname, empl_name, empl_patronymic);");

            ExecuteNonQuery("PRAGMA foreign_keys = on;");

        }

        public GoodsInStock GetGoodsByUPC(string upc)
        {
            string sql = @"SELECT p.IdProduct, p.ProductName, p.Producer, p.Characteristics, sp.UPC, sp.SellingPrice, sp.ProductsNumber, sp.PromotionalProduct
                   FROM Products p
                   JOIN StoreProducts sp ON p.IdProduct = sp.IdProduct
                   WHERE sp.UPC = @upc";
            return ExecuteQuery(sql, reader => new GoodsInStock
            {
                IdProduct = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Producer = reader.GetString(2),
                Characteristics = reader.GetString(3),
                UPC = reader.GetString(4),
                SellingPrice = reader.GetDecimal(5),
                ProductsNumber = reader.GetInt32(6),
                PromotionalProduct = reader.GetBoolean(7)
            }, new SQLiteParameter("@upc", upc)).FirstOrDefault();
        }

        public List<GoodsInStock> GetGoods()
        {
            string sql = @"SELECT p.id_product, p.product_name, p.producer, p.characteristics, sp.UPC, sp.selling_price, sp.products_number, sp.promotional_product
                   FROM Product p
                   INNER JOIN Store_Product sp ON p.id_product = sp.id_product";
            return ExecuteQuery(sql, reader => new GoodsInStock
            {
                IdProduct = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Producer = reader.GetString(2),
                Characteristics = reader.GetString(3),
                UPC = reader.GetString(4),
                SellingPrice = reader.GetDecimal(5),
                ProductsNumber = reader.GetInt32(6),
                PromotionalProduct = reader.GetBoolean(7)
            }).ToList();
        }

        public List<GoodsInStock> GetGoodsInStockByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public (int TotalChecks, int TotalSales) GetProductSoldNumberByCategoryID(int categoryId)
        {
            string sql = @"SELECT COUNT(S.check_number) AS total_checks, SUM(S.product_number) AS total_sold
                FROM Category C
                JOIN Product P ON C.category_number = P.category_number
                JOIN Sale S ON P.id_product = S.product_number
                WHERE C.category_number = @categoryId
                GROUP BY C.category_name;";
            return ExecuteQuery(sql, reader => (
                TotalChecks: Convert.ToInt32(reader["total_checks"]),
                TotalSales: Convert.ToInt32(reader["total_sold"])
            ), new SQLiteParameter("@categoryId", categoryId)).FirstOrDefault();
        }

        public List<(string EmployeeSurname, string EmployeeName, string CustomerSurname, string CustomerName, int TotalSharedSales)> GetEmployeesAndCustomersWithMaxSharedSales()
        {
            string sql = @"
                SELECT E.empl_surname, E.empl_name, CC.cust_surname, CC.cust_name, COUNT(C.check_number) AS total_shared_sales
                FROM Employee E
                JOIN ""Check"" C ON E.id_employee = C.id_employee
                JOIN Costumer_Card CC ON C.card_number = CC.card_number
                GROUP BY E.empl_surname, E.empl_name, CC.cust_surname, CC.cust_name
                HAVING COUNT(C.check_number) = (
                    SELECT MAX(shared_sales) FROM (
                        SELECT COUNT(C.check_number) AS shared_sales
                        FROM Employee E
                        JOIN ""Check"" C ON E.id_employee = C.id_employee
                        JOIN Costumer_Card CC ON C.card_number = CC.card_number
                        GROUP BY E.empl_surname, E.empl_name, CC.cust_surname, CC.cust_name
                    )
                );
            ";

            return ExecuteQuery(sql, reader => (
                EmployeeSurname: reader["empl_surname"].ToString(),
                EmployeeName: reader["empl_name"].ToString(),
                CustomerSurname: reader["cust_surname"].ToString(),
                CustomerName: reader["cust_name"].ToString(),
                TotalSharedSales: Convert.ToInt32(reader["total_shared_sales"])
            )).ToList();
        }

        public int GetTotalSoldProductsForProducer(string producer)
        {
            string sql = @"
                SELECT SUM(S.product_number) AS total_products
                FROM Product P
                JOIN Store_Product SP ON P.id_product = SP.id_product
                JOIN Sale S ON SP.UPC = S.UPC
                WHERE P.producer = @Producer
            ";

            return ExecuteQuery(sql, reader => reader["total_products"] != DBNull.Value ? Convert.ToInt32(reader["total_products"]) : 0,
        new SQLiteParameter("@Producer", producer)).FirstOrDefault();

        }


        public void InsertCategory(Category category)
        {
            string sql = "INSERT INTO Category (category_name) VALUES (@CategoryName)";
            ExecuteNonQuery(sql, new SQLiteParameter("@CategoryName", category.CategoryName));
        }

        public void InsertCheck(Check check)
        {
            string sql = "INSERT INTO \"Check\" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES (@CheckNumber, @IdEmployee, @CardNumber, @PrintDate, @SumTotal, @Vat)";
            ExecuteNonQuery(sql, new SQLiteParameter("@CheckNumber", check.CheckNumber), new SQLiteParameter("@IdEmployee", check.IdEmployee), new SQLiteParameter("@CardNumber", check.CardNumber), new SQLiteParameter("@PrintDate", check.PrintDate), new SQLiteParameter("@SumTotal", check.SumTotal), new SQLiteParameter("@Vat", check.Vat));
        }
        public void InsertCostumerCard(CustomerCard costumerCard)
        {
            string sql = "INSERT INTO Costumer_Card (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street, \"index\", percentage) VALUES (@CardNumber, @CustSurname, @CustName, @CustPatronymic, @PhoneNumber, @City, @Street, @Index, @Percentage)";
            ExecuteNonQuery(sql, new SQLiteParameter("@CardNumber", costumerCard.CardNumber), new SQLiteParameter("@CustSurname", costumerCard.CustSurname), new SQLiteParameter("@CustName", costumerCard.CustName), new SQLiteParameter("@CustPatronymic", costumerCard.CustPatronymic), new SQLiteParameter("@PhoneNumber", costumerCard.PhoneNumber), new SQLiteParameter("@City", costumerCard.City), new SQLiteParameter("@Street", costumerCard.Street), new SQLiteParameter("@Index", costumerCard.Index), new SQLiteParameter("@Percentage", costumerCard.Percentage));
        }
        public void InsertEmployee(Employee employee)
        {
            string sql = "INSERT INTO Employee (id_employee, empl_surname, empl_name, empl_patronymic, empl_role, salary, date_of_birth, date_of_start, phone_number, city, street, zip_code) VALUES (@IdEmployee, @EmplSurname, @EmplName, @EmplPatronymic, @EmplRole, @Salary, @DateOfBirth, @DateOfStart, @PhoneNumber, @City, @Street, @ZipCode)";
            ExecuteNonQuery(sql, new SQLiteParameter("@IdEmployee", employee.IdEmployee), new SQLiteParameter("@EmplSurname", employee.EmplSurname), new SQLiteParameter("@EmplName", employee.EmplName), new SQLiteParameter("@EmplPatronymic", employee.EmplPatronymic), new SQLiteParameter("@EmplRole", employee.EmplRole), new SQLiteParameter("@Salary", employee.Salary), new SQLiteParameter("@DateOfBirth", employee.DateOfBirth), new SQLiteParameter("@DateOfStart", employee.DateOfStart), new SQLiteParameter("@PhoneNumber", employee.PhoneNumber), new SQLiteParameter("@City", employee.City), new SQLiteParameter("@Street", employee.Street), new SQLiteParameter("@ZipCode", employee.ZipCode));
        }
        public void InsertProduct(Product product)
        {
            string sql = "INSERT INTO Product (category_number, product_name, producer, characteristics) VALUES (@CategoryNumber, @ProductName, @Producer, @Characteristics)";
            ExecuteNonQuery(sql, new SQLiteParameter("@CategoryNumber", product.CategoryNumber), new SQLiteParameter("@ProductName", product.ProductName), new SQLiteParameter("@Producer", product.Producer), new SQLiteParameter("@Characteristics", product.Characteristics));
        }
        public void InsertSale(Sale sale)
        {
            string sql = "INSERT INTO Sale (UPC, check_number, product_number, selling_price) VALUES (@UPC, @CheckNumber, @ProductNumber, @SellingPrice)";
            ExecuteNonQuery(sql, new SQLiteParameter("@UPC", sale.UPC), new SQLiteParameter("@CheckNumber", sale.CheckNumber), new SQLiteParameter("@ProductNumber", sale.ProductNumber), new SQLiteParameter("@SellingPrice", sale.SellingPrice));
        }
        public void InsertStoreProduct(StoreProduct storeProduct)
        {
            string sql = "INSERT INTO Store_Product (UPC, UPC_prom, id_product, selling_price, products_number, promotional_product) VALUES (@UPC, @UPCProm, @IdProduct, @SellingPrice, @ProductsNumber, @PromotionalProduct)";
            ExecuteNonQuery(sql, new SQLiteParameter("@UPC", storeProduct.UPC), new SQLiteParameter("@UPCProm", storeProduct.UPC_prom), new SQLiteParameter("@IdProduct", storeProduct.IdProduct), new SQLiteParameter("@SellingPrice", storeProduct.SellingPrice), new SQLiteParameter("@ProductsNumber", storeProduct.ProductsNumber), new SQLiteParameter("@PromotionalProduct", storeProduct.PromotionalProduct));
        }
        public void InsertUserAccount(UserAccount userAccount)
        {
            string sql = "INSERT INTO User_Account (username, hashed_password, id_employee) VALUES (@Username, @HashedPassword, @IdEmployee)";
            ExecuteNonQuery(sql, new SQLiteParameter("@Username", userAccount.Username), new SQLiteParameter("@HashedPassword", userAccount.HashedPassword), new SQLiteParameter("@IdEmployee", userAccount.IdEmployee));
        }


        public List<Employee> GetEmployeesWithoutUserAccountAndSales()
        {
            string sql = @"
                SELECT *
                FROM Employee e
                WHERE NOT EXISTS (
                    SELECT *
                    FROM User_Account ua
                    WHERE ua.id_employee = e.id_employee
                )
                OR e.id_employee NOT IN (
                    SELECT c.id_employee
                    FROM ""Check"" c
                );
            ";
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
            }).ToList();
        }
        public List<Product> GetProductWithoutEmployeeSurnameAndProduceNameStatsWithSelectedLetter(string letter)
        {
  
            string sql = @"
                SELECT *
                FROM Product p
                JOIN Store_Product sp ON sp.id_product = p.id_product
                WHERE sp.UPC NOT IN (
                    SELECT s.UPC
                    From Sale s
                    WHERE s.check_number IN (
                        SELECT c.check_number
                        FROM ""Check"" c
                        JOIN Employee e ON e.id_employee = c.id_employee
                        WHERE e.empl_surname LIKE @letter
                    )
                ) AND p.producer NOT LIKE @letter
            ";

            return ExecuteQuery(sql, reader => new Product
            {
                IdProduct = reader.GetInt32(0),
                CategoryNumber = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Producer = reader.GetString(3),
                Characteristics = reader.GetString(4)
            }, new SQLiteParameter("@letter", letter)).ToList();
        }

        public List<Employee> GetEmployeesWithoutSalesInCategory(string categoryName)
        {
            string sql = @"
                SELECT DISTINCT *
                FROM Employee e
                WHERE e.id_employee NOT IN (
                    SELECT DISTINCT c.id_employee
                    FROM Sale s
                    INNER JOIN Product p ON s.product_number = p.id_product
                    INNER JOIN ""Check"" c ON s.check_number = c.check_number
                    WHERE p.category_number NOT IN (
                        SELECT category_number
                        FROM Category
                        WHERE category_name = @CategoryName
                    )
                );
            ";

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
            }, new SQLiteParameter("@CategoryName", categoryName)).ToList();
        }

		public List<Sale> GetSalesListByCheckNumber(string receiptNumber)
		{
            string sql = @"SELECT * FROM Sale WHERE check_number = @CheckNumber";
            return ExecuteQuery(sql, reader => new Sale
            {
                UPC = reader.GetString(0),
                CheckNumber = reader.GetString(1),
                ProductNumber = reader.GetInt32(2),
                SellingPrice = reader.GetDecimal(3)
            }, new SQLiteParameter("@CheckNumber", receiptNumber)).ToList();
        }

		public Product GetProductByUPC(string upc)
		{
            string sql = @"
                SELECT p.*
                FROM Product p
                JOIN Store_Product sp ON p.id_product = sp.id_product
                WHERE sp.UPC = @UPC";

            return ExecuteQuery(sql, reader => new Product
            {
                IdProduct = reader.GetInt32(0),
                CategoryNumber = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Producer = reader.GetString(3),
                Characteristics = reader.GetString(4)
            }, new SQLiteParameter("@UPC", upc)).FirstOrDefault();
        }
    }
}
