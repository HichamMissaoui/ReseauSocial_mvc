using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koombu.Dal;

namespace Koombu.Bll.ViewModel
{
	public class GroupVm
	{

		public int Id { get; set; }
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public string GroupPicture { get; set; }

		public int GroupOwnerId { get; set; }
		public User Owner { get; set; }
	}
}
