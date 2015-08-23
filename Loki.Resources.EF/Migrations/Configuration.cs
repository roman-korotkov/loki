using System.Data.Entity.Migrations;

namespace Loki.Resources.EF.Migrations
{
	/// <summary>
	/// Defines migration configuration.
	/// </summary>
	internal sealed class Configuration : DbMigrationsConfiguration<LokiContext>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Configuration" /> class.
		/// </summary>
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		/// <summary>
		/// Seeds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void Seed(LokiContext context)
		{
		}
	}
}
