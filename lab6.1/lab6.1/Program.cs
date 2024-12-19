using System;
using System.Collections.Generic;
using System.Linq;

public delegate bool Criteria<T>(T item);

public class Repository<T>
{
    private readonly List<T> _items;

    public Repository()
    {
        _items = new List<T>();
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public bool Remove(T item)
    {
        return _items.Remove(item);
    }

    public IEnumerable<T> Find(Criteria<T> criteria)
    {
        return _items.Where(item => criteria(item));
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }
}

class Program
{
    static void Main()
    {
        var intRepository = new Repository<int>();
        intRepository.Add(1);
        intRepository.Add(2);
        intRepository.Add(3);
        intRepository.Add(4);

        Criteria<int> evenCriteria = item => item % 2 == 0;

        var evenNumbers = intRepository.Find(evenCriteria);

        Console.WriteLine("Парнi числа:");
        foreach (var number in evenNumbers)
        {
            Console.WriteLine(number);
        }

        var stringRepository = new Repository<string>();
        stringRepository.Add("Apple");
        stringRepository.Add("Banana");
        stringRepository.Add("Cherry");
        stringRepository.Add("Date");

        Criteria<string> startsWithBCriteria = item => item.StartsWith("B");

        var stringsStartingWithB = stringRepository.Find(startsWithBCriteria);

        Console.WriteLine("Строки, що починаються з 'B':");
        foreach (var str in stringsStartingWithB)
        {
            Console.WriteLine(str);
        }
    }
}
