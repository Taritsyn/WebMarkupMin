<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
	CodeBehind="Default.aspx.cs" Inherits="WebMarkupMin.Sample.AspNet45.WebForms.Default" %>
<%@ OutputCache CacheProfile="CacheCompressedContent5Minutes" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">
<div class="l-main-content">
	<h2>Project Description</h2>
	<%= Body %>
</div>
</asp:Content>