' Copyright (c) 2018  npiweb.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.Utilities
Imports TAC.DNN.Modules.TAC_NPIDal2.Components
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.UI.WebControls

''' <summary>
''' The View class displays the content
''' 
''' Typically your view control would be used to display content or functionality in your module.
''' 
''' View may be the only control you have in your project depending on the complexity of your module
''' 
''' Because the control inherits from TAC_NPIinspectorSearchModuleBase you have access to any custom properties
''' defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
''' 
''' </summary>
Partial Class View
    Inherits TAC_NPIinspectorSearchModuleBase
    Implements IActionable

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Page_Load runs when the control is loaded
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
#Region "Controls"
    Protected WithEvents cmdSearchZip As CommandButton
    Protected WithEvents cmdSearchCCS As CommandButton
    Protected WithEvents txtMainSearch As TextBox
    Protected WithEvents ddlMiles As DropDownList
    Protected WithEvents ddlStateSearch As DropDownList
    Protected WithEvents lblResults As Label
    Protected WithEvents dgSearchResults As DataGrid
    Protected WithEvents lblZipCode As Label
    Protected WithEvents lblZipCode2 As Label
    Protected WithEvents lblStateText As Label
    Protected WithEvents lblStateText2 As Label
#End Region

#Region "Page Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then

                Dim sc As New StatesController
                If CType(Settings("DSource"), String) = "GPI" Then
                    Me.lblZipCode.Text = "Postal Code"
                    Me.lblZipCode2.Text = "Postal Code"
                    Me.lblStateText.Text = "Province"
                    Me.lblStateText2.Text = "Province"
                    Me.ddlStateSearch.DataSource = sc.GetStatesByCountry("Canada")
                    Me.ddlStateSearch.DataBind()
                    Dim newItem As New ListItem
                    newItem.Value = ""
                    newItem.Text = "Select A Province"
                    Me.ddlStateSearch.Items.Insert(0, newItem)
                Else
                    Me.ddlStateSearch.DataSource = sc.GetStatesByCountry("USA")
                    Me.ddlStateSearch.DataBind()
                    Dim newItem As New ListItem
                    newItem.Value = ""
                    newItem.Text = "Select A State"
                    Me.ddlStateSearch.Items.Insert(0, newItem)
                End If

                If Not (Request.Params("searchFor") Is Nothing) And Len(Request.Params("searchFor")) > 0 Then
                    If CType(Settings("DSource"), String) = "GPI" Then
                        searchCanadaZips(Request.Params("searchFor"), 30)
                    Else
                        searchUSAZips(Request.Params("searchFor"), 30)
                        Me.txtMainSearch.Text = Request.Params("searchFor")
                    End If
                End If

                If Not (Request.Params("searchState") Is Nothing) And Len(Request.Params("searchState")) > 0 Then
                    If CType(Settings("DSource"), String) = "GPI" Then
                        searchCanada(Null.NullString, Null.NullString, Request.Params("searchState"))
                    Else
                        searchUSA(Null.NullString, Null.NullString, Request.Params("searchState"))
                    End If
                End If

            End If
        Catch exc As Exception
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

    Public Sub dgSearchResults_itemDataBind(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dgSearchResults.ItemDataBound

        'copied from decompiled code using refactor - AJL 6-21-2010
        If ((e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem)) Then
            Dim enumerator As IEnumerator
            Dim link As HyperLink = DirectCast(e.Item.FindControl("hpWebSite"), HyperLink)
            Dim portalAliasArrayByPortalID As ArrayList = New PortalAliasController().GetPortalAliasArrayByPortalID(CType(DataBinder.Eval(e.Item.DataItem, "PortalID"), Integer))
            Try
                enumerator = portalAliasArrayByPortalID.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As PortalAliasInfo = DirectCast(enumerator.Current, PortalAliasInfo)
                    link.NavigateUrl = ("http://" & current.HTTPAlias)
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator, IDisposable).Dispose()
                End If
            End Try
        End If

    End Sub

    Sub searchUSAZips(ByVal searchZip As String, ByVal searchMiles As Integer)
        Try

            Dim cc As New CompanyInfoController

            'Check for Single Zip
            Dim zlc As New MembersZipCodeLinkController
            'Dim objZip As NPI_members_zipcodelinkCollection = DataRepository.NPI_members_zipcodelinkProvider.GetByZip(searchZip)

            Dim sc As New StatesController
            Dim objZip As NPI_members_zipcodelinkCollection = DataRepository.NPI_members_zipcodelinkProvider.GetByZipActive(searchZip)
            If objZip.Count = 1 Then
                'Dim objMember As NPI_members = DataRepository.NPI_membersProvider.GetByMemberID(objZip(0).MemberID)
                'If Not objMember Is Nothing Then

                Dim objCompany As CompanyInfo
                'objCompany = DataRepository.NPI_CompanyInfoProvider.GetByCompanyID(objMember.CompanyID)
                objCompany = cc.GetCompanyInfoByCompanyId(objZip(0).CompanyID)
                'check to make sure portal is not zero
                If objCompany.PortalID <> 0 Then
                    Dim arr As ArrayList
                    Dim p As New PortalAliasController
                    Dim pa As PortalAliasInfo
                    arr = p.GetPortalAliasArrayByPortalID(objCompany.PortalID)
                    For Each pa In arr
                        Response.Redirect("http://" & pa.HTTPAlias)
                    Next
                End If
                'End If
            End If

            Dim objResults As DataSet = DataRepository.NPI_membersProvider.FindCloseUSA(searchZip, searchMiles)
            If objResults.Tables(0).Rows.Count > 0 Then
                Me.dgSearchResults.DataSource = objResults
                Me.dgSearchResults.DataBind()
                Me.lblResults.Visible = False
            Else
                Me.lblResults.Visible = True
            End If
        Catch exc As Exception
            ProcessModuleLoadException(Me, exc)
        End Try

    End Sub

    Sub searchUSA(ByVal searchCity As String, ByVal searchCounty As String, ByVal searchState As String)
        Try
            Dim mc As New MembersInfoController
            Dim objResults As DataSet = DataRepository.NPI_membersProvider.FindByCityCountyStateUSA(searchCity, searchCounty, searchState)
            If objResults.Tables.Count > 0 Then
                If objResults.Tables(0).Rows.Count > 0 Then
                    Me.dgSearchResults.DataSource = objResults
                    Me.dgSearchResults.DataBind()
                    Me.lblResults.Visible = False
                Else
                    Me.lblResults.Visible = True
                End If
            Else
                Me.lblResults.Visible = True
            End If
        Catch exc As Exception
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

    Sub searchCanadaZips(ByVal searchZip As String, ByVal searchMiles As Integer)

        Dim cc As New CompanyInfoController
        Dim zlc As New MembersZipCodeLinkController
        Dim objZip As IEnumerable(Of MembersZipCodeLink) = DataRepository.NPI_members_zipcodelinkProvider.GetByZipActive(searchZip)
        If objZip.Count = 0 And searchZip.Length = 6 Then
            Dim searchZipWithSpace As String = searchZip.Substring(0, 3) & " " & searchZip.Substring(3, 3)
            objZip = DataRepository.NPI_members_zipcodelinkProvider.GetByZipActive(searchZipWithSpace)
        End If
        If objZip.Count = 1 Then
            'Dim objMember As NPI_members = DataRepository.NPI_membersProvider.GetByMemberID(objZip(0).MemberID)
            'If Not objMember Is Nothing Then

            Dim objCompany As CompanyInfo
            'objCompany = DataRepository.NPI_CompanyInfoProvider.GetByCompanyID(objMember.CompanyID)
            objCompany = cc.GetCompanyInfoByCompanyId(objZip(0).CompanyID)
            'check to make sure portal is not zero
            If objCompany.PortalID <> 0 Then
                Dim arr As ArrayList
                Dim p As New PortalAliasController
                Dim pa As PortalAliasInfo
                arr = p.GetPortalAliasArrayByPortalID(objCompany.PortalID)
                For Each pa In arr
                    Response.Redirect("http://" & pa.HTTPAlias)
                Next
            End If
            'End If
        End If

        Dim objResults As DataSet = DataRepository.NPI_membersProvider.FindCloseCanada(searchZip, searchMiles)
        If objResults.Tables.Count > 0 Then
            If objResults.Tables(0).Rows.Count > 0 Then
                Me.dgSearchResults.DataSource = objResults
                Me.dgSearchResults.DataBind()
                Me.lblResults.Visible = False
            Else
                Me.lblResults.Visible = True
            End If
        Else
            Me.lblResults.Visible = True
        End If
    End Sub

    Sub searchCanada(ByVal searchCity As String, ByVal searchCounty As String, ByVal searchState As String)
        Dim mc As New MembersInfoController
        Dim objResults As DataSet = DataRepository.NPI_membersProvider.FindByCityCountyStateCanada(searchCity, searchCounty, searchState)
        If objResults.Tables.Count > 0 Then
            If objResults.Tables(0).Rows.Count > 0 Then
                Me.dgSearchResults.DataSource = objResults
                Me.dgSearchResults.DataBind()
                Me.lblResults.Visible = False
            Else
                Me.lblResults.Visible = True
            End If
        Else
            Me.lblResults.Visible = True
        End If
    End Sub

#End Region

#Region "Button Clicks"

#Region "Search"
    Private Sub cmdSearchZip_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearchZip.Click
        If Page.IsValid Then
            If CType(Settings("DSource"), String) = "GPI" Then
                searchCanadaZips(Me.txtMainSearch.Text, CType(Me.ddlMiles.SelectedValue, Integer))
            Else
                searchUSAZips(Me.txtMainSearch.Text, CType(Me.ddlMiles.SelectedValue, Integer))
            End If
        End If
    End Sub

    Private Sub cmdSearchCCS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearchCCS.Click
        If Page.IsValid Then
            If CType(Settings("DSource"), String) = "GPI" Then
                searchCanada(Null.NullString, Null.NullString, Me.ddlStateSearch.SelectedValue)
            Else
                searchUSA(Null.NullString, Null.NullString, Me.ddlStateSearch.SelectedValue)
            End If
        End If
    End Sub
#End Region

#End Region

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Registers the module actions required for interfacing with the portal framework
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements IActionable.ModuleActions
        Get
            Dim Actions As New ModuleActionCollection
            Actions.Add(GetNextActionID, Localization.GetString("EditModule", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
            Return Actions
        End Get
    End Property

End Class