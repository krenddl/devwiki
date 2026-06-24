using System;
using System.Collections.Generic;
using System.Text;

namespace DevWiki.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Viewer"; // Роли: "Admin" или "Viewer"
    }
}
