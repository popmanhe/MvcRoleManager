using MvcRoleManager.Security.DAL;
using MvcRoleManager.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcRoleManager.Security.ViewModels;

namespace MvcRoleManager.Security.BSO
{
  public  class ActionGroupManagerBso
    {
        private UnitOfWork unitOfWork = new UnitOfWork(RoleManagerDbContext.Create());

        public void AddGroup(ActionGroup group)
        {
            unitOfWork.Repository<ActionGroup>().Insert(group);
            unitOfWork.Save();
        }

       

        public void DeleteGroup(ActionGroup group)
        {
            unitOfWork.Repository<ActionGroup>().Delete(group);
            unitOfWork.Save();
        }

        public void UpdateGroup(ActionGroup group)
        {
            unitOfWork.Repository<ActionGroup>().Update(group);
            unitOfWork.Save();
        }
    }
}
