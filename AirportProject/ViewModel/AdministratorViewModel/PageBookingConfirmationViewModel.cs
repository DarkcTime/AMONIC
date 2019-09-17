using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AirportProject.ViewModel.AdministratorViewModel
{
    class PageBookingConfirmationViewModel : HelperClassViewModel
    {
        #region Свойства
        //регистрация нажатий на кнопки
        public ICommand AddPassengerCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        //выбранный полет 
        public static Model.NewListShedulesSearch CurrentOutboundFlight { get; set; }
        public static Model.NewListShedulesSearch CurrentReturnFlight { get; set; }
        public static Model.NewTypeCabin CabinType { get; set; }

        //новый билет
        private Model.Tickets ticket;
        public Model.Tickets Ticket
        {
            get => this.ticket;
            set => Set<Model.Tickets>(ref this.ticket, value);
        }

        //выбор страны 
        public List<Model.Countries> ListCountries { get; set; }
        private Model.Countries selectedCountry;
        public Model.Countries SelectedCountry
        {
            get => this.selectedCountry;
            set => Set<Model.Countries>(ref selectedCountry, value);
        }

        //выбор пассажира
        private List<Model.Tickets> listPassengers;
        public List<Model.Tickets> ListPassengers
        {
            get => this.listPassengers;
            set => Set<List<Model.Tickets>>(ref listPassengers, value);
        }
        private Model.Tickets selectedPassenger;
        public Model.Tickets SelectedPassenger
        {
            get => this.selectedPassenger;
            set => Set<Model.Tickets>(ref selectedPassenger, value);
        }

        //динамическое изменение размеров окна 
        private int heightReturnFlight, heightOutbondFlight;
        public int HeightReturnFlight
        {
            get => this.heightReturnFlight;
            set => this.Set<int>(ref this.heightReturnFlight, value);
        }
        public int HeightOutbondFlight
        {
            get => this.heightOutbondFlight;
            set => this.Set<int>(ref this.heightOutbondFlight, value);
        }



        #endregion


        // выполняется при создании экземпляра класса 
        public PageBookingConfirmationViewModel()
        {
            try
            {
                this.ConfirmCommand = new Command(ConfirmCommandClick, CanConfirmCommand);
                this.RemoveCommand = new Command(RemoveCommandClick);
                this.AddPassengerCommand = new Command(AddPassengerCommandClick, CanAddPassengerCommand);
                this.BackCommand = new Command(BackCommandClick);

                this.ListCountries = context.Countries.ToList();
                this.Ticket = new Model.Tickets();
                this.ListPassengers = new List<Model.Tickets>();

                if (CurrentOutboundFlight != null)
                {
                    this.HeightOutbondFlight = 100;
                }
                else
                {
                    this.HeightOutbondFlight = 0;
                }
                if (CurrentReturnFlight != null)
                {
                    this.HeightReturnFlight = 100;
                }
                else
                {
                    this.HeightReturnFlight = 0;
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);

            }

        }

        #region Обработчики кнопок
        //если пользотели не введены кнопка недоступна 
        private bool CanConfirmCommand(object arg)
        {
            if (this.ListPassengers == null) return false;
            return true;
        }

        //открывает окно оплаты 
        private void ConfirmCommandClick(object obj)
        {
            ViewModel.AdministratorViewModel.DialogWindowBillingConfirmationViewModel.ListPassenger = this.ListPassengers;
            var dialog = new View.Administrator.DialogWindowBillingConfirmation();
            if (dialog.ShowDialog().Value)
            {
                foreach (var ticket in ListPassengers)
                {
                    this.context.Tickets.Add(ticket);
                }
                this.MessageBoxInformation("Билеты забронированы", "успешно");
            }
        }

        //открывает страницу поиска полетов 
        private void BackCommandClick(object obj)
        {
            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.SearchFlightPage());
        }

        //удаляет пассажира из таблицы 
        private void RemoveCommandClick(object obj)
        {
            if (this.MessageBoxQuestion("Удалить пассажира", "удаление") == MessageBoxResult.Yes)
            {
                this.ListPassengers.Remove(this.selectedPassenger);
                this.ListPassengers = this.ListPassengers.ToList();
            }
        }
        // пока не заполнены все поля кнопка недоступна
        private bool CanAddPassengerCommand(object arg)
        {
            if (this.selectedCountry == null) return false;
            return true;
        }
        //добавление пассажира 
        private void AddPassengerCommandClick(object obj)
        {
            try
            {

                this.ListPassengers = this.context.Tickets.Where(i => i.ID == -1).ToList();

                var schedule = this.context.Schedules.Where(i => i.ID == CurrentOutboundFlight.Schedule.ID).FirstOrDefault();

                Model.Tickets ticket = new Model.Tickets()
                {
                    UserID = ViewModel.LogoutVIewModel.LoginWindowViewModel.CurrentUser.ID,
                    ScheduleID = CurrentOutboundFlight.Schedule.ID,
                    CabinTypeID = CabinType.Id,
                    Schedules = schedule, 
                    Firstname = this.ticket.Firstname,
                    Lastname = this.ticket.Lastname,
                    Phone = this.ticket.Phone,
                    PassportNumber = this.ticket.PassportNumber,
                    Countries = this.selectedCountry,
                    PassportCountryID = this.ticket.PassportCountryID,
                    Birthdate = this.ticket.Birthdate,
                    Confirmed = true,
                    BookingReference = "FSDJKLFD"
                };

             

                this.ListPassengers.Add(ticket);

                
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }


           
        }
        #endregion

    }
}
