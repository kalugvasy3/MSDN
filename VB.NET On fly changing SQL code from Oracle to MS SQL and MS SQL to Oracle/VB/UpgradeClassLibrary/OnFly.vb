Public Class OnFly


    Public Function aryCommandForMsSql(ByVal aryCommands() As String) As String()

        Dim aryTmp() As String = aryCommands.Clone()

        'INSERT INTO ACCOUNTING_TRANSACTION_LINE (INSTANCE_ID,     

        Dim i As Integer
        For i = aryTmp.GetLowerBound(0) To aryTmp.GetUpperBound(0)
            aryTmp(i) = aryTmp(i).ToUpper.Replace("  ", " ") ' 2 spaces to one
            If ((InStr(aryTmp(i), "INSERT") < InStr(aryTmp(i), "INTO")) And (InStr(aryTmp(i), "INTO") < InStr(aryTmp(i), "ACCOUNTING_TRANSACTION_LINE"))) Then

                Dim intPos1 As Integer = InStr(aryTmp(i), "(") ' find position for "("
                Dim intPos2 As Integer = InStr(intPos1, aryTmp(i), ",") ' find position for first "," after "("
                aryTmp(i) = aryTmp(i).Remove(intPos1, intPos2 - intPos1) ' remove with ","

                Dim intPos0 As Integer = InStr(aryTmp(i), "VALUES")     ' find position for "VALUE"
                intPos1 = InStr(intPos0, aryTmp(i), "(") ' find position for "("
                intPos2 = InStr(intPos1, aryTmp(i), ",") ' find position for first "," after "("
                aryTmp(i) = aryTmp(i).Replace(aryTmp(i).Substring(intPos1, intPos2 - intPos1), "") ' replace with ","
            End If
        Next

        Return aryTmp
    End Function



    Public Function aryCommandForOracle(ByVal aryCommands() As String, ByVal strScope As String) As String()

        Dim aryTmp() As String = aryCommands.Clone()

        Dim i As Integer
        For i = aryTmp.GetLowerBound(0) To aryTmp.GetUpperBound(0)
            aryTmp(i) = aryTmp(i).ToUpper.Replace("  ", " ") ' 2 spaces to one

            If ((InStr(aryTmp(i), "INSERT") < InStr(aryTmp(i), "INTO")) And (InStr(aryTmp(i), "INTO") < InStr(aryTmp(i), "ACCOUNTING_TRANSACTION_LINE"))) Then
                Dim intPos0 As Integer = InStr(aryTmp(i), "VALUES")     ' find position for "VALUE"
                Dim intPos1 As Integer = InStr(intPos0, aryTmp(i), "(") ' find position for "("
                Dim intPos2 As Integer = InStr(intPos1, aryTmp(i), ",") ' find position for first "," after "("
                aryTmp(i) = aryTmp(i).Replace(aryTmp(i).Substring(intPos1, intPos2 - intPos1 - 1), strScope) ' replace without ","
            End If

        Next
        Return aryTmp
    End Function




End Class
