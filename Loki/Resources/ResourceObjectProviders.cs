namespace Loki.Resources
{
	/// <summary>
	/// Allows to customize the way the locale dependant string resources are retrieved.
	/// </summary>
	public static class ResourceObjectProviders
	{
		private static ResourceObjectProviderBase _current;

		static ResourceObjectProviders()
		{
			_current = new ResourceObjectProvider();
		}

		/// <summary>
		/// Gets or sets the current resource provider.
		/// </summary>
		public static ResourceObjectProviderBase Current
		{
			get { return _current; }
			set { _current = value ?? new ResourceObjectProvider(); }
		}
	}
}