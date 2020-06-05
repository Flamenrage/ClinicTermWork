<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPatientTreatments.aspx.cs" Inherits="ClinicClientView.FormPatientTreatments" Async="true"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body style="background-color: #D0FFFF">
    <form id="form1" runat="server">
        <div>
            C<asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="16px" Width="148px">
                <DayHeaderStyle BackColor="#33CCFF" Font-Bold="True" Font-Size="7pt" />
                <NextPrevStyle VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#808080" />
                <SelectedDayStyle BackColor="#33CCFF" Font-Bold="True" ForeColor="White" />
                <SelectorStyle BackColor="#D0FFFF" />
                <TitleStyle BackColor="#33CCFF" BorderColor="Black" Font-Bold="True" />
                <TodayDayStyle BackColor="#A9E9FF" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />
            </asp:Calendar>
            По<asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="159px" Width="131px">
                <DayHeaderStyle BackColor="#33CCFF" Font-Bold="True" Font-Size="7pt" />
                <NextPrevStyle VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#808080" />
                <SelectedDayStyle BackColor="#33CCFF" Font-Bold="True" ForeColor="White" />
                <SelectorStyle BackColor="#D0FFFF" />
                <TitleStyle BackColor="#33CCFF" BorderColor="Black" Font-Bold="True" />
                <TodayDayStyle BackColor="#A9E9FF" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />
            </asp:Calendar>
            <asp:Button ID="ButtonMake" runat="server" OnClick="ButtonMake_Click" Text="Отправить отчет" Width="196px" BackColor="#33CCFF" />
            <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Вернуться" Width="196px" BackColor="#33CCFF" />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                 <Columns>
                    <asp:BoundField DataField="Date" HeaderText="Дата" SortExpression="Date" />
                    <asp:BoundField DataField="Name" HeaderText="Имя" SortExpression="Name" />
                    <asp:BoundField DataField="MedicationName" HeaderText="Название лекарства" SortExpression="MedicationName"/>
                    <asp:BoundField DataField="Count" HeaderText="Количество" SortExpression="Count" />
                </Columns>
                <SelectedRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
