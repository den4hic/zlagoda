using SupermarketDAL.DB;
using SupermarketDAL.Entities;

public class ManagerController
{
    private DatabaseHelper dbHelper;

    public ManagerController()
    {
        dbHelper = new DatabaseHelper("zlagoda.db");
    }

}