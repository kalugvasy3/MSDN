using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using System.Data.Common;

namespace WpfCommonDataAccess
{
    /// <summary>
    /// You can also read 

    /// 
    /// </summary>
    public partial class MainWindow : Window
    {


       private string strProvider = "";
       private bool blnIsConnectionTrue = false;

        public MainWindow()
        {
            InitializeComponent();

            LoadProviders();
        }

       public void LoadProviders()
        {
            // Retrieve the installed providers and factories.
            DataTable table = DbProviderFactories.GetFactoryClasses();

            comboProviders.DisplayMemberPath = "Name";
            comboProviders.SelectedValuePath = "InvariantName";
            comboProviders.ItemsSource = table.DefaultView;

            txtSQL.Text = sqlCalendar;
        
            txtConnectionString.Text = @"Server=.\SQLExpress; Database=tempdb; Trusted_Connection=Yes;";
        }

        private void comboProviders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            strProvider = comboProviders.SelectedValue.ToString();
            btnTest.Background = new SolidColorBrush(Colors.LightGray);
            blnIsConnectionTrue = false;
            txtStatus.Text = "";

            if (strProvider.IndexOf("OleDb")>=0)
            {
                DataOut newDataOut = new DataOut();
                newDataOut.dsResult = null;
                newDataOut.Show();

                // Need to subscribe ...

                newDataOut.runDBSql();
                listOpenDataOut.Add(newDataOut);
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            DataBase db = new DataBase();
           
            string strError = "";

            blnIsConnectionTrue = db.TestConnection(strProvider,txtConnectionString.Text,out strError) ;
            
            if (blnIsConnectionTrue)
            {
              btnTest.Background = new SolidColorBrush(Colors.LightGreen);
              btnTest.Background = new SolidColorBrush(Colors.Green);
              txtStatus.Text = "Connection - OK.";
              txtStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                btnTest.Background = new SolidColorBrush(Colors.Red);
                txtStatus.Text = strError;
                txtStatus.Foreground = new SolidColorBrush(Colors.Red);
            }       
        }
 
        private void txtConnectionString_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btnTest != null)
            {
              btnTest.Background = new SolidColorBrush(Colors.LightGray);
            }
            blnIsConnectionTrue = false;
        }

        private List<DataOut> listOpenDataOut = new List<DataOut>();

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (blnIsConnectionTrue) 
            {
                DataOut newDataOut = new DataOut();
                newDataOut.dsResult = null;
                newDataOut.Show();

                listOpenDataOut.Add(newDataOut);

                // Need to subscribe ...

                newDataOut.runSQL(strProvider, txtConnectionString.Text, new StringBuilder(txtSQL.Text));
                txtStatus.Text = "RUN";
                txtStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                txtStatus.Text = "Please TEST Connection FIRST...";
                txtStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        /// <summary>
        /// Close ALL child ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WPFCommonData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (DataOut dout in listOpenDataOut)
            {
                try
                {
                  dout.Close();
                }
                catch{}
            }
            
        }

        private string sqlCalendar = @"
----- This code works with SqlClient Data Provider -- PLEASE SELECT SqlClient Data Provider and choose right for you Connection String and [TEST]
----- Button [TEST] must be green ...

----- Examle -- Missouri Holidays Calendar 
----- SET parameter @firstDayYear to first day of your year ..


SET DATEFIRST 7;

