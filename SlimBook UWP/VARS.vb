Module VARS
    Public localSettings As Windows.Storage.ApplicationDataContainer = Windows.Storage.ApplicationData.Current.LocalSettings
    Public SetFullScreen As Object = localSettings.Values("FullScreen")
    Public LockTopBar As Object = localSettings.Values("LockTopBar")
    Public ShowRecentPosts As Object = localSettings.Values("ShowRecentPosts")
    Public HideAds As Object = localSettings.Values("hideAds")
    Public View As ApplicationView = ApplicationView.GetForCurrentView

    Public MyWebViewSource As String = "https://touch.facebook.com/home.php"

    Public PrivacyInfo As String = "No personal, or private, information of either you, or your device, is collected by this app." & vbCrLf & "Neither is ANY information transmitted by this app."

    Public DevNotes As String = "Build 0.1.1 (04/11/18)" & vbCrLf & "Initial Build"

End Module