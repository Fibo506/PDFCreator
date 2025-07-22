<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.testPage_btn = New System.Windows.Forms.Button()
        Me.jpegSettings_btn = New System.Windows.Forms.Button()
        Me.mergedFiles_btn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'testPage_btn
        '
        Me.testPage_btn.Location = New System.Drawing.Point(161, 36)
        Me.testPage_btn.Name = "testPage_btn"
        Me.testPage_btn.Size = New System.Drawing.Size(145, 45)
        Me.testPage_btn.TabIndex = 0
        Me.testPage_btn.Text = "TestPage2PDF"
        Me.testPage_btn.UseVisualStyleBackColor = True
        '
        'jpegSettings_btn
        '
        Me.jpegSettings_btn.Location = New System.Drawing.Point(312, 36)
        Me.jpegSettings_btn.Name = "jpegSettings_btn"
        Me.jpegSettings_btn.Size = New System.Drawing.Size(145, 45)
        Me.jpegSettings_btn.TabIndex = 1
        Me.jpegSettings_btn.Text = "JpegSettings"
        Me.jpegSettings_btn.UseVisualStyleBackColor = True
        '
        'mergedFiles_btn
        '
        Me.mergedFiles_btn.Location = New System.Drawing.Point(10, 36)
        Me.mergedFiles_btn.Name = "mergedFiles_btn"
        Me.mergedFiles_btn.Size = New System.Drawing.Size(145, 45)
        Me.mergedFiles_btn.TabIndex = 2
        Me.mergedFiles_btn.Text = "MergedMultipleFiles2Tif"
        Me.mergedFiles_btn.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 115)
        Me.Controls.Add(Me.mergedFiles_btn)
        Me.Controls.Add(Me.jpegSettings_btn)
        Me.Controls.Add(Me.testPage_btn)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents testPage_btn As System.Windows.Forms.Button
    Friend WithEvents jpegSettings_btn As System.Windows.Forms.Button
    Friend WithEvents mergedFiles_btn As System.Windows.Forms.Button

End Class