DECLARE @firstDayYear AS DATE = '01/01/2021';

    WITH DATERANGE(DT) AS (

    --SELECT DISTINCT CONVERT(DATE, CAST(MONTH(GETDATE()) AS CHAR(2) ) + '/01/' + CAST(YEAR(GETDATE()) AS CHAR(4)), 101) 
    SELECT DISTINCT @firstDayYear 

    UNION ALL 

    SELECT  DATEADD(DAY, 1, DT)
    FROM DATERANGE
    WHERE -- MONTH(DT) = MONTH(GETDATE()) AND
        YEAR(DT) = YEAR(@firstDayYear) 
    )


    SELECT * FROM
    (

	SELECT DT            As ""Date"",
            MONTH(DT)     AS ""Month"",
            DAY(DT)       AS ""Day"",
            DATEPART (dw,DT) AS ""DayOfWeek"",

        CASE   
-- NEW YEAR ------------------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 1  AND DAY(DT) = 1  THEN 'NEW YEAR'
            WHEN MONTH(DT) = 12 AND DAY(DATEADD(DAY, 1, DT)) = 1 
                                AND DATEPART (dw, DATEADD(DAY, 1, DT)) = 7 THEN 'OBSERVED -  BEFORE NEW YEAR  (IF SATURDAY))'
            WHEN MONTH(DT) = 1  AND DAY(DATEADD(DAY, -1, DT)) = 1 
                                AND DATEPART (dw, DATEADD(DAY, -1, DT)) = 1 THEN 'OBSERVED -  AFTER NEW YEAR  (IF SUNDAY)'
-----------------------------------------------------------------------------------------------------------------------
-- MARTIN LUTHER KING DAY - 3d MONDAY----------------------------------------------------------------------------------       
            WHEN MONTH(DT) = 1  AND DATEPART (dw, DT) = 2 AND MONTH(DATEADD(DAY, -21, DT)) = 12 AND MONTH(DATEADD(DAY, -14, DT)) = 1  THEN 'MARTIN LUTHER KING DAY - 3d MONDAY'   
-----------------------------------------------------------------------------------------------------------------------
-- LINCOLN DAY'S DAY ---------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 2  AND DAY(DT) = 12   THEN 'LINCOLN DAY''S DAY'           
            WHEN MONTH(DT) = 2  AND DAY(DATEADD(DAY, 1, DT)) = 12 
                                AND DATEPART (dw,DATEADD(DAY, 1, DT)) = 7 THEN 'OBSERVED -  BRFORE LINCOLN DAY''S DAY  (IF SATURDAY))'
            WHEN MONTH(DT) = 2  AND DAY(DATEADD(DAY, -1, DT)) = 12 
                                AND DATEPART (dw,DATEADD(DAY, -1, DT)) = 1 THEN 'OBSERVED -  AFTER LINCOLN DAY''S DAY  (IF SUNDAY)'
------------------------------------------------------------------------------------------------------------------------
-- WASHINGTON'S DAY       - 3d MONDAY ----------------------------------------------------------------------------------
            WHEN MONTH(DT) = 2  AND DATEPART (dw,DT) = 2 AND MONTH(DATEADD(DAY, -21, DT)) = 1 AND MONTH(DATEADD(DAY, -14, DT)) = 2      THEN 'WASHINGTON''S DAY'   
-----------------------------------------------------------------------------------------------------------------------
-- TRUMAN DAY DAY'S DAY ------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 5  AND DAY(DT) = 8   THEN 'TRUMAN DAY DAY''S DAY'  
            WHEN MONTH(DT) = 5  AND DAY(DATEADD(DAY, 1, DT)) = 8 
                                AND DATEPART (dw,DATEADD(DAY, 1, DT)) = 7 THEN 'OBSERVED -  BEFORE TRUMAN DAY DAY''S DAY  (IF SATURDAY))'
            WHEN MONTH(DT) = 5  AND DAY(DATEADD(DAY, -1, DT)) = 8 
                                AND DATEPART (dw,DATEADD(DAY, -1, DT)) = 1 THEN 'OBSERVED -  AFTER TRUMAN DAY DAY''S DAY  (IF SUNDAY)'
------------------------------------------------------------------------------------------------------------------------ 
-- MEMORIAL DAY -- LAST MONDAY -----------------------------------------------------------------------------------------  
            WHEN MONTH(DT) = 5  AND DATEPART (dw,DT) = 2 AND MONTH(DATEADD(DAY, 7, DT)) = 6 THEN 'MEMORIAL DAY -- LAST MONDAY'    
-----------------------------------------------------------------------------------------------------------------------  
-- INDEPENDENCE DAY ----------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 7  AND DAY(DT) = 4                THEN 'INDEPENDENCE DAY'
            WHEN MONTH(DT) = 7  AND DAY(DATEADD(DAY, 1, DT)) = 4 
                                AND DATEPART (dw,DATEADD(DAY, 1, DT)) = 7 THEN 'OBSERVED -  BEFORE INDEPENDENCE DAY (IF SATURDAY))'
            WHEN MONTH(DT) = 7  AND DAY(DATEADD(DAY, -1, DT)) = 4 
                                AND DATEPART (dw,DATEADD(DAY, -1, DT)) = 1 THEN 'OBSERVED -  AFTER INDEPENDENCE DAY (IF SUNDAY)'
-----------------------------------------------------------------------------------------------------------------------
-- LABOR DAY ----------------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 9  AND DATEPART (dw,DT) = 2 AND MONTH(DATEADD(DAY, -7, DT)) = 8 THEN 'LABOR DAY (FIRST MONDAY OF SEPTEMBER)' 
----------------------------------------------------------------------------------------------------------------------- 
-- COLUMBUS DAY -------------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 10 AND DATEPART (dw,DT) = 2 AND  MONTH(DATEADD(DAY, -14, DT)) = 9 AND MONTH(DATEADD(DAY, -7, DT)) = 10  THEN 'COLUMBUS DAY'    
-----------------------------------------------------------------------------------------------------------------------      
-- VETERANS DAY ------------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 11 AND DAY(DT) = 11 THEN 'VETERANS DAY'
            WHEN MONTH(DT) = 11 AND DAY(DATEADD(DAY, 1, DT)) = 11 
                                AND DATEPART (dw,DATEADD(DAY, 1, DT)) = 7 THEN 'OBSERVED -  BEFORE VETERANS DAY'
            WHEN MONTH(DT) = 11 AND DAY(DATEADD(DAY, -1, DT)) = 11 
                                AND DATEPART (dw,DATEADD(DAY, -1, DT)) = 1 THEN 'OBSERVED -  AFTER VETERANS DAY'
-----------------------------------------------------------------------------------------------------------------------   
-- THANKS GIVING ----------------------------------------------------------------------------------------------------- 
            WHEN MONTH(DT) = 11 AND DATEPART (dw,DT) = 5 AND MONTH(DATEADD(DAY, -21, DT)) = 11 AND MONTH(DATEADD(DAY, -28, DT)) = 10   THEN 'THANKS GIVING - 4th Thursday   '  
            WHEN MONTH(DT) = 11 AND DATEPART (dw,DT) = 6 AND MONTH(DATEADD(DAY, -22, DT)) = 11 AND MONTH(DATEADD(DAY, -29, DT)) = 10   THEN 'GRANTED DAY AFTER THANKS GIVING' 
 
-- X'MAS --------------------------------------------------------------------------------------------------------------
            WHEN MONTH(DT) = 12 AND DAY(DT) = 25               THEN 'X''MAS'
            WHEN MONTH(DT) = 12 AND DAY(DATEADD(DAY, 1, DT)) = 25 
                                AND DATEPART (dw,DATEADD(DAY, 1, DT)) = 7 THEN 'OBSERVED -  BEFORE X''MAS' 
            WHEN MONTH(DT) = 12 AND DAY(DATEADD(DAY, -1, DT)) = 25 
                                AND DATEPART (dw,DATEADD(DAY, -1, DT)) = 1 THEN 'OBSERVED -  AFTER  X''MAS' 
-----------------------------------------------------------------------------------------------------------------------
        
            ELSE ''
        END AS ""IsUSPublicHoliday"",

        CASE 
            WHEN DATEPART (dw,DT) = 1 THEN 1
            WHEN DATEPART (dw,DT) = 7 THEN 1
            ELSE 0
        END AS ""IsWeekEnd""  

		FROM
		(
            SELECT DT FROM DATERANGE
            WHERE  YEAR(DT) = YEAR(@firstDayYear) 

		) AS tbl

	) AS tAll

	    WHERE tAll.""IsUSPublicHoliday"" <> ''

		option (maxrecursion 366);

                        ";
            


    }
}
