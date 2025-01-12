using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using LMS_SASS.RepositoryInterfaces;
using LMS_SASS.Databases;

namespace LMS_SASS.Models
{
    public class AssignmentRepository:IAssignmentRepositories
    {
        private readonly DatabaseContext _appDbContext;

        public AssignmentRepository(DatabaseContext appDbContext) { 
            _appDbContext = appDbContext;
        }

        public List<AssignmentModel> GetAssignments(int courseId) { 
            return _appDbContext.Assignments.Where(assignment => assignment.CourseId == courseId).ToList();
        }
        
        public AssignmentModel CreateAssignment(AssignmentModel assignment) { 
            _appDbContext.Assignments.Add(assignment);
            _appDbContext.SaveChanges();
            return assignment;
        }

        public bool DeleteAssignment(int id)
        {
            var assignment = _appDbContext.Assignments.Find(id);
            if (assignment != null)
            {
                _appDbContext.Assignments.Remove(assignment);
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public bool EditAssignment(AssignmentModel assignmentModel) {
            AssignmentModel assignment = _appDbContext.Assignments.Find(assignmentModel.Id);

            if (assignment != null) {
                assignment.Name = assignmentModel.Name;
                assignment.Description = assignmentModel.Description;
                assignment.StartDate = assignmentModel.StartDate;
                assignment.EndDate = assignmentModel.EndDate;
                assignment.PassingGrade = assignmentModel.PassingGrade;
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public AssignmentModel FindAssignment(int assignmentId) {
            AssignmentModel assignment = _appDbContext.Assignments.Find(assignmentId);
            return assignment;
        }

    }
}
