# Mvc Role Manager

This project is using Asp.net MVC, Identity framework, OAuth security, Token/Claims based authentication/authorization, Owin, Web Api2 and Angular.js.

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
<li><h3>Front end stack</h3> Angular.js, bootstrap, angular ui bootstrap, angular schema form. Try to avoid jquery.</li>
<li><h3>Back end stack</h3> ASP.NET mvc, web api 2, entityframe work 6, identity framework</li>
<li><h3>Database</h3> SQL server.  Mysql and more types of databases later.</li>
</ul>
<h3>What kind of projects can use it?</h3>
If you are going to create a new project, your project can use it as scaffolding and start from there.
If you have an existing project, which uses <strong>ASP.NET MVC/WEB API</strong> as well as <strong>asp.net identity framework(Token/Claim based authentication)</strong>, you can integrate Mvc Role Manager into your project very easily.
All you have to do is:
<ul><li>
Add config.Filters.Add(new ApiAuthoraiztionFilter()); to App_Start\WebApiConfig.cs file  for web api authorization;
</li>
<li> Add filters.Add(new HandleErrorAttribute()); to App_Start\FilterConfig.cs for MVC web page authorization;
</li>
<li>Copy Areas/RoleManager folder to your project. MvcRoleManager resides in this folder only. </li>
<li>The project will create two new tables, Action and ActionRoles. Action stores actions of controllers that will be configured for authorization; ActionRoles have the links between Actions and roles.</li>
<li>Configuration page locates at http://yourproject/RoleManager/ . 
<a href="https://drive.google.com/open?id=0B_vc8f3gs88KbUV4empfQ1k2WEk" target=_blank>SCREENSHOT</a>
<a href="https://drive.google.com/open?id=0B_vc8f3gs88KZWV1Z3ZuN2dtckU" target=_blank>SCREENSHOT1</a>
</li>
</ul>

<h3>How to use</h3>
<ul>
<li>Download the code and open in Visual Studio. </li>
<li>Restore dependency files and change the database connection string in web.config.</li>
<li>Build the solution and you are good for a test ride. </li>
<li></li>
</ul>

<p>Any questions, email: popman.he@gmail.com.</p>
