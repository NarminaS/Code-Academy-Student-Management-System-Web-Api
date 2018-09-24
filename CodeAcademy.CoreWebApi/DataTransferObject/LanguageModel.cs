using System.ComponentModel.DataAnnotations;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class LanguageModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }    
    }
}