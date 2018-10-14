using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class GetByIdModel
    {
        public string UserId { get; set; }

        public int? Id { get; set; }    
    }
}
