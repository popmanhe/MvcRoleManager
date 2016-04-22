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
        string ReturnType { get; set; }
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
        public string ReturnType { get; set; }
        public List<MvcAction> ActionCollection { get; set; }

    }

    public interface IMvcAction
    {
        string ActionName { get; set; }
        string Description { get; set; }
        string ReturnType { get; set; }
    }

    public class MvcAction : IMvcAction
    {
        public string ActionName { get; set; }
        public string Description { get; set; }

        public string ReturnType { get; set; }
    }

    public class ControllersActions
    {
        private string _dllPath;
        private Assembly _assembly;
        private List<MvcController> _controllers;
        private Type[] _types;

        public ControllersActions()
        {
            //Get executing assembly path, but remove "file://" prefix;
            _dllPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);

            _dllPath += "\\" + ConfigurationManager.AppSettings["ControllersAssembly"];
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

        public List<MvcController> GetControllers(bool includeActions = false)
        {
            if (_types == null)
                this._types = this._assembly.GetTypes();

            this._controllers = _types
                .Where(c => c.BaseType == typeof(Controller) || c.BaseType == typeof(ApiController))
                .Select(b => new MvcController(b.Name, b.BaseType.Name)).ToList();

            if (includeActions)
            {
                _controllers.ForEach(c => this.GetActions(c));
            }

            return this._controllers;
        }
        public List<MvcAction> GetActions(MvcController controller)
        {
            if (_types == null)
                this._types = this._assembly.GetTypes();

            var actions = _types
                .Where(type => type.CustomAttributes.Any(c => (c.AttributeType != typeof(System.Web.Http.AllowAnonymousAttribute)
                                                           || c.AttributeType != typeof(System.Web.Mvc.AllowAnonymousAttribute))
                                                           && (c.AttributeType != typeof(System.Web.Http.NonActionAttribute)
                                                           || c.AttributeType != typeof(System.Web.Mvc.NonActionAttribute))))
                    .SelectMany(type => type.GetMethods((BindingFlags.Instance | BindingFlags.Public)))
                    .Where(m => m.DeclaringType.Name == controller.ControllerName)
                    .Select(x => new MvcAction
                    {
                        ActionName = x.Name,
                        Description = "",
                        ReturnType = x.ReturnType.ToString()
                    }).ToList();

            controller.ActionCollection = actions;

            return actions;
        }


    }
}
