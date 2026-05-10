using System;

namespace Musica;

public class RegistrationModel
{
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string DisplayName { get; set;} = "";
}
