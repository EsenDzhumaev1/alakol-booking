using Microsoft.AspNetCore.Identity;
namespace Alakol.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}