using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class GroupMembers
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int GroupId { get; set; }
		public int UserId { get; set; }
		public string UserFirstName { get; set; }
		public string UserLastName { get; set; }
		public string UserPicture { get; set; }
	}
}
