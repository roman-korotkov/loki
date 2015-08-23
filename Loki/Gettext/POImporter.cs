using System.Globalization;
using System.Linq;
using Loki.Gettext.PortableObject;
using Loki.Resources;

namespace Loki.Gettext
{
	/// <summary>
	/// Imports the portable object file.
	/// </summary>
	public class POImporter
	{
		/// <summary>
		/// Gets or sets the culture.
		/// </summary>
		public CultureInfo Culture { get; set; }

		/// <summary>
		/// Imports the portable object file and updates resources according to it.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="editor">The editor.</param>
		public virtual void Import(File file, ResourceEditor editor)
		{
			var culture = Culture ?? CultureInfo.GetCultureInfo(file.Language);

			var resourceGroups = file.Entries.Select(x => new
			{
				Set = x.Context.Substring(0, x.Context.LastIndexOf('.')),
				Key = x.Context.Substring(x.Context.LastIndexOf('.') + 1),
				Value = x.TranslatedText
			}).GroupBy(x => x.Set);

			foreach (var group in resourceGroups)
			{
				var collection = editor.GetResourceCollection(culture, group.Key);
				foreach (var resource in group)
				{
					collection.Set(resource.Key, resource.Value);
				}
			}

			CachedResourceObjectProvider.RemoveAllCachedObjects();
		}
	}
}