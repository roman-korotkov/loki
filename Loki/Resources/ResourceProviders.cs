namespace Loki.Resources
{
	/// <summary>
	/// Allows to customize the way the locale dependant string resources are retrieved.
	/// </summary>
	public static class ResourceProviders
	{
		private static ResourceProvider _current;
		private static ResourceEditor _editor;

		static ResourceProviders()
		{
			_current = EmptyResourceProvider.Instance;
			_editor = null;
		}

		/// <summary>
		/// Gets or sets the current resource provider.
		/// </summary>
		public static ResourceProvider Current
		{
			get { return _current; }
			set { _current = value ?? EmptyResourceProvider.Instance; }
		}

		/// <summary>
		/// Gets or sets the current resource provider.
		/// </summary>
		public static ResourceEditor Editor
		{
			get { return _editor ?? Current as ResourceEditor; }
			set { _editor = value; }
		}
	}
}