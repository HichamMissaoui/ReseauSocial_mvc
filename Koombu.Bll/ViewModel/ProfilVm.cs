using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class ProfilVm
	{
		public User User { get; set; }
		public ICollection<UsersListVm> FollowedUsersObjects { get; set; }
	}
}
