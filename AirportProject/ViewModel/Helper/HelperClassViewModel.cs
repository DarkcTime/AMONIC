using System;
using System.Windows;

namespace AirportProject.ViewModel.Helper
{
    //дополнительный класс для хранения общих методов 

    class HelperClassViewModel : ViewModelProp
    {

        //создание объекта Entity
        protected Model.AirportEntities context = new Model.AirportEntities();

        //диалоговые окна 

        protected void MessageBoxInformation(string message)
        {
            MessageBox.Show(message, "succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        protected void MessageBoxInformation(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        protected void MessageBoxError(Exception exception)
        {
            MessageBox.Show(exception.Message, exception.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        protected void MessageBoxError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected MessageBoxResult MessageBoxQuestion(string message, string title)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result;
        }
    }
}
