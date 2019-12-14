Public Class OraToSql

    Public Function isContinueSql(ByVal aryCommands() As String) As Boolean
        'EXAMPL
        Dim i As Integer
        For i = 0 To aryCommands.Length - 1
            If InStr(aryCommands(i).ToUpper, "TC_") <> 0 Or (InStr(aryCommands(i).ToUpper, "DC_") <> 0 And InStr(aryCommands(i).ToUpper, "DC_CONTACT_CONTACT_TITLE") = 0) Or InStr(aryCommands(i).ToUpper, "DISTRICT_EMPLOYEE_COUNTS") <> 0 _
            Or InStr(aryCommands(i).ToUpper, "EXEC ") <> 0 Then

                Return False
            End If
        Next

        Return True
    End Function


    Public Function ConvertToSQL(ByVal strCom As String) As String

        'Dim strCurrDate As String = ("'" + Format(Now(), "dd-MMM-yyyy") + "'").ToUpper
        strCom = strCom.ToUpper
        'strCom = strCom.Replace(strCurrDate, "GETDATE()")

        'strCurrDate = "'" + Format(Now(), "dd-MMM-yyyy hh:mm:ss").ToUpper + "'"
        'strCom = strCom.Replace(strCurrDate, "GETDATE()")

        'strCurrDate = "'" + Format(Now().AddSeconds(1), "dd-MMM-yyyy hh:mm:ss").ToUpper + "'"
        'strCom = strCom.Replace(strCurrDate, "GETDATE()")

        'strCurrDate = "'" + Format(Now().AddSeconds(-1), "dd-MMM-yyyy hh:mm:ss").ToUpper + "'"
        'strCom = strCom.Replace(strCurrDate, "GETDATE()")

        strCom = strCom.Replace(", ", ",")
        strCom = strCom.Replace(" , ", ",")
        strCom = strCom.Replace(" ,", ",")

        strCom = strCom.Replace(") ", ")")
        strCom = strCom.Replace(" ) ", ")")
        strCom = strCom.Replace(" )", ")")
        strCom = strCom.Replace(" (", "(")

        strCom = strCom.Replace("||", " + ")
        strCom = strCom.Replace("+ BDGT_CYCLE0.SUBMITTED_DATE +", "+ CONVERT(VARCHAR,BDGT_CYCLE0.SUBMITTED_DATE) + ")
        strCom = strCom.Replace("SYSDATE", "GETDATE()")
        strCom = strCom.Replace("SELECT COUNT(*) FROM ", "SELECT COUNT(*) ""COUNT(*)""  FROM ")
        strCom = strCom.Replace("NVL", "ISNULL")
        strCom = strCom.Replace("TRUNC(", "CONVERT(DATE,")

        strCom = strCom.Replace(" PERCENT FROM", " PERCENT0 FROM")  ' Payment Manag...
        strCom = strCom.Replace("SUSPENSE,", " ")                 ' DC_CONTACT - SUSPENSE - not exist 

        strCom = strCom.Replace("''',", "'' ',")
        strCom = strCom.Replace("'',", "NULL,")

        strCom = strCom.Replace("SUBSTR", "SUBSTRING")
        strCom = strCom.Replace("TO_DATE(", "CONVERT(DATETIME,")

        strCom = strCom.Replace("TO_NUMBER(", "CONVERT(INT,")
        strCom = strCom.Replace("TO_CHAR(", "CONVERT(VARCHAR,")

        strCom = strCom.Replace(",'MM/DD/YYYY')", ",101)")

        strCom = strCom.Replace("'DD-MON-YYYY HH24:MI:SS'", "113")

        strCom = strCom.Replace("INITCAP", "DESEPROC.INITCAP")
        'DD-MM-YYYY HH24:MI:SS'

        strCom = strCom.Replace("'1/1/0001 12:00:00 AM'", "'01/01/1753 12:00:00 AM'")
        strCom = strCom.Replace("01-JAN-0001", "01-JAN-1753")
        strCom = strCom.Replace(",'DD-MON-YYYY')", ",106)")
        strCom = strCom.Replace(",'YYYYMMDD')", ",112)")
        strCom = strCom.Replace(",'DD/MM/YYYY')", "103)")

        strCom = strCom.Replace("FROM DUAL", "")

        strCom = strCom.Replace("MINUS", "EXCEPT")

        strCom = strCom.Replace("+  DC_EDUCATOR.SOC_SEC_NO", "+  convert(varchar,DC_EDUCATOR.SOC_SEC_NO) ")


        'County Clerk
        strCom = strCom.Replace("MAX(SEQUENCE_NO), TYPE_CODE", " MAX(SEQUENCE_NO) ""MAX(SEQUENCE_NO)"" , TYPE_CODE")

        ' EpEgs

        strCom = strCom.Replace(" A.YEAR", " convert(varchar,A.YEAR)")
        strCom = strCom.Replace("(A.YEAR - 1)", " convert(varchar,(convert(int,A.YEAR) - 1))")
        strCom = strCom.Replace("ORDER BY ADDED_TO_HISTORY_TIMESTAMP DESC) WHERE ROWNUM = 1", "GROUP BY LAST_MODIFIED_USERID")
        strCom = strCom.Replace("SELECT * FROM (SELECT ADDED_TO_HISTORY_TIMESTAMP,", "SELECT MAX(ADDED_TO_HISTORY_TIMESTAMP) ADDED_TO_HISTORY_TIMESTAMP,")

        strCom = strCom.Replace(" ORDER BY CAST(REPLACE(CYCLE_NAME, 'REQUEST ') AS NUMBER) DESC", "ORDER BY   CONVERT(INT,SUBSTRING(A.CYCLE_NAME,9,2)) DESC")
        strCom = strCom.Replace("SELECT DISTINCT A.CYCLE_NAME, A.STATUS_CODE,", "SELECT DISTINCT  CONVERT(INT,SUBSTRING(A.CYCLE_NAME,9,2)) NUM,A.CYCLE_NAME, A.STATUS_CODE, ")
        strCom = strCom.Replace("FROM DUAL", "FROM YEAR_TYPE WHERE TYPE=1")


        strCom = strCom.Replace("DECODE (RTRIM(A.FK_BDGT_CYCLE0_RSP_TYPE_CODE),  'T1',       'T1-ARRA',  'T2D',      'T2D-ARRA',  'TID-LEA',  'TID-LEA-AR',  'SE_PBENT', 'PARTB_ARRA',  A.FK_BDGT_CYCLE0_RSP_TYPE_CODE)", "case when RTRIM(A.FK_BDGT_CYCLE0_RSP_TYPE_CODE) = 'T1'  then 'T1-ARRA' when RTRIM(A.FK_BDGT_CYCLE0_RSP_TYPE_CODE) = 'T2D' then 'T2D-ARRA' when RTRIM(A.FK_BDGT_CYCLE0_RSP_TYPE_CODE) = 'TID-LEA' then 'TID-LEA-AR' when RTRIM(A.FK_BDGT_CYCLE0_RSP_TYPE_CODE) = 'SE_PBENT' then 'PARTB_ARRA' else  A.FK_BDGT_CYCLE0_RSP_TYPE_CODE end ")

        strCom = strCom.Replace(" +  BDGT_CYCLE0.SUBMITTED_DATE  +", "+  convert(varchar,BDGT_CYCLE0.SUBMITTED_DATE)  +")
        strCom = strCom.Replace(" TRIM(LEASE_PURCHASE)", " LTRIM(RTRIM(LEASE_PURCHASE))")
        strCom = strCom.Replace(" TRIM(TYPE_CODE_3)", " LTRIM(RTRIM(TYPE_CODE_3))")
        strCom = strCom.Replace(" TRIM(TYPE_CODE_2)", " LTRIM(RTRIM(TYPE_CODE_2))")
        strCom = strCom.Replace(" TRIM(TYPE_CODE_1)", " LTRIM(RTRIM(TYPE_CODE_1))")
        strCom = strCom.Replace(" TRIM(BDGT_BUDGET_SUPPORT_DATA.TYPE_CODE_1)", " LTRIM(RTRIM(BDGT_BUDGET_SUPPORT_DATA.TYPE_CODE_1))")
        strCom = strCom.Replace(" TRIM(BDGT_BUDGET_SUPPORT_DATA.TYPE_CODE_2)", " LTRIM(RTRIM(BDGT_BUDGET_SUPPORT_DATA.TYPE_CODE_2))")
        strCom = strCom.Replace(" TRIM(COURSE_ASSIGNMENT.GRADE)", " LTRIM(RTRIM(COURSE_ASSIGNMENT.GRADE))")
        strCom = strCom.Replace("ORDER BY FK_SEC_OF_BRD_SEQUENCE_NUMBER DESC  ) SUP,", " ) SUP,")
        strCom = strCom.Replace(", ROWID ", " ")
        strCom = strCom.Replace(",ROWID ", " ")
        strCom = strCom.Replace(" TRIM(COUNT_VALUE)", " COUNT_VALUE")

        ConvertToSQL = strCom

    End Function
End Class
