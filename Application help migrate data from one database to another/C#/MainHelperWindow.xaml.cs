using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace WpfHelperCopyData
{
    /// <summary>
    /// Interaction logic for MainHelperWindow.xaml
    /// </summary>
    public partial class MainHelperWindow : Window
    {

        private String strExcludeOf = System.Configuration.ConfigurationManager.AppSettings["excludeOf"].ToString().ToUpper();

        private Boolean blnTestedA = false;
        private Boolean blnTestedB = false;

        private List<HelperSQLcode> listSQLcode = new List<HelperSQLcode>();

        public MainHelperWindow()
        {
            InitializeComponent();

        }


        private void btnTestA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OleDbDataBase db = new OleDbDataBase();
                if (db.blnTestConnection(txtConnA.Text) == false)
                {
                    MessageBox.Show("Wrong Connection String [A] ...");
                    return;
                }
                blnTestedA = true;
                btnTestA.Background = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void btnTestB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OleDbDataBase db = new OleDbDataBase();
                if (db.blnTestConnection(txtConnB.Text) == false)
                {
                    MessageBox.Show("Wrong Connection String [B] ...");
                    return;
                }
                blnTestedB = true;
                btnTestB.Background = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void btnBringFromA_Click(object sender, RoutedEventArgs e)
        {
            if (blnTestedA & blnTestedB)
            {
                var newSQLCode = new HelperSQLcode();
                newSQLCode.Show();

                newSQLCode.strConnectionStringFrom = txtConnA.Text;
                newSQLCode.strConnectionStringTo = txtConnB.Text;

                listSQLcode.Add(newSQLCode);
            }
            else
            {
                MessageBox.Show("Please Test Connection String ...");
            }
        }

        private void txtConnA_TextChanged(object sender, TextChangedEventArgs e)
        {
                blnTestedA = false;
                try
                {
                    btnTestA.Background = new SolidColorBrush(Colors.Silver);
                    foreach (HelperSQLcode lst in listSQLcode)
                    {
                        lst.Close();
                    }
                }
                catch { }
        }

        private void txtConnB_TextChanged(object sender, TextChangedEventArgs e)
        {
                blnTestedB = false;
                try
                {
                    btnTestB.Background = new SolidColorBrush(Colors.Silver);

                    foreach (HelperSQLcode lst in listSQLcode)
                    {
                        lst.Close();
                    }

                    foreach (string str in strExcludeOf.Split(';'))
                    {
                        if (txtConnB.Text.ToUpper().IndexOf(str) >= 0)
                        {
                            MessageBox.Show("Server Name OR Port in exclusion list - see app.config!");
                            txtConnB.Text = txtConnB.Text.Replace(str, "*********");
                            return;
                        }
                    }
                }
                catch { }   
 

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (HelperSQLcode lst in listSQLcode )
            {
                lst.Close();
            }
        }



    }
}
