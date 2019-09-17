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
    /// Логика взаимодействия для DialogWindowBillingConfirmation.xaml
    /// </summary>
    public partial class DialogWindowBillingConfirmation : Window
    {

        // логика для данного окна находится в классе ViewModel/AdministratorViewModel/DialogWindowBillingConfirmationViewModel
        public DialogWindowBillingConfirmation()
        {
            InitializeComponent();
            //связывает окно с классом 
            DataContext = new ViewModel.AdministratorViewModel.DialogWindowBillingConfirmationViewModel();
            //передает классу метод закрытия окна
            ViewModel.AdministratorViewModel.DialogWindowBillingConfirmationViewModel.DialogResult = new Action(() => this.DialogResult = true);
        }
    }
}
