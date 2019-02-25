using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class GroupPostVm
	{
		public Group Group { get; set; }
		public PostListVm[] PostList { get; set; }
		public User UserConnected { get; set; }
		public GroupMembers[] GroupMembersObjects { get; set; }
	}
}
