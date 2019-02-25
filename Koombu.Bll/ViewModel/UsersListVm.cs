using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class UsersListVm
	{
		

		public UsersListVm()
		{
		}

		public int UserId { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Department { get; set; }
		public string Title { get; set; }
		public string ProfilPicture { get; set; }
		public string EmailAdress { get; set; }
		public DateTime DateOfBirth { get; set; }

		public ICollection<User> FollowedUsersList { get; set; }
	}
}
