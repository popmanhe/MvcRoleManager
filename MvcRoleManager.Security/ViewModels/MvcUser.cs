namespace MvcRoleManager.Security.ViewModels
{
    public class MvcUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public  bool Selected { get; set; }
    }
}