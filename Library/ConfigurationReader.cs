using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ConfigurationManagement.DAL;
using ConfigurationManagement.DAL.Entities;

namespace ConfigurationManagement.Library
{
	public class ConfigurationReader : IDisposable
	{
		private readonly string _applicationName;
		private readonly string _connectionString;
		private readonly int _refreshIntervalMs;
		private readonly Timer _refreshTimer;

		private ConcurrentDictionary<string, ConfigurationItem> _cache = new();
		private bool _disposed;

		public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
		{
			_applicationName = applicationName;
			_connectionString = connectionString;
			_refreshIntervalMs = refreshTimerIntervalInMs;

			LoadFromStorage();

			_refreshTimer = new Timer(_ => LoadFromStorage(), null, _refreshIntervalMs, _refreshIntervalMs);
		}

		private void LoadFromStorage()
		{
			try
			{
				var options = new DbContextOptionsBuilder<AppDbContext>()
					.UseSqlServer(_connectionString)
					.Options;

				using var context = new AppDbContext(options);
				var activeItems = context.ConfigurationItems
										 .Where(c => c.IsActive && c.ApplicationName == _applicationName)
										 .AsNoTracking()
										 .ToList();

				var newCache = new ConcurrentDictionary<string, ConfigurationItem>(
					(IEqualityComparer<string>?)activeItems.ToDictionary(c => c.Name, c => c)
				);

				Interlocked.Exchange(ref _cache, newCache);
			}
			catch
			{

			}
		}

		public T GetValue<T>(string key)
		{
			if (_cache.TryGetValue(key, out var item))
			{
				return (T)Convert.ChangeType(item.Value, typeof(T));
			}

			throw new KeyNotFoundException($"Configuration key '{key}' not found for application '{_applicationName}'.");
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_refreshTimer.Dispose();
				_disposed = true;
			}
		}
	}
}
