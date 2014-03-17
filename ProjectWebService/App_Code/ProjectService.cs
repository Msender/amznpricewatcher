using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using Amazon.ECS;
using CAmazonAddons;
using System.Data.OleDb;

/// <summary>
/// Summary description for ProjectService
/// </summary>
[WebService(Namespace = "http://www.uwm.edu/rtgreuel")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ProjectService : System.Web.Services.WebService {
    public ProjectServiceLogic serviceLogic = new ProjectServiceLogic();

    public ProjectService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
       
    }

    [WebMethod(Description="Check if the service is running.")]
    public string CheckAlive() {
        return "The service is alive!";
    }

    [WebMethod(Description="This method takes the Amazon Standard Identification Number (unique number identifying an item) and interacts with the database in the following ways: Inserts a record into User_Item to show the item is added to the watchlist, selects the current price of the item and stores in database if it’s not already stored there, and adds the item to the Item table.")]
    public bool addItemToDatabase(string asin)
    {
        try
        {
            serviceLogic.addItemToDatabase(asin);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    [WebMethod(Description="This method removes a specific item from the user’s watchlist (this is done at runtime- the URL of the selected gridview row item is identifying (since it’s unique) and used to key into the Item Table. The corresponding ASIN is taken from the Item table and used to key into the User_Item table to delete the record denoting the item is being watched by the user.")]
    public bool removeItemFromDatabase(string imageURL)
    {
        try
        {
            OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings["databaseString"].ConnectionString);
            conn.Open();
            OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Delete From User_Item Where ItemID = (Select ItemID From Item Where ImageURL = '" + imageURL + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    [WebMethod(Description="This simple method takes the imageURL of an item and finds its corresponding ASIN number from the Item table.")]
    public string getASIN(string imageURL)
    {
        string asin;

        OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings["databaseString"].ConnectionString);
        conn.Open();
        OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
        cmd.Connection = conn;
        cmd.CommandText = "Select ItemID From Item Where ImageURL = '" + imageURL + "'";
        asin = (cmd.ExecuteScalar()).ToString();
        conn.Close();

        return asin;
    }

    [WebMethod(Description="This method takes the itemID (ASIN) as input and checks the Amazon API for its current lowest price (including all sellers) and returns the results in the form of a string.")]
    public string getCurrentPrice(string itemID)
    {
        return serviceLogic.getCurrentPrice(itemID);
    }

   /* [WebMethod(Description="This method is run (currently at page load) to check all of the items in the database for price changes. If there is no change, it returns false. If there is a change, the price changes are stored in the database and the method returns true, so as to alert the user to the changes.")]
    public bool checkForPriceChanges()
    {
        return serviceLogic.checkForPriceChanges();
    }*/

    [WebMethod(Description="This method is a simple method that takes the asin number and retrieves the item’s title (as well as returns it in a way to be displayed as a hyperlink in the gridview) which links to the item’s product page on Amazon.")]
    public string getItemDescription(string asin)
    {
        return serviceLogic.getItemDescription(asin);
    }

    [WebMethod(Description="This method gets the URL for a big picture of the item. This is used for the item’s “price history” page.")]
    public string getLargeImageURL(string asin)
    {
        return serviceLogic.getLargeImageURL(asin);
    }
}
