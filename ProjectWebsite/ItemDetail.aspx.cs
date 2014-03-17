using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ItemDetail : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        wrefProjectService.ProjectService service = new wrefProjectService.ProjectService();
        string asin = Request.QueryString["asin"];
        lblItemDesc.Text = service.getItemDescription(asin);
        imgItem.ImageUrl = service.getLargeImageURL(asin);
    }

    protected void onItemSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string asin = Request.QueryString["asin"];

        // Assign asin to select parameter
        e.Command.Parameters["@asin"].Value = asin;
    }
}