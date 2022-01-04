#if NET35

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace CommunityToolkit.Common;

/// <summary>
/// Helpers for working with <see cref="CancellationTokenSource"/>.
/// </summary>
public static class CancellationTokenSourceExtensions
{
    /// <summary>
    /// <see cref="Timer"/>s used by CancelAfter overloads.
    /// </summary>
    private volatile static ConcurrentDictionary<int, Timer> _timers = new();

    /// <summary>
    /// Delegate used with <see cref="Timer"/> to trigger cancellation of a <see cref="CancellationTokenSource"/>.
    /// </summary>
    private static readonly TimerCallback s_timerCallback = TimerCallback;
    private static void TimerCallback(object? state) => // separated out into a named method to improve Timer diagnostics in a debugger
        ((CancellationTokenSource)state!).NotifyCancellation(throwOnFirstException: false); // skip ThrowIfDisposed() check in Cancel()

    private const uint MAX_SUPPORTED_TIMEOUT = 4294967294u;

    private static void NotifyCancellation(this CancellationTokenSource source, bool throwOnFirstException)
    {
        MethodInfo notifyCancellation = source.GetType().GetMethod("NotifyCancellation", BindingFlags.NonPublic | BindingFlags.Instance);
        _ = notifyCancellation.Invoke(source, new object[] { throwOnFirstException });
    }

    /// <summary>
    /// A simple helper to determine whether disposal has occured.
    /// </summary>
    public static bool IsDisposed(this CancellationTokenSource source)
    {
        PropertyInfo isDisposed = source.GetType().GetProperty("IsDisposed", BindingFlags.NonPublic | BindingFlags.Instance);
        return isDisposed.GetValue(source) as bool? ?? true;
    }

    /// <summary>Schedules a Cancel operation on this <see cref="CancellationTokenSource"/>.</summary>
    /// <param name="delay">The time span to wait before canceling this <see cref="CancellationTokenSource"/>.
    /// </param>
    /// <exception cref="ObjectDisposedException">The exception thrown when this <see
    /// cref="CancellationTokenSource"/> has been disposed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The <paramref name="delay"/> is less than -1 or greater than maximum allowed timer duration.
    /// </exception>
    /// <remarks>
    /// <para>
    /// The countdown for the delay starts during this call.  When the delay expires,
    /// this <see cref="CancellationTokenSource"/> is canceled, if it has
    /// not been canceled already.
    /// </para>
    /// <para>
    /// Subsequent calls to CancelAfter will reset the delay for this
    /// <see cref="CancellationTokenSource"/>, if it has not been canceled already.
    /// </para>
    /// </remarks>
    public static void CancelAfter(this CancellationTokenSource token, TimeSpan delay)
    {
        long totalMilliseconds = (long)delay.TotalMilliseconds;
        if (totalMilliseconds < -1 || totalMilliseconds > MAX_SUPPORTED_TIMEOUT)
        {
            throw new ArgumentOutOfRangeException("delay");
        }

        token.CancelAfter((uint)totalMilliseconds);
    }

    /// <summary>
    /// Schedules a Cancel operation on this <see cref="CancellationTokenSource"/>.
    /// </summary>
    /// <param name="millisecondsDelay">The time span to wait before canceling this <see
    /// cref="CancellationTokenSource"/>.
    /// </param>
    /// <exception cref="ObjectDisposedException">The exception thrown when this <see
    /// cref="CancellationTokenSource"/> has been disposed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The exception thrown when <paramref name="millisecondsDelay"/> is less than -1.
    /// </exception>
    /// <remarks>
    /// <para>
    /// The countdown for the millisecondsDelay starts during this call.  When the millisecondsDelay expires,
    /// this <see cref="CancellationTokenSource"/> is canceled, if it has
    /// not been canceled already.
    /// </para>
    /// <para>
    /// Subsequent calls to CancelAfter will reset the millisecondsDelay for this
    /// <see cref="CancellationTokenSource"/>, if it has not been
    /// canceled already.
    /// </para>
    /// </remarks>
    public static void CancelAfter(this CancellationTokenSource token, int millisecondsDelay)
    {
        if (millisecondsDelay < -1)
        {
            throw new ArgumentOutOfRangeException("millisecondsDelay");
        }

        token.CancelAfter((uint)millisecondsDelay);
    }

    private static void CancelAfter(this CancellationTokenSource token, uint millisecondsDelay)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }

        // There is a race condition here as a Cancel could occur between the check of
        // IsCancellationRequested and the creation of the timer.  This is benign; in the
        // worst case, a timer will be created that has no effect when it expires.

        // Also, if Dispose() is called right here (after ThrowIfDisposed(), before timer
        // creation), it would result in a leaked Timer object (at least until the timer
        // expired and Disposed itself).  But this would be considered bad behavior, as
        // Dispose() is not thread-safe and should not be called concurrently with CancelAfter().

        Timer? timer = _timers.GetOrAdd(token.GetHashCode(), code =>
        {
            // Lazily initialize the timer in a thread-safe fashion.
            // Initially set to "never go off" because we don't want to take a
            // chance on a timer "losing" the initialization and then
            // cancelling the token before it (the timer) can be disposed.
            return new Timer(s_timerCallback, token, Timeout.Infinite, Timeout.Infinite);
        });

        _ = timer.Change(millisecondsDelay, Timeout.Infinite);
    }
}

#endif
