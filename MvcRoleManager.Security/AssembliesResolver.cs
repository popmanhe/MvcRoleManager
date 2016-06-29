using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace MvcRoleManager.Security
{
    /// <summary>
    /// For asp.net to find the web api controllers in external assembly.
    /// Register this resolver in WebApiConfig
    /// </summary>
    public class AssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            ICollection<Assembly> baseAssemblies = base.GetAssemblies();
            List<Assembly> assemblies = new List<Assembly>(baseAssemblies);
            var controllersAssembly = Assembly.GetExecutingAssembly();
            baseAssemblies.Add(controllersAssembly);
            return assemblies;
        }
    }
}
