﻿using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.Security.DAL;
 using MvcRoleManager.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.BSO
{
  public  class RoleManagerBso
    {
        private UnitOfWork unitOfWork = new UnitOfWork(ApplicationDbContext.Create());

        /// <summary>
        /// Get controllers and actions from assemblies
        /// </summary>
        /// <returns></returns>
        public List<MvcController> GetControllers()
        {
             ControllersActions _controllersActions = new ControllersActions();
       
            return _controllersActions.GetControllers(true);
        }

        public List<IdentityRole> GetRoles()
        {
            var roles = unitOfWork.Repository<IdentityRole>().Get().OrderBy(r => r.Name);
            return roles.ToList();
        }

        public string GetActionRoles(MvcAction action)
        {
            //Only ControllerName and ActionName have indexes.
            //Most of cases, controller and action name should be able to identify the right record.
            var actionRoles = unitOfWork.Repository<ActionRolePermission>()
                .Get(a=>a.ControllerName == action.ControllerName && a.ActionName == action.ActionName).ToList();

            if (actionRoles.Count() == 1)
            {
                return actionRoles.FirstOrDefault().Roles;
            }
            else if (actionRoles.Count() > 1)
            {
                //if there are more than 1 records returned, use return type and parameter types to identify the record.
                string parameterTypes = "";
                if (action.ParametersTypes != null)
                {
                    parameterTypes = string.Join(",", action.ParametersTypes.ToArray());
                }

                return actionRoles.Where(a => a.ReturnType == action.ReturnType && a.ParameterTypes == parameterTypes)
                .FirstOrDefault().Roles;
            }

            return string.Empty;
        }

        public Task<int> SaveActionRoles(List<MvcAction> actions)
        {
            var a = actions;

            return null;
        }
    }
}