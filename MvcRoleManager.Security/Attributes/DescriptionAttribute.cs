using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
   public class DescriptionAttribute: System.Attribute
    {
        private readonly string _title;
        public string Title
        {
            get { return _title; }
        }

        public DescriptionAttribute(string title)
        {
            _title = title;
        }
    }
}
