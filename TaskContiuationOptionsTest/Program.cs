using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskContiuationOptionsTest
{
    class Program
    {
        static async Task Main()
        {
            Random random = new();
            while (true)
            {
                Console.Clear();
                CancellationTokenSource cancellationTokenSource = new(random.Next(5, 20));
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                Task task = Task.Run(async () =>
                {
                    Console.WriteLine("Parent task");
                    await Task.Delay(random.Next(1, 10));
                    cancellationToken.ThrowIfCancellationRequested();
                    _ = 5 / random.Next(0, 3);
                }, cancellationToken);

                Task task1 = task.ContinueWith(_ => Console.WriteLine("Ran To Completion 1"), TaskContinuationOptions.OnlyOnRanToCompletion);
                Task task2 = task.ContinueWith(_ => Console.WriteLine("Ran To Completion 2"), TaskContinuationOptions.OnlyOnRanToCompletion);
                Task task3 = task.ContinueWith(_ => Console.WriteLine("Ran To Completion 3"), TaskContinuationOptions.OnlyOnRanToCompletion);
                Task task4 = task.ContinueWith(_ => Console.WriteLine("Faulted"), TaskContinuationOptions.OnlyOnFaulted);
                Task task5 = task.ContinueWith(_ => Console.WriteLine("Canceled"), TaskContinuationOptions.OnlyOnCanceled);

                try { await task; } catch { }

                await Task.WhenAll(new[] { task1, task2, task3, task4, task5 }).ContinueWith(_ =>
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("{0}Press any Key to try again{0}", Environment.NewLine);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                });
            }
        }
    }
}