using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koombu.Dal
{
	public class KoombuContext : DbContext
	{
		public KoombuContext() : base("ApplicationConnection")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PostLike>()
				.HasRequired(c => c.User)
				.WithMany()
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserFollow>()
				.HasRequired(c => c.User)
				.WithMany()
				.WillCascadeOnDelete(false);


		}

		public DbSet<User> UserSet { get; set; }
		public DbSet<Post> PostSet { get; set; }
		public DbSet<Group> GroupSet { get; set; }
		public DbSet<Comment> CommentSet { get; set; }
		public DbSet<PostLike> PostLikeSet { set; get; }
		public DbSet<UserFollow> UserFollowsSet { set; get; }
		public DbSet<GroupMembers> GroupMembersSet { set; get; }
	}
}