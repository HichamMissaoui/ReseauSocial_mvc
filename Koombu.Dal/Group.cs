using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class Group
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string GroupName { get; set; }
		[StringLength(200)]
		public string GroupDescription { get; set; }
		public string GroupPicture { get; set; }
	
		public int GroupOwnerId { get; set; }

	}
}
