using System.Collections.Generic;

namespace MyEvernote.WebApp.ViewModals
{
    public class NotifyViewModelBase<T>
    {
        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeOut = 3000;
            Items = new List<T>();
        }

        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeOut { get; set; }
    }
}