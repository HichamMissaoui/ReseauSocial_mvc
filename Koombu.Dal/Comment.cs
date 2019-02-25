using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class Comment
	{
		[Key]
		public int Id { get; set; }
		[StringLength(500)]
		public string Content { get; set; }
		public DateTime Date { get; set; }


		public int OwnerId { get; set; }
		public virtual User User { get; set; }

		public int PostId { get; set; }
		public virtual Post Post { get; set; }


	}
}
