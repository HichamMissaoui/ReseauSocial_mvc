using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Koombu.Bll.Abstract;
using Koombu.Bll.ViewModel;
using Koombu.Dal;

namespace Koombu.Bll.Concrete
{
	public class KoombuBll : IKoombuBll
	{
		public bool CheckEmail(string email)
		{
			bool exist = false;
			using (var ctx = new KoombuContext())
			{
				var existEmail = ctx.UserSet.Count(a => a.EmailAdress == email);
				if (existEmail != 0)
				{
					exist = true;
				}

				return exist;

			}

		}

		public void CreatePost(string content, int postUserId, int groupOwnerId, string postImgtUrl,
			string postAttachementUrl, string attachementName)
		{
			
			Post post = new Post
			{
				Content = content,
				OwnerId = postUserId,
				GroupId = groupOwnerId,
				PictureUrl = postImgtUrl,
				AttachementUrl = postAttachementUrl,
				AttachementName = attachementName,
				PostDate = DateTime.Now
			};

			using (var ctx = new KoombuContext())
			{
				ctx.PostSet.Add(post);
				ctx.SaveChanges();
			}
		}

		public PostListVm[] GetAllPostForUser(int userId)
		{
			PostListVm[] postList;
			IList<int> l = GetFollowedUsersId(userId);
			l.Add(userId);
			using (var ctx = new KoombuContext())
			{
				postList = ctx.PostSet.AsNoTracking()
					.Where(o => l.Contains(o.OwnerId) && o.GroupId==0)
					.Select(o => new PostListVm()
					{
						Id = o.PostId,
						Content = o.Content,
						PostOwnerId = o.OwnerId,
						GroupOwnerId = o.GroupId,
						PictureUrl = o.PictureUrl,
						AttachementUrl = o.AttachementUrl,
						AttachementName = o.AttachementName,
						PostDate = o.PostDate
					}).ToArray();


			}

			foreach (var post in postList)
			{
				post.Likes = GetLikesNumber(post.Id);
				post.CommentsList = GetAllCommentForPost(post.Id);
			}

			return postList;
		}

		public UsersListVm[] GetAllUsers()
		{
			using (var ctx = new KoombuContext())
			{
				return ctx.UserSet.AsNoTracking()
					.Select(o => new UsersListVm()
					{
						UserId = o.UserId,
						FirstName = o.FirstName,
						LastName = o.LastName,
						ProfilPicture = o.ProfilPicture,
						EmailAdress = o.EmailAdress,
						Department = o.Department,
						Title = o.Title,
						DateOfBirth = o.DateOfBirth

					}).ToArray();
			}
		}

		public Group GetGroup(int groupId)
		{
			using (var ctx = new KoombuContext())
			{
				var group = ctx.GroupSet.AsNoTracking().SingleOrDefault(o => o.Id == groupId);

				return group;
			}
		}

		public User GetUser(int userId)
		{
			using (var ctx = new KoombuContext())
			{
				var user = ctx.UserSet.AsNoTracking().SingleOrDefault(o => o.UserId == userId);

				return user;
			}
		}



		public User RegisterUser(string firstName, string lastName, string email, DateTime birthDate, string password,
			string department, string title)
		{
			User user = new User
			{
				FirstName = firstName,
				LastName = lastName,
				EmailAdress = email,
				Password = password,
				Department = department,
				Title = title,
				DateOfBirth = birthDate
			};

			using (var ctx = new KoombuContext())
			{
				ctx.UserSet.Add(user);

				ctx.SaveChanges();
			}

			return user;
		}

		public void RemovePost(int postId)
		{
			using (var ctx = new KoombuContext())
			{
				var x = (from y in ctx.PostSet
					where y.PostId == postId
					select y).FirstOrDefault();
				if (x != null)
				{
					ctx.PostSet.Remove(x);
					ctx.SaveChanges();
				}
			}
		}

