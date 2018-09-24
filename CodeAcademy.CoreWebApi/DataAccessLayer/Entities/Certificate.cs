using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Certificate:IEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime DateAdded { get; set; }
       
        public Certificate()
        {
            DateAdded = DateTime.Now;
        }

        public string AppIdentityUserId { get; set; }
        public AppIdentityUser User { get; set; }
    }
}
