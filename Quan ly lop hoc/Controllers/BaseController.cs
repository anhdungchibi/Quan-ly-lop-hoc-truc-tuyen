using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LMS_SASS.Databases;
using LMS_SASS.RepositoryInterfaces;

namespace LMS_SASS.Controllers;

public class BaseController : Controller {

	private readonly IUserRepositories userRepositories;

	public BaseController(IUserRepositories userRepositories)
	{
		this.userRepositories = userRepositories;
	}

	public override void OnActionExecuting(ActionExecutingContext context) {
		// Assuming you've already set the username in your session upon login
		var userId = HttpContext.Session.GetInt32("UserId");
		var valid = true;

		if (userId == null) {
			valid = false;
		} else {
			var user = userRepositories.FindUser(userId);

			if (user == null) {
				valid = false;
			} else {
				ViewBag.User = user;
			}
		}

		if (!valid) {
			context.Result = new RedirectToActionResult("LoginPage", "Login", null);
			HttpContext.Session.Clear();
		}

		base.OnActionExecuting(context);
	}
}
