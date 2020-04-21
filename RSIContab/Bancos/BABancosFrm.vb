﻿Public Class BABancosFrm
    Dim dbCls As New BAClassLib.BAClass
    Dim ListaRegistrosAbiertos As New Collection
    Private Sub BABancosFrm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If ListaRegistrosAbiertos.Count > 0 Then
            MsgBox("Cierre Todos los Formularios Abiertos", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
            e.Cancel = True
        End If
    End Sub

    Private Sub BABancosFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 10
        dbCls.DBconStr = strcnCAD
        Me.BABancosTableAdapter.Connection.ConnectionString = strcnCAD
        Me.BABancosTableAdapter.Fill(Me.BABancosListaDataSet.BABancos)
        ListaDataNavBarPrin.ToolStripLabelBuscar.Text = "Buscar Directamente por Identidad:"
        ListaDataNavBarPrin.ToolStripLabelBuscar.Text = ""
        ListaDataNavBarPrin.ToolStripLabelNoRegistros.Text = PrincipalBindingSource.Count
        Me.ListaDataNavBarPrin.ToolStripButtonEditar.Visible = True
        If Me.BABancosListaDataSet.BABancos.Rows.Count = 0 Then
            Me.ListaDataNavBarPrin.HabilitarSoloBotonNuevo()
        Else
            Me.ListaDataNavBarPrin.HabilitarTodosBotones()
        End If

    End Sub

    Private Sub ListaDataNavBarPrin_ConsultarRegistroClick(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.ConsultarRegistroClick
        CargarForma("CONSULTAR")
    End Sub

    Private Sub ListaDataNavBarPrin_EditarRegistroClick(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.EditarRegistroClick
        CargarForma("EDITAR")
    End Sub

    Private Sub ListaDataNavBarPrin_EliminarRegistroClick(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.EliminarRegistroClick
        CargarForma("ELIMINAR")
    End Sub

    Private Sub ListaDataNavBarPrin_NuevoRegistroClick(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.NuevoRegistroClick
        CargarForma("NUEVO")
    End Sub

    Private Sub ListaDataNavBarPrin_RecargarDatosClick(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.RecargarDatosClick
        RecargarDatos()
    End Sub

    Private Sub ListaDataNavBarPrin_SalirClick(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.SalirClick
        Me.Close()
    End Sub

    Private Sub CargarForma(strModalidad As String)
        If strModalidad = "NUEVO" Then
            Dim EditFormaFrm As New BABancosEdicionFrm
            EditFormaFrm.FormularioPrincipal = Me
            EditFormaFrm.MdiParent = Me.MdiParent
            EditFormaFrm.Modalidad = strModalidad
            EditFormaFrm.Show()
        Else
            If ListaDataNavBarPrin.ToolStripLabelBuscar.Text.Length > 0 Then
                If dbCls.ExisteBanco(ListaDataNavBarPrin.ToolStripLabelBuscar.Text) Then
                    If Not ListaRegistrosAbiertos.Contains(ListaDataNavBarPrin.ToolStripLabelBuscar.Text) Then
                        Dim EditFormaFrm As New BABancosEdicionFrm
                        EditFormaFrm.FormularioPrincipal = Me
                        EditFormaFrm.MdiParent = Me.MdiParent
                        EditFormaFrm.Modalidad = strModalidad
                        EditFormaFrm.intBanco = ListaDataNavBarPrin.ToolStripLabelBuscar.Text
                        ListaRegistrosAbiertos.Add(ListaDataNavBarPrin.ToolStripLabelBuscar.Text, ListaDataNavBarPrin.ToolStripLabelBuscar.Text)
                        ListaDataNavBarPrin.ToolStripLabelBuscar.Text = ""
                        EditFormaFrm.Show()
                    End If
                Else
                    MsgBox("Cuenta no existe...", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
                End If
            Else
                Dim reg As DataRowView = PrincipalBindingSource.Current
                If Not ListaRegistrosAbiertos.Contains(reg("Identidad")) Then
                    Dim EditFormaFrm As New BABancosEdicionFrm
                    EditFormaFrm.FormularioPrincipal = Me
                    EditFormaFrm.MdiParent = Me.MdiParent
                    EditFormaFrm.Modalidad = strModalidad
                    EditFormaFrm.intBanco = reg("CodigoBanco")
                    ListaRegistrosAbiertos.Add(reg("CodigoBanco"), reg("CodigoBanco"))
                    EditFormaFrm.Show()
                End If
            End If
        End If
    End Sub

    Private Sub RecargarDatos()
        Me.BABancosTableAdapter.Fill(Me.BABancosListaDataSet.BABancos)
        If Me.BABancosListaDataSet.BABancos.Rows.Count = 0 Then
            Me.ListaDataNavBarPrin.HabilitarSoloBotonNuevo()
        Else
            Me.ListaDataNavBarPrin.HabilitarTodosBotones()
        End If
    End Sub

    Private Sub ListaDataNavBarPrin_Load(sender As Object, e As EventArgs) Handles ListaDataNavBarPrin.Load

    End Sub
End Class