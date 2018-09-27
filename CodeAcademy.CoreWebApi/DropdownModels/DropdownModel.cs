using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DropdownModels
{
    public class DropdownModel
    {
        public DropdownModel(IEntity entity)
        {
            //...
        }
        public int Id { get; set; }

        public string Name { get; set; }    
    }
}
