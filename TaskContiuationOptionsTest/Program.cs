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
                    _ = 5 / random.Next(0, 4);
                }, cancellationToken);

                _ = task.ContinueWith(_ => Console.WriteLine("Ran To Completion 1"), TaskContinuationOptions.OnlyOnRanToCompletion);
                _ = task.ContinueWith(_ => Console.WriteLine("Ran To Completion 2"), TaskContinuationOptions.OnlyOnRanToCompletion);
                _ = task.ContinueWith(_ => Console.WriteLine("Ran To Completion 3"), TaskContinuationOptions.OnlyOnRanToCompletion);
                _ = task.ContinueWith(_ => Console.WriteLine("Faulted"), TaskContinuationOptions.OnlyOnFaulted);
                _ = task.ContinueWith(_ => Console.WriteLine("Canceled"), TaskContinuationOptions.OnlyOnCanceled);

                try { await task; } catch { }
                Console.WriteLine("{0}Press any Key to try again{0}", Environment.NewLine);
                Console.ReadKey();
            }
        }
    }
}