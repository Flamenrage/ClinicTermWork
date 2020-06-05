<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormMain.aspx.cs" Inherits="ClinicClientView.FormMain" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        #form1 {
            height: 994px;
            width: 2016px; 
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
</head>
<body style="background-color: #D0FFFF">
    <form id="form1" runat="server">
        <asp:Menu ID="Menu" runat="server" BackColor="#D0FFFF" ForeColor="Black" Height="150px">
            <Items>
                <asp:MenuItem Text="Каталог рецептов" Value="Каталог рецептов" NavigateUrl="~/FormPrescriptions.aspx"></asp:MenuItem>
                <asp:MenuItem Text="Отчеты" Value="Отчеты" NavigateUrl="~/FormPatientTreatments.aspx"></asp:MenuItem>
                <asp:MenuItem Text="Диаграмма" Value="Диаграмма" NavigateUrl="~/FormDiagram.aspx"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Label ID="LabelName" runat="server" Text="Введите формат: doc или xls" style="text-align: center"></asp:Label>
        <br />
        <asp:Button ID="ButtonReserve" runat="server" OnClick="ButtonReserve_Click" Text="Резерв лечения" Width="161px" BackColor="#33CCFF" />
        <asp:TextBox ID="textBoxReport" runat="server" Width="79px"></asp:TextBox>
        <asp:Button ID="ButtonCreateTreatment" runat="server" Text="Выбрать лечение" OnClick="ButtonCreateTreatment_Click" BackColor="#33CCFF" />
        <asp:Button ID="ButtonReviewTreatment" runat="server" Text="Просмотреть лечение" OnClick="ButtonReviewTreatment_Click" BackColor="#33CCFF" />
        <asp:Button ID="ButtonRef" runat="server" Text="Обновить список" OnClick="ButtonRef_Click" Width="177px" BackColor="#33CCFF" />
        <asp:Button ID="ButtonXML" runat="server" Text="Бэкап XML" OnClick="ButtonXML_Click" BackColor="#33CCFF" />
        <asp:Button ID="ButtonJSON" runat="server" Text="Бэкап JSON" OnClick="ButtonJSON_Click" BackColor="#33CCFF" />
        <br />
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