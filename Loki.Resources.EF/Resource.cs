using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loki.Resources.EF
{
	/// <summary>
	/// Defines terminal authentication configuration.
	/// </summary>
	[Table("loki.Resource")]
	public partial class Resource
	{
		/// <summary>
		/// Gets or sets the culture LCID.
		/// </summary>
		[Key, Column(Order = 0)]
		public int Culture { get; set; }

		/// <summary>
		/// Gets or sets the identifier of this resource record.
		/// </summary>
		[Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the resource set this resource belongs to.
		/// </summary>
		[Required, StringLength(256)]
		public string Set { get; set; }

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		[Required]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public string Value { get; set; }
	}
}
