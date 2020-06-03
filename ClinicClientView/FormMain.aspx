<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormMain.aspx.cs" Inherits="ClinicClientView.FormMain" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        #form1 {
            height: 595px;
            width: 1155px;
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Menu ID="Menu" runat="server" BackColor="White" ForeColor="Black" Height="150px">
            <Items>
                <asp:MenuItem Text="Каталог рецептов" Value="Каталог рецептов" NavigateUrl="~/FormPrescriptions.aspx"></asp:MenuItem>
                <asp:MenuItem Text="Отчеты" Value="Отчеты" NavigateUrl="~/FormPatientTreatments.aspx"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Label ID="LabelName" runat="server" Text="Введите формат: doc или xls" style="text-align: center"></asp:Label>
        <br />
        <asp:Button ID="ButtonReserve" runat="server" OnClick="ButtonReserve_Click" Text="Резерв лечения" Width="161px" />
        <asp:TextBox ID="textBoxReport" runat="server" Width="79px"></asp:TextBox>
        <asp:Button ID="ButtonCreateTreatment" runat="server" Text="Выбрать лечение" OnClick="ButtonCreateTreatment_Click" />
        <asp:Button ID="ButtonReviewTreatment" runat="server" Text="Просмотреть лечение" OnClick="ButtonReviewTreatment_Click" />
        <asp:Button ID="ButtonRef" runat="server" Text="Обновить список" OnClick="ButtonRef_Click" Width="177px" />
        <asp:Button ID="ButtonXML" runat="server" Text="Бэкап XML" OnClick="ButtonXML_Click" />
        <asp:Button ID="ButtonJSON" runat="server" Text="Бэкап JSON" OnClick="ButtonJSON_Click" />
        <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" Width="1092px">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                <asp:CommandField ShowSelectButton="true" SelectText=">>" />
                <asp:BoundField DataField="Name" HeaderText="Наименование" SortExpression="Name" />
                <asp:BoundField DataField="Date" HeaderText="Дата" SortExpression="Date" />
                <asp:BoundField DataField="TotalPrice" HeaderText="Стоимость" SortExpression="TotalPrice" />
                <asp:BoundField DataField="IsReserved" HeaderText="Бронь" SortExpression="IsReserved" />
            </Columns>
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
    </form>
</body>
</html>