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
/// Summary description for ProjectServiceLogic
/// </summary>
public class ProjectServiceLogic
{
	public ProjectServiceLogic()
	{
		
	}

    public void addItemToDatabase(string asin)
    {
        string description = getItemDescription(asin);
        string price = getCurrentPrice(asin);
        string lastStoredPrice = "0";
        string url = getImageURL(asin);
        DateTime date = DateTime.Now;

        OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings["databaseString"].ConnectionString);
        conn.Open();
        OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
        cmd.Connection = conn;
        cmd.CommandText = "Insert Into User_Item (UserID, ItemID) values ('testuser', '" + asin + "')";
        cmd.ExecuteNonQuery();

        //only insert into price if price is different from current price
        try
        {
            cmd.CommandText = "Select Price From Price Where ItemID = '" + asin + "' Order By PriceDate DESC";
            lastStoredPrice = (cmd.ExecuteScalar()).ToString();
        }
        catch (Exception)
        {
            //do nothing if no record found in price
        }

        if (lastStoredPrice != price)
        {
            cmd.CommandText = "Insert Into Price(ItemID, Price, PriceDate) values ('" + asin + "', '" + price + "', '" + date + "')";
            cmd.ExecuteNonQuery();
        }

        try
        {
            cmd.CommandText = "Insert Into Item (ItemID, Description, ImageURL, CurPrice, CurDate) values ('" + asin + "', '" + description + "', '" + url + "', '" + price + "', '" + date + "')";
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            //do nothing if duplicate
        }
        conn.Close();
    }

    public string getCurrentPrice(string itemID)
    {
        const string accessKeyId = "AKIAINHOZEYXDKHXMYUQ";
        const string secretKey = "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";

        // create a WCF Amazon ECS client
        AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
            new BasicHttpBinding(BasicHttpSecurityMode.Transport),
            new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

        // add authentication to the ECS client
        client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

        // prepare an ItemSearch request
        ItemLookupRequest request = new ItemLookupRequest();
        request.ItemId = new string[] { itemID };
        request.ResponseGroup = new string[] { "OfferSummary" };
        request.MerchantId = "Amazon";

        ItemLookup itemSearch = new ItemLookup();
        itemSearch.Request = new ItemLookupRequest[] { request };
        itemSearch.AWSAccessKeyId = accessKeyId;
        itemSearch.AssociateTag = "rgreuel-20";

        // issue the ItemSearch request
        ItemLookupResponse response = client.ItemLookup(itemSearch);

        // write out the results
        Item returnedItem = response.Items[0].Item.First();

        return (returnedItem.OfferSummary.LowestNewPrice.FormattedPrice).Substring(1, returnedItem.OfferSummary.LowestNewPrice.FormattedPrice.Length - 1);
    }

   /* public bool checkForPriceChanges()
    {
        DateTime date = DateTime.Now;
        bool changed = false;
        List<string> items = new List<string>();
        string lastStoredPrice;
        string currentPrice;
        // count = (Int32)cmd.ExecuteScalar();
        OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings["databaseString"].ConnectionString);
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
            currentPrice = getCurrentPrice(itemNum);

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
                    changed = true;
                }
            }
            catch
            {
                //do nothing if the price was too low to display
            }
        }

        //no changes
        return changed;
    } */

    public string getItemDescription(string asin)
    {
        const string accessKeyId = "AKIAINHOZEYXDKHXMYUQ";
        const string secretKey = "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";

        // create a WCF Amazon ECS client
        AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
            new BasicHttpBinding(BasicHttpSecurityMode.Transport),
            new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

        // add authentication to the ECS client
        client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

        // prepare an ItemSearch request
        ItemLookupRequest request = new ItemLookupRequest();
        request.ItemId = new string[] { asin };
        request.ResponseGroup = new string[] { "ItemAttributes" };
        request.MerchantId = "Amazon";

        ItemLookup itemSearch = new ItemLookup();
        itemSearch.Request = new ItemLookupRequest[] { request };
        itemSearch.AWSAccessKeyId = accessKeyId;
        itemSearch.AssociateTag = "rgreuel-20";

        // issue the ItemSearch request
        ItemLookupResponse response = client.ItemLookup(itemSearch);

        Item returnedItem = response.Items[0].Item.First();

        return "<a href=\"http://www.amazon.com/dp/" + asin + "\">" + returnedItem.ItemAttributes.Title + "</a>";
    }

    public string getLargeImageURL(string asin)
    {
        const string accessKeyId = "AKIAINHOZEYXDKHXMYUQ";
        const string secretKey = "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";

        // create a WCF Amazon ECS client
        AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
            new BasicHttpBinding(BasicHttpSecurityMode.Transport),
            new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

        // add authentication to the ECS client
        client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

        // prepare an ItemSearch request
        ItemLookupRequest request = new ItemLookupRequest();
        request.ItemId = new string[] { asin };
        request.ResponseGroup = new string[] { "Images" };
        request.MerchantId = "Amazon";

        ItemLookup itemSearch = new ItemLookup();
        itemSearch.Request = new ItemLookupRequest[] { request };
        itemSearch.AWSAccessKeyId = accessKeyId;
        itemSearch.AssociateTag = "rgreuel-20";

        // issue the ItemSearch request
        ItemLookupResponse response = client.ItemLookup(itemSearch);

        Item returnedItem = response.Items[0].Item.First();

        return returnedItem.LargeImage.URL;
    }

    private string getImageURL(string asin)
    {
        const string accessKeyId = "AKIAINHOZEYXDKHXMYUQ";
        const string secretKey = "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";

        // create a WCF Amazon ECS client
        AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
            new BasicHttpBinding(BasicHttpSecurityMode.Transport),
            new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

        // add authentication to the ECS client
        client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

        // prepare an ItemSearch request
        ItemLookupRequest request = new ItemLookupRequest();
        request.ItemId = new string[] { asin };
        request.ResponseGroup = new string[] { "Images" };
        request.MerchantId = "Amazon";

        ItemLookup itemSearch = new ItemLookup();
        itemSearch.Request = new ItemLookupRequest[] { request };
        itemSearch.AWSAccessKeyId = accessKeyId;
        itemSearch.AssociateTag = "rgreuel-20";

        // issue the ItemSearch request
        ItemLookupResponse response = client.ItemLookup(itemSearch);

        Item returnedItem = response.Items[0].Item.First();

        return "<img src=\"" + returnedItem.MediumImage.URL + "\"/>";
    }
}