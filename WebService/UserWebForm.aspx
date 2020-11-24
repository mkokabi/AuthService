<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserWebForm.aspx.cs" Inherits="WebService.UserWebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Username"></asp:Label>
                        <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Password"></asp:Label>
                        <asp:TextBox ID="tbPassword" runat="server" type="password"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnSubmit" runat="server" Text="Create" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

        </div>
    </form>
</body>
</html>
