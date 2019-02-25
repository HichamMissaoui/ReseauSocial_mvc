using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class Post
	{
		[Key]
		public int PostId { get; set; }
		[StringLength(500)]
		public string Content { get; set; }
		[StringLength(200)]
		public string PictureUrl { get; set; }
		public string AttachementUrl { get; set; }
		public string AttachementName { get; set; }
		public DateTime PostDate { get; set; }

		public  ICollection<Comment> CommentsList { get; set; }


		public int OwnerId { get; set; }
		public virtual User User { get; set; }

		public int GroupId { get; set; }

		


	}
}
