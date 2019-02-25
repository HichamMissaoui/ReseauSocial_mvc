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
    public class PostController : Controller
    {
        
      
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

				    KoombuBll kBll = new KoombuBll();
				    kBll.AddProfilPic(userId, profilPicUrl);
			    }




		    }
		    return RedirectToAction("Profil", new { id = userId });

	    }


		public ActionResult CreatePost(int groupId, string content, HttpPostedFileBase picture, HttpPostedFileBase attachement)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			if (content == "" && picture == null && attachement == null)
		    {
				return RedirectToAction("Home", "Home");
			}
			else
		    {
				int postOwnerId = Int32.Parse(Session["UserId"].ToString());
				string postAttachementUrl = null;
				string pictureUrl = null;
				string attachementName = null;

				if (picture != null && picture.ContentLength > 0)
				{
					var fileName = Path.GetFileName(picture.FileName);
					List<string> imgExtension =
						new List<string>(new string[] { ".gif", ".png", ".PNG", ".jpg", ".jpeg", ".bmp", ".tiff" });

					string extension = Path.GetExtension(fileName);
					if (imgExtension.Contains(extension))
					{

						var path = Path.Combine(Server.MapPath("~/Content/post/images"),
							postOwnerId.ToString() + DateTime.Now.ToString("yyyyMMdd") + fileName);

						if (!System.IO.File.Exists(fileName))
						{
							picture.SaveAs(path);
						}

						pictureUrl = "~/Content/post/images/" + postOwnerId.ToString() + DateTime.Now.ToString("yyyyMMdd") +
									 Path.GetFileName(picture.FileName);
					}
				}

				if (attachement != null && attachement.ContentLength > 0)
				{
					attachementName = Path.GetFileName(attachement.FileName);
					List<string> filesExtension =
						new List<string>(new string[] { ".txt", ".pdf", ".ppt", ".docx" });

					string extension = Path.GetExtension(attachementName);
					if (filesExtension.Contains(extension))
					{

						var path = Path.Combine(Server.MapPath("~/Content/post/attachement"),
							postOwnerId.ToString() + DateTime.Now.ToString("yyyyMMdd") + attachementName);

						if (!System.IO.File.Exists(attachementName))
						{
							attachement.SaveAs(path);
						}

						postAttachementUrl = "~/Content/post/attachement/" + postOwnerId.ToString() + DateTime.Now.ToString("yyyyMMdd") +
									 Path.GetFileName(attachement.FileName);
					}
				}

				KoombuBll kBll = new KoombuBll();
				kBll.CreatePost(content, postOwnerId, groupId, pictureUrl, postAttachementUrl, attachementName);
				if (groupId != 0)
				{
					return RedirectToAction("GroupWall", "Group", new { groupId = groupId });
				}
			}
		    
		    return RedirectToAction("Home", "Home");
	    }

	    public ActionResult RemovePost(int postOwnerId, int postId, int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userConnectedId  = Int32.Parse(Session["UserId"].ToString());
			if (postOwnerId == userConnectedId)
		    {
			    KoombuBll kBll = new KoombuBll();
			    kBll.RemovePost(postId);
		    }
		    if (groupId != 0)
		    {
			    return RedirectToAction("GroupWall", "Group", new { groupId = groupId });
		    }
			return RedirectToAction("Home", "Home");
	    }

	    public ActionResult Like(int id,int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    kBll.LikePost(id, userId);
		    if (groupId != 0)
		    {
			    return RedirectToAction("GroupWall", "Group", new { groupId = groupId });
		    }
			return RedirectToAction("Home", "Home");
	    }

	    public ActionResult UnLike(int id,int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
		    KoombuBll kBll = new KoombuBll();
		    kBll.UnlikePost(id, userId);
		    if (groupId != 0)
		    {
			    return RedirectToAction("GroupWall", "Group", new { groupId = groupId });
		    }
			return RedirectToAction("Home", "Home");
	    }

	    public ActionResult SearchPostWall(string postString)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			if (postString == null)
		    {
			    return RedirectToAction("Home", "Home");
		    }
		    int userConnectedId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    HomeVm homeVm = new HomeVm();
		    var postList = kBll.SearchPost(postString, userConnectedId,0);
		    homeVm.UserPostList = kBll.SetPostUserObject(postList);
			homeVm.FollowedUsers = kBll.GetFollowedUserList(userConnectedId);
		    homeVm.UserConnected = kBll.GetUser(userConnectedId);
		    homeVm.FollowedUsersObjects = kBll.GetFollowedUsersObject(userConnectedId);

		    return View(homeVm);
	    }

	    


		public ActionResult Comment(string commentContent, int postId,int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    kBll.CommentPost(commentContent, postId, userId);
		    if (groupId != 0)
		    {
			    return RedirectToAction("GroupWall", "Group", new { groupId = groupId });
		    }
			return RedirectToAction("Home", "Home");
	    }

	    public ActionResult RemoveComment(int commentId,int groupId)
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			KoombuBll kBll = new KoombuBll();
			kBll.RemoveComment(commentId);
			if (groupId != 0)
			{
				return RedirectToAction("GroupWall", "Group", new { groupId = groupId });
			}

			return RedirectToAction("Home", "Home");
	    }
	}
}