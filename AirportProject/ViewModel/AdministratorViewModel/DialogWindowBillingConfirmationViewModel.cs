using AirportProject.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportProject.ViewModel.AdministratorViewModel
{
    class DialogWindowBillingConfirmationViewModel : HelperClassViewModel
    {
        #region Свойства
        //результат диалогового окна 
        public static Action DialogResult { get; set; }
        
        //список пассажиров
        public static List<Model.Tickets> ListPassenger { get; set; }

        //обработчики кнопок 
        public ICommand  IssueCommand { get; set; }

        //сумма оплаты 
        private decimal sumBuy; 
        public decimal SumBuy
        {
            get => this.sumBuy;
            set => this.Set<decimal>(ref sumBuy, value); 
        }
        #endregion 

        //выполняется при запуске окна
        public DialogWindowBillingConfirmationViewModel()
        {

          
            try
            {
                SumBuy = 0;
                if (ListPassenger != null)
                {
                    foreach (var passenger in ListPassenger)
                    {
                        SumBuy += passenger.Schedules.EconomyPrice;
                    }
                }

                this.IssueCommand = new Command(IssueCommandClick);
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }

           
        }

        //закрывает окно и сообщает о выборе пользователя 
        private void IssueCommandClick(object obj)
        {
            try
            {
                DialogResult?.Invoke();
            }
            catch (Exception ex)
            {
                this.MessageBoxError(ex);
            }
        }
    }
}
