<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View.ascx.vb" Inherits="TAC.DNN..Modules.TAC_NPIinspectorSearch.View" %>

<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>

<table>
	<tr><td colspan="2">Search by <asp:Label ID="lblZipCode" Runat="server">Zip Code</asp:Label>:</td></tr>
	<tr>
		<td><b><asp:Label ID="lblZipCode2" Runat="server">Zip Code</asp:Label>:</b></td>
		<td>
			<asp:TextBox ID="txtMainSearch" Runat="server"></asp:TextBox>
			Within <asp:DropDownList ID="ddlMiles" Runat="server">
				<asp:ListItem Value="10">10 Miles</asp:ListItem>
				<asp:ListItem Value="30" Selected>30 Miles</asp:ListItem>
				<asp:ListItem Value="50">50 Miles</asp:ListItem>
				<asp:ListItem Value="100">100 Miles</asp:ListItem>
			</asp:DropDownList>&nbsp; 
			<dnn:commandbutton id="cmdSearchZip" CssClass="CommandButton" runat="server" imageurl="~/images/icon_search_16px.gif" text="Search" />
		</td>
	</tr>
	<tr><td height="25"></td></tr>
	<tr><td colspan="2">Or Search <asp:Label ID="lblStateText" Runat="server">State</asp:Label></td></tr>
	<!--
	<tr>
		<td><b>City:</b></td>
		<td><asp:TextBox ID="txtCitySearch" Runat="server" Width="200"></asp:TextBox></td>
	</tr>
	<tr>
		<td><b>County:</b></td>
		<td><asp:TextBox ID="txtCountySearch" Runat="server" Width="200"></asp:TextBox></td>
	</tr>
	-->
	<tr>
		<td><b><asp:Label ID="lblStateText2" Runat="server">State</asp:Label>:</b></td>
		<td>
			<asp:DropDownList ID="ddlStateSearch" Runat="server" DataValueField="Abbr" DataTextField="Name" Width="200"></asp:DropDownList> &nbsp;
			<dnn:commandbutton id="cmdSearchCCS" CssClass="CommandButton" runat="server" imageurl="~/images/icon_search_16px.gif" text="Search" />
		</td>
	</tr>
</table>

<br>
<asp:Label ID="lblResults" Runat="server" Visible="False"><b>Your search returned 0 results</b></asp:Label>
<asp:DataGrid ID="dgSearchResults" DataKeyField="memberID" Runat="server" ShowHeader="False" EnableViewState="False" AutoGenerateColumns="False" Width="100%" CellPadding="5">
<AlternatingItemStyle BackColor="#EAEAEA"></AlternatingItemStyle>
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="50%" ItemStyle-VerticalAlign="Top">
			<ItemTemplate>
			
				<b><%# DataBinder.Eval(Container, "DataItem.webLocation3")%></b><br>
				Serving <%# DataBinder.Eval(Container, "DataItem.webLocation2")%><br>
				<%#IIf((Len(DataBinder.Eval(Container, "DataItem.miles")) > 0), DataBinder.Eval(Container, "DataItem.miles") & " miles away", "") %>
					
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ItemStyle-Width="50%" ItemStyle-VerticalAlign="Top">
			<ItemTemplate>
				<b><%# DataBinder.Eval(Container, "DataItem.displayname")%></b>
				<%#IIf((DataBinder.Eval(Container, "DataItem.phoneShow") = "True"), "<br>" & DataBinder.Eval(Container, "DataItem.phone"), "") %>
				<%# iif((DataBinder.Eval(Container, "DataItem.phone2Show") = "True"), "<br>" & DataBinder.Eval(Container, "DataItem.phone2"), "") %>
				<%# iif((DataBinder.Eval(Container, "DataItem.phone3Show") = "True"), "<br>" & DataBinder.Eval(Container, "DataItem.phone3"), "") %>
				<%# iif((DataBinder.Eval(Container, "DataItem.phone4Show") = "True"), "<br>" & DataBinder.Eval(Container, "DataItem.phone4"), "") %>
				<%# iif((DataBinder.Eval(Container, "DataItem.cellShow") = "True"), "<br>cell: " & DataBinder.Eval(Container, "DataItem.cell"), "") %>
				<br><a href="mailto:<%# DataBinder.Eval(Container, "DataItem.email")%>"><%# DataBinder.Eval(Container, "DataItem.email")%></a>
				<br><asp:HyperLink ID="hpWebSite" runat="server">Visit my Website >></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>