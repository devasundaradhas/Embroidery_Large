Partial Class InventoryDataSet
    Partial Public Class ReportTempDataTable
        Private Sub ReportTempDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.Int8Column.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class
End Class

Namespace InventoryDataSetTableAdapters
    
    Partial Public Class DataTable1TableAdapter

    End Class
End Namespace
