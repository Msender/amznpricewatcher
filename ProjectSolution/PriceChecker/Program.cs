using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace PriceChecker
{
    class Program
    {
        static wrefProjectService.ProjectService service = new wrefProjectService.ProjectService();
        static string conString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Ryan\Dropbox\School Stuff\Fall 2013\BUS ADM 531\Project\ProjectWebService\Database.accdb";

        static void Main(string[] args)
        {
            Console.WriteLine("Amazon Price Checker");
            Console.WriteLine("---------------------");

            bool changed = checkForPriceChanges();
            Console.WriteLine("---------------------");
            if (changed)
            {
                Console.WriteLine("Price changes have been saved to the database.");
            }
            else
            {
                Console.WriteLine("There have been no price changes since you last checked.");
            }

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        public static bool checkForPriceChanges()
        {
            DateTime date = DateTime.Now;
            bool changed = false;
            List<string> items = new List<string>();
            string lastStoredPrice;
            string currentPrice;
            // count = (Int32)cmd.ExecuteScalar();
            OleDbConnection conn = new System.Data.OleDb.OleDbConnection(conString);
            //first read unique items into a list
            try
            {
                conn.Open();
                OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select ItemID From User_Item Where UserID = 'testuser'";
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(reader.GetString(0));
                }
            }

            catch (Exception)
            {
                //do nothing if no items found
            }
            conn.Close();

            //compare current price for each item to latest stored price
            foreach (string itemNum in items)
            {
                conn.Open();
                OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.Connection = conn;
                currentPrice = service.getCurrentPrice(itemNum);

                //wait 1.5 seconds for service
                System.Threading.Thread.Sleep(1500);

                cmd.CommandText = "Select Price From Price Where ItemID = '" + itemNum + "' Order By PriceDate DESC";
                lastStoredPrice = (cmd.ExecuteScalar()).ToString();
                conn.Close();

                //sometimes an actual price can't be retrieved because it's lower than a manufacturer's minimum advertised price. in this case "Too low to display" is returned.
                try
                {
                    if (Convert.ToDecimal(lastStoredPrice) != Convert.ToDecimal(currentPrice))
                    {
                        //update currentprice and currentdate in item table
                        conn.Open();
                        cmd.CommandText = "Update Item Set CurPrice = '" + Convert.ToDecimal(currentPrice) + "' Where ItemID = '" + itemNum + "'";
                        cmd.ExecuteNonQuery();
                        //write current date
                        cmd.CommandText = "Update Item Set CurDate = '" + date + "' Where ItemID = '" + itemNum + "'";
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        //add price record to price table
                        conn.Open();
                        cmd.CommandText = "Insert Into Price (ItemID, Price, PriceDate) values ('" + itemNum + "', '" + Decimal.Parse(currentPrice) + "', '" + date + "')";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        Console.WriteLine(itemNum + " change from $" + lastStoredPrice + " to $" + currentPrice);
                        changed = true;
                    }
                    else
                    {
                        Console.WriteLine(itemNum + " no price change.");
                    }
                }
                catch
                {
                    //do nothing if the price was too low to display
                }
            }

            //no changes
            return changed;
        }
    }
}
