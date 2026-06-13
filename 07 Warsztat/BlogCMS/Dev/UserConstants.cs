using BlogCMS.Models;

namespace BlogCMS.Dev
{
    public class UserConstants
    {
        public static List<LoginModel> Users = new()
        {
                new LoginModel(){ Username="Kacper",Password="TajneHaslo_1234",Role="Admin"}
        };
    }
}
