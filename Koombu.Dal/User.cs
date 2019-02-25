using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class User
	{
		[Key]
		public int UserId { get; set; }

		[Required]
		[MinLength(2), MaxLength(50)]
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Department { get; set; }
		public string Title { get; set; }
		public string ProfilPicture { get; set; }

		[MinLength(4), MaxLength(20)]
		public string Password { get; set; }

		public string EmailAdress { get; set; }
		public DateTime DateOfBirth { get; set; }

		public virtual ICollection<Post> Posts { get; set; }

		public virtual ICollection<User> FollowedUsersList { get; set; }

	}
}
