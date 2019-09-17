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
    /// Логика взаимодействия для DialogWindowEditRole.xaml
    /// </summary>
    public partial class DialogWindowEditRole : Window
    {
        //логика для данного окна находится в файле ViewModel/AdministratorViewModel/MainPageAdministratorViewModle

        public DialogWindowEditRole()
        {
            InitializeComponent();
            
            //связывает окно с классом 
            DataContext = new ViewModel.AdministratorViewModel.DialogWindowEditUserViewModel();
            
            //передает классу метод закрытия окна
            ViewModel.AdministratorViewModel.DialogWindowEditUserViewModel.DialogResutlEditUser = new Action(() => this.DialogResult = true);
        }
    }
}
