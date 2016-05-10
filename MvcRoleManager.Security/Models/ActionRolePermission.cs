using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Models
{
    public   class ActionRolePermission
    {
      [Key]
        public int Id { get; set; }
        [Required]
        [Index("IX_RolePermission_CtrlAct", 1)]
        [MaxLength(256)]
        public string ControllerName { get; set; }
        [Required]
        [Index("IX_RolePermission_CtrlAct", 2)]
        [MaxLength(256)]
        public string ActionName { get; set; }

        public string ParameterTypes { get; set; }

        public string ReturnType { get; set; }

        public string Roles { get; set; }
    }
}
