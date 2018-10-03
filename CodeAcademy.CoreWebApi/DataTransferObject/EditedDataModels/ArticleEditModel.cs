using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.EditedDataModels
{
    public class ArticleEditModel
    {
        public ArticleEditModel(Article article)
        {
            this.Id = article.Id;
            this.Tags = article.PostTags.Select(x => new TagModel { Id = x.TagId, Name = x.Tag.Name }).ToList();
            this.Text = article.Text;
            this.HeadText = article.HeadText;
        }

        public int Id { get; set; }

        public int EditedUserId { get; set; }

        public string Text { get; set; }

        public string HeadText { get; set; }

        public List<TagModel> Tags { get; set; }
    }
}
