<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPrescriptions.aspx.cs" Inherits="ClinicClientView.FormPrescriptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="ButtonRef" runat="server" Text="Обновить" OnClick="ButtonRef_Click" />
            <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                 <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Наименование" SortExpression="Name" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Цена" SortExpression="TotalPrice" />
                </Columns>
                <SelectedRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <br />
            <asp:Button ID="ButtonCancel" runat="server" Text="Вернуться" OnClick="ButtonCancel_Click" />
        </div>
    </form>
</body>
</html>
