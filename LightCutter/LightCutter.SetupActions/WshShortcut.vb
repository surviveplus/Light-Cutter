''' <summary>
''' 限定的なラッパークラス。コンストラクタと解放の手続きを簡略化するためだけであり、IWshShortcutメンバ自体はそのまま参照して使う。
''' </summary>
''' <remarks></remarks>
Friend Class WshShortcut
	Implements IDisposable

Private shell As IWshRuntimeLibrary.IWshShell_Class
Private shortcut As IWshRuntimeLibrary.IWshShortcut

Public ReadOnly Property ShortcutObject() As IWshRuntimeLibrary.IWshShortcut
	Get
		Return Me.shortcut
	End Get
End Property

Public Sub New(ByVal shortcutPath As String)
	'WshShellを作成 
	shell = New IWshRuntimeLibrary.WshShell()	' New IWshRuntimeLibrary.WshShellClass()
	'ショートカットのパスを指定して、WshShortcutを作成 
	shortcut = DirectCast(shell.CreateShortcut(shortcutPath), IWshRuntimeLibrary.IWshShortcut)
End Sub

#Region "IDisposable Support"
	Private disposedValue As Boolean ' 重複する呼び出しを検出するには

	' IDisposable
	Protected Overridable Sub Dispose(ByVal disposing As Boolean)
		If Not Me.disposedValue Then
			If disposing Then
				'・マネージ状態を破棄します (マネージ オブジェクト)。
			End If
			'・アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
			'・大きなフィールドを null に設定します。
			System.Runtime.InteropServices.Marshal.ReleaseComObject(shortcut)
			shortcut = Nothing
			System.Runtime.InteropServices.Marshal.ReleaseComObject(shell)
			shell = Nothing
		End If
		Me.disposedValue = True
	End Sub

	'上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
	Protected Overrides Sub Finalize()
		' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
		Dispose(False)
		MyBase.Finalize()
	End Sub

	' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
	Public Sub Dispose() Implements IDisposable.Dispose
		' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub

#End Region

End Class
