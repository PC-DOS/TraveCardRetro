Imports System.Runtime.Serialization

Public Module AppSettings
    Public AppSettingsContainer As IO.IsolatedStorage.IsolatedStorageSettings
    Public Sub InitializeAppSettings()
        AppSettingsContainer = IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings

        If AppSettingsContainer.Contains(IsDataLockedKey) Then
            IsDataLocked = AppSettingsContainer(IsDataLockedKey)
        Else
            IsDataLocked = False
            AppSettingsContainer(IsDataLockedKey) = IsDataLocked
        End If
    End Sub

    'Data Locking
    Public Const IsDataLockedKey As String = "IsDataLocked"
    Public IsDataLocked As Boolean = False

    'Public User Info
    Public Const NameKey As String = "Name"
    Public Const PhoneNumberKey As String = "PhoneNumber"

    'Location History
    Public Const LocationHistoryKey As String = "LocationHistory"
End Module
