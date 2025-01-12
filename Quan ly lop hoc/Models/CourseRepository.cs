using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using LMS_SASS.RepositoryInterfaces;
using LMS_SASS.Databases;

namespace LMS_SASS.Models
{
    public class CourseRepository:ICourseRepositories
    {
        private readonly DatabaseContext _appDbContext;

        public CourseRepository(DatabaseContext appDbContext) { 
            _appDbContext = appDbContext;
        }

        public List<CourseModel> GetCourses() { 
            return _appDbContext.Courses.ToList();
        }
        
        public CourseModel CreateCourse(CourseModel Course) { 
            _appDbContext.Courses.Add(Course);
            _appDbContext.SaveChanges();
            return Course;
        }

        public bool DeleteCourse(int id)
        {
            var Course = _appDbContext.Courses.Find(id);
            if (Course != null)
            {
                _appDbContext.Courses.Remove(Course);
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public bool UpdateCourse(CourseModel courseModel) {
            CourseModel Course = _appDbContext.Courses.Find(courseModel.Id);
            
            if (Course != null) {
                Course = courseModel;
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public CourseModel FindCourse(int id) {
            CourseModel Course = _appDbContext.Courses.Find(id);
            return Course;
        }

        public bool CodeExist(string code){
            bool courseExist = _appDbContext.Courses.Any((c) => c.Code == code);
            return courseExist;
        }

        public List<CourseModel> UserCourses(int userId) {
            var query = from c in _appDbContext.Set<CourseModel>()
                        join eu in _appDbContext.Set<CourseUserModel>()
                            on c.Id equals eu.CourseId
                        where eu.UserId == userId
                        select c;

            List<CourseModel> courses = query.ToList();
            return courses;
        }

    }
}
