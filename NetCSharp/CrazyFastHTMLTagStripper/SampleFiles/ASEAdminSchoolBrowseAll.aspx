<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.master" AutoEventWireup="true" Inherits="ASEAdminSchoolBrowseAll" Codebehind="ASEAdminSchoolBrowseAll.aspx.cs" %>
<%@ Register src="Controls/DetailSchoolBrowseAll.ascx" tagname="DetailSchoolBrowseAll" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentSecondaryNav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentDetail" Runat="Server">
    <uc3:DetailSchoolBrowseAll ID="DetailSchoolBrowseAll1" runat="server" />
</asp:Content>


