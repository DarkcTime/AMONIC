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

namespace AirportProject.View.Administrator
{
    /// <summary>
    /// Логика взаимодействия для PageBookingConfirmation.xaml
    /// </summary>
    public partial class PageBookingConfirmation : Page
    {
        public PageBookingConfirmation()
        {
            InitializeComponent();
            DataContext = new ViewModel.AdministratorViewModel.PageBookingConfirmationViewModel();
        }
    }
}
