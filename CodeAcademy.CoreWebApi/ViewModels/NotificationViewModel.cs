using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class NotificationViewModel
    {
        public NotificationViewModel(Notification notification)
        {
            this.Id = notification.Id;
            this.Message = notification.Message;
            this.SenderId = notification.Sender.Id;
            this.SenderPhoto = notification.Sender.Photo.Url;
            this.Type = notification.Type;
            this.Date = notification.Date;
            this.IsVisited = notification.IsVisited;
        }
        public int Id { get; set; }

        public string Message { get; set; }

        public string SenderId { get; set; }

        public string SenderFullName { get; set; }  

        public string SenderPhoto { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public bool IsVisited { get; set; }
    }
}
