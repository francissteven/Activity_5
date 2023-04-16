Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32


Public Class Form1

    Dim sqlConn As New MySqlConnection
    Dim sqlComm As New MySqlCommand
    Dim sqlRd As MySqlDataReader
    Dim sqlDt As New DataTable
    Dim sqlDa As New MySqlDataAdapter
    Dim sqlQuery As String

    Dim server As String = "localhost"
    Dim username As String = "root"
    Dim password As String = "kiko5456"
    Dim database As String = "crud"

    Private Sub updateTable()
        sqlConn.ConnectionString = "server=localhost;port=3305;user id=" + username + ";password=" + password + ";database=" + database

        sqlConn.Open()
        sqlComm.Connection = sqlConn
        sqlComm.CommandText = "SELECT * FROM info"

        sqlRd = sqlComm.ExecuteReader
        sqlDt.Load(sqlRd)
        sqlRd.Close()
        sqlConn.Close()
        DGV.DataSource = sqlDt
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        updateTable()
    End Sub

    Private Sub btnexit_Click(sender As Object, e As EventArgs) Handles btnexit.Click
        Dim btnexit As DialogResult
        btnexit = MessageBox.Show("Confirm if you want to exit?", "MySQL Connector", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If btnexit = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        sqlConn.ConnectionString = "server=localhost;port=3305;user id=" + username + ";password=" + password + ";database=" + database

        Try
            sqlConn.Open()
            sqlQuery = "INSERT INTO info(ID, FName, LName, Age, Phone, Gender) VALUES('" & txtID.Text & "', '" & txtFname.Text & "', '" & txtLname.Text & "', '" & txtage.Text & "', '" & txtphone.Text & "', '" & txtgender.Text & "')"

            sqlComm = New MySqlCommand(sqlQuery, sqlConn)
            sqlRd = sqlComm.ExecuteReader
            sqlConn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "MySQL Connector", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Finally
            sqlConn.Dispose()

        End Try
        updateTable()
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        sqlConn.ConnectionString = "server=localhost;port=3305;user id=" + username + ";password=" + password + ";database=" + database

        Try
            sqlConn.Open()

            Dim selectedRow As DataGridViewRow = DGV.SelectedRows(0)
            Dim id As String = selectedRow.Cells("ID").Value.ToString()

            sqlQuery = "UPDATE info SET FName='" & txtFname.Text & "', LName='" & txtLname.Text & "', Age='" & txtage.Text & "', Phone='" & txtphone.Text & "', Gender='" & txtgender.Text & "' WHERE ID='" & id & "'"

            sqlComm = New MySqlCommand(sqlQuery, sqlConn)
            sqlRd = sqlComm.ExecuteReader
            sqlConn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "MySQL Connector", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Finally
            sqlConn.Dispose()

        End Try
        updateTable()
    End Sub


    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        For Each row As DataGridViewRow In DGV.SelectedRows
            DGV.Rows.Remove(row)
        Next
        updateTable()
    End Sub

    Private Sub txtsearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtsearch.KeyPress

        Try
            If Asc(e.KeyChar) = 13 Then
                Dim dv As DataView
                dv = sqlDt.DefaultView
                dv.RowFilter = String.Format("Firstname like '%{0}%'", txtsearch.Text)
                DGV.DataSource = dv.ToTable()

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
End Class
