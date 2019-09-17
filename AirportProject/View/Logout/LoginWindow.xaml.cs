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

namespace AirportProject.View.Logout
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //логика для данного окна находится в файле ViewModel/LogoutViewModel/LoginViewModel
        public LoginWindow()
        {
            InitializeComponent();
            //связывает окно с классом 
            DataContext = new ViewModel.LogoutVIewModel.LoginWindowViewModel();
            //передает классу метод закрытия окна
            ViewModel.LogoutVIewModel.LoginWindowViewModel.CloseWindow = new Action(() => this.Close());
        }

    }
}
