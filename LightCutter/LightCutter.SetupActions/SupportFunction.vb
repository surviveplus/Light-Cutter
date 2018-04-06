Friend Class SupportFunction

Friend Class IconInfo
Public FilePath As String
Public Index As Integer
End Class

Public Shared Function GetIconInfoFromIconLocation(ByVal iconLocation As String) As IconInfo
	If String.IsNullOrEmpty(iconLocation) = False Then
		'Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("^(?<filename>.+),(?<index>[\-]?[0-9a-fA-F]*?)$")
		Dim rgx As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("^(?<filename>.*),(?<index>[\-]?[0-9a-fA-F]*?)$")
		Dim mat As System.Text.RegularExpressions.Match = rgx.Match(iconLocation)

		Dim filename As String = ""
		Dim iconIndexInteger As Integer = 0, iconIndexString As String = ""

		If mat.Groups.Count = 3 Then
			filename = mat.Groups("filename").ToString()
			iconIndexString = mat.Groups("index").ToString()

			If Integer.TryParse(iconIndexString, iconIndexInteger) Then
				'整数として認識できた。
				'しかし、正の整数の場合、実際は16進表記である。また、負の整数の場合は 符号を反転させてさらに-1する必要がある。
				If iconIndexInteger >= 0 Then
					Try
						iconIndexInteger = Convert.ToInt32(iconIndexString, 16)
					Catch ex As Exception
						'通常あり得ないが・・なぜか失敗した。全体をファイル名とみなす。
						iconIndexInteger = 0
						filename = iconLocation
					End Try
				Else
					iconIndexInteger = -iconIndexInteger - 1
				End If
			Else
				'整数として認識できなかった。16進表記の可能性がある。
				Try
					iconIndexInteger = Convert.ToInt32(iconIndexString, 16)
				Catch ex As Exception
					'通常あり得ないが・・16進表記ではなかった。全体をファイル名とみなす。
					iconIndexInteger = 0
					filename = iconLocation
				End Try
			End If
		Else
			'ファイル名とインデックスの2グループに分解されなかった。全体をファイル名とみなす。
			filename = iconLocation
		End If

		'ファイル名に引用符がついていたら取り除く
		If filename Like "['""]?*['""]" Then
				filename = filename.Substring(1, filename.Length - 2)
		End If

		'アイコン情報を返す
		Return New IconInfo() With {.FilePath = filename, .Index = iconIndexInteger}
	End If

	Return Nothing
End Function

End Class
