using LMS_SASS.Models;

namespace LMS_SASS.RepositoryInterfaces
{
    public interface ICourseUserRepositories
    {
        public List<CourseUserModel> GetCourseUsers();
        public CourseUserModel CreateCourseUser(CourseUserModel courseUser);
        public bool DeleteCourseUser(int courseUserId);
        public CourseUserModel FindCourseUser(int courseId, int userId);
        public List<CourseUserModel> FindCourseUsers(int courseId);
        public List<int?> FindCourseEnrolledUserIds(int courseId);
    }
}
