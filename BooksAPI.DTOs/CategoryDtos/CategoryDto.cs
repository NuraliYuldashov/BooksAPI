using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTOs.CategoryDtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Book> Books = new();
}