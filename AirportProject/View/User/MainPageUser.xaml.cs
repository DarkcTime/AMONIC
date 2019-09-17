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

namespace AirportProject.View.User
{
    /// <summary>
    /// Логика взаимодействия для MainPageUser.xaml
    /// </summary>
    public partial class MainPageUser : Page
    {
        //логика для данного окна находится в файле ViewModel/LogoutViewModel/LoginViewModel

        public MainPageUser()
        {
            InitializeComponent();
            //связывает окно с классом 
            DataContext = new ViewModel.UserViewModel.MainPageUserViewModel();
        }
    }
}
