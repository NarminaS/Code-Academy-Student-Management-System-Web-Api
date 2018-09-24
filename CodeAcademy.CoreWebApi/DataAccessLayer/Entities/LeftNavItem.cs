using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class LeftNavItem:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public string IconClassname { get; set; }
        public string RouterLink { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
