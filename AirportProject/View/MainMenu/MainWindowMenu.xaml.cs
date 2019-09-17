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

namespace AirportProject.View.MainMenu
{
    /// <summary>
    /// Логика взаимодействия для MainWindowMenu.xaml
    /// </summary>
    public partial class MainWindowMenu : Window
    {
        //логика для данного окна находится в классе ViewModel/MainMenuViewModel/MainWindowMenuViewModel
        public MainWindowMenu()
        {
            InitializeComponent();
            //связывает окно с классом 
            DataContext = new ViewModel.MainMenuViewModel.MainWindowMenuViewModel();
            //передает метод закрытия окна классу 
            ViewModel.Helper.HelperClassMainWindowViewModel.CloseMainWindow = new Action(() => this.Close());
        }
    }
}
