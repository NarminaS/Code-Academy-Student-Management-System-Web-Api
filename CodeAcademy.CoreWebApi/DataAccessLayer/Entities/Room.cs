using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Room:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Capacity { get; set; }

        public Room()
        {
            Groups = new List<Group>();
        }

        public List<Group> Groups { get; set; }
    }
}
