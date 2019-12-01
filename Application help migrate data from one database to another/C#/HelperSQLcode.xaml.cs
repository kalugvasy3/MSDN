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
using System.Windows.Shapes;

using System.Configuration;

using System.Data;

namespace WpfHelperCopyData
{
    /// <summary>
    /// Interaction logic for HelperSQLcode.xaml
    /// </summary>
    /// 

   

    public partial class HelperSQLcode : Window
    {
        public HelperSQLcode()
        {
            InitializeComponent();
            dsResult = null;
            strConnectionStringFrom = "";
            strConnectionStringTo = "";
        }

        private String strExcludeOfword = ConfigurationManager.AppSettings["excludeOfword"].ToString();

        public string[] allTableName { get; set; }
        public string strConnectionStringFrom { get; set; }
        public string strConnectionStringTo { get; set; }      
        public DataSet dsResult { get; set; }
        public List<StringBuilder> listSB { get; set; }


        private void btnInsetTo_Click(object sender, RoutedEventArgs e)
        {
            if (newDataScrambled == null )
            {
                MessageBox.Show("Please RUN -> [Run (Bring Data A)] ... ");
                return;
            }

            if (newDataScrambled.dsResult == null)
            {
                MessageBox.Show(" Result is NULL ... DataSet MUST has at least one Table and one Row! ");
                return;
            }

            listSB = new List<StringBuilder>();
            OleDbDataBase db = new OleDbDataBase();
            listSB = db.BuildDB2DeleteSQL(newDataScrambled.dsResult);

            foreach (DataTable dt in newDataScrambled.dsResult.Tables)
            {
              listSB.AddRange(db.BuildDB2InsertSQL(dt));
            }

            if (newRunInsert != null)
            {
                newRunInsert.Close();
            }
            newRunInsert = new RunInsert();
            newRunInsert.strConnectionStringTo = strConnectionStringTo;
            newRunInsert.listSB = listSB;
            newRunInsert.Show();


      }

        private RunInsert newRunInsert;

        private HelperDataScrambled newDataScrambled; 

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            //'txtSQLSelect

            foreach (string str in strExcludeOfword.Split(';'))
                    {
                        if (txtSQLSelect.Text.ToUpper().IndexOf(str) >= 0)
                        {
                            MessageBox.Show("SQL code should not include DROP,DELETE,..., JOIN, ... - see app.config!");
                            txtSQLSelect.Text = txtSQLSelect.Text.Replace(str, "*********");
                            return;
                        }
                    }


            if (newDataScrambled != null)
            {
                newDataScrambled.Close();
            }

            if (newRunInsert != null) newRunInsert.Close();

            newDataScrambled = new HelperDataScrambled();
            newDataScrambled.dsResult = null;
            newDataScrambled.Show();

            // Need to subscribe ...

            newDataScrambled.runSQL(strConnectionStringFrom, new StringBuilder(txtSQLSelect.Text));
        }


        private void chkSpell_Click(object sender, RoutedEventArgs e)
        {
            txtSQLSelect.SpellCheck.IsEnabled = txtSQLSelect.SpellCheck.IsEnabled?false:true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (newDataScrambled != null)
            {
                newDataScrambled.Close();
            }
            if (newRunInsert != null)
            {
                newRunInsert.Close();
            }

        }


        private OleDbDataBase db = new OleDbDataBase();

        private void btnHelpSQL_Click(object sender, RoutedEventArgs e)
        {
            string[] strSQL = txtSQLSelect.Text.Split(';');

            string strCommented = "";

            foreach (string str in txtSQLSelect.Text.Split('\n'))
            {
                strCommented += "-- " + str.Replace('\r', ' ') + Environment.NewLine;
            }


            foreach (string str in strSQL)
            {
                if (str.Trim() == "") continue;

                DataTable dt;
                string tablename = db.findTableNameHelpSQL(str, strConnectionStringFrom, out dt);  //  dt.TableName = tablename;  Name Was SET UP inside db.findTableNameHelpSQL

                if (tablename == "") continue;
                 
                if (tablename == null)
                {
                    MessageBox.Show("Somthing wrong with SQL code, maybe you lost ';' ...");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb = db.BuildSelectSQL(dt);

                strCommented =  strCommented
                                + Environment.NewLine 
                                + Environment.NewLine + "-------------------------------------------------------"
                                + Environment.NewLine 
                                + Environment.NewLine + sb.ToString();

                txtSQLSelect.Text = strCommented;                   
            }
         }

        private void chkSpell_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtSQLSelect_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
