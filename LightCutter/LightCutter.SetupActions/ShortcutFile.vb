Public Class ShortcutFile

#Region "プライベート変数"

Private isEdit As Boolean = False

#End Region

#Region "公開プロパティ（プロパティ用のEnum宣言なども含む）"

Private shortcutFilePathValue As String
Public ReadOnly Property ShortcutFilePath() As String
	Get
		Return Me.shortcutFilePathValue
	End Get
End Property

Private executablePathValue As String
Public Property ExecutablePath() As String
	Get
		Return Me.executablePathValue
	End Get
	Set(ByVal value As String)
		Me.executablePathValue = value
	End Set
End Property

Private argumentsValue As String
''' <summary>
''' 実行時コマンドライン引数を指定します。/a /b /c のようにスペースで区切ります。
''' <para>注意：この設定はフォルダーのショートカットの場合は効果がありません。</para>
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
Public Property Arguments() As String
	Get
		Return Me.argumentsValue
	End Get
	Set(ByVal value As String)
		Me.argumentsValue = value
	End Set
End Property

Private workingDirectoryPathValue As String = ""
Public Property WorkingDirectoryPath() As String
	Get
		Return Me.workingDirectoryPathValue
	End Get
	Set(ByVal value As String)
		Me.workingDirectoryPathValue = value
	End Set
End Property

Private iconPathValue As String = ""
Public Property IconPath() As String
	Get
		Return Me.iconPathValue
	End Get
	Set(ByVal value As String)
		Me.iconPathValue = value
	End Set
End Property

Private iconIndexValue As Integer = 0
Public Property IconIndex() As Integer
	Get
		Return Me.iconIndexValue
	End Get
	Set(ByVal value As Integer)
		Me.iconIndexValue = value
	End Set
End Property

Public Enum WindowStyleEnum As Integer
	Normal = 1 '通常
	Maximized = 3 '最大化
	Minimized = 7 '最小化
End Enum
Private windowStyleValue As WindowStyleEnum = WindowStyleEnum.Normal
Public Property WindowStyle() As WindowStyleEnum
	Get
		Return Me.windowStyleValue
	End Get
	Set(ByVal value As WindowStyleEnum)
		Me.windowStyleValue = value
	End Set
End Property

Private hotkeyValue As String
''' <summary>
''' ホットキーの設定。Ctrl+Alt+Shift+F12 のように文字列で指定します。
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
Public Property Hotkey() As String
	Get
		Return Me.hotkeyValue
	End Get
	Set(ByVal value As String)
		Me.hotkeyValue = value
	End Set
End Property

Private commentValue As String = ""
Public Property Comment() As String
	Get
		Return Me.commentValue
	End Get
	Set(ByVal value As String)
		Me.commentValue = value
	End Set
End Property

#End Region

#Region "コンストラクタ と 新規作成メソッド"

''' <summary>
''' 非公開。ショートカット新規作成用。
''' <para>新規作成は、ShortcutFile.NewFile 共有メソッドを使用してください。</para>
''' </summary>
''' <remarks></remarks>
Private Sub New()
End Sub
''' <summary>
''' 既存のショートカットファイルを指定してインスタンスを作成します。
''' <para>新規作成は、ShortcutFile.NewFile 共有メソッドを使用してください。</para>
''' </summary>
''' <remarks></remarks>
Private Sub New(ByVal shortcutFilePath As String)
	'ファイルのチェック
	If System.IO.File.Exists(shortcutFilePath) = False Then
		Throw New ApplicationException("ファイルがありません。")
	End If

	'編集モードとして準備
	Me.isEdit = True
	Me.shortcutFilePathValue = shortcutFilePath

	'既存のショートカットファイルから情報を取得
	Using shortcutWrapper = New WshShortcut(Me.shortcutFilePathValue)
		With shortcutWrapper.ShortcutObject

			Me.executablePathValue = .TargetPath
			Me.argumentsValue = .Arguments
			Me.workingDirectoryPathValue = .WorkingDirectory

			Dim iconInfo = SupportFunction.GetIconInfoFromIconLocation(.IconLocation)
			Me.iconPathValue = iconInfo.FilePath
			Me.iconIndexValue = iconInfo.Index

			Me.windowStyleValue = .WindowStyle
			Me.hotkeyValue = .Hotkey
			Me.commentValue = .Description

		End With
	End Using
End Sub

''' <summary>
''' 新しくショートカットを作成します。Saveメソッドを呼ぶまで、実際にディスクに保存されません。
''' </summary>
''' <param name="shotcutFilePath">ショートカットファイルのフルパス。通常、拡張子は .lnk にします。</param>
''' <param name="targetExecutablefilePath">実行ファイルまたはフォルダーへのフルパス。</param>
''' <returns></returns>
''' <remarks></remarks>
Public Shared Function NewFile(ByVal shotcutFilePath As String, ByVal targetExecutablefilePath As String) As ShortcutFile
	Return New ShortcutFile() With {.shortcutFilePathValue = shotcutFilePath, .executablePathValue = targetExecutablefilePath}
End Function

''' <summary>
''' 既存のショートカットを編集します。Saveメソッドを呼ぶまで、実際にディスクに保存されません。
''' </summary>
''' <param name="shotcutFilePath">ショートカットファイルのフルパス。通常、拡張子は .lnk にします。</param>
''' <returns></returns>
''' <remarks></remarks>
Public Shared Function LoadFile(ByVal shotcutFilePath As String) As ShortcutFile
	Return New ShortcutFile(shotcutFilePath)
End Function

#End Region

#Region "公開メソッド"

Public Sub Save(Optional ByVal OverwriteFile As Boolean = False)
	'ソース元：http://dobon.net/vb/dotnet/file/createshortcut.html


	'既存ファイルの確認
	If Me.isEdit = False AndAlso System.IO.File.Exists(Me.shortcutFilePathValue) = True Then
		If OverwriteFile = True Then
			System.IO.File.Delete(Me.shortcutFilePathValue)
		Else
			Throw New ApplicationException("既存の同名ファイルが存在します。")
		End If
	End If


	'作成するショートカットのパスを渡してオブジェクトインスタンスを作成
	Using shortcutWrapper = New WshShortcut(Me.shortcutFilePathValue)
		Dim shortcut = shortcutWrapper.ShortcutObject

		'リンク先 
		shortcut.TargetPath = Me.executablePathValue

		'コマンドパラメータ（リンク先 の後ろに付く）
		shortcut.Arguments = argumentsValue

		'作業フォルダ 
		shortcut.WorkingDirectory = Me.workingDirectoryPathValue

		'アイコンのパス（Path,Index）
		If String.IsNullOrEmpty(Me.iconPathValue) = False Then
			shortcut.IconLocation = Me.iconPathValue & "," & Me.iconIndexValue
		Else
			'なにもしない
		End If

		'実行時の大きさ 1が通常、3が最大化、7が最小化 
		shortcut.WindowStyle = Me.windowStyleValue

		'ショートカットキー（ホットキー）
		If String.IsNullOrEmpty(Me.hotkeyValue) = False Then
			shortcut.Hotkey = Me.hotkeyValue
		Else
			'なにもしない
		End If

		'コメント 
		shortcut.Description = Me.commentValue

		'ショートカットを作成 
		shortcut.Save()
	End Using

End Sub

#End Region

End Class
