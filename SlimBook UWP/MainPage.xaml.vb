' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Imports Windows.Phone.UI.Input
Imports Windows.UI.Popups
Imports Windows.ApplicationModel.Resources
Imports Windows.UI.Core

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public Async Function displayMessageAsync(ByVal title As String, ByVal content As String, ByVal dialogType As String) As Task
        Dim messageDialog = New MessageDialog(content, title)
        If dialogType = "notification" Then
        Else
            messageDialog.Commands.Add(New UICommand("Yes", Nothing))
            messageDialog.Commands.Add(New UICommand("No", Nothing))
            messageDialog.DefaultCommandIndex = 0
        End If

        messageDialog.CancelCommandIndex = 1
        Dim cmdResult = Await messageDialog.ShowAsync()
        If cmdResult.Label = "Yes" Then
            Application.Current.Exit()
        End If
    End Function

    Private Sub Go_Home()
        Dim mwv As Uri 'Contains the source URL for Facebook Touch
        mwv = New Uri(MyWebViewSource & "?sk=h_chr")
        SlimBookUWPWebView.Navigate(New Uri(MyWebViewSource))
    End Sub

    Public Sub CloseSplit()
        If MySplitView.IsPaneOpen Then
            MySplitView.IsPaneOpen = Not MySplitView.IsPaneOpen
        End If
    End Sub

    Private Sub ShowSettings()
        PivotSettingsAbout.SelectedIndex = 0
        Dim number As PackageVersion = Package.Current.Id.Version
        version.Text = String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
        myScrollView.ChangeView(Nothing, 0, Nothing, True)
        privacy.Text = PrivacyInfo
        Info.Visibility = Visibility.Visible
    End Sub

    Private Sub SettingsSetup()
        Dim number As PackageVersion = Package.Current.Id.Version
        version.Text = String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
        privacy.Text = PrivacyInfo
        myScrollView.ChangeView(Nothing, 0, Nothing, True)
    End Sub

    Async Sub BackPressed(sender As Object, e As BackPressedEventArgs)
        CloseSplit()
        Dim AppName As String = Package.Current.DisplayName
        'Handles any Back button presses.
        e.Handled = True
        If SlimBookUWPWebView.CanGoBack Then
            SlimBookUWPWebView.GoBack()
        Else
            Await displayMessageAsync(AppName, "Are you sure you want to exit the app?", "")
        End If
    End Sub

    Private Async Sub Back_Click(sender As Object, e As RoutedEventArgs) Handles Back.Click
        If SlimBookUWPWebView.CanGoBack Then
            SlimBookUWPWebView.GoBack()
        Else
            Await displayMessageAsync("Quit SlimBook UWP", "Are you sure you want to quit the app?", "")
        End If
    End Sub

    Private Async Sub Top_Click(sender As Object, e As RoutedEventArgs) Handles Top.Click
        Dim ScrollToTopString = "var int = setInterval(function(){window.scrollBy(0, -36); if( window.pageYOffset === 0 ) clearInterval(int); }, 0.1);"
        Await SlimBookUWPWebView.InvokeScriptAsync("eval", New String() {ScrollToTopString})
    End Sub

    Private Sub About_Click(sender As Object, e As RoutedEventArgs) Handles About.Click
        PivotSettingsAbout.SelectedIndex = 0
        ShowSettings()
        Info.Visibility = Visibility.Visible
    End Sub

    Private Async Sub GitHub_Click(sender As Object, e As RoutedEventArgs) Handles GitHub.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom/SlimBook-UWP")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Sub HamburgerButton_Click(sender As Object, e As RoutedEventArgs) Handles HamburgerButton.Click
        MySplitView.IsPaneOpen = Not MySplitView.IsPaneOpen
    End Sub

    Private Async Sub hyperLogo_Click(sender As Object, e As RoutedEventArgs) Handles hyperLogo.Click
        Dim logoURL = New Uri("http://www.iconarchive.com/show/outline-icons-by-iconsmind/Book-icon.html")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Async Sub hyperDev_Click(sender As Object, e As RoutedEventArgs) Handles hyperDev.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom/")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Sub SlimBookUWPWebView_NewWindowRequested(sender As WebView, args As WebViewNewWindowRequestedEventArgs) Handles SlimBookUWPWebView.NewWindowRequested
        If args.Uri.AbsoluteUri.Contains(".gif") OrElse args.Uri.AbsoluteUri.Contains("video") Then
            SlimBookUWPWebView.Navigate(args.Uri)
            args.Handled = True
        End If
    End Sub

    Private Sub SlimBookUWPWebView_NavigationFailed(sender As Object, e As WebViewNavigationFailedEventArgs) Handles SlimBookUWPWebView.NavigationFailed
        Dim loader = New ResourceLoader()
        Dim noConnection As String = loader.GetString("noConnection")
        SlimBookUWPWebView.NavigateToString(noConnection)
    End Sub

    Private Sub Home_Click(sender As Object, e As RoutedEventArgs) Handles Home.Click
        Go_Home()
    End Sub

    Private Sub CloseGrid_Click(sender As Object, e As RoutedEventArgs) Handles CloseGrid.Click
        Info.Visibility = Visibility.Collapsed
    End Sub

    Private Sub BlankPage1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.InitializeComponent()
        AddHandler HardwareButtons.BackPressed, AddressOf BackPressed
        If localSettings.Values.ContainsKey("fullScreen") Then
            If CBool(localSettings.Values("fullScreen")) Then View.TryEnterFullScreenMode()
        Else View.ExitFullScreenMode()
        End If
        Info.Visibility = Visibility.Collapsed
        ProgRing.IsActive = True
        Go_Home()
        AddHandler SystemNavigationManager.GetForCurrentView().BackRequested, Sub(s, a)

                                                                                  If SlimBookUWPWebView.CanGoBack Then
                                                                                      SlimBookUWPWebView.GoBack()
                                                                                      a.Handled = True
                                                                                  End If
                                                                              End Sub
    End Sub

    Private Async Sub SlimBookUWPWebView_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles SlimBookUWPWebView.LoadCompleted
        Dim cssToApply As String = ""
        cssToApply += "#header {position: fixed; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;}"
        Dim h = ApplicationView.GetForCurrentView().VisibleBounds.Height - 44
        Dim density As Single = DisplayInformation.GetForCurrentView().LogicalDpi
        Dim barHeight As Integer = CInt((h / density))
        cssToApply += ".flyout {max-height:" & barHeight & "px; overflow-y:scroll;}"
        cssToApply += "#m_newsfeed_stream article[data-ft*=""\\""ei\\"":\\""""] {display:none !important;}"
        Await SlimBookUWPWebView.InvokeScriptAsync("eval", {"javascript:function addStyleString(str) { var node = document.createElement('style'); node.innerHTML = " & "str; document.body.appendChild(node); } addStyleString('" & cssToApply & "');"})
        ProgRing.IsActive = False
    End Sub
End Class