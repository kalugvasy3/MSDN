Imports System
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.Windows.Controls
Imports System.IO
Imports System.Windows.Markup
Imports System.Reflection
Imports System.Windows

Class MainWindow

    Private baseAddress As Uri = New Uri("http://localhost:8080/WindowsServiceSQL")
    Private host As ServiceHost = New ServiceHost(GetType(SqlServiceContract), baseAddress)

    Private Sub btnStart_Click(sender As Object, e As RoutedEventArgs) Handles btnStart.Click
        Dim sb As ServiceMetadataBehavior = New ServiceMetadataBehavior()
        sb.HttpGetEnabled = True
        host.Description.Behaviors.Add(sb)

        Dim binding As BasicHttpBinding = New BasicHttpBinding
        binding.MaxBufferPoolSize = 2147483647
        binding.MaxReceivedMessageSize = 2147483647
        binding.MaxBufferSize = 2147483647

        host.AddServiceEndpoint(GetType(ISqlServiceContract), binding, baseAddress)

        Try
            host.Open()
            btnStart.IsEnabled = False
        Catch ex As Exception
            MessageBox.Show("You Must Run Application as Administrator.")
        End Try

    End Sub

    Private Sub btnStop_Click(sender As Object, e As RoutedEventArgs) Handles btnStop.Click
        host.Close()
        Environment.Exit(0)
    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        host.Close()
    End Sub
End Class
