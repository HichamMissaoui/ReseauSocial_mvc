using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class GroupUsersVm
	{
		public Group Group { get; set; }
		public ICollection<UsersListVm> AllUsers { get; set; }
		public GroupMembers[] GroupMembersObjects { get; set; }
		public ICollection<int> GroupMembersId{ get; set; }
		public ICollection<UsersListVm> FollowedUsersObjects { get; set; }
	}
}
