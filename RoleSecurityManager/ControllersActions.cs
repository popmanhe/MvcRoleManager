using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security
{
    public class ProjectController
    {
        public ProjectController(string controllerName, string controllerType)
        {
            this.ControllerName = controllerName;
            this.ControllerType = controllerType;
            this.ActionCollection = new List<string>();
        }
        public string ControllerName { get; private set; }
        public string ControllerType { get; private set; }
        public List<string> ActionCollection { get; private set; }

    }
    public class ControllersActions
    {
        //string dllPath = HttpContext.Current.Server.MapPath("~/bin/" + "MvcRoleManager.Web.dll");
        public List<ProjectController> GetControllers(string dllPath)
        {
            ControllersActions ca = new ControllersActions();

            var DLL = Assembly.LoadFrom(dllPath);

            var controllerActionList = DLL.GetTypes()
                .Where(c => c.FullName.Contains("Controller"))
                .ToList();

            List<ProjectController> controllerList = controllerActionList.Where(c => c.Name.Contains("Controller")).Select(b => new ProjectController(b.Name, b.BaseType.Name)).ToList();

            var controlleractionlist = DLL.GetTypes()
                    .SelectMany(type => type.GetMethods((BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType
                        //, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) 
                    })
                    .Where(c => c.ReturnType.FullName != null && c.ReturnType.FullName.Contains("ActionResult"))
                    .ToList();

            controlleractionlist.ForEach(x => { controllerList.Where(c => c.ControllerName == x.Controller).FirstOrDefault().ActionCollection.Add(x.Action); });
            return controllerList;
        }
    }
}
