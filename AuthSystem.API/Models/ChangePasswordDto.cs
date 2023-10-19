namespace AuthSystem.API.Models
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } // Contraseña actual
        public string NewPassword { get; set; } // Nueva contraseña
    }
}
