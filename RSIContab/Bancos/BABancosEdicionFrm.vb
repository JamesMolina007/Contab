Imports System.Drawing.Printing

Public Class BABancosEdicionFrm
    Private _FormularioPrincipal As BABancosFrm
    Private _CodigoCentro As Int32
    Private _Modalidad As String
    Private _intBanco As Int16

    Private _Año As String
    Public Property año() As Int16
        Get
            Return _año
        End Get
        Set(ByVal value As Int16)
            _año = value
        End Set
    End Property

    Private _Mes As Int16
    Public Property mes() As Int16
        Get
            Return _Mes
        End Get
        Set(ByVal value As Int16)
            _Mes = value
        End Set
    End Property


    Public Property intBanco() As Int16
        Get
            Return _intBanco
        End Get
        Set(ByVal value As Int16)
            _intBanco = value
        End Set
    End Property

    Public Property FormularioPrincipal() As BABancosFrm
        Get
            Return _FormularioPrincipal
        End Get
        Set(ByVal value As BABancosFrm)
            _FormularioPrincipal = value
        End Set
    End Property
    Public Property CodigoCentro() As Int32
        Get
            Return _CodigoCentro
        End Get
        Set(ByVal value As Int32)
            _CodigoCentro = value
        End Set
    End Property
    Public Property Modalidad() As String
        Get
            Return _Modalidad
        End Get
        Set(ByVal value As String)
            _Modalidad = value
        End Set
    End Property
    Dim strIdentidadActual As String
    'Dim dbCls As New AfCoopCls.DBUtil
    Dim CadCls As New Rsierpgencl.Rsierpcl.Cadenas
    Dim drCentros As SqlClient.SqlDataReader
    Dim drConf As SqlClient.SqlDataReader
    Dim mCentroSeleccionado As Boolean
    Dim ListaCatalogoFrm As New CtListaCatGenFrm

    Private Sub EditDataNavBarPrin_Load(sender As Object, e As EventArgs) Handles EditDataNavBarPrin.Load
        Me.BABancosTableAdapter.Connection.ConnectionString = strcnCAD
        Me.BATransaccionesEncabezadoTableAdapter.Connection.ConnectionString = strcnCAD
        If Modalidad = "NUEVO" Then
            PrincipalBindingSource.AddNew()
        End If
        If Modalidad = "EDITAR" Then
            Me.BATransaccionesEncabezadoTableAdapter.Fill(Me.BABancosEdicionDataSet1.BATransaccionesEncabezado, _intBanco, _Año, _Mes)
        End If
    End Sub

    Private Sub EditDataNavBarPrin_GuardarClick(sender As Object, e As EventArgs) Handles EditDataNavBarPrin.GuardarClick
        Me.Validate()
        PrincipalBindingSource.EndEdit()
        Try
            Me.BABancosTableAdapter.Update(Me.BABancosEdicionDataSet.BABancos)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End Try

    End Sub

    Private Sub BABancosEdicionFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strImpInstaladas As String
        EditDataNavBarPrin.ToolStripButtonEliminar.Visible = False
        For i As Int16 = 0 To PrinterSettings.InstalledPrinters.Count - 1
            strImpInstaladas = PrinterSettings.InstalledPrinters.Item(i)
            C1ComboBoxImpresora.Items.Add(strImpInstaladas)
        Next
        Dim DefPrn As New PrinterSettings
        C1ComboBoxImpresora.Text = DefPrn.PrinterName
    End Sub

    Private Sub C1ButtonCuentaContable_Click(sender As Object, e As EventArgs) Handles C1ButtonCuentaContable.Click
        ListaCatalogoFrm.ShowDialog()
        If ListaCatalogoFrm.DialogResult = DialogResult.OK Then
            Dim drvCat As DataRowView = ListaCatalogoFrm.ListaCatBindingSource.Current
            C1TextBoxCuentaContable.Text = drvCat("Cuenta")
            LabelCuentaContable.Text = drvCat("Descripcion")

        End If
    End Sub

    Private Sub EditDataNavBarPrin_SalirClick(sender As Object, e As EventArgs) Handles EditDataNavBarPrin.SalirClick
        Me.Close()
    End Sub
End Class