using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LMS_SASS.Models;
using LMS_SASS.Databases;
using LMS_SASS.RepositoryInterfaces;

namespace LMS_SASS.Controllers;

public class LoginController(IUserRepositories userRepositories) : Controller {
    [HttpGet]
    [Route("/login")]
    public IActionResult LoginPage() {
        return View("Index");
    }

	[HttpPost]
	[Route("/login")]
	public IActionResult PerformLogin(LoginFormCredentialModel data) {
		var user = userRepositories.FindUserByUsername(data.Username);
		
		if (user == null) {
			ModelState.AddModelError("Username", "Username không tồn tại trong hệ thống!");
			return View("Index");
		}

		if (user.Password != data.Password) {
			ModelState.AddModelError("Password", "Password không tồn tại trong hệ thống!");
			return View("Index");
		}

		HttpContext.Session.SetInt32("UserId", user.Id);
		return RedirectToAction("Index", "Home");
	}

	[HttpGet]
	[Route("/login/register")]
	public IActionResult RegisterPage() {
		return View("Register");
	}

	[HttpPost]
	[Route("/login/register")]
	public IActionResult PerformRegister(UserModel user) {
		if (!ModelState.IsValid)
			return View("Register");

		var existingUser = userRepositories.FindUserByUsername(user.Username);
		
		if (existingUser != null) {
			ModelState.AddModelError("Username", "Username này đã tồn tại trong hệ thống!");
			return View("Register");
		}

		user.Created = DateTime.Now;

		userRepositories.CreateUser(user);

		HttpContext.Session.SetInt32("UserId", user.Id);
		return RedirectToAction("Index", "Home");
	}

	[HttpGet]
	[Route("/login/logout")]
	public IActionResult Logout() {
		HttpContext.Session.Clear();
		return RedirectToAction("LoginPage");
	}
}
