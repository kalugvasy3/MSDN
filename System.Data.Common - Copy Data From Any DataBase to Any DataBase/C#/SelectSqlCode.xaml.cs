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
    

    public partial class SelectSqlCode : Window
    {
        public SelectSqlCode()
        {
            InitializeComponent();
            dsResult = null;
            strConnectionStringFrom = "";
            strConnectionStringTo = "";
        }

        private String strExcludeOfword = ConfigurationManager.AppSettings["excludeOfword"].ToString();

        public string[] allTableName { get; set; }
        public string strConnectionStringFrom { get; set; }
        public string strProviderNameFrom { get; set; }
        public string strConnectionStringTo { get; set; }
        public string strProviderNameTo { get; set; } 

        public DataSet dsResult { get; set; }
        public List<StringBuilder> listSB { get; set; }


        private void btnInsetTo_Click(object sender, RoutedEventArgs e)
        {
            if (newDataSetResult == null )
            {
                MessageBox.Show("Please RUN -> [Run (Bring Data A)] ... ");
                return;
            }

            if (newDataSetResult.dsResult == null)
            {
                MessageBox.Show(" Result is NULL ... DataSet MUST has at least one Table and one Row! ");
                return;
            }

            listSB = new List<StringBuilder>();
            DataBase db = new DataBase();
            listSB = db.BuildDeleteSQL(newDataSetResult.dsResult);

            foreach (DataTable dt in newDataSetResult.dsResult.Tables)
            {
              listSB.AddRange(db.BuildInsertSQL(dt, strProviderNameTo));
            }

            if (newRunInsert != null)
            {
                newRunInsert.Close();
            }
            newRunInsert = new RunInsert();
            newRunInsert.strConnectionStringTo = strConnectionStringTo;
            newRunInsert.strProviderNameTo = strProviderNameTo;

            newRunInsert.listSB = listSB;
            newRunInsert.Show();


      }

        private RunInsert newRunInsert;

        private DataSetResult newDataSetResult; 

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


            if (newDataSetResult != null)  newDataSetResult.Close();
            if (newRunInsert != null) newRunInsert.Close();

            newDataSetResult = new DataSetResult();
            newDataSetResult.dsResult = null;
            newDataSetResult.Show();

            // Need to subscribe ...

            newDataSetResult.runSQL(strProviderNameFrom, strConnectionStringFrom, new StringBuilder(txtSQLSelect.Text));
        }


        private void chkSpell_Click(object sender, RoutedEventArgs e)
        {
            txtSQLSelect.SpellCheck.IsEnabled = txtSQLSelect.SpellCheck.IsEnabled?false:true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (newDataSetResult != null)
            {
                newDataSetResult.Close();
            }
            if (newRunInsert != null)
            {
                newRunInsert.Close();
            }

        }


        private DataBase db = new DataBase();

 
        private void chkSpell_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtSQLSelect_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
