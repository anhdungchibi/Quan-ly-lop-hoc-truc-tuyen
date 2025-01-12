using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LMS_SASS.Models;
using LMS_SASS.Databases;
using LMS_SASS.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using LMS_SASS.RepositoryInterfaces;

namespace LMS_SASS.Controllers;

public class HomeController(IUserRepositories userRepositories, ISessionService Session, ICourseRepositories courseRepositories) : BaseController(userRepositories) {

    [HttpGet]
    [Route("/")]
    public IActionResult Index() {
        var user = Session.CurrentUser();

        if (user == null)
            return RedirectToAction("LoginPage", "Login");

        List<CourseModel> courses;
        
        if (user.Role == UserModel.ROLE_ADMIN) {
            courses = courseRepositories.GetCourses();
        } else {
            courses = courseRepositories.UserCourses(user.Id);
        }

        return View(courses);
    }

    [HttpGet]
    [Route("/calendar")]
    public IActionResult Calendar() {
        return View();
    }

    [HttpGet]
    [Route("/sandbox")]
    public IActionResult Sandbox() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
