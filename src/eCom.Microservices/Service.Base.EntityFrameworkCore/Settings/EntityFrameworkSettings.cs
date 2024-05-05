using System.ComponentModel.DataAnnotations;

namespace Service.Base.EntityFrameworkCore.Settings;

public class EntityFrameworkSettings
{
    [Required]
    public required string Host { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public required string Database { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}