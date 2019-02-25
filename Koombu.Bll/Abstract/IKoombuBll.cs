using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Bll.ViewModel;
using Koombu.Dal;

namespace Koombu.Bll.Abstract
{
	interface IKoombuBll
	{
		User RegisterUser(string firstName, string lastName, string email, DateTime birthDate, string password, string department, string title);
		Boolean CheckEmail(string email);
		User SignIn(string userEmail, string userPassword);
		User GetUser(int userId);
		Group GetGroup(int groupId);
		void CreatePost(string content, int postUserId, int groupOwnerId, string postImgtUrl, string postAttachementUrl, string attachementName);

		PostListVm[] GetAllPostForUser(int userId);

		UsersListVm[] GetAllUsers();

		PostListVm[] SetPostUserObject(PostListVm[] postlistVm);

		void RemovePost(int postId);

		void LikePost(int postId, int userId);
		void UnlikePost(int postId, int userId);

		int GetLikesNumber(int postId);

		void FollowUser(int userId, int userIdToFollowId);
		void UnFollowUser(int userId, int userIdToUnFollowId);
		UserFollow[] GetFollowedUserList(int userId);
		IList<int> GetFollowedUsersId(int userId);

		ICollection<UsersListVm> GetFollowedUsersObject(int userId);
		
		PostListVm[] SearchPost(string postString, int userId,int groupId);
		UsersListVm[] SearchUser(string userName);

		void AddProfilPic(int userId, string urlPicture);

		void CommentPost(string content, int postId, int userId);

		CommentListVm[] GetAllCommentForPost(int postId);
		GroupVm[] GetAllGroupsForUser(int userId);
		PostListVm[] GetAllPostForGroup(int groupId);

		void RemoveComment(int commentId);

		void CreateGroup(int groupOwnerId, string groupName, string groupDescription, string groupImgtUrl);
		void DeleteGroup(int groupId);

		void AddUserToGroup(int userToAddId, int groupId);
		void RemoveUserFromGroup(int userToRemoveId, int groupId);

		GroupMembers[] GroupMembersObjects(int groupId);
	}
}
