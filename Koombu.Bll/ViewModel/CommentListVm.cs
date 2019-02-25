using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Bll.Concrete;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class CommentListVm
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime Date { get; set; }
		public int OwnerId { get; set; }
		public User Owner { get; set; }
		public int PostId { get; set; }


	}
}
