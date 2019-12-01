using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;
using System.Data.Common;

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

        private string strProviderA = "";
        private string strProviderB = "";

        private string strError = "";

        private List<SelectSqlCode> listSQLcode = new List<SelectSqlCode>();

        public MainHelperWindow()
        {
            InitializeComponent();
            LoadProviders();
        }

        public void LoadProviders()
        {
            // Retrieve the installed providers and factories.
            DataTable table = DbProviderFactories.GetFactoryClasses();
            
            comboProvidersA.DisplayMemberPath = "Name";
            comboProvidersA.SelectedValuePath = "InvariantName";
            comboProvidersA.ItemsSource = table.DefaultView;

            comboProvidersB.DisplayMemberPath = "Name";
            comboProvidersB.SelectedValuePath = "InvariantName";
            comboProvidersB.ItemsSource = table.DefaultView;

        }

        private void comboProvidersA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            strProviderA = comboProvidersA.SelectedValue.ToString();
            btnTestA.Background = new SolidColorBrush(Colors.LightGray);
            blnTestedA = false;
            txtStatus.Text = "comboProvidersA - changed";
        }

        private void comboProvidersB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            strProviderB = comboProvidersB.SelectedValue.ToString();
            btnTestB.Background = new SolidColorBrush(Colors.LightGray);
            blnTestedB = false;
            txtStatus.Text = "comboProvidersB - changed";
        }

        private void btnTestA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBase db = new DataBase();
                if (db.TestConnection(strProviderA, txtConnA.Text, out strError) == false)
                {
                    txtStatus.Text = strError;
                    MessageBox.Show("Wrong Connection String [A] OR Select Provider ...");
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
                DataBase db = new DataBase();
                if (db.TestConnection(strProviderB, txtConnB.Text, out strError) == false)
                {
                    txtStatus.Text = strError;
                    MessageBox.Show("Wrong Connection String [B] ... OR Select Provider");
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
                var newSQLCode = new SelectSqlCode();
                newSQLCode.Show();

                //strProviderNameA 
                newSQLCode.strConnectionStringFrom = txtConnA.Text;
                newSQLCode.strProviderNameFrom = strProviderA;

                newSQLCode.strConnectionStringTo = txtConnB.Text;
                newSQLCode.strProviderNameTo = strProviderB;

                txtStatus.Text = "";
                listSQLcode.Add(newSQLCode);
            }
            else
            {
                MessageBox.Show("Please Test Connection String || Provider(s) ...");
            }
        }

        private void txtConnA_TextChanged(object sender, TextChangedEventArgs e)
        {
                blnTestedA = false;
                try
                {
                    btnTestA.Background = new SolidColorBrush(Colors.Silver);
                    foreach (SelectSqlCode lst in listSQLcode) // Close ALL windows which was opened with previous connection ... 
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

                    foreach (SelectSqlCode lst in listSQLcode)
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
            foreach (SelectSqlCode lst in listSQLcode )
            {
                lst.Close();
            }
        }



    }
}
