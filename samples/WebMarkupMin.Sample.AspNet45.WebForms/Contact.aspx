<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
	CodeBehind="Contact.aspx.cs" Inherits="WebMarkupMin.Sample.AspNet45.WebForms.Contact" %>
<%@ OutputCache CacheProfile="CacheCompressedContent5Minutes" VaryByParam="*" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">
	<div class="l-main-content">
		<h2><%: Page.Title %></h2>
		<%= Body %>
	</div>
	<script>
		!function (d, s, id) {
			var js, fjs = d.getElementsByTagName(s)[0],
				p = /^http:/.test(d.location) ? 'http' : 'https';
			if (!d.getElementById(id)) {
				js = d.createElement(s);
				js.id = id;
				js.src = p + '://platform.twitter.com/widgets.js';
				fjs.parentNode.insertBefore(js, fjs);
			}
		}(document, 'script', 'twitter-wjs');
	</script>
</asp:Content>