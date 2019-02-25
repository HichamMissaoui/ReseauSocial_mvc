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
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

	    public ActionResult Groups()
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
		    KoombuBll kBll = new KoombuBll();
		    GroupListVm groupListVm = new GroupListVm();
		    groupListVm.GroupList = kBll.GetAllGroupsForUser(userId);
		    groupListVm.FollowedUsersObjects = kBll.GetFollowedUsersObject(userId);
		    groupListVm.UserConnected = kBll.GetUser(userId);
		    return View(groupListVm);
	    }

	    public ActionResult CreateGroup()
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }


			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    var followedUser = kBll.GetFollowedUsersObject(userId);
		    return View(followedUser);
	    }

	    public ActionResult MakeGroup(string groupName, string groupDescription, HttpPostedFileBase groupPicture)
	    {
		    if (Session["UserName"] == null) {return RedirectToAction("Error");}

			int groupOwnerId = Int32.Parse(Session["UserId"].ToString());

			string groupImgUrl = null;


		    if (groupPicture != null)
		    {
			    var fileName = Path.GetFileName(groupPicture.FileName);
			    List<string> imgExtension =
				    new List<string>(new string[] {".gif", ".png", ".PNG", ".jpg", ".jpeg", ".bmp", ".tiff"});

			    string extension = Path.GetExtension(fileName);
			    if (imgExtension.Contains(extension))
			    {

				    var path = Path.Combine(Server.MapPath("~/Content/images/group"), groupOwnerId.ToString() + fileName);

				    if (!System.IO.File.Exists(fileName))
				    {
					    groupPicture.SaveAs(path);
				    }

				    groupImgUrl = "~/Content/images/group/" + groupOwnerId.ToString() +
				                          Path.GetFileName(groupPicture.FileName);

			    }
		    }

		    KoombuBll kBll = new KoombuBll();
		    kBll.CreateGroup(groupOwnerId, groupName, groupDescription, groupImgUrl);

		    return RedirectToAction("Groups");
	    }

	    public ActionResult GroupWall(int groupId)
	    {
			if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();

			GroupPostVm groupPostVm = new GroupPostVm();
		    groupPostVm.PostList = kBll.GetAllPostForGroup(groupId);
		    groupPostVm.Group = kBll.GetGroup(groupId);
		    groupPostVm.UserConnected = kBll.GetUser(userId);
		    groupPostVm.GroupMembersObjects= kBll.GroupMembersObjects(groupId);
		    return View(groupPostVm);
	    }

	    public ActionResult DeleteGroup(int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    kBll.DeleteGroup(groupId);
		    return RedirectToAction("Groups");
	    }

	    public ActionResult EditGroupUsers(int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    var group = kBll.GetGroup(groupId);
		    GroupUsersVm groupUsersVm = new GroupUsersVm();

		    if (group.GroupOwnerId == userId)
		    {
			    groupUsersVm.AllUsers = kBll.GetAllUsers();
			    groupUsersVm.Group = group;
			    groupUsersVm.GroupMembersObjects = kBll.GroupMembersObjects(groupId);
			    groupUsersVm.GroupMembersId = kBll.GetGroupMembersId(groupUsersVm.GroupMembersObjects);
		    }
		    else
		    {
			    return RedirectToAction("Groups");
		    }


		    return View(groupUsersVm);
	    }

	    public ActionResult AddUser(int groupId, int userId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			KoombuBll kBll = new KoombuBll();
		    kBll.AddUserToGroup(userId, groupId);
		    return RedirectToAction("EditGroupUsers", new { groupId = groupId });
	    }

	    public ActionResult DeleteUser(int groupId, int userId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

			KoombuBll kBll = new KoombuBll();
		    kBll.RemoveUserFromGroup(userId, groupId);
		    return RedirectToAction("EditGroupUsers", new { groupId = groupId });
	    }

	    public ActionResult GroupMembers(int groupId)
	    {
		    if (Session["UserName"] == null) { return RedirectToAction("Error"); }

		    int userId = Int32.Parse(Session["UserId"].ToString());
		    KoombuBll kBll = new KoombuBll();
		    var group = kBll.GetGroup(groupId);
		    GroupUsersVm groupUsersVm = new GroupUsersVm();

			    groupUsersVm.Group = group;
			    groupUsersVm.GroupMembersObjects = kBll.GroupMembersObjects(groupId);
			    groupUsersVm.GroupMembersId = kBll.GetGroupMembersId(groupUsersVm.GroupMembersObjects);



		    return View(groupUsersVm);
	    }

	    public ActionResult SearchPostGroup(string postString, int groupId)
	    {
		    int userConnectedId = Int32.Parse(Session["UserId"].ToString());
		    KoombuBll kBll = new KoombuBll();
		    var postList = kBll.SearchPost(postString, 0, groupId);
			GroupPostVm groupPostVm = new GroupPostVm
		    {
			    PostList = kBll.SetPostUserObject(postList),
			    GroupMembersObjects = kBll.GroupMembersObjects(groupId),
			    Group = kBll.GetGroup(groupId),
				UserConnected = kBll.GetUser(userConnectedId)
				
		    };

		    return View(groupPostVm);
	    }

	    public ActionResult RemoveSelf(int groupId)
	    {
		    int userId = Int32.Parse(Session["UserId"].ToString());
			KoombuBll kBll = new KoombuBll();
		    kBll.RemoveUserFromGroup(userId, groupId);
		    return RedirectToAction("Groups");
	    }
	}
}