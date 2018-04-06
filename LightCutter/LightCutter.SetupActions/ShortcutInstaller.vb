Imports System.ComponentModel
Imports System.Configuration.Install


	'■■■■■■■■■■■■■■■■■■■■■■■■
	'カスタム動作 パラメーター 仕様
	'
	' カスタムアクションデータ
	'
	'  /Actions=パラメータ（複数 半角カンマ区切り）  ・・・【必須】。どこにショートカットを作成するか指定する。例： desktop,startmenu
	'    ※パラメータ（複数 半角カンマ区切り）
	'      desktop ・・ デスクトップ
	'      startmenu ・・ スタートメニュー
	'      sendto・・送る
	'
	'  /TargetExePath="exeのフルパス"  ・・・【必須】。ダブルクォーテーションで囲う。exeのフルパス。例："[TARGETDIR]\MyApplication.exe"
	'
	'  /ProductName="[ProductName]" ・・・【必須】。ダブルクォーテーションで囲う。製品名。ショートカットの名前として採用されます。/ProductName="[ProductName]" を記述してください。
	'
	'  /Manufacturer="[Manufacturer]" ・・・【必須】。ダブルクォーテーションで囲う。制作者。スタートメニューのグループ名として採用されます。/Manufacturer="[Manufacturer]" を記述してください。
	'
	'  /InstallAllUsers=[ALLUSERS] ・・・【必須】。全ユーザーか現在のユーザーのみか判定するために使用します。/InstallAllUsers=[ALLUSERS] を記述してください。手動で設定する場合は、1 （true） または 空欄（false）を記述してください。
	'
	'  /IconLocation="アイコンのフルパス,アイコンインデックス"  ・・・任意。ダブルクォーテーションで囲う。ショートカットのアイコン。省略すると exeの第1アイコンを使用。 例："[TARGETDIR]\MyApplication.exe,0"
	'    ※アイコンのフルパスとアイコンインデックスは半角カンマで区切ってください。
	'    ※アイコンのフルパスは、ico exe dll のファイルが有効です。また、インデックスが無い場合でも、必ずパスの後ろに「,0」を指定してください。
	'
	'  ※相関必須性 など
	'    /Actions の値が指定されていない場合（空欄の場合）は、他のすべてのパラメーターは無視されます。
	'    /Actionsキーは省略できません。「/Actions=」が最も少ない設定値です。
	'
	'
	'  例（ヘルプ図の作成）：
	'    /Actions=desktop,startmenu /TargetExePath="[TARGETDIR]\HlpDiagramMaker.exe" /IconLocation="[TARGETDIR]\HlpDiagramMaker.exe,0" /ProductName="[ProductName]" /Manufacturer="[Manufacturer]" /InstallAllUsers=[ALLUSERS]
	'
	'■■■■■■■■■■■■■■■■■■■■■■■■

'※この Installer プロジェクトは、AnyCPUビルドのみです。x86ターゲットでもAnyCPUビルドします。


