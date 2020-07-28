<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="Settings.ascx.vb" Inherits="TAC.DNN..Modules.TAC_NPIinspectorSearch.Settings" %>

<table>
	<tr>
		<td>
			Data Source:
		</td>
		<td>
			<asp:DropDownList ID="ddlDSource" Runat="server">
				<asp:ListItem>NPI</asp:ListItem>
				<asp:ListItem>GPI</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
</table>