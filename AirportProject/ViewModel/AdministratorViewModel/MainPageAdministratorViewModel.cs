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
    //TODO сделать триггер к таблице показывающий заблокированных пользователей 
    class MainPageAdministratorViewModel : Helper.HelperClassMainWindowViewModel
    {

        #region Свойства
        //обработчики кнопок
        public ICommand AddUserCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ManagerCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ChangeRoleCommand { get; set; }
        public ICommand EnableCommand { get; set; }
        public ICommand PurchaseCommand { get; set; }

        //проверка роли
        public static bool CheckRoleAdmin { get; set; }
        public static bool CheckRoleUser { get; set; }

        //фильтрация по офисам
        public List<Model.Offices> ListOffices { get; set; }
        private Model.Offices selectedOffice;
        public Model.Offices SelectedOffice
        {
            get
            {
                this.UpdateData();
                return this.selectedOffice;
            }
            set
            {
                Set<Model.Offices>(ref this.selectedOffice, value);
            }
        }

        //список пользователей 
        private List<NewListUsers> listUsers;
        public List<NewListUsers> ListUsers
        {
            get => this.listUsers;
            set => Set<List<NewListUsers>>(ref listUsers, value);
        }

        //выбранный пользователь
        private NewListUsers selectedUser;
        public NewListUsers SelectedUser
        {
            get => this.selectedUser;
            set => Set<NewListUsers>(ref selectedUser, value);
        }
        #endregion 

        //выполняет при создании экземпляра класса
        public MainPageAdministratorViewModel()
        {
            try
            {
               

                this.ListOffices = context.Offices.ToList();
                this.ListOffices.Add(new Model.Offices() { Title = "All offices", ID = 0 });
                this.ListOffices = this.ListOffices.OrderBy(i => i.ID).ToList();
                this.SelectedOffice = this.ListOffices.Where(i => i.ID == 0).FirstOrDefault();

                this.ListUsers = context.Users.ToList().Select(i => new NewListUsers
                {
                    User = i,
                    Age = DateTime.Now.Year - i.Birthdate.Value.Year

                }).ToList();

                this.PurchaseCommand = new Command(PurchaseCommandClick);
                this.SearchCommand = new Command(SearchCommandClick);               
                this.ChangeRoleCommand = new Command(ChangeRoleCommandClick, CanChangeRoleCommand);
                this.EnableCommand = new Command(EnableCommandClick , CanEnableCommand);
                this.ManagerCommand = new Command(ManagerCommandClick);

                this.AddUserCommand = new Command(AddUserCommandClick);
                this.ExitCommand = new Command(ExitCommandClick);
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }


        #region Обработчики кнопок 
        //если пользователь не выбран или выбран администратор кнопка недоступна
        private bool CanEnableCommand(object arg)
        {
            if (this.SelectedUser?.User.RoleID == 1 || this.CanExecuteButtons() == false) return false;
            return true;
        }

        //еслм пользователь не выбран кнопка недоступна
        private bool CanChangeRoleCommand(object arg)
        {
            return this.CanExecuteButtons();
        }

        //блокирует доступ пользователю для входа
        private void EnableCommandClick(object obj)
        {
            try
            {
                var user = context.Users.Where(i => i.ID == this.selectedUser.User.ID).FirstOrDefault();
                if (user.Active == true)
                {
                    user.Active = false;
                    MessageBoxInformation("Аккаунт пользователя заблокирован");
                }
                else
                {
                    user.Active = true;
                    MessageBoxInformation("Аккаунт пользователя разблокирован");
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }       

        //открывает окно редактирования роли 
        private void ChangeRoleCommandClick(object obj)
        {
            //TODO оптимизировать блокировку аккаунта
            try
            {
                ViewModel.AdministratorViewModel.DialogWindowEditUserViewModel.SelectedUser = this.SelectedUser;
                View.Administrator.DialogWindowEditRole dialog = new View.Administrator.DialogWindowEditRole();
                if (dialog.ShowDialog() == true)
                {
                    var user = context.Users.Where(i => i.ID == SelectedUser.User.ID).FirstOrDefault();

                    if (CheckRoleUser == true && SelectedUser.User.RoleID == 2)
                    {

                    }
                    else if (CheckRoleAdmin == true && SelectedUser.User.RoleID == 1)
                    {

                    }
                    else if (CheckRoleUser == true)
                    {

                        user.RoleID = 2;
                        context.SaveChanges();
                        MessageBoxInformation("Роль  изменена на пользователя");

                    }
                    else if (CheckRoleAdmin == true)
                    {
                        user.RoleID = 1;
                        context.SaveChanges();
                        MessageBoxInformation("Роль  изменена на администратора");

                    }
                    this.UpdateData();
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
            
        }


        #endregion

        #region Меню 


        //добавление пользователя
        private void AddUserCommandClick(object obj)
        {
            try
            {
                View.Administrator.DialogWindowAddUser dialog = new View.Administrator.DialogWindowAddUser();
                if (dialog.ShowDialog() == true)
                {

                    MessageBoxInformation("Пользователь добавлен");
                    this.UpdateData();
                }
            }
            catch (Exception ex)
            {

                this.MessageBoxError(ex);
            }
        }
        
        private void PurchaseCommandClick(object obj)
        {
            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.PagePurchaseAmenities());
        }

        //открывает страницу поиска полетов 
        private void SearchCommandClick(object obj)
        {
            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.SearchFlightPage());
        }

        //открывает окно управления полетами 
        private void ManagerCommandClick(object obj)
        {
            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.SchedulesPage());
        }

        //закрывает окно
        private void ExitCommandClick(object obj)
        {
            this.ExitUser();
        }
        #endregion

        //проверяет условия для доступа к кнопке
        private bool CanExecuteButtons()
        {
            if (this.SelectedUser == null) return false;
            return true;
        }

        //фильтрация по офису 
        private void FilteringByOffice()
        {
            if (this.selectedOffice == null)
            {
                return;
            }

            if(this.selectedOffice.ID == 0)
            {

                this.ListUsers = context.Users.ToList().Select(i => new NewListUsers
                {
                    User = i,
                    Age = DateTime.Now.Year - i.Birthdate.Value.Year

                }).ToList();
                return;
            }

            this.ListUsers = context.Users.Where(i => i.OfficeID == selectedOffice.ID).Select(i => new NewListUsers
            {
                User = i,
                Age = DateTime.Now.Year - i.Birthdate.Value.Year

            }).ToList();

        }

        //обновление данных 
        private void UpdateData()
      {
            this.ListUsers = null;
            this.FilteringByOffice();
      }
        

    }
}
