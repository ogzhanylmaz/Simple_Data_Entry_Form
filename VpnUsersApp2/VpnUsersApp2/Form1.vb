Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Public Class Form1

    Dim con As New SqlConnection
    Dim cmd As New SqlCommand

    Dim i As Integer
    Dim k As Integer
    Dim j As Integer
    Dim a As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        con.ConnectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\lenovo\Documents\Visual Studio 2019\VpnUsersApp2\VpnUsersApp2\vpnusersDB.mdf;Integrated Security=True"

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        disp_data()

    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        k = 0
        j = 0

        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT * FROM vpn_users WHERE username = @user"
        cmd.Parameters.AddWithValue("@user", TextBoxUsername.Text)

        k = cmd.ExecuteScalar()

        cmd.CommandText = "SELECT * FROM vpn_users WHERE password = @password"
        cmd.Parameters.AddWithValue("@password", TextBoxPassword.Text)

        j = cmd.ExecuteScalar()

        If TextBoxUsername.Text = "" Then
            MessageBox.Show("Username cannot be empty")
            Return
        End If

        If TextBoxPassword.Text = "" Then
            MessageBox.Show("Password cannot be empty")
            Return
        End If

        If (k > 0) And (j = 0) Then
            MessageBox.Show("Username Already Exists")
        ElseIf (j > 0) And (k = 0) Then
            MessageBox.Show("Password Already Exists")
        ElseIf (j > 0) And (k > 0) Then
            MessageBox.Show("Username and Password Already Exists")
        Else
            cmd.CommandText = "INSERT INTO vpn_users VALUES('" + TextBoxUsername.Text + "','" + TextBoxPassword.Text + "','" + TextBoxFirstname.Text + "','" + TextBoxSurname.Text + "','" + TextBoxRecordedBy.Text + "')"
            cmd.ExecuteNonQuery()
            disp_data()
            MessageBox.Show("Record Inserted Succesfully")
        End If

        ClearInputUpdateData()

    End Sub
    Public Sub disp_data()

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT * FROM vpn_users"
        cmd.ExecuteNonQuery()

        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        DataGridView1.DataSource = dt

    End Sub

    Private Sub ButtonClearAll_Click(sender As Object, e As EventArgs) Handles ButtonClearAll.Click

        ClearInputUpdateData()

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Try

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            i = Convert.ToInt32(DataGridView1.SelectedCells.Item(0).Value.ToString())

            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM vpn_users WHERE id =" & i & ""
            cmd.ExecuteNonQuery()

            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            Dim dr As SqlClient.SqlDataReader
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            While dr.Read

                TextBoxUsername.Text = dr.GetString(1).ToString()
                TextBoxPassword.Text = dr.GetString(2).ToString()
                TextBoxFirstname.Text = dr.GetString(3).ToString()
                TextBoxSurname.Text = dr.GetString(4).ToString()
                TextBoxRecordedBy.Text = dr.GetString(5).ToString()

            End While

            dr.Close()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        If TextBoxUsername.Text = "" Then
            MessageBox.Show("Username cannot be empty")
            Return
        End If

        If TextBoxPassword.Text = "" Then
            MessageBox.Show("Password cannot be empty")
            Return
        End If

        If DataGridView1.SelectedRows.Count > 1 Then
            MessageBox.Show("Cannot edit multiple rows")
            Return
        End If

        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "UPDATE vpn_users SET username='" + TextBoxUsername.Text + "', password='" + TextBoxPassword.Text + "', firstname='" + TextBoxFirstname.Text + "', surname='" + TextBoxSurname.Text + "', recorded_by='" + TextBoxRecordedBy.Text + "' WHERE id=" & i & ""
        cmd.ExecuteNonQuery()

        disp_data()

    End Sub

    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        If DataGridView1.RowCount = 0 Then
            MessageBox.Show("Cannot delete, table data is empty")
            Return
        End If

        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Cannot delete, select the table data to be deleted")
            Return
        End If

        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "DELETE FROM vpn_users WHERE username='" + TextBoxUsername.Text + "'"
        cmd.ExecuteNonQuery()

        disp_data()

        ClearInputUpdateData()

    End Sub

    Private Sub ButtonSearch_Click(sender As Object, e As EventArgs) Handles ButtonSearch.Click

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text

        If CheckBoxByUsername.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                cmd.CommandText = "SELECT * FROM vpn_users"
                cmd.ExecuteNonQuery()
            Else
                cmd.CommandText = "SELECT * FROM vpn_users WHERE username='" + TextBoxSearch.Text + "'"
                cmd.ExecuteNonQuery()
            End If
        End If

        If CheckBoxByPassword.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                cmd.CommandText = "SELECT * FROM vpn_users"
                cmd.ExecuteNonQuery()
            Else
                cmd.CommandText = "SELECT * FROM vpn_users WHERE password='" + TextBoxSearch.Text + "'"
                cmd.ExecuteNonQuery()
            End If
        End If

        If CheckBoxByPassword.Checked = True And CheckBoxByUsername.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                cmd.CommandText = "SELECT * FROM vpn_users"
                cmd.ExecuteNonQuery()
            Else
                cmd.CommandText = "SELECT * FROM vpn_users WHERE password='" + TextBoxSearch.Text + "' or username='" + TextBoxSearch.Text + "'"
                cmd.ExecuteNonQuery()
            End If
        End If

        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        DataGridView1.DataSource = dt

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        disp_data()

        ClearInputUpdateData()

    End Sub

    Private Sub ButtonReport_Click(sender As Object, e As EventArgs) Handles ButtonReport.Click

        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()

        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        Dim dr As SqlClient.SqlDataReader
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        Dim appXL As Excel.Application
        Dim wbXl As Excel.Workbook
        Dim shXL As Excel.Worksheet
        Dim raXL As Excel.Range

        appXL = CreateObject("Excel.Application")
        appXL.Visible = True
        wbXl = appXL.Workbooks.Add
        shXL = wbXl.ActiveSheet

        shXL.Cells(1, 1).Value = "Username"
        shXL.Cells(1, 2).Value = "Passowrd"
        shXL.Cells(1, 3).Value = "Firstname"
        shXL.Cells(1, 4).Value = "Surname"
        shXL.Cells(1, 5).Value = "Recorded By"

        With shXL.Range("A1", "E1")
            .Font.Bold = True
            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
        End With

        a = 2

        While dr.Read


            With shXL
                .Cells(a, 1).Value = dr.GetValue(1)
                .Cells(a, 2).Value = dr.GetValue(2)
                .Cells(a, 3).Value = dr.GetValue(3)
                .Cells(a, 4).Value = dr.GetValue(4)
                .Cells(a, 5).Value = dr.GetValue(5)
            End With

            a = a + 1

        End While

        dr.Close()

        ClearInputUpdateData()

        appXL.Visible = True
        appXL.UserControl = True

        raXL = Nothing
        shXL = Nothing
        wbXl = Nothing
        appXL.Quit()
        appXL = Nothing
        Exit Sub

    End Sub

    Private Sub ClearInputUpdateData()

        TextBoxUsername.Text = ""
        TextBoxPassword.Text = ""
        TextBoxFirstname.Text = ""
        TextBoxSurname.Text = ""
        TextBoxRecordedBy.Text = ""

    End Sub

    Private Function AllCellsSelected(dgv As DataGridView) As Boolean
        AllCellsSelected = (DataGridView1.SelectedCells.Count = (DataGridView1.RowCount * DataGridView1.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function

End Class
