using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoRedis.Extensions
{
	public static class DistributedCacheExtensions
	{
		public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId,
			T data,
			TimeSpan? absoluteExpirationTime = null,
			TimeSpan? unusedExpirationTime = null)
		{
			var options = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = absoluteExpirationTime ?? TimeSpan.FromSeconds(60),
				SlidingExpiration = unusedExpirationTime
			};

			var jsonData = JsonSerializer.Serialize(data);
			await cache.SetStringAsync(recordId, jsonData, options);
		}

		public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
		{
			var jsonData = await cache.GetStringAsync(recordId);
			return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
		}
	}
}
