using System.ComponentModel.DataAnnotations;

namespace DevWiki.Client.Models
{
    public class CreateArticleDto
    {
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Заголовок должен быть от 3 до 200 символов")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Текст статьи обязателен")]
        [MinLength(10, ErrorMessage = "Текст статьи слишком короткий")]
        public string Content { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;
    }
}
