using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class UsersVm
	{
		public ICollection<UsersListVm> AllUsers { get; set; }
		public ICollection<UserFollow> FollowedUsers { get; set; }
		public ICollection<UsersListVm> FollowedUsersObjects { get; set; }
	}
}
