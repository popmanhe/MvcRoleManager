using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Models
{
    public class ActionGroup
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique=true)]
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Action> Actions { get; set; }
    }
}
