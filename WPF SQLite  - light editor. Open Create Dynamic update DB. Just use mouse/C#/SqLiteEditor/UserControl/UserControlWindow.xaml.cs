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

using Microsoft.Win32;

namespace SqLiteEditor
{
    /// <summary>
    /// Interaction logic for UserControlWindow.xaml
    /// </summary>


    public partial class UserControlWindow : System.Windows.Controls.UserControl
    {
        private importCSVfile newImport;
        private resultInGrid newResult ;
        private resultInGrid schemaGrid;

        private DataBase db = new DataBase();
        private classHelper helper = new classHelper(); 
        private int intStyle = 0;  // None
        private string strTextOfCommand = "";

        // Event Handler ------------------------------------------------------------------------------------------------------------------
        // Create Event for sending "Style" outside UserControlWindow ...

        public delegate void routedEventHandler(object sender, routedEventArgs e);
        public event routedEventHandler styleChanged;

        public class routedEventArgs
        {
            public routedEventArgs(int _intStyle) { intStyle = _intStyle; }
            public int intStyle { get; private set; } // readonly
        }

        // Event Handler - END -------------------------------------------------------------------------------------------------------------


        private double origW = 768.0;
        private double origH = 535.0;


        // Initialize UserControlWindow ...
        public UserControlWindow()
        {
            InitializeComponent();

            InitTabsOfTextBoxes();

            origH = userMainControl.Height;
            origW = userMainControl.Width;
        }

        private TextBox newTextBox(string strInit)
        {
            TextBox tb = new TextBox();
            tb.AcceptsReturn = true;
            tb.FontSize = 14.0;
            tb.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            tb.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            tb.SpellCheck.IsEnabled = false;
            tb.TextWrapping = TextWrapping.NoWrap;
            tb.UndoLimit = 1000;
            tb.Text = strInit;
            tb.Visibility = Visibility.Visible;
            tb.Background = Brushes.White;

            return tb;
        }


        private void InitTabsOfTextBoxes()
        {
            AddNewTextBox("S" + tabPlaceHolder.Items.Count.ToString() );  // 
            listOfTables.Visibility = Visibility.Collapsed;
        }

        private void AddNewTextBox(string strTxt)
        {
            TextBox tbNew = newTextBox(strTxt);
            tbNew.TextChanged += TbNew_TextChanged;
            TabItem ti = new TabItem();

            ti.ToolTip = strTxt;
            ti.Header = strTxt;


            ti.Content = tbNew;
            tabPlaceHolder.Items.Add(ti);
            tabPlaceHolder.SelectedIndex = tabPlaceHolder.Items.Count - 1;
        }

        private void TbNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;

            string strContent = ((TextBox)ti.Content).Text;

            if (strContent.Length >= 200)
            {
                ti.ToolTip = strContent.Substring(0, 200) + ".........................";
            }
            else
            {
                ti.ToolTip = strContent;
            }
        }


        private void btnStyle_MouseEnter(object sender, MouseEventArgs e)
        {
            btnStyle.Foreground = Brushes.Magenta;
        }

        private void btnStyle_MouseLeave(object sender, MouseEventArgs e)
        {
            btnStyle.Foreground = Brushes.Black;
        }

