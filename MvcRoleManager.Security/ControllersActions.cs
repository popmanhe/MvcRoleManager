using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Configuration;
using MvcRoleManager.Security.Attributes;
using MvcRoleManager.Security.Model;

namespace MvcRoleManager.Security
{
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
        /// <summary>
        /// Get controllers from specified assembly
        /// </summary>
        /// <param name="includeActions">Indicates whether action methods should be  included in the result</param>
        /// <returns></returns>
        public List<MvcController> GetControllers(bool includeActions = false)
        {
            if (_types == null)
                this._types = this._assembly.GetTypes();

            this._controllers = _types
                .Where(c => c.BaseType == typeof(Controller) || c.BaseType == typeof(ApiController))
                .Select(b => new MvcController()
                {
                    ControllerName = b.Name.Substring(0, b.Name.Length - 10),
                    DeclareTypeName = b.Name,
                    Description = b.GetCustomAttribute<DescriptionAttribute>() != null ? b.GetCustomAttribute<DescriptionAttribute>().Title : "",
                    ControllerType = b.BaseType.Name
                })
                   .ToList();

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
                    .SelectMany(type => type.GetMethods((BindingFlags.Instance | BindingFlags.Public)))
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && (m.DeclaringType.Name == controller.DeclareTypeName) && !m.CustomAttributes.Any(c =>
                                                                   c.AttributeType == typeof(System.Web.Http.AllowAnonymousAttribute)
                                                            || c.AttributeType == typeof(System.Web.Mvc.AllowAnonymousAttribute)
                                                           || c.AttributeType == typeof(System.Web.Http.NonActionAttribute)
                                                           || c.AttributeType == typeof(System.Web.Mvc.NonActionAttribute)))
                    .Select(x => new MvcAction
                    {
                        ActionName = x.Name,
                        Description = x.GetCustomAttribute<DescriptionAttribute>() != null ? x.GetCustomAttribute<DescriptionAttribute>().Title : "",
                        ReturnType = x.ReturnType.ToString()
                    }).ToList();

            controller.ActionCollection = actions;

            return actions;
        }


    }
}
