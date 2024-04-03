<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EncryptTest.aspx.cs" Inherits="Project.Donna.Test.EncryptTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtPlain" runat="server"></asp:TextBox>
            <asp:Button ID="btnEncrypt" runat="server" Text="암호화" OnClick="btnEncrypt_Click" />
            <asp:TextBox ID="txtEncrypt" runat="server"></asp:TextBox>
            <asp:Button ID="btnDecrypt" runat="server" Text="복호화" OnClick="btnDecrypt_Click" />

            <asp:Literal ID="lblEncrypt" runat="server"></asp:Literal>

            <asp:Literal ID="lblDecrypt" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
