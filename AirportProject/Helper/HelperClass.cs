using System;
using System.Windows.Controls;

namespace AirportProject.Helper
{
    //позволяет хранить данные о странице
    public delegate void SetPageDelegete(Page page);
    
    class HelperClass
    {
        
        //хранит метод для смены страницы 
        public static SetPageDelegete setPage { get; set; }


        //осуществляет переход между страницами
        public static void SetPage(Page page)
        {
            setPage?.Invoke(page);
        }

    }
}