		public PostListVm[] SetPostUserObject(PostListVm[] postlistVm)
		{
			foreach (var post in postlistVm)
			{
				post.PostOwner = GetUser(post.PostOwnerId);
			}

			return postlistVm;
		}

		public User SignIn(string userEmail, string userPassword)
		{

			using (var ctx = new KoombuContext())
			{
				var user = ctx.UserSet.AsNoTracking()
					.SingleOrDefault(o => o.EmailAdress == userEmail && o.Password == userPassword);

				return user;
			}
		}

		public void LikePost(int postId, int userId)
		{
			PostLike postLike = new PostLike();
			postLike.UserId = userId;
			postLike.PostId = postId;

			using (var ctx = new KoombuContext())
			{
				var exist = ctx.PostLikeSet.AsNoTracking().SingleOrDefault(o => o.UserId == userId && o.PostId == postId);
				if (exist == null)
				{
					ctx.PostLikeSet.Add(postLike);
					ctx.SaveChanges();
				}

			}
		}

		public void UnlikePost(int postId, int userId)
		{

			using (var ctx = new KoombuContext())
			{
				var like = (from y in ctx.PostLikeSet
					where y.PostId == postId && y.UserId == userId
					select y).FirstOrDefault();

				if (like != null)
				{
					ctx.PostLikeSet.Remove(like);
					ctx.SaveChanges();
				}

			}
		}

		public int GetLikesNumber(int postId)
		{
			PostLike[] pL;
			using (var ctx = new KoombuContext())
			{
				pL = ctx.PostLikeSet.AsNoTracking()
					.Where(o => o.PostId == postId)
					.ToArray();


			}

			return pL.Length;

		}

		public void FollowUser(int userId, int userIdToFollowId)
		{
			UserFollow uF = new UserFollow();
			uF.UserId = userId;
			uF.FollowedUserId = userIdToFollowId;
			using (var ctx = new KoombuContext())
			{
				var exist = ctx.UserFollowsSet.AsNoTracking()
					.SingleOrDefault(o => o.UserId == userId && o.FollowedUserId == userIdToFollowId);
				if (exist == null)
				{
					ctx.UserFollowsSet.Add(uF);
					ctx.SaveChanges();
				}

			}
		}

		public void UnFollowUser(int userId, int userIdToUnFollowId)
		{
			using (var ctx = new KoombuContext())
			{
				var user = (from y in ctx.UserFollowsSet
					where y.UserId == userId && y.FollowedUserId == userIdToUnFollowId
					select y).FirstOrDefault();

				if (user != null)
				{
					ctx.UserFollowsSet.Remove(user);
					ctx.SaveChanges();
				}

			}
		}

		public UserFollow[] GetFollowedUserList(int userId)
		{
			using (var ctx = new KoombuContext())
			{
				return ctx.UserFollowsSet.AsNoTracking()
					.Where(o => o.UserId == userId)
					.ToArray();
			}
		}

		public IList<int> GetFollowedUsersId(int userId)
		{
			IList<int> usersFollowId = new List<int>();
			var usersList = GetFollowedUserList(userId);
			foreach (var u in usersList)
			{
				usersFollowId.Add(u.FollowedUserId);
			}

			return usersFollowId;
		}

		public PostListVm[] SearchPost(string postString, int userId, int groupId)
		{
			ICollection<PostListVm> list = new List<PostListVm>();
			string postContent = "";

			postString = postString.Replace(" ", String.Empty);
			postString = postString.ToLower();
			if (groupId == 0)
			{
				var allUserPosts = GetAllPostForUser(userId);

				foreach (var post in allUserPosts)
				{
					postContent = post.Content.Replace(" ", String.Empty);
					if (postContent.ToLower().Contains(postString))
					{
						list.Add(post);
					}

				}
			}
			else
			{
				var allGroupPosts = GetAllPostForGroup(groupId);

				foreach (var post in allGroupPosts)
				{
					postContent = post.Content.Replace(" ", String.Empty);
					if (postContent.ToLower().Contains(postString))
					{
						list.Add(post);
					}

				}
			}
			

			return list.ToArray();
		}

