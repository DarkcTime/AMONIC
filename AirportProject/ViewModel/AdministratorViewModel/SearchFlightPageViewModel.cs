using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportProject.ViewModel.AdministratorViewModel
{
    class SearchFlightPageViewModel : HelperClassViewModel
    {
        #region Свойства 
        //привязка нажатий кнопок 
        public ICommand ApplyCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand BookFlightCommand { get; set; }
       
        //динамическое редактирование размеров окна
        private int heightSearched , heighteReturnFlight;
        public int HeightSearched
        {
            get => this.heightSearched;
            set => Set<int>(ref heightSearched, value);
        }
        public int HeightReturnFlight
        {
            get => this.heighteReturnFlight;
            set => Set<int>(ref heighteReturnFlight, value); 
        }
      
        //привязка коллекции аэропортов и типов кабин
        public List<Model.Airports> ListAirports { get; set; }
        public List<Model.NewTypeCabin> ListCabinTypes { get; set; }
       
        //привязка событий выбора аэропорта вылета, прилета, типа кабины
        private Model.Airports selectedAirportFrom , selectedAirportTo;
        public Model.Airports SelectedAirportFrom
        {
            get => this.selectedAirportFrom;
            set => Set<Model.Airports>(ref selectedAirportFrom, value);
        }
        public Model.Airports SelectedAirportTo
        {
            get => this.selectedAirportTo;
            set => Set<Model.Airports>(ref this.selectedAirportFrom, value); 
        }       
        private Model.NewTypeCabin selectedCabinType;
        public Model.NewTypeCabin SelectedCabinType
        {
            get => this.selectedCabinType;
            set => Set<Model.NewTypeCabin>(ref this.selectedCabinType, value);
        }
        
        //привязка выбора выводимых аэропортов
        private bool isCheckReturn, isCheckOneWay;
        public bool IsCheckReturn
        {
            get => this.isCheckReturn;
            set => Set<bool>(ref this.isCheckReturn, value);
        }
        public bool IsCheckOneWay
        {
            get => this.isCheckOneWay;
            set => Set<bool>(ref this.isCheckOneWay, value);
        }
        
        //привязка выбра даты 
        private DateTime selectedDateOutbond, selectedDateReturn;
        public DateTime SelectedDateOutbond
        {
            get => this.selectedDateOutbond;
            set => Set<DateTime>(ref this.selectedDateOutbond, value);
        }
        public DateTime SelectedDateReturn
        {
            get => this.selectedDateReturn;
            set => Set<DateTime>(ref selectedDateReturn, value);
        }
        
        //привязка данных к спискам аэропортов
        private List<Model.NewListShedulesSearch> listOutboundFlight , listReturnFlight;
        public List<Model.NewListShedulesSearch> ListOutboundFlight
        {
            get => this.listOutboundFlight;
            set => Set<List<Model.NewListShedulesSearch>>(ref listOutboundFlight, value); 
        }
        public List<Model.NewListShedulesSearch> ListReturnFlight
        {
            get => this.listReturnFlight;
            set => Set<List<Model.NewListShedulesSearch>>(ref listReturnFlight, value);
        }
       
        //тип кабины 
        public int CabinType { get; set; }
      
        //выбор полета 
        private Model.NewListShedulesSearch selectedOutbondFlight, selectedReturnFlight;
        public Model.NewListShedulesSearch SelectedOutbondFlight
        {
            get => this.selectedOutbondFlight;
            set => Set<Model.NewListShedulesSearch>(ref selectedOutbondFlight, value);

        }
        public Model.NewListShedulesSearch SelectedReturnFlight
        {
            get => this.selectedReturnFlight;
            set => Set<Model.NewListShedulesSearch>(ref selectedReturnFlight, value);
        }

        //количество пассажиров 
        private int numberPassegers;
        public int NumberPassegers
        {
            get => this.numberPassegers;
            set => Set<int>(ref numberPassegers, value);
        }

        //определяет нужна ли фильтрация по временному промежутку 
        private bool isCheckDisplayReturn, isCheckOutbondReturn;
        public bool IsCheckDisplayReturn
        {

            get => this.isCheckDisplayReturn;
            set => this.Set<bool>(ref this.isCheckDisplayReturn, value);
        }
        public bool IsCheckOutbondReturn
        {
            get => this.isCheckOutbondReturn;
            set => this.Set<bool>(ref isCheckOutbondReturn, value);
        }
        #endregion

        // Выполняет код при создании экземпляра класса 
        public SearchFlightPageViewModel()
        {
            try
            {
                this.BookFlightCommand = new Command(BookFlightCommandClick , CanBookFlightCommand);
                this.ApplyCommand = new Command(ApplyCommandClick);
                this.ExitCommand = new Command(ExitCommandClick);

                this.IsCheckReturn = true;
                this.HeightSearched = 0;
                this.HeightReturnFlight = 0;
                this.SelectedDateOutbond = Convert.ToDateTime("10.06.2017");
                this.SelectedDateReturn = Convert.ToDateTime("10.06.2017");
                this.ListAirports = this.context.Airports.ToList();
                this.ListCabinTypes = new List<Model.NewTypeCabin>();
                ListCabinTypes.Add(new Model.NewTypeCabin() { Id = 1, Name = "Economy" });
                ListCabinTypes.Add(new Model.NewTypeCabin() { Id = 2, Name = "Business" });
                ListCabinTypes.Add(new Model.NewTypeCabin() { Id = 3, Name = "First class" });
                this.SelectedCabinType = this.ListCabinTypes.Where(i => i.Id == 1).FirstOrDefault();
                this.IsCheckDisplayReturn = true;
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }         

        }




        #region Поиск полетов
        //выводит цену полета в зависимости от типа кабины 
        private decimal MadePrice(decimal economyPrice , int typeCabin)
        {

            switch (typeCabin)
            {
                case 1:
                    return Math.Round(economyPrice); 
                case 2:
                    return Math.Round(economyPrice * 35 / 100 + economyPrice); 
                case 3:
                    return Math.Round((economyPrice * 35 / 100 + economyPrice) * 30 / 100 + economyPrice);                
            }
            return 0; 
        }
       
        // формирует список полетов 
        private List<Model.NewListShedulesSearch> FinnalyListSchedules(List<Model.Schedules> listSchedules , List<Model.NewListShedulesSearch> newListSchedules , int typeCabin)
        {

            newListSchedules = listSchedules.Select(i => new Model.NewListShedulesSearch()
            {
                Schedule = i,
                Date = i.Date.ToShortDateString(),
                Time = i.Date.ToShortTimeString(),
                NumberOfStops = 0,
                CabinType = MadePrice(i.EconomyPrice , typeCabin)
            }).ToList();
            return newListSchedules;
     
        }
       
        //выполняет поиск полетов 
        private List<Model.NewListShedulesSearch> SearchSchedule(List<Model.Schedules> listSchedules, List<Model.NewListShedulesSearch> newListSchedules,  DateTime selectedDate , bool isCheckDisplay)
        {

            this.CabinType = this.selectedCabinType.Id;         
            
            if(this.selectedAirportFrom != null)
            {
                listSchedules  = listSchedules.Where(i => i.RouteID == selectedAirportFrom.ID).ToList();
            }

            if(this.selectedAirportTo != null)
            {
                listSchedules = listSchedules.Where(i => i.RouteID == selectedAirportTo.ID).ToList();
            }


            if(this.selectedDateOutbond != null)
            {
                if (isCheckDisplay)
                {                    
                    listSchedules = listSchedules.Where(i => i.Date < selectedDate.Date.AddDays(3) && i.Date > selectedDate.Date.AddDays(-3)).ToList();
                }
                else
                {
                    listSchedules = listSchedules.Where(i => i.Date == selectedDate.Date).ToList();
                }                
            }

            return FinnalyListSchedules(listSchedules , newListSchedules , CabinType);
        }
        #endregion 
       


        #region Обработчики событий 
        

        //осуществляет поиск полетов 
        private void ApplyCommandClick(object obj)
        {
            try
            {

                List<Model.Schedules> listOutboundFlight = this.context.Schedules.ToList();
                List<Model.Schedules> listReturnFlight = this.context.Schedules.ToList();

                this.ListOutboundFlight = SearchSchedule(listOutboundFlight, ListOutboundFlight, this.selectedDateOutbond , this.IsCheckOutbondReturn);

            
                if (this.isCheckReturn)
                {

                    this.HeightSearched = 500;
                    this.HeightReturnFlight = 200;
                    this.ListReturnFlight = SearchSchedule(listReturnFlight, ListReturnFlight, this.SelectedDateReturn , this.IsCheckDisplayReturn);
                }
                else
                {
                    this.HeightReturnFlight = 0;
                    this.HeightSearched = 300;
                }
            }
            catch (Exception ex)
            {
                MessageBoxError(ex);
            }
        }
        
        // если полет не выбран нельзя открыть форму потверждения 
        private bool CanBookFlightCommand(object arg)
        {
            if (this.selectedOutbondFlight == null && this.selectedReturnFlight == null) return false;
            return true;
        }
       
        //открывает форму потверждения бронирования 
        private void BookFlightCommandClick(object obj)
        {
            
            ViewModel.AdministratorViewModel.PageBookingConfirmationViewModel.CabinType = this.SelectedCabinType;
            ViewModel.AdministratorViewModel.PageBookingConfirmationViewModel.CurrentOutboundFlight = this.selectedOutbondFlight;
            ViewModel.AdministratorViewModel.PageBookingConfirmationViewModel.CurrentReturnFlight = this.selectedReturnFlight;

            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.PageBookingConfirmation());
        }
        
        //возращает на главное окно администратора 
        private void ExitCommandClick(object obj)
        {
            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.MainPageAdministrator());
        }

        #endregion
    }
}
