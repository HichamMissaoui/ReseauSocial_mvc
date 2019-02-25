using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koombu.Bll.Concrete;
using Koombu.Bll.ViewModel;

namespace SocialKoombu.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			
			return View();
		}

		public ActionResult Home()
		{
			if (Session["UserName"] == null)
			{
				return RedirectToAction("Error");
			}

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
			HomeVm homeVm = new HomeVm();
			var postList = kBll.GetAllPostForUser(userId);
			homeVm.UserPostList = kBll.SetPostUserObject(postList);						
			homeVm.UserConnected = kBll.GetUser(userId);
			homeVm.FollowedUsersObjects = kBll.GetFollowedUsersObject(userId);
			return View(homeVm);
		}

		public ActionResult CreateAccount(string firstName, string lastName, string email, DateTime bday, string password,string confirmPassword,string department, string title)
		{
			if (password != confirmPassword)
			{
				ViewData["PassError"] = "true";
				return View("Index");
			}

			KoombuBll kBll = new KoombuBll();
			if (kBll.CheckEmail(email))
			{
				ViewData["EmailExist"] = "true";
				return View("Index");
			}

			
			var user = kBll.RegisterUser(firstName, lastName, email, bday, password, department, title);

			Session["UserName"] = user.FirstName + " " + user.LastName;
			Session["UserId"] = user.UserId;
			if (user.ProfilPicture != null)
			{
				Session["UserPic"] = user.ProfilPicture;
			}
			else
			{
				Session["UserPic"] = "~/Content/images/user.png";
			}

			return RedirectToAction("Home");
		}

		public ActionResult SignIn(string email,string password)
		{
			KoombuBll kBll = new KoombuBll();
			var user = kBll.SignIn(email, password);
			if (user != null)
			{
				Session["UserName"] = user.FirstName + " " + user.LastName;
				Session["UserId"] = user.UserId;
				if (user.ProfilPicture != null)
				{
					Session["UserPic"] = user.ProfilPicture;
				}
				else
				{
					Session["UserPic"] = "~/Content/images/user.png";
				}
				return RedirectToAction("Home");
			}

			ViewData["SignError"] = "true";
			return View("Index");
		}

		public ActionResult LogOut()
		{
			Session.Clear();

			return RedirectToAction("Index");
		}

		public ActionResult Error()
		{
			return View();
		}
	}
}