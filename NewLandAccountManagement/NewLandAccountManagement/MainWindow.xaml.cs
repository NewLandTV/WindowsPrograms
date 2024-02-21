using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace NewLandAccountManagement
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // MySQL 데이터베이스 접속 정보 (서버;포트;데이터베이스이름;유저아이디;비밀번호)
        private string connectionStr;

        public MainWindow()
        {
            InitializeComponent();

            connectionStr = File.ReadAllText("Config.txt");
        }

        /// <summary>Create NewLand account.</summary>
        private void CreateNewLandAccount()
        {
            if (nameTextBox.Text.Length > 0 && idTextBox.Text.Length > 0 && passwordTextBox.Text.Length > 0)
            {
                if (MessageBox.Show("계정을 만드시겠습니까?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // MySQL을 사용하여 NewLand 계정 생성
                    using (MySqlConnection connection = new MySqlConnection(connectionStr))
                    {
                        try
                        {
                            connection.Open();

                            MySqlCommand mySqlCmd = new MySqlCommand(string.Format("SELECT user_id FROM account WHERE user_id=\"{0}\";", idTextBox.Text), connection);

                            MySqlDataReader mySqlDataRender = mySqlCmd.ExecuteReader();

                            if (mySqlDataRender.HasRows)
                            {
                                MessageBox.Show("NewLand 계정 만들기 실패 (이미 존재하는 계정)", "Info", MessageBoxButton.OK, MessageBoxImage.Hand);

                                mySqlDataRender.Close();
                            }
                            else
                            {
                                mySqlDataRender.Close();

                                mySqlCmd.CommandText = string.Format("INSERT INTO account (user_id, user_pw, user_price, user_name, user_status) VALUES(\"{0}\", \"{1}\", 0, \"{2}\", 1);", idTextBox.Text, passwordTextBox.Text, nameTextBox.Text);

                                mySqlCmd.ExecuteNonQuery();

                                MessageBox.Show("NewLand 계정 만들기 성공!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("비어있는 정보가 있습니다.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (IsValidEmail(idTextBox.Text))
                {
                    idTextBox.Text += "@nacc.com";

                    CreateNewLandAccount();
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewLandAccount();
        }

        private bool IsValidEmail(string id)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(id, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
        }
    }
}
