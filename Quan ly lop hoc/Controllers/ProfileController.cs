using FluentNHibernate.Conventions.AcceptanceCriteria;
using LMS_SASS.Databases;
using LMS_SASS.Interfaces;
using LMS_SASS.Models;
using LMS_SASS.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LMS_SASS.Controllers
{
    public class ProfileController(IUserRepositories userRepositories) : BaseController(userRepositories)
    {
 

        [HttpGet("/user/{id}")]
        public IActionResult Index(int id)
        {
            
            var user = userRepositories.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return View(user);
            }
        }

        public IActionResult Edit(int id)
        {
            var user = userRepositories.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Edit(UserModel userModel)
        {
            if (!ModelState.IsValid)
                return View("Edit");

            var existingUser = userRepositories.FindUserByUsername(userModel.Username);

			if (existingUser!=null && existingUser.Id != userModel.Id)
			{
				ModelState.AddModelError("Username", "Username này đã tồn tại trong hệ thống!");
				return View("Edit");
			}

			existingUser = userRepositories.FindUser(userModel.Id);

			if (existingUser == null)
			{
				return View("Edit");
			}
			existingUser.Username = userModel.Username;
			existingUser.Password = userModel.Password;
			existingUser.Name = userModel.Name;
			existingUser.Email = userModel.Email;
			existingUser.Phone = userModel.Phone;
			existingUser.Created = DateTime.Now;
            existingUser.DOB = userModel.DOB;

			userRepositories.UpdateUser(existingUser);

			HttpContext.Session.SetInt32("UserId", userModel.Id);
			return RedirectToAction("Index", new { id = userModel.Id });
		}
    }
}

	
