using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class PostLike
	{

		[Key, Column(Order = 0), ForeignKey("Post")]
		public int PostId { get; set; }
		[Key, Column(Order = 1), ForeignKey("User")]
		public int UserId { get; set; }

		public virtual Post Post { get; set; }
		public virtual User User { get; set; }
	}
}
