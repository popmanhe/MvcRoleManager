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


