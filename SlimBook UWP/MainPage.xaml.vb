' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Imports Windows.ApplicationModel.DataTransfer
Imports Windows.ApplicationModel.Resources
Imports Windows.UI.Popups
''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public MyWebViewSource As String = "https://touch.facebook.com/home.php"

    Public PrivacyInfo As String = "No personal, or private, information of either you, or your device, is collected by this app." & vbCrLf & "Neither is ANY information transmitted by this app."

    Private Sub Home()
        Dim mwv As Uri
        mwv = New Uri(MyWebViewSource & "?sk=h_chr")
        facebookWebView.Navigate(New Uri(MyWebViewSource))
    End Sub

    Private Async Sub CloseApp()
        Dim loader = New ResourceLoader()
        Dim exitDialog As String = loader.GetString("exitDialog")
        Dim exitTitleDialog As String = loader.GetString("exitTitleDialog")
        Dim yesCommand As String = loader.GetString("yesCommand")
        Dim noCommand As String = loader.GetString("noCommand")
        Dim appClosing As MessageDialog = New MessageDialog(exitDialog, exitTitleDialog)
        appClosing.Commands.Add(New UICommand(yesCommand) With {
                .Id = 0
            })
        appClosing.Commands.Add(New UICommand(noCommand) With {
                .Id = 1
            })
        appClosing.DefaultCommandIndex = 0
        appClosing.CancelCommandIndex = 1
        Dim result = Await appClosing.ShowAsync()
        If CInt(result.Id) = 0 Then Application.Current.[Exit]()
    End Sub

    Private Async Sub facebookWebView_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles facebookWebView.LoadCompleted
        Dim cssToApply As String = ""
        If localSettings.Values.ContainsKey("LockTopBar") Then

            If CBool(localSettings.Values("LockTopBar")) Then
                cssToApply += "#header {position: fixed; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;}"
                Dim h = ApplicationView.GetForCurrentView().VisibleBounds.Height - 44
                Dim density As Single = DisplayInformation.GetForCurrentView().LogicalDpi
                Dim barHeight As Integer = CInt((h / density))
                cssToApply += ".flyout {max-height:" & barHeight & "px; overflow-y:scroll;}"
            End If
        End If

        If localSettings.Values.ContainsKey("hideAds") Then
            If CBool(localSettings.Values("hideAds")) Then cssToApply += "#m_newsfeed_stream article[data-ft*=""\\""ei\\"":\\""""] {display:none !important;}"
        End If
        If Not facebookWebView.CanGoBack Then
            abbBack.Label = "Exit"
        Else
            abbBack.Label = "Back"
        End If
        ProgRing.IsActive = False
    End Sub

    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.InitializeComponent()
        If localSettings.Values.ContainsKey("fullScreen") Then
            If CBool(localSettings.Values("fullScreen")) Then view.TryEnterFullScreenMode()
        Else view.ExitFullScreenMode()
        End If
        _Setting.Visibility = Visibility.Collapsed
        ProgRing.IsActive = True
    End Sub

    Private Sub abbRefresh_Click(sender As Object, e As RoutedEventArgs) Handles abbRefresh.Click
        ProgRing.IsActive = True
        facebookWebView.Refresh()
    End Sub

    Private Sub abbHome_Click(sender As Object, e As RoutedEventArgs) Handles abbHome.Click
        Home()
    End Sub

    Private Sub abbAbout_Click(sender As Object, e As RoutedEventArgs) Handles abbAbout.Click
        'ShowAbout("About", "More stuff will be added into this dialog, just as soon as I work out what to put into it.")
        PivotSettingsAbout.SelectedIndex = 1
        _Setting.Visibility = Visibility.Visible
    End Sub

    Private Sub abbBack_Click(sender As Object, e As RoutedEventArgs) Handles abbBack.Click
        If facebookWebView.CanGoBack Then
            facebookWebView.GoBack()
        Else
            'CloseApp()
            Application.Current.Exit()
        End If
    End Sub

    Private Sub ShowSettings(ByVal title As String)
        PivotSettingsAbout.SelectedIndex = 0
        myScrollView_Settings.ChangeView(Nothing, 0, Nothing, True)
        _Setting.Visibility = Visibility.Visible
        txtSETTINGSTitle.Text = title
        SettingsSetup()
    End Sub

    Private Sub abbSettings_Click(sender As Object, e As RoutedEventArgs) Handles abbSettings.Click
        ShowSettings("Settings")
    End Sub

    Private Sub btnSETTINGS_X_Click(sender As Object, e As RoutedEventArgs) Handles btnSETTINGS_X.Click
        _Setting.Visibility = Visibility.Collapsed
        If localSettings.Values.ContainsKey("fullScreen") Then
            If CBool(localSettings.Values("fullScreen")) Then View.TryEnterFullScreenMode()
        Else View.ExitFullScreenMode()
        End If
    End Sub

    Private Sub SettingsSetup()
        Dim number As PackageVersion = Package.Current.Id.Version
        version.Text = String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
        privacy.Text = PrivacyInfo
        If localSettings.Values.ContainsKey("fullScreen") Then FullScreen.IsOn = CBool(localSettings.Values("fullScreen"))
        If localSettings.Values.ContainsKey("LockTopBar") Then BlockTopBar.IsOn = CBool(localSettings.Values("LockTopBar"))
        If localSettings.Values.ContainsKey("ShowRecentPosts") Then ShowRecentNews.IsOn = CBool(localSettings.Values("ShowRecentPosts"))
        If localSettings.Values.ContainsKey("hideAds") Then HideAds.IsOn = CBool(localSettings.Values("hideAds"))
    End Sub

    Private Async Sub hyperLogo_Click(sender As Object, e As RoutedEventArgs) Handles hyperLogo.Click
        Dim logoURL = New Uri("http://www.iconarchive.com/show/outline-icons-by-iconsmind/Book-icon.html")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Async Sub hyperDev_Click(sender As Object, e As RoutedEventArgs) Handles hyperDev.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Sub FullScreen_Toggled(sender As Object, e As RoutedEventArgs) Handles FullScreen.Toggled
        If Not localSettings.Values.ContainsKey("fullscreen") Then
            localSettings.Values.Add("fullscreen", FullScreen.IsOn)
        Else
            localSettings.Values("fullscreen") = FullScreen.IsOn
        End If
        If localSettings.Values.ContainsKey("fullScreen") Then
            If CBool(localSettings.Values("fullScreen")) Then
                View.TryEnterFullScreenMode()
            Else
                View.ExitFullScreenMode()
            End If
        End If
    End Sub

    Private Sub BlockTopBar_Toggled(sender As Object, e As RoutedEventArgs) Handles BlockTopBar.Toggled
        If Not localSettings.Values.ContainsKey("LockTopBar") Then
            localSettings.Values.Add("LockTopBar", FullScreen.IsOn)
        Else
            localSettings.Values("LockTopBar") = FullScreen.IsOn
        End If
    End Sub

    Private Sub ShowRecentNews_Toggled(sender As Object, e As RoutedEventArgs) Handles ShowRecentNews.Toggled
        If Not localSettings.Values.ContainsKey("ShowRecentPosts") Then
            localSettings.Values.Add("ShowRecentPosts", FullScreen.IsOn)
        Else
            localSettings.Values("ShowRecentPosts") = FullScreen.IsOn
        End If
    End Sub

    Private Sub HideAds_Toggled(sender As Object, e As RoutedEventArgs) Handles HideAds.Toggled
        If Not localSettings.Values.ContainsKey("hideAds") Then
            localSettings.Values.Add("hideAds", FullScreen.IsOn)
        Else
            localSettings.Values("hideAds") = FullScreen.IsOn
        End If
    End Sub
End Class
