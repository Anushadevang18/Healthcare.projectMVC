using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Healthcare.projectMVC.Models
{
    public class LoginViewModel
    {
       
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
        }

    public class RegisterViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }

    public class RolesRegisterViewModel
    {
        public string SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
        public RegisterViewModel RegisterRoles { get; set; }

    }



}

