using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class PostApproveModel
    {
        public int PostId { get; set; }

        public string PostAuthorId { get; set; }

        public string Reason { get; set; }  
    }
}
