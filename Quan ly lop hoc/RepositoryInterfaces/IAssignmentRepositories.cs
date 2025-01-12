using LMS_SASS.Models;

namespace LMS_SASS.RepositoryInterfaces
{
    public interface IAssignmentRepositories
    {
        public List<AssignmentModel> GetAssignments(int courseId);
        public AssignmentModel CreateAssignment(AssignmentModel assignment);
        public bool DeleteAssignment(int assignmentId);
        public AssignmentModel FindAssignment(int assignmentId);
        public bool EditAssignment(AssignmentModel assignmentModel);
    }
}
