using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LMS_SASS.Controllers;
using LMS_SASS.Databases;
using LMS_SASS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate.Util;
using LMS_SASS.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LMS_SASS.Controllers
{
  public class ActivityController(IAssignmentRepositories assignmentRepositories, IMeetingRepositories meetingRepositories, IUserRepositories userRepositories, ICourseRepositories courseRepositories, ICourseUserRepositories courseUserRepositories) : BaseController(userRepositories)
  {
    public IActionResult Index()
    {
      return View();
    }
    
    [HttpGet]
    [Route("/course/{courseId:int}/activities")]
    public IActionResult GetCourseActivities(int courseId)
    {
    var listAssignment = assignmentRepositories.GetAssignments(courseId);
    var listMeeting = meetingRepositories.GetMeetings(courseId);
    var combinedList = listAssignment.Select(x => (object)x)
    .Concat(listMeeting.Select(x => (object)x))
    .ToList();
      return Json(combinedList);
    }

    [HttpGet]
    [Route("/course/{courseId:int}/activity/meeting/create")]
    public IActionResult CreateMeetingView(int courseId)
    {
      ViewBag.Action = "create";
      ViewBag.CourseId = courseId;
      return View("CreateMeeting");
    }

    [HttpPost]
    [Route("/activity/meeting/create")]
    public IActionResult CreateMeeting(ActivityQuizViewModel model)
    {
      if (model.Meeting.StartDate >= DateTime.UtcNow && model.Meeting.EndDate > model.Meeting.StartDate)
      {
        MeetingModel meeting = new MeetingModel
        {
          Id = model.Meeting.Id,
          url = model.Meeting.url,
          CourseId = model.Meeting.CourseId,
          Created = DateTime.UtcNow,
          Course = model.Meeting.Course,
          Name = model.Meeting.Name,
          StartDate = model.Meeting.StartDate,
          EndDate = model.Meeting.EndDate,
        };
        meetingRepositories.CreateMeeting(meeting);
        
        return RedirectToAction("ViewCourse", "Course", new { Id = model.Meeting.CourseId });
      }
      return RedirectToAction("CreateMeetingView", new { courseId = model.Meeting.CourseId });
    }

    [HttpGet]
    [Route("/course/{courseId:int}/activity/assignment/create")]
    public IActionResult CreateAssignmentView(int courseId)
    {
      ViewBag.Action = "create";
      ViewBag.CourseId = courseId;
      return View("CreateAssignment");
    }

    [HttpPost]
    [Route("/activity/assignment/create")]
    public IActionResult CreateAssignment(string Name, string Description,
    double PassingGrade,
    DateTime StartDate,
    DateTime EndDate,
    int CourseId)
    {
      if (StartDate >= DateTime.UtcNow && EndDate > StartDate)
      {
        CourseModel courseData = courseRepositories.FindCourse(CourseId);
        AssignmentModel assignment = new AssignmentModel
        {
          Id = 0,
          Description = Description,
          PassingGrade = PassingGrade,
          CourseId = CourseId,
          Created = DateTime.UtcNow,
          Course = courseData,
          Name = Name,
          StartDate = StartDate,
          EndDate = EndDate,
        };
        assignmentRepositories.CreateAssignment(assignment);

        return Json(new { Message = "OK", Status = "success" });
      }
      return Json(new { Message = "Fail", Status = "error" });

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View("Error!");
    }

    [Route("/assignment/{Id:int}")]
    public IActionResult Assignment(int Id) {
      var assignment = assignmentRepositories.FindAssignment(Id);
      int userId = (int) HttpContext.Session.GetInt32("UserId");
      var courseUser = courseUserRepositories.FindCourseUser(assignment.CourseId, userId);
      string role = courseUser.Role;
      if (assignment == null)
          return NotFound();
      
      ViewBag.RoleInCourse = role;
      return View(assignment);
    }

    [Route("/meeting/{Id:int}")]
    public IActionResult Meeting(int Id) {
      var meetingModel = meetingRepositories.FindMeeting(Id);
      int userId = (int) HttpContext.Session.GetInt32("UserId");
      var courseUser = courseUserRepositories.FindCourseUser(meetingModel.CourseId, userId);
      string role = courseUser.Role;
      if (meetingModel == null)
          return NotFound();
      
      ViewBag.RoleInCourse = role;
      return View(meetingModel);
    }

    [HttpGet]
    [Route("/assignment/edit/{Id:int}")]
    public IActionResult EditAssignmentView(int Id) {
      var assignment = assignmentRepositories.FindAssignment(Id);
      if (assignment == null)
          return NotFound();
      
      return View("EditAssignment", assignment);
    }

    [HttpPost]
    [Route("/assignment/edit")]
    public IActionResult EditAssignment(int Id, string Name, string Description, double PassingGrade, DateTime StartDate, DateTime EndDate, int CourseId) {
      CourseModel courseData = courseRepositories.FindCourse(CourseId);
      AssignmentModel assignment = new AssignmentModel
        {
          Id = Id,
          Description = Description,
          PassingGrade = PassingGrade,
          CourseId = CourseId,
          Created = DateTime.UtcNow,
          Course = courseData,
          Name = Name,
          StartDate = StartDate,
          EndDate = EndDate,
        };
      if (assignmentRepositories.EditAssignment(assignment)) {
        return RedirectToAction("Assignment", new { Id = Id });
      }
      return RedirectToRoute("EditAssignmentView", new { Id = Id });
    }

    [HttpGet]
    [Route("/meeting/edit/{Id:int}")]
    public IActionResult EditMeetingView(int Id) {
      var meeting = meetingRepositories.FindMeeting(Id);
      if (meeting == null)
          return NotFound();
      
      return View("EditMeeting", meeting);
    }

    [HttpPost]
    [Route("/meeting/edit")]
    public IActionResult EditMeeting(int Id, string url, int CourseId, string Name, DateTime StartDate, DateTime EndDate) {
      CourseModel courseData = courseRepositories.FindCourse(CourseId);
      MeetingModel meeting = new MeetingModel
        {
          Id = Id,
          url = url,
          CourseId = CourseId,
          Created = DateTime.UtcNow,
          Course = courseData,
          Name = Name,
          StartDate = StartDate,
          EndDate = EndDate,
        };
      if (meetingRepositories.EditMeeting(meeting)) {
        return RedirectToAction("Meeting", new { Id = Id });
      }
      return RedirectToRoute("EditMeetingView", new { Id = Id });
    }
  }
}