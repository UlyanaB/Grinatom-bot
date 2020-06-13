<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BestUsers.aspx.cs" Inherits="WebAdmin.WebForms.BestUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Menu ID="Menu1" runat="server" BackColor="#B5C7DE" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" StaticSubMenuIndent="10px">
                <DynamicHoverStyle BackColor="#284E98" ForeColor="White" />
                <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                <DynamicMenuStyle BackColor="#B5C7DE" />
                <DynamicSelectedStyle BackColor="#507CD1" />
                <Items>
                    <asp:MenuItem NavigateUrl="~/WebForms/LoginForm.aspx" Text="На главную страницу" Value="Up"></asp:MenuItem>
                </Items>
                <StaticHoverStyle BackColor="#284E98" ForeColor="White" />
                <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                <StaticSelectedStyle BackColor="#507CD1" />
            </asp:Menu>
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="id" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" OnSelectedIndexChanging="GridView1_SelectedIndexChanging" AllowPaging="True" AllowSorting="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="tlgUserId" HeaderText="tlgUserId" SortExpression="tlgUserId" />
                <asp:BoundField DataField="tlgUserName" HeaderText="tlgUserName" SortExpression="tlgUserName" />
                <asp:BoundField DataField="firstEnter" HeaderText="firstEnter" SortExpression="firstEnter" />
                <asp:BoundField DataField="lastEnter" HeaderText="lastEnter" SortExpression="lastEnter" />
                <asp:BoundField DataField="bestResult" HeaderText="bestResult" SortExpression="bestResult" />
                <asp:ButtonField CommandName="Select" Text="Select" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TelegramBotConnectionString %>" SelectCommand="SELECT top 5 [id], [tlgUserId], [tlgUserName], [firstEnter], [lastEnter], [bestResult] FROM [BotUsers] ORDER BY [bestResult] DESC"></asp:SqlDataSource>
        <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Send" Width="50px" />
        <p>
        <asp:Label ID="Label1" runat="server" Text="."></asp:Label>
            <asp:Label ID="Label2" runat="server" Text="."></asp:Label>
        </p>
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="id" DataSourceID="SqlDataSourceLog" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="dt" HeaderText="dt" SortExpression="dt" />
                <asp:BoundField DataField="BotUsersId" HeaderText="BotUsersId" SortExpression="BotUsersId" />
                <asp:BoundField DataField="TrueAns" HeaderText="TrueAns" SortExpression="TrueAns" />
                <asp:BoundField DataField="AskQuantity" HeaderText="AskQuantity" SortExpression="AskQuantity" />
                <asp:BoundField DataField="TrueAnsQuantity" HeaderText="TrueAnsQuantity" SortExpression="TrueAnsQuantity" />
                <asp:BoundField DataField="AskTxt" HeaderText="AskTxt" SortExpression="AskTxt" />
                <asp:BoundField DataField="AnsTxt" HeaderText="AnsTxt" SortExpression="AnsTxt" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceLog" runat="server" ConnectionString="<%$ ConnectionStrings:TelegramBotConnectionString %>" SelectCommand="SELECT * FROM [UsersLog] where botUsersId = @id ORDER BY [dt] DESC">
            <SelectParameters>
                <asp:Parameter Name="id" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
