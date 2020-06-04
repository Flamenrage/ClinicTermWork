<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormDiagram.aspx.cs" Inherits="ClinicClientView.FormDiagram" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="height: 656px">
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Button ID="ButtonReturn" runat="server" OnClick="ButtonReturn_Click" Text="Вернуться" />
        <asp:Button ID="ButtonSendMail" runat="server" OnClick="ButtonSendMail_Click" Text="Отправить на почту" />
        <div>
        <asp:Chart ID="ChartDiagram" runat="server" Height="496px" Width="575px">
            <series>
                <asp:Series Name="Series1">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
        </div>
    </form>
</body>
</html>
