using System.Collections.Concurrent;
using System.Reflection;
using TaskStackTest;

List<Model> models = new List<Model>
{
    new Model { Id = 1 },
    new Model { Id = 2 },
    new Model { Id = 3 }
};
ConcurrentStack<Model> modelStack = new ConcurrentStack<Model>(models);

List<Task> tasks = new List<Task>();
for (int i = 0; i < 3; i++)
{
    tasks.Add(Task.Factory.StartNew(() =>
    {
        Model model;
        if (modelStack.TryPop(out model))
        {
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} processing model with Id {model.Id}");
            // 在这里添加您需要在每个Model上执行的操作
        }
    }));
}

Task.WaitAll(tasks.ToArray());

Console.WriteLine("All models processed.");