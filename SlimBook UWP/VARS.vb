Module VARS
    Public localSettings As Windows.Storage.ApplicationDataContainer = Windows.Storage.ApplicationData.Current.LocalSettings
    Public SetFullScreen As Object = localSettings.Values("FullScreen")
    Public LockTopBar As Object = localSettings.Values("LockTopBar")
    Public ShowRecentPosts As Object = localSettings.Values("ShowRecentPosts")
    Public HideAds As Object = localSettings.Values("hideAds")
    Public View As ApplicationView = ApplicationView.GetForCurrentView
End Module
