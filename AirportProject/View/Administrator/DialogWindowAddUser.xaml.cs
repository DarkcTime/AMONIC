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

namespace AirportProject.View.Administrator
{
    /// <summary>
    /// Логика взаимодействия для DialogWindowAddUser.xaml
    /// </summary>
    public partial class DialogWindowAddUser : Window
    {
        //логика данного окна находится в классе ViewModel/AdministratorViewModel/DialogAddUserViewModel
        public DialogWindowAddUser()
        {
            InitializeComponent();
            //связывает окно с класом 
            DataContext = new ViewModel.AdministratorViewModel.DialogWindowAddUserViewModel();
            //передает классу метод закрытия окна
            ViewModel.AdministratorViewModel.DialogWindowAddUserViewModel.DialogResultAddUser = new Action(() => this.DialogResult = true);
        }
    }
}
