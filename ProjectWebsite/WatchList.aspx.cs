using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WatchList : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        /* NOTE: CHECK FOR PRICE CHANGES BY RUNNING THE PRICECHECKER CONSOLE APP
         * 
        //check for price changes goes here
        wrefProjectService.ProjectService service = new wrefProjectService.ProjectService();
        bool changes = service.checkForPriceChanges();
        if (changes)
        {
            MessageBox.Show("An item's price has changed since your last visit!");
        }*/

        if (GridView1.Rows.Count == 0)
        {
            lblMessage.Visible = true;
        }
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string imageUrl;
        wrefProjectService.ProjectService service = new wrefProjectService.ProjectService();

        TableCell cell = GridView1.Rows[e.RowIndex].Cells[0];
        imageUrl = cell.Text;
        service.removeItemFromDatabase(imageUrl);
        
        e.Cancel = true;
        Response.Redirect("WatchList.aspx");
        
    }
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string imageUrl, asin;
        wrefProjectService.ProjectService service = new wrefProjectService.ProjectService();

        TableCell cell = GridView1.Rows[e.NewSelectedIndex].Cells[0];
        imageUrl = cell.Text;
        asin = service.getASIN(imageUrl);

        e.Cancel = true;
        Response.Redirect("ItemDetail.aspx?asin=" + asin);

    }
}