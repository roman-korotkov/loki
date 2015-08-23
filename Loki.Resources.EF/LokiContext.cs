using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Represents a string resource data context.
	/// </summary>
	internal class LokiContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LokiContext"/> class.
		/// </summary>
		public LokiContext()
			: base("name=LokiContext")
		{
		}

		/// <summary>
		/// Gets or sets the resources.
		/// </summary>
		public virtual DbSet<Resource> Resources { get; set; }
	}
}
