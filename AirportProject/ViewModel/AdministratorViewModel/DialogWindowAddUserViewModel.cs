using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportProject.ViewModel.AdministratorViewModel
{

    //TODO переделать удаления с помощью одного объекта

    class DialogWindowAddUserViewModel : HelperClassViewModel
    {

        #region Свойства

        public static Action DialogResultAddUser { get; set; }

        private string emailAddress, firstName, lastName, password; 
        public string EmailAddress
        {
            get => this.emailAddress;
            set
            {
                this.Set<string>(ref emailAddress, value);
            }
        }
        public string FirstName
        {
            get => this.firstName;
            set
            {
                this.Set<string>(ref firstName, value);
            }
        }

        public string LastName
        {
            get => this.lastName;
            set => Set<string>(ref lastName , value);
        }

        public string Password
        {
            get => this.password;
            set => Set<string>(ref password, value);
        }

        public List<Model.Offices> ListOffice { get; set; }

        private Model.Offices selectedOffice;

        public Model.Offices SelectedOffice
        {
            get => this.selectedOffice;
            set => Set<Model.Offices>(ref selectedOffice, value);
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get => this.selectedDate;
            set => Set<DateTime>(ref selectedDate , value);
        }

        public ICommand SaveCommand { get; set; }

        #endregion

        //выполняется при создании экземпляра класса 
        public DialogWindowAddUserViewModel()
        {
            try
            {
                ListOffice = context.Offices.ToList();
                this.SelectedDate = Convert.ToDateTime("01.01.1990");
                SaveCommand = new Command(AddUserCommandClick, CanAddUserCommand);
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }

        #region Обработчики кнопок 
        //пока не заполнены все поля кнопка добавить не доступна
        private bool CanAddUserCommand(object arg)
        {
            if (string.IsNullOrWhiteSpace(this.firstName) || string.IsNullOrWhiteSpace(this.lastName)
                || string.IsNullOrWhiteSpace(this.password) || string.IsNullOrWhiteSpace(this.emailAddress) || this.selectedOffice == null ||
                this.selectedDate == null) return false;
            return true;
        }

        //добавление пользователя
        private void AddUserCommandClick(object obj)
        {

            try
            {
                DateTime minDate = Convert.ToDateTime("01.01.1900");
                DateTime maxDate = Convert.ToDateTime("01.01.2018");

                if (this.selectedDate < minDate || this.selectedDate > maxDate)
                {
                    this.MessageBoxError("Введите дата рождение в промежутке от 01.01.1900 до 01.01.2018" , "неверная дата");
                    return; 
                }

                Model.Users user = new Model.Users()
                {
                    Active = true,
                    RoleID = 2,
                    Email = this.emailAddress,
                    Password = this.password,
                    FirstName = this.firstName,
                    LastName = this.lastName,
                    OfficeID = this.selectedOffice.ID,
                    Birthdate = this.selectedDate
                };
                this.context.Users.Add(user);
                this.context.SaveChanges();
                DialogResultAddUser();

            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

        }

        #endregion


    }
}