		public UsersListVm[] SearchUser(string userName)
		{
			ICollection<UsersListVm> list = new List<UsersListVm>();
			var allUsers = GetAllUsers();
			string fullname = "";
			foreach (var user in allUsers)
			{
				fullname = (user.FirstName + user.LastName).Replace(" ", String.Empty);
				if (fullname.ToLower().Contains(userName.Replace(" ", String.Empty).ToLower()))
				{
					list.Add(user);
				}
			}

			return list.ToArray();
		}

		public ICollection<UsersListVm> GetFollowedUsersObject(int userId)
		{
			ICollection<UsersListVm> usersList = new List<UsersListVm>();
			var usersIdList = GetFollowedUsersId(userId);
			foreach (var u in usersIdList)
			{
				var user = GetUser(u);
				usersList.Add(new UsersListVm()
				{
					UserId = user.UserId,
					FirstName = user.FirstName,
					LastName = user.LastName,
					ProfilPicture = user.ProfilPicture,
					DateOfBirth = user.DateOfBirth,
					Title = user.Title,
					EmailAdress = user.EmailAdress,
					Department = user.Department
				});
			}

			return usersList;
		}

		public void AddProfilPic(int userId, string urlPicture)
		{
			using (var ctx = new KoombuContext())
			{
				(from x in ctx.UserSet
					where x.UserId == userId
					select x).ToList().ForEach(xx => xx.ProfilPicture = urlPicture);
				ctx.SaveChanges();
			}

		}

		public void CommentPost(string content, int postId, int userId)
		{
			Comment comment = new Comment();
			comment.Content = content;
			comment.OwnerId = userId;
			comment.PostId = postId;
			comment.Date = DateTime.Now;

			using (var ctx = new KoombuContext())
			{
				ctx.CommentSet.Add(comment);

				ctx.SaveChanges();
			}

		}

		public CommentListVm[] SetUsersToComments(CommentListVm[] commentList)
		{
			foreach (var comment in commentList)
			{
				comment.Owner = GetUser(comment.OwnerId);
			}
			return commentList;

		}

		public CommentListVm[] GetAllCommentForPost(int postId)
		{
			CommentListVm[] list;
			using (var ctx = new KoombuContext())
			{
				list = ctx.CommentSet.AsNoTracking()
					.Where(o => o.PostId == postId)
					.Select(o => new CommentListVm()
					{
						Id = o.Id,
						Content = o.Content,
						OwnerId = o.OwnerId,
						PostId = o.PostId,
						Date = o.Date,
					}).ToArray();
			}

			var commentList = SetUsersToComments(list);

			return commentList;
		}

		public void RemoveComment(int commentId)
		{
			using (var ctx = new KoombuContext())
			{
				var comment = (from y in ctx.CommentSet
					where y.Id == commentId
					select y).FirstOrDefault();

				if (comment != null)
				{
					ctx.CommentSet.Remove(comment);
					ctx.SaveChanges();
				}

			}
		}

		public void CreateGroup(int groupOwnerId, string groupName, string groupDescription, string groupImgtUrl)
		{
			Group group = new Group();
			group.GroupOwnerId = groupOwnerId;
			group.GroupName = groupName;
			group.GroupDescription = groupDescription;
			group.GroupPicture = groupImgtUrl;

			using (var ctx = new KoombuContext())
			{
				ctx.GroupSet.Add(group);

				ctx.SaveChanges();
			}
		}

