using Hanssens.Net;
using HR.LeaveManagement.MVC.Contracts;

namespace HR.LeaveManagement.MVC.Services
{
	public class LocalStorageService : ILocalStorageService
	{
		private LocalStorage storage;

		public LocalStorageService()
		{
			var config = new LocalStorageConfiguration()
			{
				AutoLoad = true,
				AutoSave = true,
				Filename = "HR.LEAVEMGMT"
			};
			this.storage = new LocalStorage(config);
		}
		public void ClearStorage(List<string> keys)
		{
			foreach (var key in keys)
			{
				storage.Remove(key);
			}
		}

		public void SetStorageValue<T>(string key, T value)
		{
			storage.Store(key, value);
			storage.Persist();
		}

		public T GetStorageValue<T>(string key)
		{
			return storage.Get<T>(key);
		}

		public bool Exists(string key)
		{
			return storage.Exists(key);
		}
	}
}
