using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTOs.CategoryDtos;

public class AddCategoryDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;
}
