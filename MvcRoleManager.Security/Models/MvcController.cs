using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace MvcRoleManager.Security.Models
{
    
    public class MvcController
    {

        public MvcController() { }
        public MvcController(string controllerName, string controllerType)
        {
            this.ControllerName = controllerName;
            this.ControllerType = controllerType;
            this.Actions = new List<MvcAction>();
        }
        /// <summary>
        /// Controller name
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// Controller type name
        /// </summary>
        public string DeclareTypeName { get; set; }
        /// <summary>
        /// MVC controller or Web Api controller
        /// </summary>
        public string ControllerType { get; set; }
        public string Description { get; set; }
       [JsonProperty("Actions")]
        public List<MvcAction> Actions { get; set; }

    }
}
