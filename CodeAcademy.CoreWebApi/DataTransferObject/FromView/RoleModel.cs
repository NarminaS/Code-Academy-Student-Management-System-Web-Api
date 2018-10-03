using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class RoleModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
