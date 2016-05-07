using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace MvcRoleManager.Security.Model
{
    public interface IMvcController
    {
        string ControllerName { get; set; }
        string DeclareTypeName { get; set; }
        string ControllerType { get; set; }
        string Description { get; set; }
        string ReturnType { get; set; }
        List<MvcAction> ActionCollection { get; set; }

    }
    public class MvcController : IMvcController
    {

        public MvcController() { }
        public MvcController(string controllerName, string controllerType)
        {
            this.ControllerName = controllerName;
            this.ControllerType = controllerType;
            this.ActionCollection = new List<MvcAction>();
        }
        public string ControllerName { get; set; }
        public string DeclareTypeName { get; set; }
        public string ControllerType { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
       [JsonProperty("Actions")]
        public List<MvcAction> ActionCollection { get; set; }

    }
}
