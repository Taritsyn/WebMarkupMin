<%@ Page Title="Change log" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangeLog.aspx.cs" Inherits="WebMarkupMin.Sample.AspNet45.WebForms.ChangeLog" %>
<%@ OutputCache CacheProfile="CacheCompressedContent5Minutes" VaryByParam="*" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">
<div class="l-main-content">
	<h2><%: Page.Title %></h2>
	<%= Body %>
</div>
</asp:Content>