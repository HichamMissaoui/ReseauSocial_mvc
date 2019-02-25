using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class UserFollow
	{
		[Key]
		public int Id { get; set; }

		[ Column(Order = 0), ForeignKey("User")]
		public int UserId { get; set; }
		[Column(Order = 1), ForeignKey("UserFollowed")]
		public int FollowedUserId { get; set; }

		public virtual User User { get; set; }
		public virtual User UserFollowed { get; set; }
	}
}
