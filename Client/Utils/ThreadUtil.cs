using System.Threading;

namespace UI.Utils
{
    public static class ThreadUtil
    {
        public static int GetWorkingThreads()
        {
            int maxThreads;
            int completionPortThreads;
            ThreadPool.GetMaxThreads(out maxThreads, out completionPortThreads);

            int availableThreads;
            ThreadPool.GetAvailableThreads(out availableThreads, out completionPortThreads);

            return maxThreads - availableThreads;
        }
    }
}
