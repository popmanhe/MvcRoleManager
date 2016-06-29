using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Configuration;
using System.ComponentModel;
using MvcRoleManager.Security.Store;
using MvcRoleManager.Security.ViewModels;

namespace MvcRoleManager.Security.BSO
{

    /// <summary>
    /// Retrieve controllers and actions from assembly
    /// </summary>
    public class ControllersActions
    {
        private string _dllPath;
        private Assembly _assembly;
        private List<MvcController> _controllers;
        private Type[] _types;
        private IRolePermissionStore _rolePermissionStore;

        public ControllersActions()
        {
            _dllPath = HttpContext.Current.Server.MapPath("\\bin\\" + ConfigurationManager.AppSettings["ControllersAssembly"] + ".dll");
            this._assembly = Assembly.LoadFrom(this._dllPath);

            //will use autofac to inject later
            this._rolePermissionStore = new FileRolePermissionStore();
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
                    Description = b.GetCustomAttribute<DescriptionAttribute>() != null ? b.GetCustomAttribute<DescriptionAttribute>().Description : "",
                    ControllerType = b.BaseType.Name
                })
                   .ToList();

            if (includeActions)
            {
                _controllers.ForEach(c => this.GetActions(c));
            }

            this._rolePermissionStore.SaveActionPermissions(_controllers);
            return this._controllers;
        }
        public List<MvcAction> GetActions(MvcController controller)
        {
            if (_types == null)
                this._types = this._assembly.GetTypes();

            var actions = _types
                    .SelectMany(type => type.GetMethods((BindingFlags.Instance | BindingFlags.Public)))
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && (m.DeclaringType.Name == controller.DeclareTypeName)
                        && !m.CustomAttributes.Any(c =>
                            c.AttributeType == typeof(System.Web.Http.AllowAnonymousAttribute)
                                                            || c.AttributeType == typeof(System.Web.Mvc.AllowAnonymousAttribute)
                                                           || c.AttributeType == typeof(System.Web.Http.NonActionAttribute)
                                                           || c.AttributeType == typeof(System.Web.Mvc.NonActionAttribute) )
                                                           );
            if (actions.Count() > 0)
            {
                controller.Actions = actions.Select(x => new MvcAction
                {   
                    ControllerName=controller.ControllerName,
                    ActionName = x.Name,
                    Description = x.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    ReturnType = x.ReturnType.ToString(),
                    ParameterTypes = x.GetParameters().Select(p => p.ParameterType.ToString())
                }).ToList();

                return controller.Actions;
            }
            return null;
        }


    }
}
