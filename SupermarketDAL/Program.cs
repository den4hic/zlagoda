using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL
{
	internal class Program
	{
        static void Main(string[] args)
        {
            // Ініціалізація DatabaseHelper з шляхом до бази даних
            DatabaseHelper dbHelper = new DatabaseHelper("../../../zlagoda.db");
            dbHelper.ResetDatabase();
            dbHelper.PutTestsData();
            TestGetsList(dbHelper);
            TestGets(dbHelper);
        }

        static void TestGetsList(DatabaseHelper dbHelper)
        {
            TestGetEmployeesList(dbHelper);
            TestGetProductsList(dbHelper);
            TestGetChecksList(dbHelper);
            TestGetCustomerCardsList(dbHelper);
            TestGetStoreProductsList(dbHelper);
            TestGetSalesList(dbHelper);
            TestGetProductsListByCategory(dbHelper, 1);
            TestGetGoods(dbHelper);
            TestGetCategoriesList(dbHelper);
        }


        static void TestGets(DatabaseHelper dbHelper)
        {
            TestGetEmployeeById(dbHelper, "EMP001"); // Припустимо, що  існує
            TestGetEmployeeById(dbHelper, "EMP9999"); // Припустимо, що не існує

            TestGetProductById(dbHelper, 1);
            TestGetProductById(dbHelper, 9999);

            TestGetCheckById(dbHelper, "CHECK001"); // Припустимо, що "CHECK001" існує
            TestGetCheckById(dbHelper, "CHECK9999"); // Припустимо, що "CHECK9999" не існує

            TestGetCustomerCardById(dbHelper, "1234567890123");
            TestGetCustomerCardById(dbHelper, "CARD9999");

            TestGetStoreProductById(dbHelper, "123456789012");
            TestGetStoreProductById(dbHelper, "UPC9999999999");

            TestGetSaleById(dbHelper, "123456789012", "CHECK001");
            TestGetSaleById(dbHelper, "SALE9999", "CHECK999");

            TestGetEmployeeByUsernameAndPassword(dbHelper, "user123", "hashed_password123");
            TestGetEmployeeByUsernameAndPassword(dbHelper, "employee456213xxxxxxx", "hashed_password456xxxxxxxxx");
        }


        static void TestGetCategoriesList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetCategoriesList...");
            var categories = dbHelper.GetCategoriesList();
            foreach (var category in categories)
            {
                Console.WriteLine($"Employee: {category.CategoryName}");
            }
        }
        static void TestGetEmployeesList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetEmployeesList...");
            var employees = dbHelper.GetEmployeesList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee: {employee.EmplName} {employee.EmplSurname}");
            }
        }

        static void TestGetProductsList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetProductsList...");
            var products = dbHelper.GetProductsList();
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.ProductName}");
            }
        }

        static void TestGetChecksList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetChecksList...");
            var checks = dbHelper.GetChecksList();
            foreach (var check in checks)
            {
                Console.WriteLine($"Check: {check.CheckNumber}");
            }
        }

        static void TestGetCustomerCardsList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetCustomerCardsList...");
            var customerCards = dbHelper.GetCustomerCardsList();
            foreach (var card in customerCards)
            {
                Console.WriteLine($"Customer Card: {card.CardNumber}");
            }
        }

        static void TestGetStoreProductsList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetStoreProductsList...");
            var storeProducts = dbHelper.GetStoreProductsList();
            foreach (var storeProduct in storeProducts)
            {
                Console.WriteLine($"Store Product: {storeProduct.UPC}");
            }
        }

        static void TestGetSalesList(DatabaseHelper dbHelper)
        {
            Console.WriteLine("Testing GetSalesList...");
            var sales = dbHelper.GetSalesList();
            foreach (var sale in sales)
            {
                Console.WriteLine($"Sale: {sale.UPC}");
            }
        }

        static void TestGetProductsListByCategory(DatabaseHelper dbHelper, int category)
        {
            Console.WriteLine($"Testing GetProductsListByCategory for category: {category}...");
            var productsByCategory = dbHelper.GetProductsListByCategoryID(category);
            foreach (var product in productsByCategory)
            {
                Console.WriteLine($"Product: {product.ProductName}");
            }
        }

        static void TestGetGoods(DatabaseHelper dbHelper)
        {
            Console.WriteLine($"Testing GetGoods...");
            var goods = dbHelper.GetGoods();
            foreach (var good in goods)
            {
                Console.WriteLine($"Product: {good.ProductName}");
            }
        }

        static void TestGetEmployeeById(DatabaseHelper dbHelper, string id)
        {
            Console.WriteLine($"Testing GetEmployeeById with ID: {id}...");
            var employee = dbHelper.GetEmployeeById(id);
            if (employee != null)
            {
                Console.WriteLine($"Employee found: {employee.EmplName} {employee.EmplSurname}");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        static void TestGetProductById(DatabaseHelper dbHelper, int id)
        {
            Console.WriteLine($"Testing GetProductById with ID: {id}...");
            var product = dbHelper.GetProductById(id);
            if (product != null)
            {
                Console.WriteLine($"Product found: {product.ProductName}");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        static void TestGetCheckById(DatabaseHelper dbHelper, string checkNumber)
        {
            Console.WriteLine($"Testing GetCheckById with check number: {checkNumber}...");
            var check = dbHelper.GetCheckById(checkNumber);
            if (check != null)
            {
                Console.WriteLine($"Check found: {check.CheckNumber}");
            }
            else
            {
                Console.WriteLine("Check not found.");
            }
        }

        static void TestGetCustomerCardById(DatabaseHelper dbHelper, string cardNumber)
        {
            Console.WriteLine($"Testing GetCustomerCardById with card number: {cardNumber}...");
            var card = dbHelper.GetCustomerCardByNumber(cardNumber);
            if (card != null)
            {
                Console.WriteLine($"Customer Card found: {card.CardNumber}");
            }
            else
            {
                Console.WriteLine("Customer Card not found.");
            }
        }

        static void TestGetStoreProductById(DatabaseHelper dbHelper, string upc)
        {
            Console.WriteLine($"Testing GetStoreProductById with UPC: {upc}...");
            var storeProduct = dbHelper.GetStoreProductByUPC(upc);
            if (storeProduct != null)
            {
                Console.WriteLine($"Store Product found: {storeProduct.UPC}");
            }
            else
            {
                Console.WriteLine("Store Product not found.");
            }
        }

        static void TestGetSaleById(DatabaseHelper dbHelper, string upc, string checkNumber)
        {
            Console.WriteLine($"Testing GetSaleById with upc: {upc} and checkNumber: {checkNumber}...");
            var sale = dbHelper.GetSaleByUPCAndCheckNumber(upc, checkNumber);
            if (sale != null)
            {
                Console.WriteLine($"Sale found: {sale.UPC}");
            }
            else
            {
                Console.WriteLine("Sale not found.");
            }
        }

        public static void TestGetEmployeeByUsernameAndPassword(DatabaseHelper dbHelper, string username, string hashedPassword)
        {
            Console.WriteLine($"Testing GetEmployeeByUsernameAndPassword with username: {username}...");
            var employee = dbHelper.GetEmployeeByUsernameAndPassword(username, hashedPassword);
            if (employee != null)
            {
                Console.WriteLine($"Employee found: {employee.IdEmployee}, {employee.EmplName} {employee.EmplSurname}");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        static void TestGetGoodsByUPC(DatabaseHelper dbHelper, string upc)
        {
            Console.WriteLine($"Testing GetGoodsByUPC with UPC: {upc}...");
            var goods = dbHelper.GetGoodsByUPC(upc);
            if (goods != null)
            {
                Console.WriteLine($"Goods found: {goods.ProductName}, UPC: {goods.UPC}");
            }
            else
            {
                Console.WriteLine("Goods not found.");
            }
        }

    }
}
