using SupermarketDAL.DB;
using SupermarketDAL.Entities;

public class CashierController
{
    private DatabaseHelper dbHelper;

    public CashierController()
    {
        dbHelper = new DatabaseHelper("zlagoda.db");
    }

    
}