		public GroupVm[] GetAllGroupsForUser(int userId)
		{
			ICollection<GroupVm> groupList=new List<GroupVm>();
			ICollection<int> groupIdList = new List<int>();
			using (var ctx = new KoombuContext())
			{
				groupList= ctx.GroupSet.AsNoTracking()
					.Where(o => o.GroupOwnerId == userId)
					.Select(o => new GroupVm()
					{
						Id = o.Id,
						GroupName = o.GroupName,
						GroupDescription = o.GroupDescription,
						GroupPicture = o.GroupPicture,
						GroupOwnerId = o.GroupOwnerId

					}).ToList();

				groupIdList = ctx.GroupMembersSet.AsNoTracking()
					.Where(g => g.UserId == userId)
					.Select(g => g.GroupId)
					.ToList();
			}
			

			foreach (var groupId in groupIdList)
			{
				var group = GetGroup(groupId);
				groupList.Add(new GroupVm()
				{
					Id = group.Id,
					GroupOwnerId = group.GroupOwnerId,
					GroupDescription = group.GroupDescription,
					GroupName = group.GroupName,
					GroupPicture = group.GroupPicture,
				});
			}

			foreach (var group in groupList)
			{
				group.Owner = GetUser(group.GroupOwnerId);
			}
			return groupList.ToArray();
		}

		public PostListVm[] GetAllPostForGroup(int groupId)
		{
			PostListVm[] postList;

			using (var ctx = new KoombuContext())
			{
				postList = ctx.PostSet.AsNoTracking()
					.Where(o => o.GroupId == groupId)
					.Select(o => new PostListVm()
					{
						Id = o.PostId,
						Content = o.Content,
						PostOwnerId = o.OwnerId,
						GroupOwnerId= o.GroupId,
						PictureUrl = o.PictureUrl,
						AttachementUrl = o.AttachementUrl,
						AttachementName = o.AttachementName,
						PostDate = o.PostDate
					}).ToArray();
			}

			foreach (var post in postList)
			{
				post.Likes = GetLikesNumber(post.Id);
				post.CommentsList = GetAllCommentForPost(post.Id);
				post.PostOwner = GetUser(post.PostOwnerId);
			}

			return postList;
		}

		public void DeleteGroup(int groupId)
		{
			using (var ctx = new KoombuContext())
			{
				var x = (from g in ctx.GroupSet
					where g.Id == groupId
					select g).FirstOrDefault();

				var groupList = (from gm in ctx.GroupMembersSet
					where gm.GroupId == groupId
					select gm).ToList();
				if (x != null)
				{
					ctx.GroupSet.Remove(x);
					ctx.SaveChanges();
				}
				if (groupList != null)
				{
					foreach (var y in groupList)
					{
						ctx.GroupMembersSet.Remove(y);
					}
					
					ctx.SaveChanges();
				}
			}
		}

		public void AddUserToGroup(int userToAddId, int groupId)
		{
			GroupMembers groupMember = new GroupMembers();
			User user = GetUser(userToAddId);
			groupMember.GroupId = groupId;
			groupMember.UserId = userToAddId;
			groupMember.UserFirstName = user.FirstName;
			groupMember.UserLastName = user.LastName;
			groupMember.UserPicture = user.ProfilPicture;
			using (var ctx = new KoombuContext())
			{
				ctx.GroupMembersSet.Add(groupMember);

				ctx.SaveChanges();
			}
		}

		public void RemoveUserFromGroup(int userToRemoveId, int groupId)
		{
			using (var ctx = new KoombuContext())
			{
				var x = (from y in ctx.GroupMembersSet
					where y.GroupId == groupId && y.UserId == userToRemoveId
					select y).FirstOrDefault();
				if (x != null)
				{
					ctx.GroupMembersSet.Remove(x);
					ctx.SaveChanges();
				}
			}
		}

		public GroupMembers[] GroupMembersObjects(int groupId)
		{


			using (var ctx = new KoombuContext())
			{
				var groupMembers = ctx.GroupMembersSet.AsNoTracking()
					.Where(o => o.GroupId == groupId)
					.ToArray();

				return groupMembers;
			}

			
		}

		public ICollection<int> GetGroupMembersId(GroupMembers[] groupMembers)
		{
			ICollection<int> groupMembersId = new List<int>();
			foreach (var member in groupMembers)
			{
				groupMembersId.Add(member.UserId);
			}

			return groupMembersId;
		}


		}

	}


