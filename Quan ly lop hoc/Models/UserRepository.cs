using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using LMS_SASS.RepositoryInterfaces;
using LMS_SASS.Databases;

namespace LMS_SASS.Models
{
    public class UserRepository:IUserRepositories
    {
        private readonly DatabaseContext _appDbContext;

        public UserRepository(DatabaseContext appDbContext) { 
            _appDbContext = appDbContext;
        }

        public List<UserModel> GetUsers() { 
            return _appDbContext.Users.ToList();
        }
        
        public UserModel CreateUser(UserModel User) { 
            _appDbContext.Users.Add(User);
            _appDbContext.SaveChanges();
            return User;
        }

        public bool DeleteUser(int id)
        {
            var User = _appDbContext.Users.Find(id);
            if (User != null)
            {
                _appDbContext.Users.Remove(User);
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public bool UpdateUser(UserModel newUser) {
            UserModel User = _appDbContext.Users.Find(newUser.Id);
            
            if (User != null) {
                User = newUser;
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public UserModel FindUser(int? id) {
            UserModel User = _appDbContext.Users.Find(id);
            return User;
        }

        public List<UserModel> FindCourseUnenrolledUsers(List<int?> enrolledUserIds){
            List<UserModel> users = _appDbContext.Users
                .Where(u => !enrolledUserIds.Contains(u.Id))
                .ToList();
            
            return users;
        }

        public UserModel FindUserByUsername(string username){
            UserModel user = _appDbContext.Users
                .Where((u) => u.Username == username)
                .FirstOrDefault();

            return user;
        }

        public List<UserModel> SearchUser(string query){
            List<UserModel> users = _appDbContext.Users.Where(u =>
                u.Username.Contains(query) ||
                u.Name.Contains(query) ||
                u.Email.Contains(query) ||
                u.Role.Contains(query)
            ).ToList();
            return users;
        }

    }
}
