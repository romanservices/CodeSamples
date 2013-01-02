<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.master" AutoEventWireup="true" Inherits="ASEAdminUserAdd" Codebehind="ASEAdminUserAdd.aspx.cs" %>
<%@ Register src="Controls/DetailUserAdd.ascx" tagname="DetailUserAdd" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentSecondaryNav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentDetail" Runat="Server">
    <uc3:DetailUserAdd ID="DetailUserAdd1" runat="server" OnUserCreated="DetailUserAdd1_UserCreated" />
</asp:Content>


