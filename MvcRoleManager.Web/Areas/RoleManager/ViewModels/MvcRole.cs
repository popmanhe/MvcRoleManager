using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcRoleManager.Web.Security.ViewModels
{
    public class MvcRole
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<MvcAction> Actions { get; set; }
        public List<MvcUser> Users { get; set; }
    }
}
