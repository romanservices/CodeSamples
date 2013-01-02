<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.master" AutoEventWireup="true" Inherits="ASEAdminSchoolAdd" Codebehind="ASEAdminSchoolAdd.aspx.cs" %>
<%@ Register src="Controls/DetailSchoolAdd.ascx" tagname="DetailSchoolAdd" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentSecondaryNav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentDetail" Runat="Server">
    <uc3:DetailSchoolAdd ID="DetailSchoolAdd1" runat="server" />
</asp:Content>

