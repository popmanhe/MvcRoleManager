using MvcRoleManager.Security.DAL;
using MvcRoleManager.Security.Models;
using System.Collections.Generic;
using System.Linq;

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

        public List<ApplicationRole> GetRoles()
        {
            var roles = unitOfWork.Repository<ApplicationRole>().Get().OrderBy(r => r.Name);
            return roles.ToList();
        }

        public ICollection<ApplicationRole> GetActionRoles(MvcAction action)
        {
            //Only ControllerName and ActionName have indexes.
            //Most of cases, controller and action name should be able to identify the right record.
            var actions = unitOfWork.Repository<Models.Action>()
                .Get(a => a.ControllerName == action.ControllerName && a.ActionName == action.ActionName).ToList();

            if (actions.Count() == 1)
            {
                return actions.FirstOrDefault().Roles;
            }
            else if (actions.Count() > 1)
            {
                //if there are more than 1 records returned, use return type and parameter types to identify the record.
                string parameterTypes = "";
                if (action.ParametersTypes != null)
                {
                    parameterTypes = string.Join(",", action.ParametersTypes.ToArray());
                }

                return actions.Where(a => a.ReturnType == action.ReturnType && a.ParameterTypes == parameterTypes)
                .FirstOrDefault()?.Roles;
            }

            return null;
        }

        public void SaveActionRoles(MvcAction mvcAction)
        {
            var actions = unitOfWork.Repository<Models.Action>().Get(act => act.ActionName == mvcAction.ActionName
            && act.ControllerName == mvcAction.ControllerName);
            Models.Action action = null;
            switch (actions.Count())
            {
                case 1: //only one returned
                    action = actions.FirstOrDefault();
                    break;
                case 0: // no found
                    action = new Models.Action
                    {
                        ActionName = mvcAction.ActionName,
                        ControllerName = mvcAction.ControllerName,
                        ParameterTypes = string.Join(",", mvcAction.ParametersTypes.ToArray()),
                        ReturnType = mvcAction.ReturnType
                    };
                    unitOfWork.Repository<Action>().Insert(action);
                    break;
                default:  // greater than 1
                    actions = actions.Where(act => act.ParameterTypes == string.Join(",", mvcAction.ParametersTypes.ToArray())
                    && act.ReturnType == mvcAction.ReturnType);

                    if (actions.Count() == 0)
                    {
                        action = new Models.Action
                        {
                            ActionName = mvcAction.ActionName,
                            ControllerName = mvcAction.ControllerName,
                            ParameterTypes = string.Join(",", mvcAction.ParametersTypes.ToArray()),
                            ReturnType = mvcAction.ReturnType
                        };
                        unitOfWork.Repository<Action>().Insert(action);
                    }
                    else { action = actions.FirstOrDefault(); }

                    break;
            }
            addRolesToAction(mvcAction.Roles, action);
             unitOfWork.Save();

        }

        private void addRolesToAction(List<ApplicationRole> roles, Models.Action action)
        {
            foreach (ApplicationRole role in roles)
            {
                action.Roles.Add(role);
            }

        }

    }
}
