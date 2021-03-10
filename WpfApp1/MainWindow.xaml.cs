using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NewWord();
            EnglishWord();
            RussianWord();
        }

        //Строка подключения
        SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=master;Integrated Security=True");
       
        //Для подсчета ошибок при неправильном выборе слов
        int a = 0;

        //Слова для колонок
        void NewWord()
        {
            try
            {
                con.Open();
                string delete = "TRUNCATE TABLE Words";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(delete, con);
                dataAdapter.SelectCommand.ExecuteNonQuery();
                con.Close();
                con.Open();
                string word = "INSERT INTO Words(EnglishWord,RussianWord) VALUES ('Pig','Свинья'),('Dog','Собака'),('Cat','Кошка'),('User','Пользователь'),('Computer','Компьютер'),('Book','Книга'),('Elephant','Слон'),('Shop','Магазин'),('Doctor','Доктор'),('Soup','Суп')";
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(word, con);
                dataAdapter1.SelectCommand.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка! " + ex.ToString());
            }
           
        }

        //Выход из приложения
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewWord();
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка! " + ex.ToString());
            }
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
                        /*   var index = engdata.SelectedIndex;
                          var index1 = rusdata.SelectedIndex;
                           //вот тут возникает ошибка
                           engdata.Items.Remove(index);
                           rusdata.Items.Remove(index1); */
                        txtenglish.Text = "";
                        txtperevod.Text = "";
                        rusdata.Height -= 20;
                        engdata.Height -= 20;
                        string idtable = dataSet.Tables[0].Rows[0]["ID_word"].ToString();
                        string drop = "DELETE FROM Words WHERE ID_Word = '" + idtable.ToString() + "'";
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(drop, con);
                        dataAdapter.SelectCommand.ExecuteNonQuery();
                        con.Close();                    
                        EnglishWord();
                        RussianWord();
                        provekrazero();
                    }
                    else
                    {
                        con.Close();
                        a++;
                        if (podskazka.IsChecked==true)
                        {
                            con.Open();
                            SqlCommand sql1 = new SqlCommand("SELECT RussianWord FROM Words Where EnglishWord = '" + txtenglish.Text + "'", con);
                            SqlDataAdapter adapter1 = new SqlDataAdapter();
                            adapter1.SelectCommand = sql1;
                            DataSet dataSet1 = new DataSet();
                            adapter1.Fill(dataSet1);
                            if (dataSet1.Tables[0].Rows.Count > 0)
                            {
                                string russlovo = dataSet1.Tables[0].Rows[0]["RussianWord"].ToString();
                                MessageBox.Show("Вы неправильно сопоставили слова! Правильно: " + txtenglish.Text + "-" + russlovo.ToString());
                            }
                            con.Close();
                        }
                        else
                        {
                            MessageBox.Show("Вы неправильно сопоставили слова!");
                        }
                        txtenglish.Text = "";
                        txtperevod.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show("Ошибка! " + ex.Message);
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
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка! " + ex.ToString());
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
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка! " + ex.ToString());
            }
        }

        //Проверка, когда слов нет
        void provekrazero()
        {
            try
            {
                con.Open();
                SqlCommand sql = new SqlCommand("SELECT * FROM Words", con);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = sql;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                if (dataSet.Tables[0].Rows.Count <= 0)
                {
                    string word = "INSERT INTO Words(EnglishWord,RussianWord) VALUES ('Pig','Свинья'),('Dog','Собака'),('Cat','Кошка'),('User','Пользователь'),('Computer','Компьютер'),('Book','Книга'),('Elephant','Слон'),('Shop','Магазин'),('Doctor','Доктор'),('Soup','Суп')";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(word, con);
                    dataAdapter.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    RussianWord();
                    EnglishWord();
                    engdata.Height = 226;
                    rusdata.Height = 226;
                    MessageBox.Show("Поздравляем! Вы угадали все слова! Ошибок: " +a);
                    a = 0;
                    MessageBoxResult result = MessageBox.Show("Вы хотите продолжить играть?", "Выбор", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            break;
                        case MessageBoxResult.No:
                            Application.Current.Shutdown();
                            break;
                    }
                }
                else
                {
                    con.Close();
                    MessageBox.Show("Вы правильно сопоставили слова! Поздравляем!");
                }
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка! " + ex.ToString());
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

        //Можем перемещать окно
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Тестирование
        public bool Sravnenie(string eng, string rus)
        {
            try
            {
                if (eng == "" || rus =="")
                {
                    MessageBox.Show("Пустое значение");
                    return false;
                }
                else
                {
                    con.Open();
                    SqlCommand sql = new SqlCommand("SELECT * FROM Words Where EnglishWord = '" + eng.ToString() + "' and RussianWord = '" + rus.ToString() + "'", con);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = sql;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        con.Close();
                        MessageBox.Show("Слова верны!");
                        return true;
                    }
                    else
                    {
                        con.Close();
                        MessageBox.Show("Слова неверны!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка! " + ex.ToString());
                return false;
            }
        }
    }
}
