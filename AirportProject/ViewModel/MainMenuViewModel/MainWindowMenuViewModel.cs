using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirportProject.ViewModel.MainMenuViewModel
{
    class MainWindowMenuViewModel : ViewModel.Helper.HelperClassViewModel
    {

        #region Свойства  
        //текущая страница 
        private Page currentPage;
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                this.currentPage = value;
                OnPropertyChanged();
            }
        }

        //заголовок окна 
        private string title;
        public string Title
        {
            get => this.title;
            set => Set<string>(ref title, value);
        }
       
        #endregion
        
       //выполняется при запуске окна 
        public MainWindowMenuViewModel()
        {
            AirportProject.Helper.HelperClass.setPage = this.SetPages;
            int numberUserPage = ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser.RoleID;

            switch (numberUserPage)
            {
                case 1:
                    AirportProject.Helper.HelperClass.SetPage(new View.Administrator.MainPageAdministrator());
                    break;
                case 2:
                    AirportProject.Helper.HelperClass.SetPage(new View.User.MainPageUser());
                    break;
            }
        }

        //осуществляет переход между страницами 
        private void SetPages(Page page)
        {
            this.Title = page.Title;
            this.CurrentPage = page;
        }

    }
}
