<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebAdmin.Models.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
        <asp:SqlDataSource ID="SqlDataSourceAns" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TelegramBotConnectionString %>" DeleteCommand="DELETE FROM [Ans] WHERE [id] = @original_id AND [id_ask] = @original_id_ask AND [ind] = @original_ind AND [true_ind] = @original_true_ind AND (([ans_txt] = @original_ans_txt) OR ([ans_txt] IS NULL AND @original_ans_txt IS NULL))" InsertCommand="INSERT INTO [Ans] ([id_ask], [ind], [true_ind], [ans_txt]) VALUES (@id_ask, @ind, @true_ind, @ans_txt)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Ans]" UpdateCommand="UPDATE [Ans] SET [id_ask] = @id_ask, [ind] = @ind, [true_ind] = @true_ind, [ans_txt] = @ans_txt WHERE [id] = @original_id AND [id_ask] = @original_id_ask AND [ind] = @original_ind AND [true_ind] = @original_true_ind AND (([ans_txt] = @original_ans_txt) OR ([ans_txt] IS NULL AND @original_ans_txt IS NULL))">
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
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSourceAsk" Width="945px">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="ask_txt" HeaderText="ask_txt" SortExpression="ask_txt" />
                <asp:BoundField DataField="ord_numb" HeaderText="ord_numb" SortExpression="ord_numb" />
            </Columns>
        </asp:GridView>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="Id" DataSourceID="SqlDataSourceAsk" Height="50px" Width="940px">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="ask_txt" HeaderText="ask_txt" SortExpression="ask_txt" />
                <asp:BoundField DataField="ord_numb" HeaderText="ord_numb" SortExpression="ord_numb" />
                <asp:CommandField ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSourceAns" Width="942px">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="id_ask" HeaderText="id_ask" SortExpression="id_ask" />
                <asp:BoundField DataField="ind" HeaderText="ind" SortExpression="ind" />
                <asp:BoundField DataField="true_ind" HeaderText="true_ind" SortExpression="true_ind" />
                <asp:BoundField DataField="ans_txt" HeaderText="ans_txt" SortExpression="ans_txt" />
            </Columns>
        </asp:GridView>
        <asp:DetailsView ID="DetailsView2" runat="server" AutoGenerateRows="False" DataKeyNames="id" DataSourceID="SqlDataSourceAns" Height="50px" Width="938px">
            <Fields>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="id_ask" HeaderText="id_ask" SortExpression="id_ask" />
                <asp:BoundField DataField="ind" HeaderText="ind" SortExpression="ind" />
                <asp:BoundField DataField="true_ind" HeaderText="true_ind" SortExpression="true_ind" />
                <asp:BoundField DataField="ans_txt" HeaderText="ans_txt" SortExpression="ans_txt" />
                <asp:CommandField ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
    </form>
</body>
</html>
