<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormAuthorization.aspx.cs" Inherits="ClinicClientView.FormAuthorization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
        <div style="height: 241px">

            <br />
            ФИО&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="textBoxFIO" runat="server" Height="16px" Width="276px" style="margin-bottom: 0px"></asp:TextBox>
            &nbsp;<br />
            <br />
            Почта&nbsp;&nbsp;&nbsp; <asp:TextBox ID="textBoxEmail" runat="server" Height="16px" Width="280px"></asp:TextBox>
            <br />
            <br />
            Пароль&nbsp;
        <asp:TextBox ID="textBoxPassword" runat="server" Height="16px" Width="280px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="RegistrationButton" runat="server" OnClick="RegistrationButton_Click" Text="Зарегистрироваться" />
            <asp:Button ID="SignInButton" runat="server" OnClick="SignInButton_Click" Text="Войти" />

        </div>
    </form>
</body>
</html>
