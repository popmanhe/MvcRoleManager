namespace MvcRoleManager.Web.Security.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Action
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Action()
        {
            Roles = new HashSet<ApplicationRole>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        [Index]
        public string ControllerName { get; set; }

        [Required]
        [StringLength(256)]
        [Index]
        public string ActionName { get; set; }

        public string ParameterTypes { get; set; }

        public string Methods { get; set; }
        public string ReturnType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationRole> Roles { get; set; }
 
    }
}
