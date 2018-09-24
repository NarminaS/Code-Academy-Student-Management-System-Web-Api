using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Entities
{
    public class Gender:IEntity
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public Gender()
        {
            Users = new List<AppIdentityUser>();
        }
        public List<AppIdentityUser> Users { get; set; }
    }
}
