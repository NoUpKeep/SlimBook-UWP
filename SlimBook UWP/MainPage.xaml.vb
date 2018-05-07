' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Imports Windows.ApplicationModel.Resources
Imports Windows.Phone.UI.Input
Imports Windows.UI.Core

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public localSettings As Windows.Storage.ApplicationDataContainer = Windows.Storage.ApplicationData.Current.LocalSettings
    Public SetFullScreen As Object = localSettings.Values("FullScreen")

    Async Sub BackPressed(sender As Object, e As BackPressedEventArgs)
        Dim AppName As String = Package.Current.DisplayName
        'Handles any Back button presses.
        e.Handled = True
        If SlimBookUWPWebView.CanGoBack Then
            SlimBookUWPWebView.GoBack()
        Else
            Await displayMessageAsync(AppName, "Are you sure you want to exit the app?", "")
        End If
    End Sub

    Private Sub ABOUT_Click(sender As Object, e As RoutedEventArgs) Handles ABOUT.Click
        SettingsSetup()
    End Sub

    Private Async Sub BACK_Click(sender As Object, e As RoutedEventArgs) Handles BACK.Click
        If SlimBookUWPWebView.CanGoBack Then
            SlimBookUWPWebView.GoBack()
        Else
            Await displayMessageAsync("Quit SlimBook UWP", "Are you sure you want to quit the app?", "")
        End If
    End Sub

    Private Sub CloseGrid_Click(sender As Object, e As RoutedEventArgs) Handles CloseGrid.Click
        Info.Visibility = Visibility.Collapsed
    End Sub

    Private Sub CloseSettings_Click(sender As Object, e As RoutedEventArgs) Handles CloseSettings.Click
        SettingsGrid.Visibility = Visibility.Collapsed
    End Sub

    Private Sub CommBar_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles CommBar.SizeChanged
        SlimBookUWPWebView.Margin = New Thickness(0, 0, 0, CommBar.ActualHeight)
    End Sub

    Private Sub FS_Click(sender As Object, e As RoutedEventArgs) Handles FS.Click
        If View.IsFullScreenMode Then
            View.ExitFullScreenMode()
            FS.Icon = New SymbolIcon(Symbol.FullScreen)
            FS.Label = "Fullscreen"
            localSettings.Values("FullScreen") = "0"
        Else
            View.TryEnterFullScreenMode()
            FS.Icon = New SymbolIcon(Symbol.BackToWindow)
            FS.Label = "Exit Fullscreen"
            localSettings.Values("FullScreen") = "1"
        End If
    End Sub

    Private Async Sub GITHUB_Click(sender As Object, e As RoutedEventArgs) Handles GITHUB.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom/SlimBook-UWP")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Sub Go_Home()
        iconRotation.Begin()
        Dim mwv As Uri 'Contains the source URL for Facebook Touch
        mwv = New Uri(MyWebViewSource & "?sk=h_chr")
        SlimBookUWPWebView.Navigate(New Uri(MyWebViewSource))
    End Sub

    Private Sub HOME_Click(sender As Object, e As RoutedEventArgs) Handles HOME.Click
        Go_Home()
    End Sub

    Private Async Sub hyperDev_Click(sender As Object, e As RoutedEventArgs) Handles hyperDev.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom/SlimBook-UWP")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Async Sub hyperLogo_Click(sender As Object, e As RoutedEventArgs) Handles hyperLogo.Click
        Dim logoURL = New Uri("http://www.iconarchive.com/show/outline-icons-by-iconsmind/Book-icon.html")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Info.Visibility = Visibility.Collapsed
        Me.InitializeComponent()
        AddHandler HardwareButtons.BackPressed, AddressOf BackPressed
        SlimBookUWPWebView.Margin = New Thickness(0, 0, 0, CommBar.ActualHeight)
        If SetFullScreen Is Nothing Then
            localSettings.Values("FullScreen") = "0"
        Else
            If SetFullScreen = "0" Then
                View.ExitFullScreenMode()
                FS.Icon = New SymbolIcon(Symbol.FullScreen)
                FS.Label = "Fullscreen"
                togg_FS.IsOn = False
            Else
                View.TryEnterFullScreenMode()
                FS.Icon = New SymbolIcon(Symbol.BackToWindow)
                FS.Label = "Exit Fullscreen"
                togg_FS.IsOn = True
            End If
        End If
        If LockCommBar Is Nothing Then
            localSettings.Values("LockCommBar") = "0"
            CommBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact
            togg_CB.IsOn = False
        Else
            If LockCommBar = "0" Then
                CommBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact
                togg_CB.IsOn = False
            Else
                CommBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal
                togg_CB.IsOn = True
            End If
        End If
        If HideAds Is Nothing Then
            localSettings.Values("Hide_Ads") = "0"
            togg_Ads.IsOn = False
        Else
            If HideAds = "0" Then
                togg_Ads.IsOn = False
            Else
                togg_Ads.IsOn = True
            End If
        End If
        If LockTopBar Is Nothing Then
            localSettings.Values("LockTopBar") = "0"
            togg_TB.IsOn = False
        Else
            If LockTopBar = "0" Then
                togg_TB.IsOn = False
            Else
                togg_TB.IsOn = True
            End If
        End If
        Go_Home()
        AddHandler SystemNavigationManager.GetForCurrentView().BackRequested, Sub(s, a)

                                                                                  If SlimBookUWPWebView.CanGoBack Then
                                                                                      SlimBookUWPWebView.GoBack()
                                                                                      a.Handled = True
                                                                                  End If
                                                                              End Sub
    End Sub

    Private Sub REFRESH_Click(sender As Object, e As RoutedEventArgs) Handles REFRESH.Click
        iconRotation.Begin()
        SlimBookUWPWebView.Refresh()
    End Sub

    Private Sub SETTINGS_Click(sender As Object, e As RoutedEventArgs) Handles SETTINGS.Click
        SettingsGrid.Visibility = Visibility.Visible
        Dim number As PackageVersion = Package.Current.Id.Version
        CAV.Text = "Current App Version: " & String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
    End Sub

    Private Sub SettingsSetup()
        Dim number As PackageVersion = Package.Current.Id.Version
        PivotSettingsAbout.SelectedIndex = 0
        version.Text = String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
        privacy.Text = PrivacyInfo
        myScrollView.ChangeView(Nothing, 0, Nothing, True)
        CAV.Text = "Current App Version: " & String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
        Info.Visibility = Visibility.Visible
    End Sub

    Private Async Sub SlimBookUWPWebView_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles SlimBookUWPWebView.LoadCompleted
        Dim cssToApply As String = ""
        If LockTopBar = "1" Then
            cssToApply += "#header {position: fixed; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;}"
        End If
        Dim h = ApplicationView.GetForCurrentView().VisibleBounds.Height - 44
        Dim density As Single = DisplayInformation.GetForCurrentView().LogicalDpi
        Dim barHeight As Integer = CInt((h / density))
        cssToApply += ".flyout {max-height:" & barHeight & "px; overflow-y:scroll;}"
        If HideAds = "1" Then
            cssToApply += "#m_newsfeed_stream article[data-ft*=""\\""ei\\"":\\""""] {display:none !important;}"
        End If
        Await SlimBookUWPWebView.InvokeScriptAsync("eval", {"javascript:function addStyleString(str) { var node = document.createElement('style'); node.innerHTML = " & "str; document.body.appendChild(node); } addStyleString('" & cssToApply & "');"})
        iconRotation.Stop()
    End Sub

    Private Sub SlimBookUWPWebView_NavigationFailed(sender As Object, e As WebViewNavigationFailedEventArgs) Handles SlimBookUWPWebView.NavigationFailed
        Dim loader = New ResourceLoader()
        Dim noConnection As String = loader.GetString("noConnection")
        SlimBookUWPWebView.NavigateToString(noConnection)
    End Sub

    Private Sub SlimBookUWPWebView_NewWindowRequested(sender As WebView, args As WebViewNewWindowRequestedEventArgs) Handles SlimBookUWPWebView.NewWindowRequested
        If args.Uri.AbsoluteUri.Contains(".gif") OrElse args.Uri.AbsoluteUri.Contains("video") Then
            SlimBookUWPWebView.Navigate(args.Uri)
            args.Handled = True
        End If
    End Sub

    Private Sub togg_Ads_Toggled(sender As Object, e As RoutedEventArgs) Handles togg_Ads.Toggled
        Dim toggleSwitch As ToggleSwitch = TryCast(sender, ToggleSwitch)
        If toggleSwitch IsNot Nothing Then
            If toggleSwitch.IsOn = True Then
                localSettings.Values("Hide_Ads") = "1"
            Else
                localSettings.Values("Hide_Ads") = "0"
            End If
        End If
    End Sub

    Private Sub togg_CB_Toggled(sender As Object, e As RoutedEventArgs) Handles togg_CB.Toggled
        Dim toggleSwitch As ToggleSwitch = TryCast(sender, ToggleSwitch)
        If toggleSwitch IsNot Nothing Then
            If toggleSwitch.IsOn = True Then
                CommBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact
                localSettings.Values("LockCommBar") = "1"
            Else
                CommBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal
                localSettings.Values("LockCommBar") = "0"
            End If
        End If
    End Sub

    Private Sub togg_FS_Toggled(sender As Object, e As RoutedEventArgs) Handles togg_FS.Toggled
        Dim toggleSwitch As ToggleSwitch = TryCast(sender, ToggleSwitch)
        If toggleSwitch IsNot Nothing Then
            If toggleSwitch.IsOn = True Then
                View.TryEnterFullScreenMode()
                FS.Icon = New SymbolIcon(Symbol.BackToWindow)
                FS.Label = "Exit Fullscreen"
                localSettings.Values("FullScreen") = "1"
            Else
                View.ExitFullScreenMode()
                FS.Icon = New SymbolIcon(Symbol.FullScreen)
                FS.Label = "Fullscreen"
                localSettings.Values("FullScreen") = "0"
            End If
        End If
    End Sub

    Private Sub togg_TB_Toggled(sender As Object, e As RoutedEventArgs) Handles togg_TB.Toggled
        Dim toggleSwitch As ToggleSwitch = TryCast(sender, ToggleSwitch)
        If toggleSwitch IsNot Nothing Then
            If toggleSwitch.IsOn = True Then
                localSettings.Values("LockTopBar") = "1"
            Else
                localSettings.Values("LockTopBar") = "0"
            End If
        End If
    End Sub

    Private Async Sub TOP_Click(sender As Object, e As RoutedEventArgs) Handles TOP.Click
        Dim ScrollToTopString = "var int = setInterval(function(){window.scrollBy(0, -36); if( window.pageYOffset === 0 ) clearInterval(int); }, 0.1);"
        Await SlimBookUWPWebView.InvokeScriptAsync("eval", New String() {ScrollToTopString})
    End Sub

End Class