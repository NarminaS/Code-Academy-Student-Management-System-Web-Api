using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class LikeViewModel
    {
        public LikeViewModel(int likeCount, bool isLiked)
        {
            this.LikeCount = likeCount;
            this.IsLiked = isLiked;
        }
        public int LikeCount { get; set; }

        public bool IsLiked { get; set; }   
    }
}
