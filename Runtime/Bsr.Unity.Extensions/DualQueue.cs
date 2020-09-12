using System.Collections.Generic;

namespace Bsr.Unity.Extensions
{
    public class DualQueue<T>
    {
        private readonly Queue<T> _first = new Queue<T>();
        private readonly Queue<T> _second = new Queue<T>();
        private int _queueIndex;

        public void SwitchQueues()
        {
            _queueIndex = _queueIndex == 0 ? 1 : 0;
        }

        public Queue<T> Current => _queueIndex == 0 ? _first : _second;
        public Queue<T> Next => _queueIndex == 0 ? _second : _first;
    }
}