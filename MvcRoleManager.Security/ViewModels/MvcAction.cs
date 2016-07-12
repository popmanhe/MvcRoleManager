using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.Security.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcRoleManager.Security.ViewModels
{
    public class MvcAction
    {
        [Required]
        public string ControllerName { get; set; }
        [Required]
        public string ActionName { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Used to identify an action when more than one action in a controller  have the same name
        /// </summary>
        public IEnumerable<string> ParameterTypes { get; set; }
        public string ReturnType { get; set; }

        public string Methods { get; set; }
        public bool Selected { get; set; }
        public List<ApplicationRole> Roles { get; set; }
    }
}
