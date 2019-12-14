Public Class SqlToOra

    Public Function isContinueOra(ByVal aryCommands() As String) As Boolean
        Dim i As Integer
        For i = 0 To aryCommands.Length - 1
            Dim strCom As String = aryCommands(i).ToUpper
            'PUBLIC   NONPUBLIC
            If (InStr(strCom, "LIC_") <> 0 And InStr(strCom, "PUBLIC") = 0) Or InStr(strCom, "GED_") <> 0 _
            Or InStr(strCom, "DESE_UPGRADE_") <> 0 Or InStr(strCom, "ACES_") <> 0 Or InStr(strCom, "SCHOOL_DIRECTORY") <> 0 _
            Or InStr(strCom, "HQT_") <> 0 Or InStr(strCom, "MIGRANT_") <> 0 Or InStr(strCom, "DESE_TREEVIEW") <> 0 _
            Or (InStr(strCom, "SW_") <> 0 And InStr(strCom, "SW_PAYMENT") = 0 And InStr(strCom, "SW_POOL") = 0 And InStr(strCom, "SW_BATCH") = 0) _
            Or (InStr(strCom, "DC_") <> 0 And InStr(strCom, "DC_CONTACT_CONTACT_TITLE") = 0) Or InStr(strCom, "DISTRICT_EMPLOYEE_COUNTS") <> 0 _
            Or InStr(strCom, "_DATAWAREHOUSE_") <> 0 Or InStr(strCom, "EDUCATOR_VACANCY") <> 0 _
            Or InStr(strCom, "DISTRICT_EMPLOYEE_COUNTS") <> 0 Or InStr(strCom, "DESE_SYSTEM_SETTINGS ") <> 0 _
            Or InStr(strCom, "EXEC ") <> 0 Or InStr(strCom, "LIBRARY_MEDIA") <> 0 _
            Or InStr(strCom, "LIBRARY_FUNDS") <> 0 Or InStr(strCom, "RECEIVING_DISTRICT") <> 0 _
            Or InStr(strCom, "PLACEMENT_") <> 0 Or InStr(strCom, "CALENDAR_") <> 0 _
            Or (InStr(strCom, "PHYSICAL_") <> 0 And InStr(strCom, "PHYSICAL_HEALTH") = 0) _
            Or (InStr(strCom, "ARRA_") <> 0 And InStr(strCom, "ARRA_STAFF") = 0 _
                And InStr(strCom, "ARRA_JOBS") = 0 And InStr(strCom, "ARRA_GRID") = 0) _
            Or InStr(strCom, "EPEGS_PLAN") <> 0 Then

                Return False
            End If
        Next
        Return True




    End Function



    Public Function ConvertToORA(ByVal strCom As String) As String

        'Dim strCurrDate As String = ("'" + Format(Now(), "dd-MMM-yyyy") + "'").ToUpper
        strCom = strCom.ToUpper.Trim
        'strCom = strCom.Replace(strCurrDate, "SYSDATE")

        'strCurrDate = "'" + Format(Now(), "dd-MMM-yyyy hh:mm:ss").ToUpper + "'"
        'strCom = strCom.Replace(strCurrDate, "SYSDATE")

        'strCurrDate = "'" + Format(Now().AddSeconds(1), "dd-MMM-yyyy hh:mm:ss").ToUpper + "'"
        'strCom = strCom.Replace(strCurrDate, "SYSDATE")

        'strCurrDate = "'" + Format(Now().AddSeconds(-1), "dd-MMM-yyyy hh:mm:ss").ToUpper + "'"
        'strCom = strCom.Replace(strCurrDate, "SYSDATE")

        strCom = strCom.Replace(" + ", " || ")
        strCom = strCom.Replace("GETDATE()", "SYSDATE")

        strCom = strCom.Replace("''',", "'' ',")
        strCom = strCom.Replace("'',", "NULL,")

        strCom = strCom.Replace(", ", ",")
        strCom = strCom.Replace(" , ", ",")
        strCom = strCom.Replace(" ,", ",")

        strCom = strCom.Replace(") ", ")")
        strCom = strCom.Replace(" ) ", ")")
        strCom = strCom.Replace(" )", ")")


        strCom = strCom.Replace("'1/1/0001 12:00:00 AM',113)", "'01/01/1753',101)")

        strCom = strCom.Replace("MAX(BOND_PAYMENT_SCHEDULE.INSTANCE_ID)", "MAX(BOND_PAYMENT_SCHEDULE.INST")
        strCom = strCom.Replace("ISNULL", "NVL")
        strCom = strCom.Replace("SUBSTRING", "SUBSTR")

        strCom = strCom.Replace("CONVERT(DATE,'31-DEC-2999')", "TO_DATE('31-DEC-2999','DD-MON-YYYY')")
        strCom = strCom.Replace("CONVERT(DATE, '31-DEC-2999')", "TO_DATE('31-DEC-2999','DD-MON-YYYY')")
        strCom = strCom.Replace("CONVERT(DATETIME,", "TO_DATE(")
        strCom = strCom.Replace("CONVERT(DATE,", "TO_DATE(")

        strCom = strCom.Replace("CONVERT(VARCHAR(10),", "TO_CHAR(")
        strCom = strCom.Replace("CONVERT(VARCHAR,", "TO_CHAR(")
        strCom = strCom.Replace("CONVERT(NUMERIC,", "TO_NUMBER(")
        strCom = strCom.Replace("CONVERT(CHAR,", "TO_CHAR(")
        strCom = strCom.Replace("CONVERT(CHAR(9),", "TO_CHAR(")
        strCom = strCom.Replace("CONVERT(VARCHAR(19),", "TO_CHAR(")
        strCom = strCom.Replace("CONVERT(CHAR(10),", "TO_CHAR(")
        strCom = strCom.Replace(",120)", ",'YYYY-MM-DD HH24:MI:SS')")
        strCom = strCom.Replace(", 120)", ",'YYYY-MM-DD HH24:MI:SS')")

        strCom = strCom.Replace(",113)", ",'DD-MON-YYYY HH24:MI:SS')")
        strCom = strCom.Replace(",113 )", ",'DD-MON-YYYY HH24:MI:SS')")
        strCom = strCom.Replace(", 113)", ",'DD-MON-YYYY HH24:MI:SS')")

        strCom = strCom.Replace("113),", "'DD-MON-YYYY HH24:MI:SS'),")
        strCom = strCom.Replace(",112)", ",'YYYYMMDD')")
        strCom = strCom.Replace(",106)", ",'DD-MON-YYYY')")
        strCom = strCom.Replace(",106 )", ",'DD-MON-YYYY')")
        strCom = strCom.Replace(", 106)", ",'DD-MON-YYYY')")

        strCom = strCom.Replace("DESEPROC.INITCAP", "INITCAP")

        strCom = strCom.Replace(",101)", ",'MM/DD/YYYY')")
        strCom = strCom.Replace(",101 )", ",'MM/DD/YYYY')")
        strCom = strCom.Replace(", 101)", ",'MM/DD/YYYY')")

        strCom = strCom.Replace(",103)", ",'DD/MM/YYYY')")

        strCom = strCom.Replace("[", """")
        strCom = strCom.Replace("]", """")

        strCom = strCom.Replace(" EXCEPT ", " MINUS ")
        strCom = strCom.Replace(" CONVERT(INT", " TO_NUMBER(")

        If InStr(strCom, "SELECT") = 1 And InStr(strCom, "FROM") = 0 Then
            strCom = strCom + " FROM DUAL"
        End If


        strCom = strCom.Replace("DBO.", "")

        strCom = strCom.Replace("1/1/1900", "01-JAN-1900")
        strCom = strCom.Replace("01/01/1900", "01-JAN-1900")
        strCom = strCom.Replace("01-JAN-0001", "01-JAN-1753")


        ConvertToORA = strCom

    End Function
End Class
