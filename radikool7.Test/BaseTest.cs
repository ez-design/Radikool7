using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace radikool7.Test
{
    public class BaseTest
    {
        protected ITestOutputHelper Output { get; private set; }

        protected BaseTest( ITestOutputHelper output)
        {
            Output = output;
        }

        /// <summary>
        /// テスト実行
        /// </summary>
        /// <param name="label"></param>
        /// <param name="action"></param>
        protected async void Execute(string label, Func<Task> action)
        {
            var sw = new Stopwatch();
            sw.Start();
            await action.Invoke();
            sw.Stop();
            Output.WriteLine($"{label}:{sw.Elapsed}");
        }
    }
}