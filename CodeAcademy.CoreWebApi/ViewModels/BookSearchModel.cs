﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class BookSearchModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
