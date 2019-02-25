using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class PostListVm
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public string PictureUrl { get; set; }
		public string AttachementUrl { get; set; }
		public string AttachementName { get; set; }
		public DateTime PostDate { get; set; }
		public int PostOwnerId { get; set; }
		public User PostOwner { get; set; }
		public CommentListVm[] CommentsList { get; set; }
		public ICollection<User> UsersLikes { get; set; }
		public int GroupOwnerId { get; set; }
		public int Likes { get; set; }

	}
}
