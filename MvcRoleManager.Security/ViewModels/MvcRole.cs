using System.Collections.Generic;

namespace MvcRoleManager.Security.ViewModels
{
 public   class MvcRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<MvcAction> Actions { get; set; }
        public List<MvcUser> Users { get; set; }
    }
}
