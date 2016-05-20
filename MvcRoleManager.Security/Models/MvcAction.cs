using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace MvcRoleManager.Security.Models
{
    public class MvcAction
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Used to identify an action when more than one action in a controller  have the same name
        /// </summary>
        public IEnumerable<string> ParametersTypes { get; set; }
        public string ReturnType { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}
