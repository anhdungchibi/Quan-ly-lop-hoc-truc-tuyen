using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using LMS_SASS.RepositoryInterfaces;
using LMS_SASS.Databases;

namespace LMS_SASS.Models
{
    public class CourseUserRepository:ICourseUserRepositories
    {
        private readonly DatabaseContext _appDbContext;

        public CourseUserRepository(DatabaseContext appDbContext) { 
            _appDbContext = appDbContext;
        }

        public List<CourseUserModel> GetCourseUsers() { 
            return _appDbContext.CourseUsers.ToList();
        }
        
        public CourseUserModel CreateCourseUser(CourseUserModel CourseUser) { 
            _appDbContext.CourseUsers.Add(CourseUser);
            _appDbContext.SaveChanges();
            return CourseUser;
        }

        public bool DeleteCourseUser(int id)
        {
            var CourseUser = _appDbContext.CourseUsers.Find(id);
            if (CourseUser != null)
            {
                _appDbContext.CourseUsers.Remove(CourseUser);
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public double UpdatePrice(int id) {
            CourseUserModel CourseUser = _appDbContext.CourseUsers.Find(id);
            
            if (CourseUser != null) {
                return 1;
            } else {
                return 0;
            }
        }

        public CourseUserModel FindCourseUser(int courseId, int userId)
        {
            CourseUserModel courseUserModel = _appDbContext.CourseUsers.FirstOrDefault(e => e.UserId == userId && e.CourseId == courseId);
            return courseUserModel;
        }

        public List<CourseUserModel> FindCourseUsers(int courseId){
            List<CourseUserModel> users = _appDbContext.CourseUsers
                .Where((cu) => cu.CourseId == courseId)
                .Include(cu => cu.User) // Tải đối tượng User liên quan
                .Include(cu => cu.Course) // Tải đối tượng Course liên quan nếu cần
                .ToList();
            
            return users;
        }

        public List<int?> FindCourseEnrolledUserIds(int courseId){
            List<int?> userIds = _appDbContext.CourseUsers
                .Where(cu => cu.CourseId == courseId)
                .Select(cu => cu.UserId)
                .ToList();
            
            return userIds;
        }
    }
}
