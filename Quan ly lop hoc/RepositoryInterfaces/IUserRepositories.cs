using LMS_SASS.Models;

namespace LMS_SASS.RepositoryInterfaces
{
    public interface IUserRepositories
    {
        public List<UserModel> GetUsers();
        public UserModel CreateUser(UserModel user);
        public bool DeleteUser(int userId);
        public UserModel FindUser(int? userId);
        public UserModel FindUserByUsername(string username);
        public List<UserModel> FindCourseUnenrolledUsers(List<int?> enrolledUserIds);
        public bool UpdateUser(UserModel userModel);
        public List<UserModel> SearchUser(string query);
    }
}
