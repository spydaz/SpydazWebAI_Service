Imports System
Imports System.ServiceProcess
Imports System.Diagnostics
Imports System.Threading
Imports System.Net.Sockets
Imports System.Text

Class AI_ServiceInterface

    Public CurrentState As AI_STATE
    Public Structure AI_STATE
        Dim State As String
    End Structure
    Public ThreadReceive As System.Threading.Thread
    Dim receivingUdpClient As UdpClient
    Dim RemoteIpEndPoint As New System.Net.IPEndPoint(System.Net.IPAddress.Any, 0)

    Public Sub New()
        RecieveMessages()
    End Sub

    Public Event DataRecieved(ByRef Str As String)

    ''' <summary>
    ''' Commands implemented in SpydazWeb AI Service (onCustomCommand)
    ''' </summary>
    Public Enum AI_ServiceCustomCommands
        StopWorker = 128
        GetState = 129
        Log_AI_Responded = 130
    End Enum 'SimpleServiceCustomCommands
    Public Shared Sub CheckStatus()
        Dim scServices() As ServiceController
        scServices = ServiceController.GetServices()
        Dim scTemp As ServiceController
        For Each scTemp In scServices

            If scTemp.ServiceName = "SpydazWebAI_Service" Then
                ' Display properties for the Simple Service sample 
                ' from the ServiceBase example
                Dim sc As New ServiceController("SpydazWebAI_Service")
                Console.WriteLine("Status = " + sc.Status.ToString())
                Console.WriteLine("Can Pause and Continue = " +
                    sc.CanPauseAndContinue.ToString())
                Console.WriteLine("Can ShutDown = " + sc.CanShutdown.ToString())
                Console.WriteLine("Can Stop = " + sc.CanStop.ToString())
                If sc.Status = ServiceControllerStatus.Stopped Then
                    sc.Start()
                    While sc.Status = ServiceControllerStatus.Stopped
                        Thread.Sleep(1000)
                        sc.Refresh()
                    End While
                End If
                ' Issue custom commands to the service
                ' enum SimpleServiceCustomCommands 
                '    { StopWorker = 128, RestartWorker, CheckWorker };
                'The only values for a custom command that you can define in your application 
                'Or use in OnCustomCommand are those between 128 And 255. 
                'Integers below 128 correspond to system-reserved values.

                'sc.ExecuteCommand(Fix(SimpleServiceCustomCommands.GetState))
                'sc.Pause()
                'While sc.Status <> ServiceControllerStatus.Paused
                'Thread.Sleep(1000)
                ' sc.Refresh()
                ' End While
                Console.WriteLine("Status = " + sc.Status.ToString())
                sc.Continue()
                While sc.Status = ServiceControllerStatus.Paused
                    Thread.Sleep(1000)
                    sc.Refresh()
                End While
                Console.WriteLine("Status = " + sc.Status.ToString())
                sc.Stop()
                While sc.Status <> ServiceControllerStatus.Stopped
                    Thread.Sleep(1000)
                    sc.Refresh()
                End While
                Console.WriteLine("Status = " + sc.Status.ToString())
                'Dim argArray() As String = {"ServiceController arg1", "ServiceController arg2"}
                'sc.Start(argArray)
                While sc.Status = ServiceControllerStatus.Stopped
                    Thread.Sleep(1000)
                    sc.Refresh()
                End While
                Console.WriteLine("Status = " + sc.Status.ToString())
                ' Display the event log entries for the custom commands
                ' and the start arguments.
                Dim el As New EventLog("Application")
                Dim elec As EventLogEntryCollection = el.Entries
                Dim ele As EventLogEntry
                For Each ele In elec
                    If ele.Source.IndexOf("SpydazWebAI_Service.OnCustomCommand") >= 0 Or ele.Source.IndexOf("SimpleService.Arguments") >= 0 Then
                        Console.WriteLine(ele.Message)
                    End If
                Next ele
            End If
        Next scTemp
        ' This sample displays the following output if the Simple Service
        ' sample is running:
        'Status = Running
        'Can Pause and Continue = True
        'Can ShutDown = True
        'Can Stop = True
        'Status = Paused
        'Status = Running
        'Status = Stopped
        'Status = Running
        '4:14:49 PM - Custom command received: 128
        '4:14:49 PM - Custom command received: 129
    End Sub 'Main 
    Private Sub DataArrived(ByRef Str As String) Handles Me.DataRecieved
        Dim CaseNo As Integer = 0
        Dim Info As String = ""
        If Str.Contains("Request for Get State") = True Then
            Info = Str.Remove("Request for Get State")
            CaseNo = 129
            SetState(Info)
        Else
        End If

    End Sub

    Public Sub PerformCommand(ByRef Cmd As AI_ServiceCustomCommands)
        Dim scServices() As ServiceController
        scServices = ServiceController.GetServices()
        Dim scTemp As ServiceController
        For Each scTemp In scServices
            If scTemp.ServiceName = "SpydazWebAI_Service" Then
                ' Display properties for the Simple Service sample 
                ' from the ServiceBase example
                Dim sc As New ServiceController("SpydazWebAI_Service")
                'Perform Cmd
                sc.ExecuteCommand(Fix(Cmd))
                sc.Pause()
                While sc.Status <> ServiceControllerStatus.Paused
                    Thread.Sleep(1000)
                    sc.Refresh()
                End While


            Else
            End If
        Next scTemp
    End Sub
    Public Sub Receiving()
        Dim receiveBytes As [Byte]() = receivingUdpClient.Receive(RemoteIpEndPoint)
        Dim strReturnData As String = System.Text.Encoding.Unicode.GetString(receiveBytes)
        Dim Str As String = Encoding.ASCII.GetChars(receiveBytes)
        RaiseEvent DataRecieved(Str)
    End Sub
    Public Sub RecieveMessages()
        receivingUdpClient = New System.Net.Sockets.UdpClient(22525)
        ThreadReceive = New System.Threading.Thread(AddressOf Receiving)
        ThreadReceive.Start()
    End Sub
    Public Sub SetState(ByRef Str As String)
        CurrentState.State = Str
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        ThreadReceive.Abort()
    End Sub
End Class 'Program

