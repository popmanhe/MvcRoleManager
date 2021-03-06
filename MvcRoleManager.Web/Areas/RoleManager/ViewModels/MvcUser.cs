﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcRoleManager.Web.Security.ViewModels
{
    public class MvcUser
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public List<MvcRole> Roles { get; set; }
        public bool Selected { get; set; }
    }
}