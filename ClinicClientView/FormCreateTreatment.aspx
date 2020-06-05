<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormCreateTreatment.aspx.cs" Inherits="ClinicClientView.FormCreateTreatment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body style="background-color: #D0FFFF">
    <form id="form1" runat="server">
        <div>
            Название
        <asp:TextBox ID="textBoxName" runat="server" Width="200px"></asp:TextBox>
            <br />
            <br />
            Цена&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="textBoxPrice" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="ButtonAdd" runat="server" Text="Добавить" OnClick="ButtonAdd_Click" BackColor="#33CCFF" />
            <asp:Button ID="ButtonChange" runat="server" Text="Изменить" OnClick="ButtonChange_Click" BackColor="#33CCFF" />
            <asp:Button ID="ButtonDelete" runat="server" Text="Удалить" OnClick="ButtonDelete_Click" BackColor="#33CCFF" />
            <asp:Button ID="ButtonUpd" runat="server" Text="Обновить" OnClick="ButtonUpd_Click" BackColor="#33CCFF" />
            <asp:GridView ID="dataGridView" runat="server" OnRowDataBound="dataGridView_RowDataBound">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" SelectText=">>" />
                </Columns>
                <SelectedRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <br />
            <asp:Button ID="ButtonSave" runat="server" Text="Сохранить" OnClick="ButtonSave_Click" BackColor="#33CCFF" />
            <asp:Button ID="ButtonCancel" runat="server" Text="Отмена" OnClick="ButtonCancel_Click" BackColor="#33CCFF" />
        </div>
    </form>
</body>
</html>
