namespace WebApplication
{
    public interface IUserRepository
    {
        public void SaveUser(
            User user);
    }

    public class UserRepository : IUserRepository
    {
        public void SaveUser(
            User user)
        {
            //save
        }
    }
}
