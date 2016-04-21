# MvcRoleManager

This project is using Asp.net MVC, Identity framework, Owin, Web Api2 and Angular.js.

Asp.net mvc provides an authorization featuers based on roles/users for controllers and actions. 
But the roles/actions are hard coded in the code.
For example: 

[Authorize(Roles = "Admin")]
public Class  MyController : Controller
{

}

Only Admin role is authorized to use this controller. It's hardcoded.

The project will provide a generic function with a web page that authorize roles/uers for controllers and actions. 
These setinggs will be saved to database or a file.

<ul>
<li><h4>Front end stack</h4> Angular.js, bootstrap, angular ui bootstrap, angular schema form. Try to avoid jquery.</li>
<li><h4>Back end stack</h4> ASP.NET mvc, web api 2, entityframe work 6, identity framework</li>
<li><h4>Database</h4> SQL server or Mysql. May add more types of databases later.</li>
</ul>

