using System;
using System.Collections.Generic;

public class FunctionCache<TKey, TResult>
{
    private readonly Func<TKey, TResult> _function;
    private readonly Dictionary<TKey, CacheEntry> _cache;
    private readonly TimeSpan _expirationTime;

    public FunctionCache(Func<TKey, TResult> function, TimeSpan expirationTime)
    {
        _function = function ?? throw new ArgumentNullException(nameof(function));
        _cache = new Dictionary<TKey, CacheEntry>();
        _expirationTime = expirationTime;
    }

    public TResult Get(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        lock (_cache)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                if (DateTime.UtcNow - entry.Timestamp <= _expirationTime)
                {
                    return entry.Value;
                }
                else
                {
                    _cache.Remove(key); 
                }
            }

            var result = _function(key);
            _cache[key] = new CacheEntry(result, DateTime.UtcNow);
            return result;
        }
    }

    public void Clear()
    {
        lock (_cache)
        {
            _cache.Clear();
        }
    }

    private class CacheEntry
    {
        public TResult Value { get; }
        public DateTime Timestamp { get; }

        public CacheEntry(TResult value, DateTime timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }
    }
}

class Program
{
    static void Main()
    {
        Func<int, int> expensiveFunction = x =>
        {
            Console.WriteLine($"Обчислення для {x}...");
            return x * x;
        };

        var cache = new FunctionCache<int, int>(expensiveFunction, TimeSpan.FromSeconds(5));

        Console.WriteLine(cache.Get(14)); 
        Console.WriteLine(cache.Get(14)); 

        System.Threading.Thread.Sleep(6000);

        Console.WriteLine(cache.Get(14)); 
    }
}
