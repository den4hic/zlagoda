using System;
using System.Reflection;
using SupermarketDAL.DB;
using SupermarketDAL.Entities;

class Program
{
    static void Main(string[] args)
    {
        DatabaseHelper dbHelper = new DatabaseHelper("zlagoda.db");
        Employee employee = dbHelper.GetEmployee("EMP001");

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            string name = property.Name;
            object value = property.GetValue(obj, null);
            Console.WriteLine("{0}: {1}", name, value);
        }
    }
}