<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormDiagram.aspx.cs" Inherits="ClinicClientView.FormDiagram" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="height: 656px; background-color: #D0FFFF">
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Button ID="ButtonReturn" runat="server" OnClick="ButtonReturn_Click" Text="Вернуться" BackColor="#33CCFF" />
        <asp:Button ID="ButtonSendMail" runat="server" OnClick="ButtonSendMail_Click" Text="Отправить на почту" BackColor="#33CCFF" />
        <asp:Button ID="ButtonForm" runat="server" OnClick="ButtonForm_Click" Text="Сформировать" BackColor="#33CCFF" />
        <div>
        <asp:Chart ID="ChartDiagram" runat="server" Height="496px" Width="575px">
            <series>
                <asp:Series Name="Series">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="ChartArea">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
        </div>
    </form>
</body>
</html>
