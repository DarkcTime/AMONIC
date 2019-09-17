using AirportProject.Model;
using AirportProject.ViewModel.Helper;
using AirportProject.ViewModel.MainMenuViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportProject.ViewModel.AdministratorViewModel
{
    class DialogWindowEditUserViewModel : HelperClassViewModel
    {

        #region Свойства
        public static Action DialogResutlEditUser { get; set; }

        public static NewListUsers SelectedUser { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private bool userCheck, administratorCheck; 
        public bool UserCheck
        {
            get => this.userCheck;
            set => Set<bool>(ref userCheck, value);
        }

        public bool AdministratorCheck
        {
            get => this.administratorCheck;
            set => Set<bool>(ref this.administratorCheck, value); 
        }

        public List<Model.Offices> ListOffice { get; set; }

        private int indexSelectedOffice;
        public int IndexSelectedOffice
        {
            get => this.indexSelectedOffice;
            set => this.Set<int>(ref indexSelectedOffice, value);
        }
        public ICommand ApplyCommand { get; set; }

        #endregion

        public DialogWindowEditUserViewModel()
        {
            this.UpLoadData();
            try
            {
                
                this.ApplyCommand = new Command(ApplyCommandClick);
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }

        private void UpLoadData()
        {
            this.ListOffice = this.context.Offices.ToList();
            this.FirstName = SelectedUser.User.FirstName;
            this.LastName = SelectedUser.User.LastName;
            this.EmailAddress = SelectedUser.User.Email;
            this.IndexSelectedOffice = Convert.ToInt32(SelectedUser.User.OfficeID - 1);
            switch (SelectedUser.User.RoleID)
            {
                case 1:
                    this.AdministratorCheck = true;
                    break;
                case 2:
                    this.UserCheck = true;
                    break; 
            }
        }
        private void ApplyCommandClick(object obj)
        {
            try
            {
                ViewModel.AdministratorViewModel.MainPageAdministratorViewModel.CheckRoleAdmin = this.AdministratorCheck;
                ViewModel.AdministratorViewModel.MainPageAdministratorViewModel.CheckRoleUser = this.UserCheck;
                DialogResutlEditUser();

            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }   
        }
    }
}
