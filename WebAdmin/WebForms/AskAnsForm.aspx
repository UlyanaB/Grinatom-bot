﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AskAnsForm.aspx.cs" Inherits="WebAdmin.Models.WebForm1" %>

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
        <asp:SqlDataSource ID="SqlDataSourceAsk" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TelegramBotConnectionString %>" DeleteCommand="DELETE FROM [Ask] WHERE [Id] = @original_Id AND (([ask_txt] = @original_ask_txt) OR ([ask_txt] IS NULL AND @original_ask_txt IS NULL)) AND [ord_numb] = @original_ord_numb" InsertCommand="INSERT INTO [Ask] ([ask_txt], [ord_numb]) VALUES (@ask_txt, @ord_numb)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Ask]" UpdateCommand="UPDATE [Ask] SET [ask_txt] = @ask_txt, [ord_numb] = @ord_numb WHERE [Id] = @original_Id AND (([ask_txt] = @original_ask_txt) OR ([ask_txt] IS NULL AND @original_ask_txt IS NULL)) AND [ord_numb] = @original_ord_numb">
            <DeleteParameters>
                <asp:Parameter Name="original_Id" Type="Int32" />
                <asp:Parameter Name="original_ask_txt" Type="String" />
                <asp:Parameter Name="original_ord_numb" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ask_txt" Type="String" />
                <asp:Parameter Name="ord_numb" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ask_txt" Type="String" />
                <asp:Parameter Name="ord_numb" Type="Int32" />
                <asp:Parameter Name="original_Id" Type="Int32" />
                <asp:Parameter Name="original_ask_txt" Type="String" />
                <asp:Parameter Name="original_ord_numb" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceAns" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TelegramBotConnectionString %>" DeleteCommand="DELETE FROM [Ans] WHERE [id] = @original_id AND [id_ask] = @original_id_ask AND [ind] = @original_ind AND [true_ind] = @original_true_ind AND (([ans_txt] = @original_ans_txt) OR ([ans_txt] IS NULL AND @original_ans_txt IS NULL))" InsertCommand="INSERT INTO [Ans] ([id_ask], [ind], [true_ind], [ans_txt]) VALUES (@id_ask, @ind, @true_ind, @ans_txt)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Ans] where id_ask = @id_ask0" UpdateCommand="UPDATE [Ans] SET [id_ask] = @id_ask, [ind] = @ind, [true_ind] = @true_ind, [ans_txt] = @ans_txt WHERE [id] = @original_id AND [id_ask] = @original_id_ask AND [ind] = @original_ind AND [true_ind] = @original_true_ind AND (([ans_txt] = @original_ans_txt) OR ([ans_txt] IS NULL AND @original_ans_txt IS NULL))">
            <DeleteParameters>
                <asp:Parameter Name="original_id" Type="Int32" />
                <asp:Parameter Name="original_id_ask" Type="Int32" />
                <asp:Parameter Name="original_ind" Type="String" />
                <asp:Parameter Name="original_true_ind" Type="String" />
                <asp:Parameter Name="original_ans_txt" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="id_ask" Type="Int32" />
                <asp:Parameter Name="ind" Type="String" />
                <asp:Parameter Name="true_ind" Type="String" />
                <asp:Parameter Name="ans_txt" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Name="id_ask0" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="id_ask" Type="Int32" />
                <asp:Parameter Name="ind" Type="String" />
                <asp:Parameter Name="true_ind" Type="String" />
                <asp:Parameter Name="ans_txt" Type="String" />
                <asp:Parameter Name="original_id" Type="Int32" />
                <asp:Parameter Name="original_id_ask" Type="Int32" />
                <asp:Parameter Name="original_ind" Type="String" />
                <asp:Parameter Name="original_true_ind" Type="String" />
                <asp:Parameter Name="original_ans_txt" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSourceAsk" Width="938px" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="ask_txt" HeaderText="ask_txt" SortExpression="ask_txt" />
                <asp:BoundField DataField="ord_numb" HeaderText="ord_numb" SortExpression="ord_numb" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" InsertVisible="False" />
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
        <asp:DetailsView ID="DetailsView1" runat="server" CellPadding="4" DataSourceID="SqlDataSourceAsk" ForeColor="#333333" GridLines="None" Height="50px" Width="936px">
            <AlternatingRowStyle BackColor="White" />
            <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
            <EditRowStyle BackColor="#2461BF" />
            <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
            <Fields>
                <asp:CommandField ShowInsertButton="True" />
            </Fields>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
        </asp:DetailsView>
        <asp:Label ID="Label1" runat="server" Text="  ___________________________        "></asp:Label>
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSourceAns" Width="937px" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="id_ask" HeaderText="id_ask" SortExpression="id_ask" />
                <asp:BoundField DataField="ind" HeaderText="ind" SortExpression="ind" />
                <asp:BoundField DataField="true_ind" HeaderText="true_ind" SortExpression="true_ind" />
                <asp:BoundField DataField="ans_txt" HeaderText="ans_txt" SortExpression="ans_txt" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" InsertVisible="False" />
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
        <asp:DetailsView ID="DetailsView2" runat="server" CellPadding="4" DataSourceID="SqlDataSourceAns" ForeColor="#333333" GridLines="None" Height="50px" Width="936px">
            <AlternatingRowStyle BackColor="White" />
            <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
            <EditRowStyle BackColor="#2461BF" />
            <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
            <Fields>
                <asp:CommandField ShowInsertButton="True" />
            </Fields>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
        </asp:DetailsView>
    </form>
</body>
</html>
