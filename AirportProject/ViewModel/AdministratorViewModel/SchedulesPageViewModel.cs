using AirportProject.Model;
using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AirportProject.ViewModel.AdministratorViewModel
{
    class SchedulesPageViewModel : HelperClassViewModel
    {
        #region Свойства
        //события клика кнопок 
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand EditFlight { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand BackCommand { get; set; }

        //список аэропортов 
        public List<Model.Airports> ListAirports { get; set; }
        private Model.Airports selectedAirportFrom, selectedAirportTo;
        public Model.Airports SelectedAirportFrom
        {
            get => this.selectedAirportFrom;
            set => Set<Model.Airports>(ref selectedAirportFrom, value);
        }
        public Model.Airports SelectedAirportTo
        {
            get => this.selectedAirportTo;
            set => Set<Model.Airports>(ref selectedAirportTo, value);
        }

        //список типа сортировки 
        public List<TypeSort> ListSortBy { get; set; }
        private int indexTypeFilter;
        public int IndexTypeFilter
        {
            get => this.indexTypeFilter;
            set => Set<int>(ref this.indexTypeFilter, value);
        }

        //список полетов 
        private List<NewShedulesList> listSchedules;
        public List<NewShedulesList> ListSchedules
        {
            get => this.listSchedules;
            set => Set<List<NewShedulesList>>(ref this.listSchedules , value);
        }             
        private NewShedulesList selectedSchedul;
        public NewShedulesList SelectedSchedul
        {
            get => this.selectedSchedul;
            set => Set<NewShedulesList>(ref selectedSchedul , value);
        }

       //поля для фильрации
        public string flightNumber , outbond;
        public string FlightNumber
        {
            get => this.flightNumber;
            set => Set<string>(ref this.flightNumber , value);
        }
        public string Outbond
        {
            get => this.outbond;
            set => Set<string>(ref outbond , value);
        }

        private int editScheduleHeight;
        public int EditScheduleHeight
        {
            get => this.editScheduleHeight;
            set => Set<int>(ref this.editScheduleHeight, value);
        }

        //блокировка поля 
        private bool isEnableOutbond;
        private bool IsEnableOutbond
        {
            get
            {
                this.SetIsEnableOutbond();
                return this.isEnableOutbond; 
            }
            set => Set<bool>(ref this.isEnableOutbond, value);
        }
        #endregion

        //выполняется при создании экземпляра класса 
        public SchedulesPageViewModel()
        {
            try
            {
                this.Outbond = "";
                this.IndexTypeFilter = 0;
                this.IsEnableOutbond = true; 

                this.ListAirports = this.context.Airports.ToList();
                this.ListSortBy = new List<TypeSort>();
                ListSortBy.Add(new TypeSort() { Name = "Date and Time" });
                ListSortBy.Add(new TypeSort() { Name = "Economy price" });
                ListSortBy.Add(new TypeSort() { Name = "Confirmed" });
                ListSortBy.Add(new TypeSort() { Name = "Not apporoved"});
               
                this.ListSchedules = context.Schedules.ToList().Select(i => new NewShedulesList
                {
                    Schedules = i,
                    Date = i.Date.ToShortDateString(),
                    Time = i.Date.ToShortTimeString(),
                    BusinessPrice = Math.Round((i.EconomyPrice * 35) / 100) + i.EconomyPrice,
                    FirstClassPrice = Math.Round(i.EconomyPrice * 35 / 100 + i.EconomyPrice * 30 / 100) + i.EconomyPrice * 35 / 100 + i.EconomyPrice
                }).ToList();
                
                this.EditScheduleHeight = 0;
                this.CloseCommand = new Command(CloseCommandClick);
                this.CancelCommand = new Command(CancelCommandClick);
                this.EditFlight = new Command(EditFlightClick);
                this.ApplyCommand = new Command(ApplyCommandClick);
                this.UpdateCommand = new Command(UpdateCommandClick);
                this.BackCommand = new Command(BackCommandClick);
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }

        #region Обработчики кнопок 

        //возращает пользователя на главное окно
        private void BackCommandClick(object obj)
        {
            AirportProject.Helper.HelperClass.SetPage(new View.Administrator.MainPageAdministrator());
        }

        //редактирование данных о полете
        private void UpdateCommandClick(object obj)
        {
            try
            {
                this.context.SaveChanges();
                this.MessageBoxInformation("Данные о полете успешно отредактированы");
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }

        //закрывает форму редактирования 
        private void CloseCommandClick(object obj)
        {
            this.EditScheduleHeight = 0;
        }
      
        //открывает форму редактирования
        private void EditFlightClick(object obj)
        {
            this.EditScheduleHeight = 230;
        }

        //отменяет полет
        private void CancelCommandClick(object obj)
        {
            try
            {
                var schedul = this.context.Schedules.Where(i => i.ID == this.selectedSchedul.Schedules.ID).FirstOrDefault();
                if (schedul.Confirmed == true)
                {

                    schedul.Confirmed = false;
                    MessageBoxInformation("Полет отменён");
                }
                else
                {
                    schedul.Confirmed = true;
                    MessageBoxInformation("Полет подтвержден");
                }

                this.context.SaveChanges();

                this.FilterSchedules();
            }
            catch (Exception ex)
            {
                MessageBoxError(ex);
            }

        }

        //редактирование полета 
        private void ApplyCommandClick(object obj)
        {
            try
            {
                this.FilterSchedules();
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }

        #endregion
        //блокирует текстовое поле если выбранна фильтрация по потвержденным рейсам 
        private void SetIsEnableOutbond()
        {
            try
            {
                if (this.IndexTypeFilter > 1)
                {
                    this.IsEnableOutbond = false;
                }
                else
                {
                    this.IsEnableOutbond = true;
                }
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
       
        }
        //фильтрация данных 
        private void FilterSchedules()
        {
            try
            {
                var listSchedules = context.Schedules.ToList();


                if (this.selectedAirportFrom != null)
                {
                    listSchedules = listSchedules.Where(i => i.Routes.DepartureAirportID == selectedAirportFrom.ID).ToList();
                }

                if (this.selectedAirportTo != null)
                {
                    listSchedules = listSchedules.Where(i => i.Routes.ArrivalAirportID == selectedAirportTo.ID).ToList();
                }

                if (this.selectedAirportFrom != null &&
                    this.selectedAirportTo != null &&
                    this.selectedAirportFrom?.ID == this.selectedAirportTo?.ID)
                {
                    MessageBoxError("Аэропорт отправления и аэропорт прибытия не могут быть одинаковыми", "некоректный фильтр");
                    return;
                }

                switch (indexTypeFilter)
                {
                    case 0:
                        if (string.IsNullOrWhiteSpace(this.Outbond))
                        {
                            break;
                        }
                        if (DateTime.TryParse(this.Outbond, out DateTime date))
                        {
                            listSchedules = listSchedules.Where(i => i.Date == date).ToList();
                        }
                        else
                        {
                            MessageBoxError("Дата введена некорекно.Пример: 12.07.2018", "некоретный ввод");
                            break;
                        }
                        break;
                    case 1:
                        if (string.IsNullOrWhiteSpace(this.Outbond)) break;
                        if (decimal.TryParse(this.Outbond, out decimal price))
                        {
                            listSchedules = listSchedules.Where(i => i.EconomyPrice == price).ToList();
                        }
                        else
                        {
                            MessageBoxError("Стоимость введена некоррекно.Пример: 750", "некоретный ввод");
                            break;
                        }
                        break;
                    case 2:
                        listSchedules = listSchedules.Where(i => i.Confirmed == true).ToList();
                        break;
                    case 3:
                        listSchedules = listSchedules.Where(i => i.Confirmed == false).ToList();
                        break;
                }

                if (!string.IsNullOrWhiteSpace(this.flightNumber))
                {
                    listSchedules = listSchedules.Where(i => i.FlightNumber == this.flightNumber).ToList();
                }


                this.ListSchedules = listSchedules.ToList().Select(i => new NewShedulesList
                {
                    Schedules = i,
                    Date = i.Date.ToShortDateString(),
                    Time = i.Date.ToShortTimeString(),
                    BusinessPrice = Math.Round((i.EconomyPrice * 35) / 100) + i.EconomyPrice,
                    FirstClassPrice = Math.Round(i.EconomyPrice * 35 / 100 + i.EconomyPrice * 30 / 100) + i.EconomyPrice * 35 / 100 + i.EconomyPrice
                }).ToList();

            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
            
            
        }


    }
}
