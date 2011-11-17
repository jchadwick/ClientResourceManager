<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>

<script runat="server">
protected void Page_Load(object o, EventArgs args)
{
	Context.ClientResources().Include("~/script/homepage.js");
}
</script>

<asp:Content ContentPlaceHolderID="Body" runat="server">
	<h2>Home page!</h2>
</asp:Content>
