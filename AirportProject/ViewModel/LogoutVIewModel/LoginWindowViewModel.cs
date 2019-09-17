using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AirportProject.ViewModel.LogoutVIewModel
{
    class LoginWindowViewModel : ViewModel.Helper.HelperClassViewModel
    {

        #region Свойства
        //служит для закрытия данного окна
        public static Action CloseWindow { get; set; }

        //получают события кнопок
        public ICommand LoginCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        //таймер
        DispatcherTimer disptcherTimer;

        //число неудачных попыток входа
        public static int NumberMistake { get; set; }

        public static DateTime DateLoginUser { get; set; }

        //счетчик секунд до разблокировки 
        public static int Counter { get; set; }
     
        //текущий пользователь
        public static Model.Users CurrentUser{get;set;}

        //поля логин и пароль
        private string userName, password;
        public string UserName
        {
            get => this.userName;
            set
            {
                Set<string>(ref userName, value);
            }
        }      
        public string Password
        {
            get => this.password;
            set
            {
                Set<string>(ref password, value);
            }
        }

        //блокировка полей после 3 неудачных попыток входа
        private string textWait;
        public string TextWait
        {
            get => this.textWait;
            set => this.Set<string>(ref textWait, value);
        }
        private bool isEnableUserName, isEnablePassword;
        public bool IsEnableUserName
        {
            get => this.isEnableUserName;
            set => Set<bool>(ref isEnableUserName, value);
        }
        public bool IsEnablePassword
        {
            get => this.isEnablePassword;
            set => Set<bool>(ref isEnablePassword, value);
        }

        #endregion

        //выполняется при запуске приложения
        public LoginWindowViewModel()
        {
            try
            {
                this.TextWait = "";
                this.IsEnableUserName = true;
                this.IsEnablePassword = true;

                this.LoginCommand = new Command(LoginCommandClick , CanLoginCommand);
                this.ExitCommand = new Command(ExitCommandClick);
            }
           catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }


        }

        #region  Обработчики кнопок

        //если поле логин или пароль пустые , делает невозможным нажатие на кнопку
        private bool CanLoginCommand(object arg)
        {
            if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password)) return false;
            return true;
        }

        //если пароль и логин верны открывает окно пользователя 
        private void LoginCommandClick(object obj)
        {
            try
            {

                var user = this.context.Users.Where(i => i.Email == this.UserName && i.Password == this.Password);
                if (user.Count() > 0)
                {

                    CurrentUser = user.FirstOrDefault();
                    DateLoginUser = DateTime.Now;

                    if (CurrentUser.Active == false)
                    {
                        MessageBoxError("Ваш аккаунт был заблокирован администратором", "ошибка входа");
                        Model.ActivityLog activity = new Model.ActivityLog
                        {
                            TypeCloseAppID = 4,
                            UserID = ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser.ID,
                            DateLogout = DateLoginUser,
                            DateExit = DateLoginUser

                        };
                        context.ActivityLog.Add(activity);
                        context.SaveChanges();
                        return;
                    }

                    var window = new View.MainMenu.MainWindowMenu();
                    window.Show();
                    CloseWindow();
                }
                else
                {
                    this.MessageBoxError("Неверный логин или пароль", "неудачно");
                    NumberMistake++;
                    this.LockFieldsIfThreeMistake();

                }
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

        }

        //выход из приложения
        private void ExitCommandClick(object obj)
        {

            try
            {
                Application.Current.Shutdown();

            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

        }
        #endregion

        #region Блокировка полей после 3 неудачных попыток входа 

        //блокирует поля и выводит время до разблокировки
        private void LockFieldsIfThreeMistake()
        {
            try
            {
                Counter = 10;
                if (NumberMistake == 3)
                {
                    NumberMistake = 0;
                    this.disptcherTimer = new DispatcherTimer();
                    disptcherTimer.Tick += TimerTick;
                    disptcherTimer.Interval = new TimeSpan(0, 0, 1);
                    disptcherTimer.Start();
                    this.IsEnableUserAndPassFalse();
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }


        }

        //поля заблокированы
        private void IsEnableUserAndPassFalse()
        {
            try
            {
                this.UserName = "";
                this.Password = "";
                this.IsEnableUserName = false;
                this.IsEnablePassword = false;
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
          
        }

        //поля разблокированы
        private void IsEnableUserAndPassTrue()
        {
            try
            {
                this.TextWait = "";
                this.IsEnableUserName = true;
                this.IsEnablePassword = true;
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
           
        }

        //считывает время блокировки полей
        private void TimerTick(object sender, EventArgs e)
        {

            if (Counter != 0)
            {
                this.TextWait = $"Fields will be available after {Counter} seconds";
                Counter--;
            }
            else
            {
                disptcherTimer.Stop();
                this.IsEnableUserAndPassTrue();
            }
        }
        #endregion

    }
}
