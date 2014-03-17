<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WatchList.aspx.cs" Inherits="WatchList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Watchlist</title>
    <link rel = "stylesheet" type = "text/css" href = "StyleSheet.css">
</head>
<body>
    <form id="form1" runat="server">
     <div id="header">
       <table> <h1>Watchlist</h1> </table>
    </div>
    <div id="nav">
        <a href="Default.aspx">Home</a>
    </div>
    <div id="body">
        <br />
        <center><asp:Label ID="lblMessage" runat="server" Text="You aren't watching any items." Visible="False"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnRowDeleting="GridView1_RowDeleting" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="4" OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="ImageURL" SortExpression="ImageURL" HtmlEncode="False" >
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HtmlEncode="False" >
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CurPrice" HeaderText="Price" SortExpression="CurPrice" DataFormatString="{0:C}" >
                <ItemStyle VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CurDate" HeaderText="Date" SortExpression="CurDate" >
                <ItemStyle VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:CommandField SelectText="History" ShowSelectButton="True" >
                <ItemStyle VerticalAlign="Middle" />
                </asp:CommandField>
                <asp:CommandField ShowDeleteButton="True" >
                <ItemStyle VerticalAlign="Middle" />
                </asp:CommandField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT i.ImageURL, i.Description, i.CurPrice, i.CurDate
FROM Item i, User_Item u
WHERE u.ItemID = i.ItemID
ORDER BY CurDate DESC"></asp:SqlDataSource></center>
        </div>
    </form>
</body>
</html>
