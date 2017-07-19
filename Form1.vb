Imports Discord

Public Class Form1

    Dim WithEvents Discord As New DiscordClient

    Dim wClient As New System.Net.WebClient

    Dim trigger As String = "?"

    Dim admins() As String = {"Kyle", "blah", "blah1"}


    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            Await Discord.Connect("MzM3MzE0ODE1NjA3MzA4MzAx.DFFEaQ.MwFepc3wsi7Po0HIMHcRXJViq4g", TokenType.Bot)

            Await Task.Delay(500)

            Label1.Text = Discord.CurrentUser.Name
            getState()

            ListBox1.Items.Add(Date.Now + " Successfully connected to Discord")

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try


    End Sub


    Private Async Sub OnMsg(sender As Object, message As Discord.MessageEventArgs) Handles Discord.MessageReceived

        If message.User.Name = "Sushi" Then

            'Ignore the Message

        Else

            Dim msg As String = message.Message.RawText

            If msg.StartsWith(trigger) Then

                If msg.Contains(" ") Then

                    Dim cmd As String = msg.Split(trigger)(1).Split(" ")(0)
                    Dim arg As String = msg.Split(" ")(1)

                    Select Case cmd.ToLower

                        Case "say"
                            Await message.Channel.SendMessage(arg)

                        Case Else
                            Await message.Channel.SendMessage("Invalid Command.")

                    End Select

                Else

                    Dim cmd As String = msg.Split(trigger)(1)

                    Select Case cmd.ToLower

                        Case "help"
                            Await message.Channel.SendMessage("Commands:" + vbNewLine + "say - repeats the message")

                        Case "joke"
                            Await message.Channel.SendMessage(wClient.DownloadString("http://api.icndb.com/jokes/random"))

                        Case "clear"
                            Dim msgs() As Discord.Message = Await message.Channel.DownloadMessages(100)

                            For Each mess As Discord.Message In msgs

                                If mess.User.Name = Discord.CurrentUser.Name Then

                                    Await mess.Delete()

                                ElseIf mess.RawText.StartsWith(trigger) Then

                                    Await mess.Delete

                                End If


                            Next

                        Case "serverinfo"
                            Await message.Channel.SendMessage("Server name: " + message.Server.Name + vbNewLine + "Members: " + CStr(message.Server.UserCount) + vbNewLine + "Roles: " + message.Server.RoleCount + vbNewLine + "Icon: " + message.Server.IconUrl)

                        Case "troll"
                            Await message.Channel.SendTTSMessage("Hello, is it me you're looking for?")



                        Case Else
                            Await message.Channel.SendMessage("Invalid Command.")

                    End Select

                End If

            Else

                'Ignore the message 

            End If

        End If

    End Sub

    Public Sub getState()

        If Discord.State = ConnectionState.Connected Then

            Label2.Text = "Connected"

        ElseIf Discord.State = ConnectionState.Connecting Then

            Label2.Text = "Connecting"
        Else

            Label2.Text = "Uknown"

        End If

    End Sub

    Public Sub Welcomer(sender As Object, e As Discord.UserEventArgs) Handles Discord.UserJoined

        Dim server = e.Server.FindChannels("general").FirstOrDefault()
        server.SendMessage("Welcome to the server" + e.User.Name)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Discord.FindServers("KB").FirstOrDefault().FindChannels("sushi-bot").FirstOrDefault().SendMessage(TextBox1.Text)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Discord.FindServers("KB").FirstOrDefault().FindChannels("sushi-bot").FirstOrDefault().SendTTSMessage(TextBox2.Text)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Discord.SetGame(New Game(TextBox3.Text, GameType.Default, " "))


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If ComboBox1.SelectedItem = "Idle" Then

            Discord.SetStatus(UserStatus.Idle)

        ElseIf ComboBox1.SelectedItem = "Online" Then

            Discord.SetStatus(UserStatus.Online)

        ElseIf ComboBox1.SelectedItem = "Do Not Disturb" Then

            Discord.SetStatus(UserStatus.DoNotDisturb)

        ElseIf ComboBox1.SelectedItem = "Invisible" Then

            Discord.SetStatus(UserStatus.Invisible)

        End If


    End Sub
End Class
