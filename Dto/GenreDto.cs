using System.ComponentModel.DataAnnotations;

namespace techboost_aspnet.Dto;

public class GenreDto
{
    [MaxLength(100)] public required string Name { get; set; }
}