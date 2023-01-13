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

namespace AuthorizationExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!BlockDragWindow)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
        }

        int RegWinCoordinates_X = -385;
        bool RegWinIsAppear = false;
        async Task RegistrationWinShowAndClose(bool isShown)
        {
            if (!isShown)
            {
                LoginAuthTB.IsEnabled = false;
                PasswordAuthTB.IsEnabled = false;

                SignInBT.IsEnabled = false;

                while (RegWinCoordinates_X < -16)
                {

                    if (RegWinCoordinates_X < -125)
                    {
                        RegWinCoordinates_X += 30;
                    }
                    else
                    {
                        RegWinCoordinates_X += 10;
                    }

                    if (RegWinCoordinates_X > -15)
                        RegWinCoordinates_X = -15;


                    RegistrationGrid.Margin = new Thickness(RegWinCoordinates_X, 10, 0, 10);
                    await Task.Delay(1);
                }
                

                RegistrationGrid.IsEnabled = true;
                
            }
            else
            {
                RegistrationGrid.IsEnabled = false;
                while (RegWinCoordinates_X >= -385)
                {
                    RegWinCoordinates_X -= 30;

                    RegistrationGrid.Margin = new Thickness(RegWinCoordinates_X, 10, 0, 10);

                    await Task.Delay(1);
                }
                LoginAuthTB.IsEnabled = true;
                PasswordAuthTB.IsEnabled = true;

                EnableAuthButton(LoginAuthTB, PasswordAuthTB, null, SignInBT);
            }
        }

        bool blockButton = false;
        private async void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!blockButton)
            {
                blockButton = true;

                if (!RegWinIsAppear)
                {
                    await RegistrationWinShowAndClose(false);
                    RegWinIsAppear = true;
                }
                else
                {
                    await RegistrationWinShowAndClose(true);
                    RegWinIsAppear = false;
                }
                blockButton = false;
            }
        }




        private async void CloseB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseBIsUsing = true;

            await DisableDrag(e);

            CloseBIsUsing = false;

        }


        private void CloseB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!HideBIsUsing)
            {
                Close();
            }
        }

        private async void Borders_MouseEnter(object sender, MouseEventArgs e)
        {
            await ChangeColorButton();
        }

        //------------------Кнопка скрытия приложения----------------------

        bool BlockDragWindow = false;

        bool CloseBIsUsing = false;
        bool HideBIsUsing = false;
        private async void HideB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideBIsUsing = true;

            await DisableDrag(e);

            HideBIsUsing = false;
        }

        private void HideB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!CloseBIsUsing)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private async void HideB_MouseEnter(object sender, MouseEventArgs e)
        {
            await ChangeColorButton();
        }
        //-------Методы для обработки кнопок -------------------------------

        async Task ChangeColorButton()
        {
            if (!HideBIsUsing)
            {
                while (CloseB.IsMouseOver)
                {

                    if (BlockDragWindow)
                    {
                        CloseB.Background = new SolidColorBrush(Color.FromRgb(255, 100, 100));
                    }
                    else
                    {
                        CloseB.Background = new SolidColorBrush(Color.FromRgb(205, 50, 50));
                    }

                    CloseLB.Foreground = new SolidColorBrush(Colors.White);

                    await Task.Delay(1);
                }

                CloseB.Background = null;
                CloseLB.Foreground = new SolidColorBrush(Colors.Black);
            }

            if (!CloseBIsUsing)
            {
                while (HideB.IsMouseOver)
                {
                    if (BlockDragWindow)
                    {
                        HideB.Background = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                    }
                    else
                    {
                        HideB.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                    }
                    await Task.Delay(1);
                }
                HideB.Background = null;
            }


        }



        async Task DisableDrag(MouseButtonEventArgs e)
        {
            BlockDragWindow = true;

            while (e.LeftButton == MouseButtonState.Pressed)
            {
                await Task.Delay(1);
            }

            BlockDragWindow = false;

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!RegWinIsAppear)
                {
                    
                    if (LoginAuthTB.IsFocused)
                    {
                        PasswordAuthTB.Focus();
                    }
                }
                else
                {
                    if (LoginRegTB.IsFocused)
                    {
                        PasswordRegTB.Focus();
                    }
                    else if (PasswordRegTB.IsFocused)
                    {
                        PasswordDubRegTB.Focus();
                    }
                }
            }
        }

        private void LoginAuthTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableAuthButton(LoginAuthTB, PasswordAuthTB,null, SignInBT);
        }

        private void PasswordAuthTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableAuthButton(LoginAuthTB,PasswordAuthTB,null,SignInBT);
        }


        private void EnableAuthButton(TextBox textBox,PasswordBox passwordBox, PasswordBox passwordBox2, Button button)
        {
            if (passwordBox2 == null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) ||
                   string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    button.IsEnabled = false;

                }
                else
                {
                    button.IsEnabled = true;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) ||
                    string.IsNullOrWhiteSpace(passwordBox.Password)||
                    string.IsNullOrWhiteSpace(passwordBox2.Password))
                {
                    button.IsEnabled = false;

                }
                else
                {
                    button.IsEnabled = true;
                }
            }

        }

        private void LoginRegTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableAuthButton(LoginRegTB, PasswordRegTB,PasswordDubRegTB ,SignUpBT);
        }

        private void PasswordRegTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableAuthButton(LoginRegTB, PasswordRegTB,PasswordDubRegTB ,SignUpBT);
        }

        private void PasswordDubRegTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableAuthButton(LoginRegTB, PasswordRegTB,PasswordDubRegTB ,SignUpBT);
        }

        bool blockreg = false;
        private async void SignUpBT_Click(object sender, RoutedEventArgs e)
        {
            if (!blockreg)
            {
                SignUpBT.IsEnabled = false;
                await Task.Delay(3000);

                LoginRegTB.Text = "";
                PasswordRegTB.Password = "";
                PasswordDubRegTB.Password = "";

                Label_MouseLeftButtonDown(sender, null);
            }
        }

    }
}