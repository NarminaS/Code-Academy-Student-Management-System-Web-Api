using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(2000)]
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsVisited { get; set; }
        public Message()
        {
            DateAdded = DateTime.Now;
        }

       //public string SenderId { get; set; }
        //public AppIdentityUser Sender { get; set; }

        //public string SubscriberId { get; set; }
        //public AppIdentityUser Subscriber { get; set; }

        //public int PhotoId { get; set; }
        //public Photo Photo { get; set; }
    }
}
