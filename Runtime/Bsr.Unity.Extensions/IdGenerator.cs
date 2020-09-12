using System.Threading;

namespace Bsr.Unity.Extensions
{
    public class IdGenerator
    {
        private static int _idInt;
        private static long _idLong;

        public static int GetNextIntId()
        {
            return Interlocked.Increment(ref _idInt);
        }

        public static long GetNextLongId()
        {
            return Interlocked.Increment(ref _idLong);
        }
    }
}