Public Class ShortcutInstaller

    Public Sub New()
        MyBase.New()

        'この呼び出しは、コンポーネント デザイナーで必要です。
        InitializeComponent()

        'InitializeComponent への呼び出し後、初期化コードを追加します

    End Sub



Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
        MyBase.Install(stateSaver)

	Dim getParameterMethod =
						Sub(key As String, ByRef retValue As String, requiredValue As Boolean)
							retValue = ""
							If Me.Context.Parameters.ContainsKey(key) Then
								retValue = Me.Context.Parameters(key)
							End If
							If requiredValue AndAlso String.IsNullOrEmpty(retValue) Then
								Throw New System.Configuration.Install.InstallException(key & "パラメーターが設定されていません。")
							End If
						End Sub

	Dim isCreateShortcutDesktop As Boolean = False
	Dim isCreateShortcutStartmenu As Boolean = False
	Dim isCreateShortcutSendTo As Boolean = False

	Dim userprofileFolderPathList As IEnumerable(Of String) = Nothing

	Dim shortcutPathDesktop As String = "" '空の場合は作らない
	Dim shortcutPathStartMenu As String = "" '空の場合は作らない
	Dim shortcutSendToPathList As IEnumerable(Of String) = {""}	'空の場合は作らない
	Dim targetExePath As String = ""
	Dim iconLocation As String = ""

	'まず、Actionsに指定された内容を解析する。ショートカットを作成するものを特定する。
	If True Then 'scope
		Dim actionsValue As String = ""
		getParameterMethod("Actions", actionsValue, False)
            isCreateShortcutDesktop = InStr(actionsValue, "desktop", CompareMethod.Text) > 0
		isCreateShortcutStartmenu = InStr(actionsValue, "startmenu", CompareMethod.Text) > 0
		isCreateShortcutSendTo = InStr(actionsValue, "sendto", CompareMethod.Text) > 0
	End If

	'ショートカットを作成する場合、必要なパラメータを取得
	If isCreateShortcutDesktop OrElse isCreateShortcutStartmenu OrElse isCreateShortcutSendTo Then
		'準備
		Dim currentUserProfileFolderPath As New System.IO.DirectoryInfo(System.Environment.GetEnvironmentVariable("UserProfile"))

		'ユーザープロファイルフォルダーの列挙
		userprofileFolderPathList = From oneFolder In currentUserProfileFolderPath.Parent.GetDirectories() Select oneFolder.FullName

		'パラメーターのチェック・取得
		Dim tempProductName As String = ""
		Dim tempManufacturer As String = ""
		Dim tempAllUsers As Boolean = False
		Dim tempShortcutFileName As String = ""
		Dim tmpStr As String = ""	'作業用
		getParameterMethod("ProductName", tempProductName, True)
		tempShortcutFileName = tempProductName & ".lnk"
		getParameterMethod("Manufacturer", tempManufacturer, True)
		getParameterMethod("InstallAllUsers", tmpStr, False)
		tempAllUsers = tmpStr = "1"	 'プロパティからは、"1"か""が得られる
		getParameterMethod("TargetExePath", targetExePath, True)
		getParameterMethod("IconLocation", iconLocation, False)

		'デスクトップのパスを設定
		If isCreateShortcutDesktop Then
			shortcutPathDesktop = If(tempAllUsers, NaviveMethods.GetCommonDesktopPath(), System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory))
			shortcutPathDesktop = System.IO.Path.Combine(shortcutPathDesktop, tempShortcutFileName)
		End If

		'スタートメニューのパスを設定
		If isCreateShortcutStartmenu Then
			shortcutPathStartMenu = If(tempAllUsers, NaviveMethods.GetCommonProgramsPath(), System.Environment.GetFolderPath(System.Environment.SpecialFolder.Programs))
                'shortcutPathStartMenu = System.IO.Path.Combine(System.IO.Path.Combine(shortcutPathStartMenu, tempManufacturer), tempShortcutFileName)
                shortcutPathStartMenu = System.IO.Path.Combine(shortcutPathStartMenu, tempShortcutFileName)
            End If

		'SendToのパスを設定
		If isCreateShortcutSendTo Then
			If tempAllUsers Then
				Dim shortcutSendToPathRelative As String = ""
				shortcutSendToPathRelative = (New System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.SendTo))).FullName.Replace(currentUserProfileFolderPath.FullName, "")	'\含む
                    shortcutSendToPathList = From oneUserFolder In userprofileFolderPathList Select oneUserFolder & shortcutSendToPathRelative & "\" & "パスワード付き圧縮 (zip 形式) フォルダー.lnk"
			Else
                    shortcutSendToPathList = {System.Environment.GetFolderPath(System.Environment.SpecialFolder.SendTo) & "\" & "パスワード付き圧縮 (zip 形式) フォルダー.lnk"}
			End If
			'shortcutPathStartMenu = If(tempAllUsers, System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonPrograms), System.Environment.GetFolderPath(System.Environment.SpecialFolder.Programs))
			'shortcutPathStartMenu = System.IO.Path.Combine(shortcutPathStartMenu, tempManufacturer, tempShortcutFileName)
		End If

		'IconLocation 設定されていない場合はexeのアイコンを使用
		If String.IsNullOrEmpty(iconLocation) Then
			iconLocation = targetExePath & ",0"
		End If
	End If

	'パラメーターの保存
        stateSaver.Add("ShortcutPathDesktop", shortcutPathDesktop)
	stateSaver.Add("ShortcutPathStartMenu", shortcutPathStartMenu)
	stateSaver.Add("ShortcutPathSendTo", If(shortcutSendToPathList.Any, shortcutSendToPathList.ToArray(), {""}))
	stateSaver.Add("TargetExePath", targetExePath)
	stateSaver.Add("IconLocation", iconLocation)
End Sub
Public Overrides Sub Commit(ByVal savedState As System.Collections.IDictionary)
        MyBase.Commit(savedState)
	Dim targetExePath As String = savedState("TargetExePath")
	Dim iconLocation As String = savedState("IconLocation")
        Dim createShortcutMethod =
      Sub(shortcutFilePath As String)
          If String.IsNullOrEmpty(shortcutFilePath) Then
              Exit Sub
          End If

          Try
              Dim iconInfo = SupportFunction.GetIconInfoFromIconLocation(iconLocation)
              Dim iconPath As String = iconInfo.FilePath
              Dim iconIndex As Integer = iconInfo.Index
              If String.IsNullOrEmpty(iconPath) = True OrElse System.IO.File.Exists(iconPath) = False Then
                  iconPath = targetExePath
                  iconIndex = 0
              End If

              Dim shortcut As ShortcutFile = Nothing

              System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(shortcutFilePath))

              If System.IO.File.Exists(shortcutFilePath) Then
                  shortcut = ShortcutFile.LoadFile(shortcutFilePath)
                  shortcut.ExecutablePath = targetExePath
              Else
                  shortcut = ShortcutFile.NewFile(shortcutFilePath, targetExePath)
              End If
              shortcut.IconPath = iconPath
              shortcut.IconIndex = iconIndex

              shortcut.Save()

          Catch ex As Exception
              '何もしない
          End Try
      End Sub

	createShortcutMethod(savedState("ShortcutPathDesktop"))
	createShortcutMethod(savedState("ShortcutPathStartMenu"))
        For Each oneShortcutPath As String In savedState("ShortcutPathSendTo")
            createShortcutMethod(oneShortcutPath)
        Next

