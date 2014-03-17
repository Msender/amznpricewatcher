<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemDetail.aspx.cs" Inherits="ItemDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Price History</title>
    <link rel = "stylesheet" type = "text/css" href = "StyleSheet.css">
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
       <table> <h1>Price History</h1> </table>
    </div>
    <div id="nav">
        <a href="WatchList.aspx">Back</a>
            
    </div>

    <div id="body">
        <center>
       <br /> <asp:Label ID="lblItemDesc" runat="server" Text=""></asp:Label><br /><br />
        <table cellspacing="10" cellpadding="10">
            <tr>
                <td style="vertical-align:top">
                    <asp:Image ID="imgItem" runat="server" />
                </td>
                <td style="vertical-align:top; align-content:center">        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="PriceDate" HeaderText="Date" SortExpression="PriceDate" />
                <asp:BoundField DataField="Price" DataFormatString="{0:C}" HeaderText="Price" SortExpression="Price" />
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
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT PriceDate, Price
FROM Price WHERE ItemID = @asin 
ORDER BY PriceDate DESC" OnSelecting ="onItemSelecting">
            <SelectParameters>
           <asp:Parameter Name="@asin" Type="String" />
        </SelectParameters>       
        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
