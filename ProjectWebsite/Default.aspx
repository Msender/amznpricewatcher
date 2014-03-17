<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Amazon Price Watcher</title>
    <link rel = "stylesheet" type = "text/css" href = "StyleSheet.css">
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <center><table><tr><td><img src ="images/Amazon-cart.jpg" height ="100px" width ="100px" />&nbsp;&nbsp;</td><td> <h1>Amazon Price Watcher</h1>&nbsp;</div></td></tr></table></center>
    </div>
        <div id="nav">
        <a class="link" href="WatchList.aspx">Watchlist</a>
    </div>
    <div id="body">
      <p>Please enter the Amazon Standard Identification Number (ASIN) of the item you would like to watch. The ASIN can be found in the "Product Details" section of the item page. For an example, see below. </p>
        <asp:Label ID="Label1" runat="server" Text="ASIN:"></asp:Label>
        <asp:TextBox ID="txtASIN" runat="server"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" /><br />
    
        <b><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></b>
        <br /><br />
       
            <h3>Example</h3>
            <img src="images/prodDetails.PNG" /><br /><br />
        </div>
    </form>
</body>
</html>
