using LMS_SASS.Models;

namespace LMS_SASS.RepositoryInterfaces
{
    public interface IMeetingRepositories
    {
        public List<MeetingModel> GetMeetings(int courseId);
        public MeetingModel CreateMeeting(MeetingModel meeting);
        public bool DeleteMeeting(int meetingId);
        public MeetingModel FindMeeting(int id);
        public bool EditMeeting(MeetingModel meetingModel);
    }
}
