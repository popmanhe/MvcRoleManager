using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Model
{

    public interface IMvcAction
    {
        string ActionName { get; set; }
        string Description { get; set; }
        string ReturnType { get; set; }
    }

   [Serializable]
    public class MvcAction : IMvcAction
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
        //public string Attribute { get; set; }
        public string CustomAttributes { get; set; }
        public List<string> ActionMethodType { get; set; }

        //public List<System.Reflection.CustomAttributeData> GetCustomAttributesData { get; set; }
        public List<string> RouteAttribute { get; set; }

        private string _hashCode;
        public async Task<string> GetEncryptedCodeAsync()
        {
            if (string.IsNullOrEmpty(_hashCode))
            { _hashCode = await Util.EncryptedActionAsync(this); }

            return _hashCode;
        }
    }
}
