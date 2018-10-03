using System.ComponentModel.DataAnnotations;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class LanguageModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }    
    }
}