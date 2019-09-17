using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirportProject.ViewModel.Helper
{
    class HelperClassMainWindowViewModel : HelperClassViewModel
    {

        public static Action CloseMainWindow { get; set; }

        protected void ExitUser()
        {
            try
            {
                if (MessageBoxQuestion("Выйти из профиля", "выход") == MessageBoxResult.Yes)
                {

                    var login = new View.Logout.LoginWindow();
                    login.Show();
                    Model.ActivityLog activity = new Model.ActivityLog
                    {
                        TypeCloseAppID = 1,
                        UserID = ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser.ID,
                        DateLogout = ViewModel.LogoutVIewModel.LoginWindowViewModel.DateLoginUser,
                        DateExit = DateTime.Now
                    };
                    this.context.ActivityLog.Add(activity);
                    this.context.SaveChanges();
                    CloseMainWindow();
                }
            }
            catch (Exception ex)
            {
                MessageBoxError(ex);
            }

        }
    }
}
