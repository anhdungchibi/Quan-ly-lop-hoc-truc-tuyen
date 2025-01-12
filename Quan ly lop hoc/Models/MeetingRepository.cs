using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using LMS_SASS.RepositoryInterfaces;
using LMS_SASS.Databases;

namespace LMS_SASS.Models
{
    public class MeetingRepository:IMeetingRepositories
    {
        private readonly DatabaseContext _appDbContext;

        public MeetingRepository(DatabaseContext appDbContext) { 
            _appDbContext = appDbContext;
        }

        public List<MeetingModel> GetMeetings(int courseId) { 
            return _appDbContext.Meetings.Where(Meeting => Meeting.CourseId == courseId).ToList();
        }
        
        public MeetingModel CreateMeeting(MeetingModel Meeting) { 
            _appDbContext.Meetings.Add(Meeting);
            _appDbContext.SaveChanges();
            return Meeting;
        }

        public bool DeleteMeeting(int id)
        {
            var Meeting = _appDbContext.Meetings.Find(id);
            if (Meeting != null)
            {
                _appDbContext.Meetings.Remove(Meeting);
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public MeetingModel FindMeeting(int id) {
            MeetingModel meeting = _appDbContext.Meetings.Find(id);
            return meeting;
        }

        public bool EditMeeting(MeetingModel meetingModel) {
            MeetingModel meeting = _appDbContext.Meetings.Find(meetingModel.Id);

            if (meeting != null) {
                meeting.Name = meetingModel.Name;
                meeting.StartDate = meetingModel.StartDate;
                meeting.EndDate = meetingModel.EndDate;
                meeting.url = meetingModel.url;
                _appDbContext.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

    }
}
