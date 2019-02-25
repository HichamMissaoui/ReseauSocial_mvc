using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koombu.Bll.Concrete;
using Koombu.Bll.ViewModel;

namespace SocialKoombu.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

	    public ActionResult MyProfil()
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
		    return RedirectToAction("Profil", new { id = userId });
	    }

	    public ActionResult Profil(int id)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int connectedUserId = Int32.Parse(Session["UserId"].ToString());

			KoombuBll kBll = new KoombuBll();
			ProfilVm profilVm =new ProfilVm();
			profilVm.User = kBll.GetUser(id);
		    profilVm.FollowedUsersObjects = kBll.GetFollowedUsersObject(connectedUserId);
			

		    if (id == connectedUserId)
		    {
			    ViewData["isConnect"] = "true";
		    }
		    else
		    {
			    ViewData["isConnect"] = "false";
		    }

		    
		    return View(profilVm);
	    }

		public ActionResult Users()
		{
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
			UsersVm usersVm = new UsersVm();
			usersVm.AllUsers = kBll.GetAllUsers();
			usersVm.FollowedUsersObjects = kBll.GetFollowedUsersObject(userId);
			return View(usersVm);
		}

	    public ActionResult FollowUser(int id)
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());

			if (id != userId)
		    {
			    KoombuBll kBll = new KoombuBll();
			    kBll.FollowUser(userId, id);
		    }


		    return RedirectToAction("Profil", new { id = id });
	    }

	    public ActionResult UnFollowUser(int id)
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());

		    if (id != userId)
		    {
			    KoombuBll kBll = new KoombuBll();
			    kBll.UnFollowUser(userId, id);
		    }


		    return RedirectToAction("Profil", new { id = id });
	    }

	    public ActionResult SearchUser(string userName)
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userConnectedId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    UsersVm usersVm = new UsersVm();

		    usersVm.AllUsers = kBll.SearchUser(userName);
		    usersVm.FollowedUsers = kBll.GetFollowedUserList(userConnectedId);
		    usersVm.FollowedUsersObjects = kBll.GetFollowedUsersObject(userConnectedId);
			return View("Users", usersVm);
	    }

	    public ActionResult FollowedUsers()
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    var followedUsers = kBll.GetFollowedUsersObject(userId);
		    return View(followedUsers);
	    }

	    public ActionResult ProfilPicture(int userId, HttpPostedFileBase pic)
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }


			if (pic != null && pic.ContentLength > 0)
		    {
			    var fileName = Path.GetFileName(pic.FileName);
				List<string> imgExtension = new List<string>(new string[] { ".gif", ".png", ".PNG", ".jpg", ".jpeg", ".bmp", ".tiff" });

			    string extension = Path.GetExtension(fileName);
				if (imgExtension.Contains(extension))
			    {
				    
				    var path = Path.Combine(Server.MapPath("~/Content/images/profil"), userId.ToString() + fileName);

				    if (!System.IO.File.Exists(fileName))
				    {
					    pic.SaveAs(path);
				    }

				    string profilPicUrl = "~/Content/images/profil/" + userId.ToString() + Path.GetFileName(pic.FileName);
				    Session["UserPic"] = profilPicUrl;

				    KoombuBll kBll = new KoombuBll();
				    kBll.AddProfilPic(userId, profilPicUrl);
				}
				
					


		    }
		    return RedirectToAction("Profil", new { id = userId });

	    }
	}
}