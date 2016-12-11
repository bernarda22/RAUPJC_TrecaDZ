using System.ComponentModel.DataAnnotations;

namespace TodoWebApp.Models.TodoViewModels
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
    }
}
