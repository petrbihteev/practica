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
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EnglishWord();
            RussianWord();
        }

        //Строка подключения
        SqlConnection con = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_bpv;Integrated Security=True");

        //Выход из приложения
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Обновление слов
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            EnglishWord();
            RussianWord();
        }

        //Сравнение слов
        private void Proverka_Click(object sender, RoutedEventArgs e)
        {
            if (txtenglish.Text == "" || txtperevod.Text == "")
            {
                MessageBox.Show("Слова не выбраны");
            }
            else
            {
                try
                {
                    con.Open();
                    SqlCommand sql = new SqlCommand("SELECT * FROM Words Where EnglishWord = '" + txtenglish.Text + "' and RussianWord = '" + txtperevod.Text + "'", con);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = sql;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        con.Close();
                        var index = engdata.SelectedIndex;
                        var index1 = rusdata.SelectedIndex;
                        //вот тут возникает ошибка
                    /*  engdata.Items.Remove(index);
                        rusdata.Items.Remove(index1);*/
                        MessageBox.Show("Вы правильно угадали слово, поздравляем!");
                    }
                    else
                    {
                        con.Close();
                        MessageBox.Show("Вы не угадали слово!");
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show("Ошибка " + ex.Message);
                }
            }
        }

        //Вывод англ.слов
        void EnglishWord()
        {
            try
            {
                con.Open();
                string sql = "SELECT EnglishWord FROM Words ORDER BY NEWID()";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                engdata.ItemsSource = dataTable.DefaultView;
                dataAdapter.Update(dataTable);
                con.Close();
            }
            catch
            {
                con.Close();
                MessageBox.Show("Ошибка");
            }
        }

        //Вывод русских слов
        void RussianWord()
        {
            try
            {
                con.Open();
                string sql = "SELECT RussianWord FROM Words ORDER BY NEWID()";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                rusdata.ItemsSource = dataTable.DefaultView;
                dataAdapter.Update(dataTable);
                con.Close();
            }
            catch
            {
                con.Close();
                MessageBox.Show("Ошибка");
            }
        }

        // Вывод слова из DataGrid в TextBlock (eng)
        private void Engdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowView = gd.SelectedItem as DataRowView;
            if (rowView != null)
            {
                txtenglish.Text = rowView["EnglishWord"].ToString();
            }
        }

        //Вывод слова из DataGrid в TextBlock(rus)
        private void Rusdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowView = gd.SelectedItem as DataRowView;
            if (rowView != null)
            {
                txtperevod.Text = rowView["RussianWord"].ToString();
            }
        }
    }
}
