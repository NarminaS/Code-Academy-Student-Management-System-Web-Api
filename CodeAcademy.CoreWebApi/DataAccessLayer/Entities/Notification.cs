using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Notification : IEntity
    {
        public Notification()
        {
            Date = DateTime.Now;
        }
        public int Id { get; set; } 

        public DateTime Date { get; set; }

        public AppIdentityUser Sender { get; set; }

        public AppIdentityUser Subscriber { get; set; }

        public int ContentId { get; set; }

        public string Type { get; set; }

        public string Action { get; set; }

        public string State { get; set; }   

        public string Message { get; set; }

        public int? Point { get; set; }

        public int? TotalPoints { get; set; }   

        public bool IsVisited { get; set; }

    }
}
