using LMS_SASS.Models;

namespace LMS_SASS.RepositoryInterfaces
{
    public interface ISubmissionRepositories
    {
        public List<SubmissionModel> GetSubmissions();
        public SubmissionModel CreateSubmission(SubmissionModel submission);
        public bool DeleteSubmission(int submissionId);
    }
}
