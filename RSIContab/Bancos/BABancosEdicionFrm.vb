Imports System.ComponentModel
Imports System.Drawing.Printing

Public Class BABancosEdicionFrm
    Private _FormularioPrincipal As BABancosFrm
    Private _CodigoCentro As Int32
    Private _Modalidad As String
    Private _intBanco As Int16

    Private _Año As String
    Public Property año() As Int16
        Get
            Return _Año
        End Get
        Set(ByVal value As Int16)
            _Año = value
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
    Dim dbCls As New CTClassLib.CTClass
    Dim CadCls As New Rsierpgencl.Rsierpcl.Cadenas
    Dim drCentros As SqlClient.SqlDataReader
    Dim drConf As SqlClient.SqlDataReader
    Dim mCentroSeleccionado As Boolean
    Dim ListaCatalogoFrm As New CtListaCatGenFrm
    Dim boCuentaContSel As Boolean
    Dim boCuentaAjusteSel As Boolean

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

        'TODO: esta línea de código carga datos en la tabla 'BABancosEdicionDataSet.CTCatalogoCuentas' Puede moverla o quitarla según sea necesario.
        Me.CTCatalogoCuentasTableAdapter.Connection.ConnectionString = strcnCAD
        Me.BABancosTableAdapter.Connection.ConnectionString = strcnCAD
        Me.BATransaccionesEncabezadoTableAdapter.Connection.ConnectionString = strcnCAD
        Me.BASaldosMensualesTableAdapter.Connection.ConnectionString = strcnCAD
        Me.CTCatalogoCuentasTableAdapter.Fill(Me.BABancosEdicionDataSet.CTCatalogoCuentas)

        If Modalidad = "NUEVO" Then
            PrincipalBindingSource.AddNew()
        End If
        If Modalidad = "EDITAR" Then
            Me.BATransaccionesEncabezadoTableAdapter.Fill(Me.BABancosEdicionDataSet.BATransaccionesEncabezado, _intBanco, _Año, _Mes)
        End If
        Dim strImpInstaladas As String
        dbCls.DBconStr = strcnCAD
        EditDataNavBarPrin.ToolStripButtonEliminar.Visible = False
        For i As Int16 = 0 To PrinterSettings.InstalledPrinters.Count - 1
            strImpInstaladas = PrinterSettings.InstalledPrinters.Item(i)
            C1ComboBoxImpresora.Items.Add(strImpInstaladas)
        Next
        Dim DefPrn As New PrinterSettings
        C1ComboBoxImpresora.Text = DefPrn.PrinterName
        boCuentaContSel = False
        boCuentaAjusteSel = False
    End Sub

    Private Sub C1ButtonCuentaContable_Click(sender As Object, e As EventArgs) Handles C1ButtonCuentaContable.Click
        boCuentaContSel = False
        ListaCatalogoFrm.ShowDialog()
        If ListaCatalogoFrm.DialogResult = DialogResult.OK Then
            boCuentaContSel = True
            Dim drvCat As DataRowView = ListaCatalogoFrm.ListaCatBindingSource.Current
            C1TextBoxCuentaContable.Text = drvCat("Cuenta")
            LabelCuentaContable.Text = drvCat("Descripcion")
            boCuentaContSel = False
        End If
    End Sub

    Private Sub EditDataNavBarPrin_SalirClick(sender As Object, e As EventArgs) Handles EditDataNavBarPrin.SalirClick
        Me.Close()
    End Sub

    Private Sub C1ButtonCuentaAjuste_Click(sender As Object, e As EventArgs) Handles C1ButtonCuentaAjuste.Click
        boCuentaAjusteSel = False
        ListaCatalogoFrm.ShowDialog()
        If ListaCatalogoFrm.DialogResult = DialogResult.OK Then
            boCuentaAjusteSel = True
            Dim drvCat As DataRowView = ListaCatalogoFrm.ListaCatBindingSource.Current
            C1TextBoxCuentaAjuste.Text = drvCat("Cuenta")
            LabelCuentaAjuste.Text = drvCat("Descripcion")
            boCuentaAjusteSel = False
        End If
    End Sub

    Private Sub C1TextBoxCuentaContable_Validating(sender As Object, e As CancelEventArgs) Handles C1TextBoxCuentaContable.Validating

        If C1TextBoxCuentaContable.Text.ToString.Length > 0 Then
            If boCuentaContSel = False Then
                Dim drCat As DataRow = Me.BABancosEdicionDataSet.CTCatalogoCuentas.FindByCuenta(C1TextBoxCuentaContable.Text)
                If drCat Is Nothing Then
                    MsgBox("Cuenta no existe!")
                    C1TextBoxCuentaContable.Text = ""
                    LabelCuentaContable.Text = ""
                    e.Cancel = True
                Else
                    If Not drCat("Posteable") Then
                        C1TextBoxCuentaContable.Text = ""
                        LabelCuentaContable.Text = ""
                        MsgBox("Cuenta no es posteable!")
                        e.Cancel = True
                    Else
                        boCuentaContSel = True
                        C1TextBoxCuentaContable.Text = drCat("Cuenta")
                        LabelCuentaContable.Text = drCat("Descripcion")

                    End If
                End If
            End If
        End If
    End Sub

    Private Sub C1ButtonCuentaAjuste_Validating(sender As Object, e As CancelEventArgs) Handles C1ButtonCuentaAjuste.Validating
        If C1TextBoxCuentaAjuste.Text.ToString.Length > 0 Then
            If boCuentaAjusteSel = False Then
                Dim drCat As DataRow = Me.BABancosEdicionDataSet.CTCatalogoCuentas.FindByCuenta(C1TextBoxCuentaAjuste.Text)
                If drCat Is Nothing Then
                    MsgBox("Cuenta no existe!")
                    C1TextBoxCuentaAjuste.Text = ""
                    LabelCuentaAjuste.Text = ""
                    e.Cancel = True
                Else
                    If Not drCat("Posteable") Then
                        C1TextBoxCuentaAjuste.Text = ""
                        LabelCuentaAjuste.Text = ""
                        MsgBox("Cuenta no es posteable!")
                        e.Cancel = True
                    Else
                        boCuentaAjusteSel = True
                        C1TextBoxCuentaAjuste.Text = drCat("Cuenta")
                        LabelCuentaAjuste.Text = drCat("Descripcion")
                    End If
                End If
            End If
        End If
    End Sub
End Class