End Sub
Public Overrides Sub Rollback(ByVal savedState As System.Collections.IDictionary)
	MyBase.Rollback(savedState)
	Try
		DeleteShortcuts(savedState("ShortcutPathDesktop"), savedState("ShortcutPathStartMenu"))
		For Each oneShortcutPath As String In savedState("ShortcutPathSendTo")
			DeleteShortcuts(oneShortcutPath)
		Next
	Catch ex As Exception
		'何もしない
	End Try
End Sub
Public Overrides Sub Uninstall(ByVal savedState As System.Collections.IDictionary)
	MyBase.Uninstall(savedState)
	Try
		DeleteShortcuts(savedState("ShortcutPathDesktop"), savedState("ShortcutPathStartMenu"))
		For Each oneShortcutPath As String In savedState("ShortcutPathSendTo")
			DeleteShortcuts(oneShortcutPath)
		Next
	Catch ex As Exception
		'何もしない
	End Try
End Sub

Private Sub DeleteShortcuts(ByVal ParamArray shortcutFilePathes() As String)
	For Each onePath In shortcutFilePathes
		If String.IsNullOrEmpty(onePath) = False Then
			Try
				System.IO.File.Delete(onePath)
				System.IO.Directory.Delete(System.IO.Path.GetDirectoryName(onePath), False)
			Catch ex As Exception
				'何もしない
			End Try
		End If
	Next
End Sub


'Protected Overrides Sub OnCommitted(ByVal savedState As System.Collections.IDictionary)
'	MyBase.OnCommitted(savedState)
'	'保存してあるアプリケーション名を取得する
'	Me.applicationFileName = savedState("RegisterApplicationFileName")
'End Sub

'Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
'		MsgBox("Install")
'		MyBase.Install(stateSaver)
'End Sub
'Public Overrides Sub Commit(ByVal savedState As System.Collections.IDictionary)
'		MsgBox("Commit")
'		MyBase.Commit(savedState)
'End Sub
'Public Overrides Sub Rollback(ByVal savedState As System.Collections.IDictionary)
'		MsgBox("Rollback")
'		MyBase.Rollback(savedState)
'End Sub
'Public Overrides Sub Uninstall(ByVal savedState As System.Collections.IDictionary)
'		MsgBox("Uninstall")
'		MyBase.Uninstall(savedState)
'End Sub


Private Class NaviveMethods

