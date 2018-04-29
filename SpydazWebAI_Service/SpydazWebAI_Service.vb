Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class SpydazWebAI_Service
    ''' <summary>
    ''' Commands implemented in SpydazWeb AI Service (onCustomCommand)
    ''' </summary>
    Public Enum AI_ServiceCustomCommands
        StopWorker = 128
        GetState = 129
        Log_AI_Responded = 130
        Joy
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
    End Enum 'SimpleServiceCustomCommands
    'Setting Service Status
    'Services report their status To the Service Control Manager, 
    'so that users can tell whether a service Is functioning correctly. 
    'By Default, services that inherit from ServiceBase report a limited Set Of status settings, 
    'including Stopped, Paused, And Running. 
    'If a service takes a little While To start up, 
    'it might be helpful To report a Start Pending status. 
    'You can also implement the Start Pending And Stop Pending status settings 
    'by adding code that calls into the Windows SetServiceStatus Function.
    Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean
    Private AI_EventLog As System.Diagnostics.EventLog
    'The identifier of the next event to write into the event log
    Private eventId As Integer = 1
    ' Add server.Dispose() to the Dispose method on the partial file.
    Public Shared MessagingPort As Integer = 22525
    ' Add timer.Dispose() to the Dispose method on the partial file.
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Set up a timer to trigger every minute. 
        'a simple polling mechanism
        Dim timer As System.Timers.Timer = New System.Timers.Timer()
        timer.Interval = 60000 ' 60 seconds  
        AddHandler timer.Elapsed, AddressOf Me.OnTimer
        timer.Start()
        ' Add any initialization after the InitializeComponent() call.
        Me.AI_EventLog = New System.Diagnostics.EventLog
        If Not System.Diagnostics.EventLog.SourceExists("SpydazWebAI") Then
            System.Diagnostics.EventLog.CreateEventSource("SpydazWebAI",
            "AI_LOG")
        End If
        AI_EventLog.Source = "SpydazWebAI"
        AI_EventLog.Log = "AI_LOG"

    End Sub
    Public Enum ServiceState
        SERVICE_STOPPED = 1
        SERVICE_START_PENDING = 2
        SERVICE_STOP_PENDING = 3
        SERVICE_RUNNING = 4
        SERVICE_CONTINUE_PENDING = 5
        SERVICE_PAUSE_PENDING = 6
        SERVICE_PAUSED = 7
    End Enum
    Protected Overrides Sub OnContinue()
        AI_EventLog.WriteEntry("In OnContinue.")
        eventId = eventId + 1
    End Sub
    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        AI_EventLog.WriteEntry("In OnStart")
        eventId = eventId + 1
        'The Service Control Manager uses 
        'the dwWaitHint And dwCheckpoint members of the SERVICE_STATUS structure 
        'to determine how much time to wait for a Windows Service to start Or shut down. 
        'If your Then OnStart And OnStop methods run Long, 
        'your service can request more time by calling SetServiceStatus again 
        'With an incremented dwCheckPoint value.

        ' Update the service state to Start Pending.  
        Dim serviceStatus As ServiceStatus = New ServiceStatus()
        serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
        serviceStatus.dwWaitHint = 10000
        SetServiceStatus(Me.ServiceHandle, serviceStatus)







        ' Update the service state to Running.  
        serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
        SetServiceStatus(Me.ServiceHandle, serviceStatus)
    End Sub
    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        'This adds an entry to the event log when the service is stopped:
        AI_EventLog.WriteEntry("In OnStop.")
        eventId = eventId + 1
    End Sub
    'a simple polling mechanism(every 60 seconds)-Do action
    Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
        ' TODO: Insert monitoring activities here.  
        AI_EventLog.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId)
        eventId = eventId + 1
    End Sub
    Public Structure ServiceStatus
        Public dwServiceType As Long
        Public dwCurrentState As ServiceState
        Public dwControlsAccepted As Long
        Public dwWin32ExitCode As Long
        Public dwServiceSpecificExitCode As Long
        Public dwCheckPoint As Long
        Public dwWaitHint As Long
    End Structure
    Public AI_state As String = "Neutral"

    Protected Overrides Sub OnCustomCommand(command As Integer)
        'Do action
        'The only values for a custom command that you can define in your application 
        'Or use in OnCustomCommand are those between 128 and 255. 
        'Integers below 128 correspond to system-reserved values.
        Dim server As New SendInfo
        Select Case command
            'GetState
            Case 129
                Try
                    server.Send("Request for Get State " & AI_state)
                    AI_EventLog.WriteEntry("Requested Get State =" & AI_state)
                    eventId = eventId + 1
                Catch ex As Exception
                    AI_EventLog.WriteEntry("SendError Get State =" & AI_state)
                End Try

            Case 130

                AI_EventLog.WriteEntry("AI Responded")
                eventId = eventId + 1

            Case 131
                'SetState Joy
                AI_EventLog.WriteEntry("AI_state = Joy")
                eventId = eventId + 1
                AI_state = "Joy"
            Case 132
                'SetState Happy
                AI_EventLog.WriteEntry("AI_state = Happy")
                eventId = eventId + 1
                AI_state = "Happy"
            Case 133
                'SetState Sad
                AI_EventLog.WriteEntry("AI_state = Sad")
                eventId = eventId + 1
                AI_state = "Sad"
            Case 134
                'SetState Love
                AI_EventLog.WriteEntry("AI_state = Love")
                eventId = eventId + 1
                AI_state = "Love"
            Case 135
                'SetState Laughing
                AI_EventLog.WriteEntry("AI_state = Laughing")
                eventId = eventId + 1
                AI_state = "Laughing"
            Case 136
                'SetState Sleepy
                AI_EventLog.WriteEntry("AI_state = Surprised")
                eventId = eventId + 1
                AI_state = "Surprised"
            Case 137
                'SetState Angry
                AI_EventLog.WriteEntry("AI_state = Sleepy")
                eventId = eventId + 1
                AI_state = "Sleepy"
            Case 138
                'SetState Serious
                AI_EventLog.WriteEntry("AI_state = Serious")
                eventId = eventId + 1
                AI_state = "Serious"
            Case 139
                'SetState Angry
                AI_EventLog.WriteEntry("AI_state = Angry")
                eventId = eventId + 1
                AI_state = "Angry"
            Case 140
                'SetState Angry
                AI_EventLog.WriteEntry("AI_state = Jealous")
                eventId = eventId + 1
                AI_state = "Jealous"
            Case 141
                'SetState curious
                AI_EventLog.WriteEntry("AI_state = curious")
                eventId = eventId + 1
                AI_state = "curious"
            Case 142
                'SetState Concerned
                AI_EventLog.WriteEntry("AI_state = Concerned")
                eventId = eventId + 1
                AI_state = "Concerned"
            Case 143
                'SetState Failure
                AI_EventLog.WriteEntry("AI_state = Failure")
                eventId = eventId + 1
                AI_state = "Failure"
            Case 144
                'SetState Fear
                AI_EventLog.WriteEntry("AI_state = Fear")
                eventId = eventId + 1
                AI_state = "Fear"
            Case 145
                'SetState Greatful
                AI_EventLog.WriteEntry("AI_state = Greatful")
                eventId = eventId + 1
                AI_state = "Greatful"
            Case 146
                'SetState Neutral
                AI_EventLog.WriteEntry("AI_state = Neutral")
                eventId = eventId + 1
                AI_state = "Neutral"
        End Select
    End Sub

    ''' <summary>
    ''' Implements Send Client
    ''' </summary>
    Public Class SendInfo
        Implements IDisposable

        Public Shared MessagingPort = 55547
        Dim bytCommand As Byte() = New Byte() {}
        Dim udpClient As New UdpClient

        Public Sub New()

            udpClient.Connect(IPAddress.Broadcast, MessagingPort)

        End Sub



        Public Sub Send(ByRef txtMessage As String)
            Try
                bytCommand = Encoding.ASCII.GetBytes(txtMessage)
                udpClient.Send(bytCommand, bytCommand.Length)
            Catch ex As Exception

            End Try

        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            udpClient.Close()
            GC.SuppressFinalize(Me)

        End Sub
    End Class

End Class

