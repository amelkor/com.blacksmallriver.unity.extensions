using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Bsr.Unity.Extensions.Threading
{
    /// <summary>
    /// Allows to manually set when awaiter should be finished.
    /// </summary>
    /// <example>
    /// <code>
    /// // This example allows to await a Unity animation event.
    /// AsyncManualFinisher _asyncReload = new AsyncManualFinisher();
    /// 
    /// public async UniTask Reload()
    /// {
    ///    // play the reload animation
    ///    animator.SetTrigger("Reload");
    ///    await _asyncReload;
    /// }
    ///
    /// //Animation event callback from an animation clip
    /// private void OnReloadDone()
    /// {
    ///    _asyncReload.Finish();
    /// }
    /// </code>
    /// </example>
    public class AsyncManualFinisher
    {
        public readonly struct ManualFinisherAwaiter : INotifyCompletion
        {
            private readonly AsyncManualFinisher _asyncManualFinisher;

            // ReSharper disable once MemberCanBeMadeStatic.Global
            public ManualFinisherAwaiter(AsyncManualFinisher e)
            {
                _asyncManualFinisher = e;
            }

            // ReSharper disable once MemberCanBeMadeStatic.Global
            [DebuggerHidden]
            public bool IsCompleted => false;

            [DebuggerHidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            // ReSharper disable once MemberCanBeMadeStatic.Global
            public void GetResult()
            {
            }

            [DebuggerHidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OnCompleted(Action continuation)
            {
                _asyncManualFinisher._continuation = continuation;
            }
        }

        private ManualFinisherAwaiter _awaiter;
        private Action _continuation;

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ManualFinisherAwaiter GetAwaiter()
        {
            _awaiter = new ManualFinisherAwaiter(this);
            return _awaiter;
        }

        /// <summary>
        /// Call this to invoke awaiter's continuation callback.
        /// </summary>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish()
        {
            _continuation();
        }
    }
}