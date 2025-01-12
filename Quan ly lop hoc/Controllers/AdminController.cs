using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LMS_SASS.Models;
using LMS_SASS.Databases;
using Microsoft.EntityFrameworkCore;
using LMS_SASS.RepositoryInterfaces;

namespace LMS_SASS.Controllers;

public class AdminController(IUserRepositories userRepositories) : BaseController(userRepositories) {

    private readonly IUserRepositories userRepositories = userRepositories;

    [HttpGet]
    [Route("/admin/accounts")]
    public IActionResult Accounts() {
        var results = userRepositories.GetUsers();
        return View( results);
    }
    [HttpGet("/admin/getdata")]
    public async Task<IActionResult> GetData()
    {
        var results = userRepositories.GetUsers();
        return new JsonResult(new { Data = results, TotalItems = results.Count });
    }
    [HttpPost("/admin/create")]
    public async Task<IActionResult> Create(UserModel model)
    {
        model.Created = DateTime.Now;
        try
        {
            userRepositories.CreateUser(model);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Ok(new { success = false });
        }
    }
    [HttpGet("/admin/search")]
    public IActionResult Search(string query)
    {
        var users = userRepositories.SearchUser(query);
        return PartialView("_UserTablePartial", users);
    }
    [HttpGet]
    [Route("/admin/courses")]
    public IActionResult Courses() {
        return View("Courses");
    }
}
