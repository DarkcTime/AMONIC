﻿using System;
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
    /// Логика взаимодействия для PagePurchaseAmenities.xaml
    /// </summary>
    public partial class PagePurchaseAmenities : Page
    {
        public PagePurchaseAmenities()
        {
            InitializeComponent();
            DataContext = new ViewModel.AdministratorViewModel.PagePurchaseAmenitiesViewModel();
        }
    }
}
