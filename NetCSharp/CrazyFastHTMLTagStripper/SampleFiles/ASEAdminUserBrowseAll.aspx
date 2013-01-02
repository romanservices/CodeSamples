<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.master" AutoEventWireup="true" Inherits="ASEAdminUserBrowseAll" Codebehind="ASEAdminUserBrowseAll.aspx.cs" %>
<%@ Register src="Controls/DetailUserBrowseAll.ascx" tagname="DetailUserBrowseAll" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentSecondaryNav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentDetail" Runat="Server">
    <uc3:DetailUserBrowseAll ID="DetailUserBrowseAll1" runat="server" />
</asp:Content>


