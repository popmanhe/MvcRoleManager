using MvcRoleManager.Security.DAL;
using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MvcRoleManager.Security.BSO
{
    public class RoleManagerBso
    {
        private UnitOfWork unitOfWork = new UnitOfWork(RoleManagerDbContext.Create());

        /// <summary>
        /// Get controllers and actions from assemblies
        /// </summary>
        /// <returns></returns>
        public List<MvcController> GetControllers()
        {
            ControllersActions _controllersActions = new ControllersActions();

            return _controllersActions.GetControllers(true);
        }

        /// <summary>
        /// Get all roles from database
        /// </summary>
        /// <returns></returns>
        public List<ApplicationRole> GetRoles()
        {
            unitOfWork.Context.Configuration.LazyLoadingEnabled = false;
            var roles = unitOfWork.Repository<ApplicationRole>().Get().OrderBy(r => r.Name);
            return roles.ToList();
        }

        public void AddRole(ApplicationRole role)
        {
            unitOfWork.Repository<ApplicationRole>().Insert(role);
            unitOfWork.Save();
        }

        public void DeleteRole(ApplicationRole role)
        {
            unitOfWork.Repository<ApplicationRole>().Delete(role);
            unitOfWork.Save();
        }

        public void UpdateRole(ApplicationRole role)
        {
            unitOfWork.Repository<ApplicationRole>().Update(role);
            unitOfWork.Save();
        }


        /// <summary>
        /// Get associated roles by action
        /// </summary>
        /// <param name="mvcAction"></param>
        /// <returns></returns>
        public List<MvcRole> GetActionRoles(MvcAction mvcAction)
        {
            //Only ControllerName and ActionName have indexes.
            //Most of cases, controller and action name should be able to identify the right record.
            var action = this.GetAction(mvcAction);
            var actionRoles = action?.Roles;
            var roles = this.GetRoles();
            List<MvcRole> selectedRoles = new List<MvcRole>();
            foreach (var role in roles)
            {
                selectedRoles.Add(new MvcRole
                {
                    Id = role.Id,
                    Name = role.Name,
                    Selected = actionRoles != null && actionRoles.Any(r => r.Id == role.Id)
                });
            }

            return selectedRoles;
        }
        /// <summary>
        /// Save roles to action
        /// </summary>
        /// <param name="mvcAction"></param>
        public void SaveActionRoles(MvcAction mvcAction)
        {
            var actions = unitOfWork.Repository<Models.Action>().Get(act => act.ActionName == mvcAction.ActionName
            && act.ControllerName == mvcAction.ControllerName);
            Models.Action action = this.GetAction(mvcAction);

            if (action == null)
            {
                action = new Models.Action
                {
                    ActionName = mvcAction.ActionName,
                    ControllerName = mvcAction.ControllerName,
                    ParameterTypes = string.Join(",", mvcAction.ParametersTypes.ToArray()),
                    ReturnType = mvcAction.ReturnType
                };
                unitOfWork.Repository<Models.Action>().Insert(action);
            }

            AddRolesToAction(mvcAction.Roles, action);
            unitOfWork.Save();
        }
        /// <summary>
        /// Get action entity from database by MvcAction object
        /// </summary>
        /// <param name="mvcAction"></param>
        /// <returns></returns>
        private Models.Action GetAction(MvcAction mvcAction)
        {
            var actions = unitOfWork.Repository<Models.Action>()
              .Get(a => a.ControllerName == mvcAction.ControllerName && a.ActionName == mvcAction.ActionName).ToList();
            string parameterTypes = "";
            if (mvcAction.ParametersTypes != null)
            {
                parameterTypes = string.Join(",", mvcAction.ParametersTypes.ToArray());
            }

            if (actions.Count() == 0)
                return null;
            return actions.Where(a => a.ReturnType == mvcAction.ReturnType && a.ParameterTypes == parameterTypes)
            .FirstOrDefault();
        }
        private void AddRolesToAction(List<ApplicationRole> roles, Models.Action action)
        {
            //Remove unselected roles associated with current action
            foreach (var role in action.Roles.ToList())
            {
                if (!roles.Any(r => r.Id == role.Id))
                    action.Roles.Remove(role);
            }

            foreach (ApplicationRole role in roles)
            {
                if (!action.Roles.Any(r => r.Id == role.Id))
                {
                    action.Roles.Add(role);
                    //We only want to add roles to ActionRoles table, not adding  role to AspnetRoles table, so change the state to unchanged;
                    unitOfWork.Context.Entry<ApplicationRole>(role).State = System.Data.Entity.EntityState.Unchanged;
                }
            }
        }

    }
}
