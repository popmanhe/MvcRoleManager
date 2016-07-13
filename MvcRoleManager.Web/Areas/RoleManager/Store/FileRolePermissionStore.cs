using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcRoleManager.Web.Security.Models;
using System.Reflection;
using System.Configuration;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using MvcRoleManager.Web.Security.ViewModels;

namespace MvcRoleManager.Web.Security.Store
{
    public class FileRolePermissionStore : IRolePermissionStore
    {
        private string _filePath;
        public FileRolePermissionStore()
        {
            this._filePath = HttpContext.Current.Server.MapPath("\\RoleManager\\permissions\\" + ConfigurationManager.AppSettings["actionPermissionFileName"]);
        }

        public FileRolePermissionStore(string filePath)
        {
            this._filePath = filePath;
        }
        public List<MvcController> ReadActionPermissions()
        {
            string json;
            using (StreamReader reader = new StreamReader(this._filePath))
            {
                json = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(json))
                return JsonConvert.DeserializeObject<List<MvcController>>(json);

            return null;

        }

              public void SaveActionPermissions(List<MvcController> actions)
        {
            string json = JsonConvert.SerializeObject(actions);
            using (StreamWriter writer = new StreamWriter(this._filePath))
            {
                writer.Write(json);
            }

        }
    }
}
