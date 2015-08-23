using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Loki.Resources.EF
{
	internal class ChangeQueue : IDisposable
	{
		private readonly List<Change> _queue;

		private readonly object _syncRoot;
		private readonly AutoResetEvent _event;
		private readonly Thread _worker;

		public ChangeQueue()
		{
			_queue = new List<Change>();

			_syncRoot = new object();
			_event = new AutoResetEvent(false);
			_worker = new Thread(Cycle);
			_worker.Start();
		}

		public void Dispose()
		{
			_worker.Abort();
		}

		public void Enqueue(Change change)
		{
			lock (_syncRoot)
			{
				_queue.Add(change);
				_event.Set();
			}
		}

		private void Cycle()
		{
			while (true)
			{
				Change[] queue;

				_event.WaitOne();

				lock (_syncRoot)
				{
					queue = _queue.ToArray();

					_queue.Clear();
				}

				using (var storage = new LokiContext())
				{
					foreach (var change in queue)
					{
						change.Execute(storage);
					}
				}
			}
		}
	}

	internal abstract class Change
	{
		public abstract void Execute(LokiContext storage);
	}

	internal class SetValue : Change
	{
		private readonly int _culture;
		private readonly string _set;
		private readonly string _key;
		private readonly string _value;

		public SetValue(int culture, string set, string key, string value)
		{
			_culture = culture;
			_set = set;
			_key = key;
			_value = value;
		}

		public override void Execute(LokiContext storage)
		{
			var resource = storage.Resources.FirstOrDefault(x => x.Culture == _culture && x.Set == _set && x.Key == _key);
			if (resource != null)
			{
				resource.Value = _value;
			}
			else
			{
				resource = new Resource {Culture = _culture, Key = _key, Set = _set, Value = _value};

				storage.Resources.Add(resource);
			}

			storage.SaveChanges();
		}
	}

	internal class RemoveValue : Change
	{
		private readonly int _culture;
		private readonly string _set;
		private readonly string _key;

		public RemoveValue(int culture, string set, string key)
		{
			_culture = culture;
			_set = set;
			_key = key;
		}

		public override void Execute(LokiContext storage)
		{
			var resources = storage.Resources.Where(x => x.Culture == _culture && x.Set == _set && x.Key == _key).ToArray();
			if (resources.Length == 0)
			{
				return;
			}

			storage.Resources.RemoveRange(resources);
			storage.SaveChanges();
		}
	}

	internal class RemoveResourceSet : Change
	{
		private readonly int _culture;
		private readonly string _set;

		public RemoveResourceSet(int culture, string set)
		{
			_culture = culture;
			_set = set;
		}

		public override void Execute(LokiContext storage)
		{
			var resources = storage.Resources.Where(x => x.Culture == _culture && x.Set == _set).ToArray();
			if (resources.Length == 0)
			{
				return;
			}

			storage.Resources.RemoveRange(resources);
			storage.SaveChanges();
		}
	}
}
