using MvcRoleManager.Web.Security.DAL;
using MvcRoleManager.Web.Security.Models;
using MvcRoleManager.Web.Security.ViewModels;
using System.Collections.Generic;
using System.Linq;


namespace MvcRoleManager.Web.Security.BSO
{
    public class RoleManagerBso
    {
        private UnitOfWork UnitOfWork { get; set; } 

        public RoleManagerBso() {
            this.UnitOfWork = new UnitOfWork(RoleManagerDbContext.Create());
        }
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
            UnitOfWork.Context.Configuration.LazyLoadingEnabled = false;
            var roles = UnitOfWork.Repository<ApplicationRole>().Get().OrderBy(r => r.Name);
            return roles.ToList();
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public MvcRole AddRole(MvcRole role)
        {
            var dbRole = UnitOfWork.Repository<ApplicationRole>().Insert(new ApplicationRole
            {
                Name = role.Name
            });
            UnitOfWork.Save();
            role.Id = dbRole.Id;
            return role;
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="role"></param>
        public void DeleteRole(MvcRole role)
        {
            UnitOfWork.Repository<ApplicationRole>().Delete(role.Id);
            UnitOfWork.Save();
        }

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="role"></param>
        public void UpdateRole(MvcRole role)
        {
            UnitOfWork.Repository<ApplicationRole>().Update(new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name
            });
            UnitOfWork.Save();
        }


        /// <summary>
        /// Get associated roles by action
        /// </summary>
        /// <param name="mvcAction"></param>
        /// <returns></returns>
        public List<MvcRole> GetRolesByAction(MvcAction mvcAction)
        {
            //Only ControllerName and ActionName have indexes.
            //Most of cases, controller and action name should be able to identify the right record.
            var action = this.GetAction(mvcAction);
            var actionRoles = action?.Roles;
            if (actionRoles == null) return null;

            List<MvcRole> selectedRoles = new List<MvcRole>();
            foreach (var role in actionRoles)
            {
                selectedRoles.Add(new MvcRole { Id = role.Id, Name=role.Name});
            }

            return selectedRoles;
        }

        /// <summary>
        /// Get actions that are assigned to role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Models.Action> GetActionsByRole(string roleId)
        {
            UnitOfWork.Context.Configuration.LazyLoadingEnabled = false;
            return UnitOfWork.Repository<Models.Action>()
                .Get(act => act.Roles.Any(r => r.Id == roleId)).ToList();
        }

        /// <summary>
        /// Add actions to role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="mvcActions"></param>
        public void AddActionsToRole(MvcRole role)
        {
            var dbRole = UnitOfWork.Repository<ApplicationRole>().GetByID(role.Id);
            var mvcActions = role.Actions;
            //add new actions to role
            if (dbRole != null)
            {
                foreach (var action in mvcActions)
                {
                    if (!dbRole.Actions.Any(act => action.ControllerName == act.ControllerName &&
                                       action.ActionName == act.ActionName &&
                                       string.Join(",", action.ParameterTypes.ToArray()) == act.ParameterTypes &&
                                       action.ReturnType == act.ReturnType))
                    {
                        var dbAction = this.GetAction(action);
                        if (dbAction == null)
                        {
                            dbRole.Actions.Add(new Models.Action
                            {
                                ActionName = action.ActionName,
                                Methods = action.Methods,
                                ControllerName = action.ControllerName,
                                ParameterTypes = string.Join(",", action.ParameterTypes.ToArray()),
                                ReturnType = action.ReturnType
                            });
                        }
                        else
                        {
                            dbRole.Actions.Add(dbAction);
                        }
                    }
                }
                //remove not selected actions from role
                foreach (var action in dbRole.Actions.ToList())
                {
                    if (!mvcActions.Any(act => action.ControllerName == act.ControllerName &&
                                       action.ActionName == act.ActionName &&
                                       string.Join(",", act.ParameterTypes.ToArray()) == action.ParameterTypes &&
                                       action.ReturnType == act.ReturnType))
                    {
                        dbRole.Actions.Remove(action);
                    }
                }
                UnitOfWork.Save();

            }
        }



        /// <summary>
        /// Save roles to action
        /// </summary>
        /// <param name="mvcAction"></param>
        public void AddRolesToAction(MvcAction mvcAction)
        {
            Models.Action action = this.GetAction(mvcAction);

            if (action == null)
            {
                action = new Models.Action
                {
                    ActionName = mvcAction.ActionName,
                    ControllerName = mvcAction.ControllerName,
                    ParameterTypes = string.Join(",", mvcAction.ParameterTypes.ToArray()),
                    ReturnType = mvcAction.ReturnType
                };
                UnitOfWork.Repository<Models.Action>().Insert(action);
            }

            AddRolesToAction(mvcAction.Roles, action);
            UnitOfWork.Save();
        }

        /// <summary>
        /// Get action entity from database by MvcAction object
        /// </summary>
        /// <param name="mvcAction"></param>
        /// <returns></returns>
        private Models.Action GetAction(MvcAction mvcAction)
        {
            var actions = UnitOfWork.Repository<Models.Action>()
              .Get(a => a.ControllerName == mvcAction.ControllerName && a.ActionName == mvcAction.ActionName).ToList();
            string parameterTypes = "";
            if (mvcAction.ParameterTypes != null)
            {
                parameterTypes = string.Join(",", mvcAction.ParameterTypes.ToArray());
            }

            if (actions.Count() == 0)
                return null;
            return actions.Where(a => a.ReturnType == mvcAction.ReturnType && a.ParameterTypes == parameterTypes)
            .FirstOrDefault();
        }

        /// <summary>
        /// Assign roles to action
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="action"></param>
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
                    UnitOfWork.Context.Entry<ApplicationRole>(role).State = System.Data.Entity.EntityState.Unchanged;
                }
            }
        }

    }
}
