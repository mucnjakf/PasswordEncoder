using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private struct Password
        {
            public string PasswordHash { get; set; }
            public string SaltHash { get; set; }
        }

        public MainWindow() => InitializeComponent();

        private void BtnMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void BtnExit_Click(object sender, RoutedEventArgs e) => Close();

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbInputPassword.Clear();
            LblPasswordResult.Text = "-";
            LblPasswordHashResult.Text = "-";
            LblSaltHashResult.Text = "-";
        }

        private void BtnEncode_Click(object sender, RoutedEventArgs e)
        {
            if (TbInputPassword.Text != string.Empty)
            {
                Password password = EncodePassword();
                ShowPassword(password);
                TbInputPassword.Clear();
            }
            else
            {
                ValidationWindow validationWindow = new ValidationWindow();
                validationWindow.ShowDialog();
            }
        }

        private Password EncodePassword()
        {
            string inputPassword = TbInputPassword.Text;
            string saltHash = GenerateSaltHash();

            byte[] data = Encoding.UTF8.GetBytes(inputPassword + saltHash);
            SHA256 SHA256hash = SHA256.Create();

            data = SHA256hash.ComputeHash(data);

            string passwordHash = Convert.ToBase64String(data);

            return new Password
            {
                PasswordHash = passwordHash,
                SaltHash = saltHash
            };
        }

        private string GenerateSaltHash()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] buffer = new byte[25];
            rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        private void ShowPassword(Password password)
        {
            LblPasswordResult.Text = TbInputPassword.Text;
            LblSaltHashResult.Text = password.SaltHash;
            LblPasswordHashResult.Text = password.PasswordHash;
        }

        private void SpToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
