using System;
using System.Collections.Generic;

public delegate void TaskExecution<TTask>(TTask task);

delegate TPriority PrioritySelector<TTask, TPriority>(TTask task);

public class TaskScheduler<TTask, TPriority> where TPriority : IComparable<TPriority>
{
    private SortedDictionary<TPriority, Queue<TTask>> _taskQueue;
    private TaskExecution<TTask> _taskExecution;

    public TaskScheduler(TaskExecution<TTask> taskExecution)
    {
        _taskQueue = new SortedDictionary<TPriority, Queue<TTask>>();
        _taskExecution = taskExecution;
    }

    public void AddTask(TTask task, TPriority priority)
    {
        if (!_taskQueue.ContainsKey(priority))
        {
            _taskQueue[priority] = new Queue<TTask>();
        }
        _taskQueue[priority].Enqueue(task);
    }

    public void ExecuteNext()
    {
        if (_taskQueue.Count == 0)
        {
            Console.WriteLine("Немає завдань у черзі.");
            return;
        }

        var highestPriority = GetHighestPriority();
        var task = _taskQueue[highestPriority].Dequeue();

        if (_taskQueue[highestPriority].Count == 0)
        {
            _taskQueue.Remove(highestPriority);
        }

        _taskExecution(task);
    }

    private TPriority GetHighestPriority()
    {
        foreach (var priority in _taskQueue.Keys)
        {
            return priority;
        }
        throw new InvalidOperationException("Черга пуста.");
    }

    public bool HasTasks() => _taskQueue.Count > 0;
}

class Program
{
    static void Main(string[] args)
    {
        var scheduler = new TaskScheduler<string, int>((task) =>
        {
            Console.WriteLine($"Виконання завдання: {task}");
        });

        while (true)
        {
            Console.WriteLine("1: Додати завдання\n2: Виконати наступне завдання\n3: Вийти");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Введіть завдання: ");
                    var task = Console.ReadLine();
                    Console.Write("Введіть пріоритет (число): ");
                    if (int.TryParse(Console.ReadLine(), out var priority))
                    {
                        scheduler.AddTask(task, priority);
                        Console.WriteLine("Завдання додано.");
                    }
                    else
                    {
                        Console.WriteLine("Невірний пріоритет.");
                    }
                    break;
                case "2":
                    scheduler.ExecuteNext();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
}