        private void btnStyle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ucScale.ScaleX == 1.0)
            {
                origH = userMainControl.Height;
                origW = userMainControl.Width;
            }

            userMainControl.Width = origW * ucScale.ScaleX;
            userMainControl.Height = origH * ucScale.ScaleY;

            if (e.Delta > 0)
            {

                ucScale.ScaleX = ucScale.ScaleX + 0.1;
                ucScale.ScaleY = ucScale.ScaleY + 0.1;

                if (ucScale.ScaleX >= 1.7)
                {
                    ucScale.ScaleX = 1.7;
                    ucScale.ScaleY = 1.7;
                }
            };

            if (e.Delta < 0)
            {
                ucScale.ScaleX = ucScale.ScaleX - 0.1;
                ucScale.ScaleY = ucScale.ScaleY - 0.1;

                if (ucScale.ScaleX <= 0.7)
                {
                    ucScale.ScaleX = 0.7;
                    ucScale.ScaleY = 0.7;
                }
            };

            sliderSc.Value = ucScale.ScaleX;
        }


        private void btnStyle_Click(object sender, RoutedEventArgs e)
        {
            intStyle++;
            intStyle = intStyle % 3;

            styleChanged(this, new routedEventArgs(intStyle));  // Event - if top level code will subscribe this....
        }

        private void btnAddNewSql_Click(object sender, RoutedEventArgs e)
        {
            int intCount = tabPlaceHolder.Items.Count;
            string str = "S" + intCount.ToString()  ;
            AddNewTextBox(str);
        }

        private DataSet dsCurrentUserTables = null;

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            bool blnExist = false;
            if (!db.blnTestConnection(txtConnectionString.Text))
            {
                MessageBox.Show("Wrong Connection String! ");
                return;
            };

            gridCode.IsEnabled = true;
            dsCurrentUserTables = helper.BringAlltables(txtConnectionString.Text);

            if (dsCurrentUserTables.Tables[0].Rows.Count != 0)
            {
                listOfTables.ItemsSource = dsCurrentUserTables.Tables[0].AsDataView();

                blnExist = true;
            }

            if (blnExist)
            {
                btnConnect.Background = Brushes.LightGreen;
                btnConnect.ToolTip = txtConnectionString.Text;
            }
            else
            {
                btnConnect.Background = Brushes.LightPink;
                btnConnect.ToolTip = "Empty Data Base!";
                MessageBox.Show("Attention! \nDataBase is empty! \nNew DataBase?");
            }
        }

        private void txtConnectionString_TextChanged(object sender, TextChangedEventArgs e)
        {
            gridCode.IsEnabled = false;
            if (btnConnect != null) btnConnect.Background = Brushes.LightGray;

        }

        private void Run()
        {

            TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;
            TextBox tb = (TextBox)ti.Content;

            string strSelected = tb.SelectedText.Trim();

            // If Selected - Run Selec6ted TEXT

            StringBuilder sbRun;
            if (strSelected != "")
            {
                sbRun = new StringBuilder(strSelected);
            }
            else
            {
                sbRun = new StringBuilder(tb.Text);
            }

            if (newResult != null) newResult.Close();

            newResult = new resultInGrid();
            newResult.Show();
            newResult.runSQL(txtConnectionString.Text, sbRun);
        }


        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void btnSchema_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sbRun = new StringBuilder("SELECT * FROM sqlite_master");

            if (schemaGrid != null) schemaGrid.Close();

            schemaGrid = new resultInGrid();
            schemaGrid.Show();
            schemaGrid.runSQL(txtConnectionString.Text, sbRun);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            String strCreate =
                                @" -- CREATE TABLE - TEMPLATE 
     
                                      CREATE TABLE `Employees` (
                                      EmployeeID int NOT NULL,
                                      LastName varchar(20) NOT NULL,
                                      FirstName varchar(10) NOT NULL,
                                      Title varchar(30),
                                      TitleOfCourtesy varchar(25),
                                      BirthDate timestamp,
                                      HireDate timestamp,
                                      Address varchar(60),
                                      City varchar(15),
                                      Region varchar(15),
                                      PostalCode varchar(10),
                                      Country varchar(15),
                                      HomePhone varchar(24),
                                      Extension varchar(4),
                                      Photo blob,
                                      Notes text,
                                      ReportsTo int,
                                      PhotoPath varchar(255),
                                      PRIMARY KEY(EmployeeID));

                                 -- CREATE VIEW  - TEMPLATE 

                                    CREATE VIEW ""Summary of Sales by Year"" AS
                                    SELECT Orders.ShippedDate, Orders.OrderID, ""Order Subtotals"".Subtotal
                                    FROM Orders INNER JOIN ""Order Subtotals"" ON Orders.OrderID = ""Order Subtotals"".OrderID
                                    WHERE Orders.ShippedDate IS NOT NULL
                                    --ORDER BY Orders.ShippedDate

                                ";
            try
            {
                TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;
                ((TextBox)ti.Content).Text = strCreate;
            }
            catch { }
        }


        private void listOfTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;

            // Insert Table Name ...
            ListBox lb = (ListBox)sender;
            DataRowView drv = (DataRowView)lb.SelectedValue;
            if (drv == null) return;
            // Put Text to current TEXT BOX.  INSERT/UPDATE
            string strTableName = drv["tbl_name"].ToString().Trim();

            //strTextOfCommand  common value...
            string strRun = strTextOfCommand.Replace("TABLE_NAME", strTableName);

            if (strTextOfCommand.IndexOf("ALTER") >= 0)
            {
                strRun = helper.BringAlterSql(txtConnectionString.Text, strTableName);
            }
            if (strTextOfCommand.IndexOf("SELECT") >= 0)
            {
                strRun = helper.BringSelectSql(txtConnectionString.Text, strTableName);
            }

            if (strTextOfCommand.IndexOf("INSERT") >= 0)
            {
                strRun = helper.BringInsertSql(txtConnectionString.Text, strTableName);
            }

            if (strTextOfCommand.IndexOf("UPDATE") >= 0)
            {
                strRun = helper.BringUpdateSql(txtConnectionString.Text, strTableName);
            }

            if (strTextOfCommand.IndexOf("DELETE") >= 0)
            {
                strRun = helper.BringDeleteSql(txtConnectionString.Text, strTableName);
            }

            ((TextBox)ti.Content).Text = strRun;

            listOfTables.SelectedIndex = -1;
            listOfTables.Visibility = Visibility.Collapsed;
        }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            listOfTables.Visibility = Visibility.Visible;
            strTextOfCommand = @"DROP TABLE IF EXISTS ""TABLE_NAME"";";
        }

        private void btnAlter_Click(object sender, RoutedEventArgs e)
        {
            listOfTables.Visibility = Visibility.Visible;
            strTextOfCommand = @"ALTER";

            // http://www.techonthenet.com/sqlite/tables/alter_table.php ";

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            listOfTables.Visibility = Visibility.Visible;
            strTextOfCommand = "SELECT";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            listOfTables.Visibility = Visibility.Visible;
            strTextOfCommand = "DELETE";
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            listOfTables.Visibility = Visibility.Visible;
            strTextOfCommand = "INSERT";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            listOfTables.Visibility = Visibility.Visible;
            strTextOfCommand = "UPDATE";
        }

        private void btnRunRight_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void btnRunRightBottom_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void btnRunLeftBottom_Copy_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void btnMinusCurrentSql_Click(object sender, RoutedEventArgs e)
        {
            int intCount = tabPlaceHolder.Items.Count;
            if (intCount <= 1) return;
            string str = "SQL ... " + intCount.ToString();
            MinusTextBox();
        }

        private void  MinusTextBox()
        {
            TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;
            tabPlaceHolder.Items.RemoveAt(tabPlaceHolder.SelectedIndex);
            listOfTables.Visibility = Visibility.Collapsed;
        }


        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
           if (newImport != null) newImport.Close();

           newImport = new importCSVfile();
           newImport.readySQLCreateTable += newImport_readySQLCreateTable;
           newImport.readySQLDataInsert += newImport_readySQLDataInsert;
           newImport.strConnection = txtConnectionString.Text;
           newImport.Show();
        }

        void newImport_readySQLDataInsert(object sender)
        {
            var s = (importCSVfile)sender;
            AddNewTextBox("INSERT " + s.strTableName);
            TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;
            TextBox tb = (TextBox)ti.Content;
            tb.Text = s.strSQLInsertData;
            tb.Background = Brushes.LightYellow;
                      
            if (tabPlaceHolder.Items.Count <=1) s.Close(); // Close if was one TAB..

            Window p = (Window)GetTopLevelControl(this);
            p.Topmost = true;
        }

        void newImport_readySQLCreateTable(object sender)
        {
            var s = (importCSVfile)sender;
            AddNewTextBox("CREAT " + s.strTableName);
            TabItem ti = (TabItem)tabPlaceHolder.SelectedItem;
            TextBox tb = (TextBox)ti.Content;
            tb.Text = s.strSQLCreateTable;
            tb.Background = Brushes.LightYellow;

            Window p = (Window)GetTopLevelControl(this);
            p.Topmost = true;  
        }

        private DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                parent = tmp;
            }
            return parent;
        }


        private void userMainControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (newImport != null) newImport.Close();
            if (newResult != null) newResult.Close();
            if (schemaGrid != null)schemaGrid.Close();
        }

        private void btnStyle_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucScale.ScaleX = 1.0;
            ucScale.ScaleY = 1.0;

            userMainControl.Height = origH;
            userMainControl.Width = origW;

            sliderSc.Value = 1.0;

            styleChanged(this, new routedEventArgs(1));  // Event - if top level code will subscribe this....DEFAULT

        }

        private void listOfTables_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                listOfTables.Visibility = Visibility.Collapsed;
                listOfTables.SelectedIndex = -1;
            };

        }

        private void listOfTables_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           ListBox lb = (ListBox)sender;
            if (lb.Visibility == Visibility.Visible) listOfTables.Focus();
        }

        private void listOfTables_MouseEnter(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.Visibility == Visibility.Visible) listOfTables.Focus();
        }

        private void sliderSc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderSc == null || ucScale == null) return;
            if (ucScale.ScaleX == sliderSc.Value) return;

            if (ucScale.ScaleX == 1.0)
            {
                origH = userMainControl.Height;
                origW = userMainControl.Width;
            }

            ucScale.ScaleX = sliderSc.Value;
            ucScale.ScaleY = sliderSc.Value;

            userMainControl.Width = origW * ucScale.ScaleX;
            userMainControl.Height = origH * ucScale.ScaleY;
   
        }
    }
}
