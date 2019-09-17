using AirportProject.Model;
using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportProject.ViewModel.UserViewModel
{
    //TODO рефакторнуть логику данного окна 

    class MainPageUserViewModel : Helper.HelperClassMainWindowViewModel
    {
        #region Свойства
        //обработчик кнопки меню
        public ICommand ExitCommand { get; set; }

        //данные о пользователе
        public string FirstName { get; set; }
        public string TimeSpent { get; set; }
        public string NumberCrush { get; set; }

        //список действий пользователя 
        private List<NewLogoutsUser> listLogoutsUsers;
        public List<NewLogoutsUser> ListLogoutsUsers
        {
            get => this.listLogoutsUsers;
            set => Set<List<NewLogoutsUser>>(ref listLogoutsUsers , value);
        }

        //текущий пользователь
        public Model.Users CurrentUser { get; set; }

        #endregion 

        //выполняется при создании экземпляра класса
        public MainPageUserViewModel()
        {            
            try
            {

                this.UpLoadDateGrid();
                var currentUser = ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser;
                List<Model.ActivityLog> activities = this.context.ActivityLog.Where(i => i.UserID == currentUser.ID).ToList();
                TimeSpan? SpentTimeUser = new TimeSpan();
                foreach (var time in activities)
                {
                    SpentTimeUser += time.DateExit - time.DateLogout;
                }
                this.TimeSpent = $"Time spent on system {Convert.ToDateTime(SpentTimeUser.ToString()).ToShortTimeString()}";


                this.FirstName = $"Hi {currentUser.FirstName} Welcome to AMONIC Airlines.";
                this.NumberCrush = $"Number of crushes: 0";
                this.ExitCommand = new Command(ExitCommandClick); 
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }

        #region Обработчики кнопок

        //выход из аккаунта пользователя
        private void ExitCommandClick(object obj)
        {
            try
            {
                this.ExitUser();
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

        }
        #endregion

        //загружает данные 
        private void UpLoadDateGrid()
        {
            if(ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser != null)
            {
                this.CurrentUser = ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser;
                this.ListLogoutsUsers = context.ActivityLog.Where(i => i.UserID == CurrentUser.ID).ToList().Select(i => new NewLogoutsUser
                {
                    Date = Convert.ToDateTime(i.DateLogout).ToShortDateString(),
                    LoginTime = Convert.ToDateTime(i.DateLogout).ToShortTimeString(),
                    LogoutTime = Convert.ToDateTime(i.DateExit).ToShortTimeString(),
                    TimeSpent = Convert.ToDateTime(Convert.ToDateTime(i.DateExit).Subtract(Convert.ToDateTime(i.DateLogout)).ToString()).ToShortTimeString(),
                    ProblemExit = i.TypeCloseApp.Name
                }).ToList();
            }
            else
            {
                MessageBoxError("Ошибка при выборе пользователя" , "ошибка");
            }
            

        }        

    }

    
}
