using LMS_SASS.Models;

namespace LMS_SASS.RepositoryInterfaces
{
    public interface ICourseRepositories
    {
        public List<CourseModel> GetCourses();
        public CourseModel CreateCourse(CourseModel course);
        public bool DeleteCourse(int courseId);
        public CourseModel FindCourse(int courseId);
        public bool CodeExist(string code);
        public bool UpdateCourse(CourseModel courseModel);
        public List<CourseModel> UserCourses(int userId);
    }
}
