using System;
using System.Reflection;
using SupermarketDAL.DB;
using SupermarketDAL.Entities;

class Program
{
    static void Main(string[] args)
    {
        DatabaseHelper dbHelper = new DatabaseHelper("../../../zlagoda.db");
        List<Employee> list = dbHelper.GetEmployeesList();
        Employee employee = list[1];

        Type type = employee.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            string name = property.Name;
            object value = property.GetValue(employee, null);
            Console.WriteLine("{0}: {1}", name, value);
        }

    }
}