using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AirportProject.ViewModel.AdministratorViewModel
{
    class PagePurchaseAmenitiesViewModel : HelperClassViewModel
    {
        #region Свойства
        //события кнопок
        public ICommand SearchCommand { get; set; }
        public ICommand ShowAmenitiesCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        //поиск по пассорту
        private string searchBookingReference;
        public string SearchBookingReference
        {
            get => this.searchBookingReference;
            set => Set<string>(ref this.searchBookingReference, value);
        }

        //найденный полет 
        private Model.Tickets currentTicket;
        public Model.Tickets CurrentTicket
        {
            get => this.currentTicket;
            set => this.Set<Model.Tickets>(ref this.currentTicket, value);
        }

        //выбранный в ComboBox полет
        private List<Model.NewListFlight> listFlight;
        public List<Model.NewListFlight> ListFlight
        {
            get => this.listFlight;
            set => Set<List<Model.NewListFlight>>(ref this.listFlight, value);
        }

        private Model.NewListFlight selectedSchedule;
        public Model.NewListFlight SelectedSchedule
        {
            get => this.selectedSchedule;
            set => Set<Model.NewListFlight>(ref selectedSchedule, value);
        }

        //полное имя пассажира
        private string fullName;
        public string FullName
        {
            get => this.fullName;
            set => Set<string>(ref fullName, value);
        }

        //динамическое изменение размеров окна 
        private int heightSelectedSchedule, heightListAmenities;
        public int HeightSelectedSchedule
        {
            get => this.heightSelectedSchedule;
            set => Set<int>(ref heightSelectedSchedule, value);
        }
        public int HeightListAmenities
        {
            get => this.heightListAmenities;
            set => Set<int>(ref heightListAmenities, value);
        }

        //выводит данные о услугах
        private List<Model.NewListAmenties> listAmenities;
        public List<Model.NewListAmenties> ListAmenities
        {

            get
            {

                return this.listAmenities;
            }
            set => Set<List<Model.NewListAmenties>>(ref this.listAmenities, value);
        }
        private Model.NewListAmenties selectedService;
        public Model.NewListAmenties SelectedService
        {
            get
            {
                this.ChoiseOfAmenities();
                return this.selectedService;
            }
            set => Set<Model.NewListAmenties>(ref selectedService, value);
        }

        //итог о выбранных услугах
        private decimal itemsSelected, dutiesAndTaxes, totalPayable;
        public decimal ItemsSelected
        {


            get => this.itemsSelected;
            set => Set<decimal>(ref itemsSelected, value);
        }
        public decimal DutiesAndTaxes
        {
            get => this.dutiesAndTaxes;
            set => Set<decimal>(ref this.dutiesAndTaxes, value);
        }
        public decimal TotalPayable
        {
            get => this.totalPayable;
            set => Set<decimal>(ref this.totalPayable, value);
        }

        public DispatcherTimer DispatcherTimer;
        #endregion


        //выполняется при созданиии класса 
        public PagePurchaseAmenitiesViewModel()
        {
            try
            {
                this.HeightListAmenities = 0;
                this.HeightSelectedSchedule = 0;

                this.SearchCommand = new Command(SearchCommandClick, CanSearchCommand);
                this.ShowAmenitiesCommand = new Command(ShowAmenitiesCommandClick, CanShowAmenitiesCommand);
                this.SaveCommand = new Command(SaveCommandClick);
                this.ExitCommand = new Command(ExitCommandClick);
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

           
        }

        //изменяет сумму оплаты услуг при выборе
        private void ChoiseOfAmenities()
        {

            this.ItemsSelected = 0;
            this.DutiesAndTaxes = 0;
            this.TotalPayable = 0;

            if (this.listAmenities == null)
            {
                return;

            }
            else
            {
                var ListSelectedAmenties = this.ListAmenities.Where(i => i.SelectedAmenities == true).ToList();

                foreach (var service in ListSelectedAmenties)
                {
                    this.ItemsSelected += service.Service.Amenities.Price;
                }

                this.TotalPayable = 0;

                this.DutiesAndTaxes = ItemsSelected * 5 / 100 + ItemsSelected;
            }


        }

        //выводит список услуг доступных для данного полета
        private void ShowAmenities()
        {
            try
            {
                this.ListAmenities = context.AmenitiesCabinType.ToList().Where(i => i.CabinTypeID == this.CurrentTicket.CabinTypeID).Select(x => new Model.NewListAmenties()
                {
                    Service = x,
                    SelectedAmenities = (bool)((x.Amenities.AmenitiesTickets.Where(i => i.TicketID == CurrentTicket.ID).Count() != 0) ? true : false)
                }).ToList();

                var wifi = this.ListAmenities.Where(i => i.Service.Amenities.ID == 7).FirstOrDefault();
                var drink = this.ListAmenities.Where(i => i.Service.Amenities.ID == 11).FirstOrDefault();

                if (wifi != null)
                    wifi.SelectedAmenities = true;
                if (drink != null)
                    drink.SelectedAmenities = true;

                DispatcherTimer = new DispatcherTimer();
                DispatcherTimer.Tick += DispatcherTimerClick;
                DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                DispatcherTimer.Start();

            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);                
            }
        }

        #region Обработчики кнопок

        //сохраняет список выбранных команд 
        private void SaveCommandClick(object obj)
        {
            var listSelectedAmenties = this.ListAmenities.Where(i => i.SelectedAmenities == true).ToList();

            var listAmenitiesTicket = this.context.AmenitiesTickets.ToList(); 

            foreach (var service in listSelectedAmenties)
            {
                var serviceTicket = new Model.AmenitiesTickets() { TicketID = CurrentTicket.ID, AmenityID = service.Service.Amenities.ID };

                foreach (var ticket in listAmenitiesTicket)
                {
                    if (Equals(serviceTicket, ticket)) continue; 
                }

                this.context.AmenitiesTickets.Add(serviceTicket);
            }

            this.context.SaveChanges();

            this.MessageBoxInformation("Перечень услуг для полета зафиксирован");

        }


        //если полет не выбран кнопка недоступна 
        private bool CanShowAmenitiesCommand(object arg)
        {
            if (this.selectedSchedule == null) return false;
            return true;
        }

        //показывает список полетов 
        private void ShowAmenitiesCommandClick(object obj)
        {
            try
            {
                this.ShowAmenities(); 
                this.HeightListAmenities = 300;
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
          
        }

        private void DispatcherTimerClick(object sender, EventArgs e)
        {
            this.ChoiseOfAmenities();
        }

        //если поле не заполнено кнопка заблокирована 
        private bool CanSearchCommand(object arg)
        {
            if (string.IsNullOrWhiteSpace(SearchBookingReference) && SearchBookingReference?.Length != 6) return false;
            return true;
        }

        //выполняет поиск билета и показывает выбранный полет 
        private void SearchCommandClick(object obj)
        {

            try
            {
                this.CurrentTicket = this.context.Tickets.Where(i => i.BookingReference == this.SearchBookingReference).FirstOrDefault();

                this.ListFlight = context.Schedules.ToList().Select(i => new Model.NewListFlight()
                {
                    Schedule = i,
                    Name = $"{i.FlightNumber}, {i.Routes.Airports.Name}-{i.Routes.Airports1.Name}, {i.Date.ToShortDateString()}, {i.Date.ToShortTimeString()}"
                }).ToList();

                this.SelectedSchedule = this.ListFlight.Where(i => i.Schedule.ID == this.CurrentTicket.ScheduleID).FirstOrDefault();

                this.FullName = $"{currentTicket.Firstname} {currentTicket.Lastname}";

                if(this.HeightSelectedSchedule == 250)
                {
                    this.ShowAmenities();
                    return;
                }

                this.HeightSelectedSchedule = 250;
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }


        }

        //переход на главное окно администратора 
        private void ExitCommandClick(object obj)
        {
            try
            {
                AirportProject.Helper.HelperClass.SetPage(new View.Administrator.MainPageAdministrator());
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

        }
        #endregion 
    }
}
