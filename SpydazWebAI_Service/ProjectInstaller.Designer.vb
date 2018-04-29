<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SpydazWebServiceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.SpydazWebServiceInstaller = New System.ServiceProcess.ServiceInstaller()
        '
        'SpydazWebServiceProcessInstaller
        '
        Me.SpydazWebServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService
        Me.SpydazWebServiceProcessInstaller.Installers.AddRange(New System.Configuration.Install.Installer() {Me.SpydazWebServiceInstaller})
        Me.SpydazWebServiceProcessInstaller.Password = Nothing
        Me.SpydazWebServiceProcessInstaller.Username = Nothing
        '
        'SpydazWebServiceInstaller
        '
        Me.SpydazWebServiceInstaller.Description = "SpydazWeb AI Statemachine"
        Me.SpydazWebServiceInstaller.DisplayName = "SpydazWebAI"
        Me.SpydazWebServiceInstaller.ServiceName = "SpydazWebAI_Service"
        Me.SpydazWebServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.SpydazWebServiceProcessInstaller})

    End Sub

    Friend WithEvents SpydazWebServiceProcessInstaller As ServiceProcess.ServiceProcessInstaller
    Friend WithEvents SpydazWebServiceInstaller As ServiceProcess.ServiceInstaller
End Class