#Region "定数"

Private Const S_OK = &H0								' Success
Private Const S_FALSE = &H1							' The Folder is valid, but does not exist
Private Const E_INVALIDARG = &H80070057	' Invalid CSIDL Value

Private Const CSIDL_LOCAL_APPDATA = &H1C&
Private Const CSIDL_FLAG_CREATE = &H8000&

Private Const CSIDL_DESKTOP = &H0	' 	デスクトップ
Private Const CSIDL_INTERNET = &H1 ' 	インターネット
Private Const CSIDL_PROGRAMS = &H2 ' 	プログラム
Private Const CSIDL_CONTROLS = &H3 ' 	コントロールパネル
Private Const CSIDL_PRINTERS = &H4 ' 	プリンタ
Private Const CSIDL_PERSONAL = &H5 ' 	マイ　ドキュメント
Private Const CSIDL_FAVORITES = &H6	' 	お気に入り
Private Const CSIDL_STARTUP = &H7	' 	スタートアップ
Private Const CSIDL_RECENT = &H8 ' 	最近使ったファイル
Private Const CSIDL_SENDTO = &H9 ' 	送る
Private Const CSIDL_BITBUCKET = &HA	' 	ゴミ箱
Private Const CSIDL_STARTMENU = &HB	' 	スタートメニュー
Private Const CSIDL_DESKTOPDIRECTORY = &H10	' 	デスクトップ
Private Const CSIDL_DRIVES = &H11	' 	マイ　コンピュータ
Private Const CSIDL_NETWORK = &H12 ' 	マイ　ネットワーク
Private Const CSIDL_NETHOOD = &H13 ' 	NetHood
Private Const CSIDL_FONTS = &H14 ' 	フォント
Private Const CSIDL_TEMPLATES = &H15 ' 	テンプレート
Private Const CSIDL_COMMON_STARTMENU = &H16	' 	全ユーザーのスタートメニュー
Private Const CSIDL_COMMON_PROGRAMS = &H17 ' 	全ユーザーのプログラム
Private Const CSIDL_COMMON_STARTUP = &H18	' 	全ユーザーのスタートアップ
Private Const CSIDL_COMMON_DESKTOPDIRECTORY = &H19 ' 	全ユーザーのデスクトップ
Private Const CSIDL_APPDATA = &H1A ' 	Application Data
Private Const CSIDL_PRINTHOOD = &H1B ' 	PrintHood
Private Const CSIDL_ALTSTARTUP = &H1D	' 	ローカライズされないスタートアップ
Private Const CSIDL_COMMON_ALTSTARTUP = &H1E ' 	全ユーザーのローカライズされないスタートアップ
Private Const CSIDL_COMMON_FAVORITES = &H1F	' 	全ユーザーのお気に入り
Private Const CSIDL_INTERNET_CACHE = &H20	' 	IEキャッシュ保存先
Private Const CSIDL_COOKIES = &H21 ' 	クッキー
Private Const CSIDL_HISTORY = &H22 ' 	履歴

Private Const SHGFP_TYPE_CURRENT = 0
Private Const SHGFP_TYPE_DEFAULT = 1
Private Const MAX_PATH = 260

#End Region

<Runtime.InteropServices.DllImport("shell32.dll")>
Private Shared Function SHGetFolderPath(ByVal hwndOwner As IntPtr, ByVal nFolder As Integer, ByVal hToken As IntPtr, ByVal dwFlags As UInteger, ByVal pszPath As System.Text.StringBuilder) As Integer
End Function

Public Shared Function GetCommonDesktopPath() As String
	Dim pathText As New System.Text.StringBuilder(256)
	Dim resultValue As Integer = SHGetFolderPath(IntPtr.Zero, CSIDL_COMMON_DESKTOPDIRECTORY Or CSIDL_FLAG_CREATE, IntPtr.Zero, 0, pathText)
	Return pathText.ToString()
End Function

Public Shared Function GetCommonProgramsPath() As String
	Dim pathText As New System.Text.StringBuilder(256)
	Dim resultValue As Integer = SHGetFolderPath(IntPtr.Zero, CSIDL_COMMON_PROGRAMS Or CSIDL_FLAG_CREATE, IntPtr.Zero, 0, pathText)
	Return pathText.ToString()
End Function

End Class

End Class
