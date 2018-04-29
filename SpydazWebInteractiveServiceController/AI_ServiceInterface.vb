Imports System
Imports System.ServiceProcess
Imports System.Diagnostics
Imports System.Threading
Imports System.Net.Sockets
Imports System.Text
Imports System.Net
Public Class Reciever
    Implements IDisposable
    Public ThreadReceive As System.Threading.Thread
    Dim receivingUdpClient As New UdpClient
    Dim RemoteIpEndPoint As New System.Net.IPEndPoint(System.Net.IPAddress.Any, 55547)

    Public Sub New()

        receivingUdpClient = New UdpClient(55547)
        ThreadReceive = New System.Threading.Thread(AddressOf Receiving)
        ThreadReceive.Start()
    End Sub
    Public Event DataRecieved(ByRef Str As String)
    Public Sub Receiving()
        Dim strReturnData As String = ""

        Do
            Dim receiveBytes As [Byte]() = receivingUdpClient.Receive(RemoteIpEndPoint)

            strReturnData = System.Text.Encoding.ASCII.GetString(receiveBytes)

            RaiseEvent DataRecieved(strReturnData)

        Loop While strReturnData <> "Stop"

    End Sub

    Protected Sub Dispose() Implements IDisposable.Dispose
        receivingUdpClient.Close()
        ThreadReceive.Abort()
    End Sub
End Class
Public Class AI_ServiceInterface

    Public CurrentState As New AI_STATE
    Public Structure AI_STATE
        Public State As String
    End Structure

    Public WithEvents Recieve As New Reciever

    Public Sub New()



    End Sub


    Public Event NewDataArrived(ByRef Str As String)
    Dim scServices() As ServiceController
    Dim scTemp As ServiceController
    Public Sub CheckStatus()

        scServices = ServiceController.GetServices()

        For Each scTemp In scServices

            If scTemp.ServiceName = "SpydazWebAI_Service" Then
                ' Display properties for the Simple Service sample 
                ' from the ServiceBase example
                Dim sc As New ServiceController("SpydazWebAI_Service")
                Dim builder As New StringBuilder
                builder.Append("Status = " + sc.Status.ToString() & vbNewLine)
                builder.Append("Can Pause and Continue = " +
                    sc.CanPauseAndContinue.ToString() & vbNewLine)
                builder.Append("Can ShutDown = " + sc.CanShutdown.ToString() & vbNewLine)
                builder.Append("Can Stop = " + sc.CanStop.ToString() & vbNewLine)
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

                'sc.ExecuteCommand(Fix(129))

                builder.Append("Status = " + sc.Status.ToString() & vbNewLine)
                sc.Continue()
                While sc.Status = ServiceControllerStatus.Paused
                    Thread.Sleep(10)
                    sc.Refresh()
                End While
                builder.Append("Status = " + sc.Status.ToString() & vbNewLine)
                sc.Continue()
                RaiseEvent NewDataArrived(builder.ToString)
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
    Private Sub DataArrived(ByRef Str As String) Handles Recieve.DataRecieved
        Dim CaseNo As Integer = 0
        Dim Info As String = ""
        If Str.Contains("Request for Get State") = True Then


            ' SetState(Info)
        Else
        End If
        RaiseEvent NewDataArrived(Str.Replace("Request for Get State", ""))
    End Sub

    Public Sub PerformCommand(ByRef Cmd As Integer)
        scServices = ServiceController.GetServices()
        For Each scTemp In scServices
            If scTemp.ServiceName = "SpydazWebAI_Service" Then
                ' Display properties for the Simple Service sample 
                ' from the ServiceBase example
                Dim sc As New ServiceController("SpydazWebAI_Service")
                'Perform Cmd
                sc.ExecuteCommand(Fix(Cmd))

                sc.Continue()

            Else
            End If
        Next scTemp
    End Sub
    ''' <summary>
    ''' The emotion type can be used to catagorize some of the basic emotions the list is just a
    ''' basic collection of emotiontypes ; the emotion finder class uses these basic emotion
    ''' types to describe the current emotion detected by the class. this type list is no a
    ''' complete list.
    ''' </summary>
    Public Enum EmotionType
        Joy = 131
        Happy
        Sad
        Love
        Laughing
        Surprised
        Sleepy
        Serious
        Angry
        Jealous
        curious
        Concerned
        Failure
        Fear
        Greatful
        Neutral
    End Enum
    Public Sub SetState(ByRef Str As EmotionType)
        Select Case Str
            Case EmotionType.Joy
                PerformCommand(131)
            Case EmotionType.Happy
                PerformCommand(132)
            Case EmotionType.Sad
                PerformCommand(133)
            Case EmotionType.Love
                PerformCommand(134)
            Case EmotionType.Laughing
                PerformCommand(135)
            Case EmotionType.Surprised
                PerformCommand(136)
            Case EmotionType.Sleepy
                PerformCommand(137)
            Case EmotionType.Serious
                PerformCommand(138)
            Case EmotionType.Angry
                PerformCommand(139)
            Case EmotionType.Jealous
                PerformCommand(140)
            Case EmotionType.curious
                PerformCommand(141)
            Case EmotionType.Concerned
                PerformCommand(142)
            Case EmotionType.Failure
                PerformCommand(143)
            Case EmotionType.Fear
                PerformCommand(144)
            Case EmotionType.Greatful
                PerformCommand(145)
            Case EmotionType.Neutral
                PerformCommand(146)
        End Select

        CurrentState.State = Str.ToString
    End Sub
    Public Sub GetState()
        PerformCommand(129)
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()

    End Sub
End Class 'Program

