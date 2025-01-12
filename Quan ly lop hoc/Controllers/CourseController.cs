using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LMS_SASS.Models;
using LMS_SASS.Databases;
using System.Data;
using LMS_SASS.Interfaces;
using Microsoft.AspNetCore.Identity;
using LMS_SASS.RepositoryInterfaces;

namespace LMS_SASS.Controllers;

public class CourseController(ICourseRepositories courseRepositories, IUserRepositories userRepositories, ICourseUserRepositories courseUserRepositories) : BaseController(userRepositories) {

    [HttpGet]
    [Route("/course/{Id:int}")]
    public IActionResult ViewCourse(int Id) {
        var course = courseRepositories.FindCourse(Id);
        int userId = (int) HttpContext.Session.GetInt32("UserId");
        CourseUserModel courseUser = courseUserRepositories.FindCourseUser(Id, userId);
        string? role = courseUser?.Role;
        if (course == null)
            return NotFound();
        
        ViewBag.RoleInCourse = role;
        return View("View", course);
    }

    [HttpGet]
    [Route("/course/create")]
    public IActionResult CreateCourseView() {
        ViewBag.Action = "create";
        return View("Create");
    }

    [HttpPost]
    [Route("/course/create")]
    public IActionResult CreateCourse(CourseModel course) {
        if (!ModelState.IsValid)
            return CreateCourseView();

        if (course.StartDate > course.EndDate) {
            ModelState.AddModelError("StartDate", "Ngày Bắt Đầu không thể sau Ngày Kết Thúc");
            ModelState.AddModelError("EndDate", "Ngày Bắt Đầu không thể sau Ngày Kết Thúc");
            return CreateCourseView();
        }

        var courseExist = courseRepositories.CodeExist(course.Code);

        if (courseExist) {
            ModelState.AddModelError("Code", "Đã có khóa học với mã này rồi!");
            return CreateCourseView();
        }

        course.InviteCode = Utils.RandomString(7);
        courseRepositories.CreateCourse(course);

        UserModel userModel = userRepositories.FindUser((int)HttpContext.Session.GetInt32("UserId"));
        CourseModel courseModel = courseRepositories.FindCourse(course.Id);
        CourseUserModel courseUser = new CourseUserModel{
            UserId = HttpContext.Session.GetInt32("UserId"),
            User = userModel,
            CourseId = course.Id,
            Course = courseModel,
            Role = "teacher"
        };

        courseUserRepositories.CreateCourseUser(courseUser);

        return RedirectToAction("ViewCourse", new { Id = course.Id });
    }

    [HttpGet]
    [Route("/course/{Id:int}/edit")]
    public IActionResult EditCourseView(int Id) {
        var course = courseRepositories.FindCourse(Id);

        if (course == null)
            return NotFound();

        ViewBag.Action = "edit";
        return View("Edit", course);
    }

    [HttpPost]
    [Route("/course/{Id:int}/edit")]
    public IActionResult EditCourse(CourseModel newCourse, int Id) {
        if (!ModelState.IsValid)
            return CreateCourseView();

        if (newCourse.StartDate > newCourse.EndDate) {
            ModelState.AddModelError("StartDate", "Ngày Bắt Đầu không thể sau Ngày Kết Thúc");
            ModelState.AddModelError("EndDate", "Ngày Bắt Đầu không thể sau Ngày Kết Thúc");
            return CreateCourseView();
        }

        courseRepositories.UpdateCourse(newCourse);

        return RedirectToAction("ViewCourse", new { Id = newCourse.Id });
    }

    [HttpGet]
    [Route("/course/{Id:int}/peoples")]
    public IActionResult Peoples(int Id) {
        var course = courseRepositories.FindCourse(Id);

        if (course == null)
            return NotFound();

        var data = new CoursePeoplesModel {
            Course = course,
            CourseUsers = courseUserRepositories.FindCourseUsers(course.Id)
        };

        return View("Peoples", data);
    }

    [HttpGet]
    [Route("/course/{Id:int}/unenrolledUsers")]
    public IActionResult UnenrolledUsers(int Id) {
        var course = courseRepositories.FindCourse(Id);

        if (course == null)
            return NotFound();

        var enrolledUserIds = courseUserRepositories.FindCourseEnrolledUserIds(course.Id);

        var unenrolledUsers = userRepositories.FindCourseUnenrolledUsers(enrolledUserIds);

        return Json(unenrolledUsers);
    }

    [HttpPost]
    [Route("/course/{Id:int}/enroll")]
    public IActionResult EnrollUser(int Id, EnrollUserModel enroll) {
        var course = courseRepositories.FindCourse(Id);
        var user = userRepositories.FindUser(enroll.UserId);

        if (course == null || user == null)
            return NotFound();

        var item = new CourseUserModel {
            UserId = user.Id,
            CourseId = course.Id,
            Role = enroll.Role
        };

        courseUserRepositories.CreateCourseUser(item);

        return Json(new { Message = "OK" });
    }

    [HttpGet]
    [Route("/course/selfEnroll")]
    public IActionResult SelfEnrollCourseView() {
        return View("SelfEnroll");
    }
}
