using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        wrefProjectService.ProjectService service = new wrefProjectService.ProjectService();

        try
        {
            //first check if asin is valid
            string test = service.getCurrentPrice(txtASIN.Text);

            //add to database
            if (service.addItemToDatabase(txtASIN.Text))
            {
                lblMessage.Text = "Item added to watchlist!";
            }
            else
            {
                lblMessage.Text = "Error: Verify ASIN is valid and not already in your watchlist!";
            }
        }
            catch (Exception)
        {
            lblMessage.Text = "Error: Verify ASIN is valid and not already in your watchlist!";
            //lblMessage.Text = ex.Message;
        }
    }
}