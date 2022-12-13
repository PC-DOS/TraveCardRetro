Imports System
Imports System.Threading
Imports System.Windows.Controls
Imports Microsoft.Phone.Controls
Imports Microsoft.Phone.Shell
Imports Coding4Fun.Toolkit
Imports Coding4Fun.Toolkit.Controls
Imports Coding4Fun.Toolkit.Controls.InputPrompt
Imports System.Windows.Input
Imports Coding4Fun.Toolkit.Controls.UserPrompt
Imports System.IO

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' 建構函式
    Public Sub New()
        InitializeComponent()

        SupportedOrientations = SupportedPageOrientation.Portrait Or SupportedPageOrientation.Landscape

        ' 將 ApplicationBar 當地語系化的程式碼範例
        'BuildLocalizedApplicationBar()

    End Sub

    ' 建置當地語系化 ApplicationBar 的程式碼範例
    'Private Sub BuildLocalizedApplicationBar()
    '    ' 將頁面的 ApplicationBar 設定為 ApplicationBar 的新執行個體。
    '    ApplicationBar = New ApplicationBar()

    '    ' 建立新的按鈕並將文字值設定為 AppResources 的當地語系化字串。
    '    Dim appBarButton As New ApplicationBarIconButton(New Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative))
    '    appBarButton.Text = AppResources.AppBarButtonText
    '    ApplicationBar.Buttons.Add(appBarButton)

    '    ' 用 AppResources 的當地語系化字串建立新的功能表項目。
    '    Dim appBarMenuItem As New ApplicationBarMenuItem(AppResources.AppBarMenuItemText)
    '    ApplicationBar.MenuItems.Add(appBarMenuItem)
    'End Sub

    Sub tmrClock_Tick()
        Dim LastUpdateTime As New Date
        LastUpdateTime = Date.Now
        txtUpdateTime.Text = "更新于：" & LastUpdateTime.Year.ToString() & "." & String.Format("{0:D2}", LastUpdateTime.Month) & "." & String.Format("{0:D2}", LastUpdateTime.Day) & " " & String.Format("{0:D2}", LastUpdateTime.Hour) & ":" & String.Format("{0:D2}", LastUpdateTime.Minute) & ":" & String.Format("{0:D2}", LastUpdateTime.Second)
    End Sub
    Private Sub MainPage_Hold(sender As Object, e As GestureEventArgs) Handles Me.Hold
        IsDataLocked = Not IsDataLocked
        AppSettingsContainer(IsDataLockedKey) = IsDataLocked

        Dim ToastPromptEx As ToastPrompt
        ToastPromptEx = New ToastPrompt
        If IsDataLocked Then
            With ToastPromptEx
                .Title = "已鎖定"
                .Message = "使用者資料已鎖定，長按本畫面以解除鎖定。"
                .Show()
            End With
        Else
            With ToastPromptEx
                .Title = "已解除鎖定"
                .Message = "使用者資料已解除鎖定，長按本畫面以重新鎖定。"
                .Show()
            End With
        End If
    End Sub
    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        InitializeAppSettings()

        'tmrClock.Interval = TimeSpan.FromMilliseconds(500)
        'AddHandler tmrClock.Tick, AddressOf tmrClock_Tick
        'tmrClock.Start()
        tmrClock_Tick()

        'Load user phone number
        If AppSettingsContainer.Contains(PhoneNumberKey) Then
            txtPhoneNumber.Text = AppSettingsContainer(PhoneNumberKey)
        Else
            txtPhoneNumber.Text = "000****0000"
        End If

        'Load location history
        If AppSettingsContainer.Contains(LocationHistoryKey) Then
            trnLocHistData.Text = AppSettingsContainer(LocationHistoryKey).ToString().Replace("*", "")
        Else
            trnLocHistData.Text = "不明的地区"
        End If

        'Load arrow image
        ImageTools.IO.Decoders.AddDecoder(Of ImageTools.IO.Gif.GifDecoder)()
        Dim ArrowImage As New ImageTools.ExtendedImage
        ArrowImage.UriSource = New Uri("Assets/LocationHistoryUIAssets/Arrow.gif", UriKind.Relative)
        imgArrow.Source = ArrowImage
    End Sub
    Private Sub txtPhoneNumber_Tap(sender As Object, e As GestureEventArgs) Handles txtPhoneNumber.Tap
        If IsDataLocked Then
            e.Handled = True
            Exit Sub
        End If
        Dim PhoneNumberInputPrompt As InputPrompt
        PhoneNumberInputPrompt = New InputPrompt
        With PhoneNumberInputPrompt
            AddHandler .Completed, AddressOf PhoneNumberInputPrompt_Completed
            .Title = "请输入手机号码"
            .Message = "请输入您的手机号码。" & vbCrLf & "格式为：XXX****XXXX。"
            .MessageTextWrapping = TextWrapping.Wrap
            .Show()
        End With
        e.Handled = True
    End Sub
    Sub PhoneNumberInputPrompt_Completed(lpSender As Object, e As PopUpEventArgs(Of String, PopUpResult))
        If e.PopUpResult = PopUpResult.Ok And e.Result.Trim() <> "" Then
            txtPhoneNumber.Text = e.Result
            AppSettingsContainer(PhoneNumberKey) = e.Result
            AppSettingsContainer.Save()
        End If
    End Sub
    Private Sub txtUpdateTime_Tap(sender As Object, e As GestureEventArgs) Handles txtUpdateTime.Tap
        tmrClock_Tick()
        e.Handled = True
    End Sub

    Private Sub txtLocationHistory_Tap(sender As Object, e As GestureEventArgs) Handles txtLocationHistory.Tap
        If IsDataLocked Then
            e.Handled = True
            Exit Sub
        End If
        Dim LocationHistoryInputPrompt As InputPrompt
        LocationHistoryInputPrompt = New InputPrompt
        With LocationHistoryInputPrompt
            AddHandler .Completed, AddressOf LocationHistoryInputPrompt_Completed
            .Title = "请输入到访地区历史记录"
            .Message = "请输入您所到访的地区的历史记录。城市之间请使用半角逗号分隔。" & vbCrLf & "若该城市当前有中高风险地区，请在城市名字后输入星号（""*""）。"
            If AppSettingsContainer.Contains(LocationHistoryKey) Then
                .Value = AppSettingsContainer(LocationHistoryKey)
            End If
            .MessageTextWrapping = TextWrapping.Wrap
            .Show()
        End With
        e.Handled = True
    End Sub
    Sub LocationHistoryInputPrompt_Completed(lpSender As Object, e As PopUpEventArgs(Of String, PopUpResult))
        If e.PopUpResult = PopUpResult.Ok And e.Result.Trim() <> "" Then
            trnLocHistData.Text = e.Result.Replace("*", "")
            AppSettingsContainer(LocationHistoryKey) = e.Result
            AppSettingsContainer.Save()
        End If
    End Sub

    Private Sub btnBack_Tap(sender As Object, e As GestureEventArgs) Handles btnBack.Tap
        If NavigationService.CanGoBack Then
            NavigationService.GoBack()
        End If
    End Sub
End Class