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
Imports DotNetNuke.Services.Exceptions

''' -----------------------------------------------------------------------------
''' <summary>
''' The Settings class manages Module Settings
''' 
''' Typically your settings control would be used to manage settings for your module.
''' There are two types of settings, ModuleSettings, and TabModuleSettings.
''' 
''' ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
''' 
''' TabModuleSettings apply only to the current module on the current page, if you copy that module to
''' another page the settings are not transferred.
''' 
''' If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
''' 
''' Below we have some examples of how to access these settings but you will need to uncomment to use.
''' 
''' Because the control inherits from TAC_NPIinspectorSearchSettingsBase you have access to any custom properties
''' defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
''' </summary>
''' -----------------------------------------------------------------------------
Public Class Settings
    Inherits TAC_NPIinspectorSearchSettingsBase

#Region "Base Method Implementations"


#Region "Controls"
#End Region

    Public Overrides Sub LoadSettings()
        Try
            If Not Page.IsPostBack Then
                ' Load settings from TabModuleSettings: specific to this instance
                Dim dSource As String = Null.NullString
                If (Settings.Contains("DSource")) Then dSource = Settings("DSource").ToString()

                If Len(dSource) > 0 Then
                    Me.ddlDSource.SelectedValue = dSource
                End If
            End If
        Catch exc As Exception
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

    Public Overrides Sub UpdateSettings()
        Try
            Dim objModules As New Entities.Modules.ModuleController

            ' Update TabModuleSettings
            objModules.UpdateTabModuleSetting(TabModuleId, "DSource", Me.ddlDSource.SelectedValue)

            ' Redirect back to the portal home page
            Response.Redirect(NavigateURL(), True)
        Catch exc As Exception
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

#End Region


End Class