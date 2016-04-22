using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Configuration;

namespace MvcRoleManager.Security
{

    public interface IMvcController
    {
        string ControllerName { get; set; }
        string ControllerType { get; set; }
        string Description { get; set; }
        List<MvcAction> ActionCollection { get; set; }

    }
    public class MvcController : IMvcController
    {
        public MvcController(string controllerName, string controllerType)
        {
            this.ControllerName = controllerName;
            this.ControllerType = controllerType;
            this.ActionCollection = new List<MvcAction>();
        }
        public string ControllerName { get; set; }
        public string ControllerType { get; set; }
        public string Description { get; set; }
        public List<MvcAction> ActionCollection { get; set; }

    }

    public interface IMvcAction
    {
        string ActionName { get; set; }
        string Description { get; set; }
    }

    public class MvcAction : IMvcAction
    {
        public string ActionName { get; set; }
        public string Description { get; set; }
    }

    public class ControllersActions
    {
        private string _dllPath;
        private Assembly _assembly;

        public ControllersActions()
        {
            //Get executing assembly path, but remove "file://" prefix;
            _dllPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);

            _dllPath +="\\"+ ConfigurationManager.AppSettings["ControllersAssembly"];
            if (!_dllPath.EndsWith(".dll"))
                _dllPath += ".dll";
            this._assembly = Assembly.LoadFrom(this._dllPath);
        }

        public ControllersActions(string dllPath)
        {
            this._dllPath = dllPath;
            this._assembly = Assembly.LoadFrom(this._dllPath);
        }

        public ControllersActions(Assembly assembly)
        {
            this._assembly = assembly;
        }
        //string dllPath = HttpContext.Current.Server.MapPath("~/bin/" + "MvcRoleManager.Web.dll");
        public List<MvcController> GetControllers()
        {
            ControllersActions ca = new ControllersActions();

            var controllerActionList = _assembly.GetTypes()
                .Where(c => c.BaseType == typeof(Controller) || c.BaseType == typeof(ApiController))
                .Select(b => new MvcController(b.Name, b.BaseType.Name)).ToList();

            var controlleractionlist = _assembly.GetTypes()
                .Where(type => type.CustomAttributes.Any(c => c.AttributeType != typeof(System.Web.Http.AllowAnonymousAttribute)
                                                           || c.AttributeType != typeof(System.Web.Mvc.AllowAnonymousAttribute)))
                    .SelectMany(type => type.GetMethods((BindingFlags.Instance | BindingFlags.Public)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType
                        //, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) 
                    })
                    .Where(c => c.ReturnType.FullName != null && c.ReturnType.BaseType == typeof(ActionResult))
                    .ToList();

            //controlleract/*ionlist.ForEach(x => { controllerActionList.Where(c => c.ControllerName == x.Controller).FirstOrDefault().ActionCollection.Add(x.Action); });
            return controllerActionList;
        }
    }
}
