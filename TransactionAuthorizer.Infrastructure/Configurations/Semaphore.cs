using System.Diagnostics.CodeAnalysis;

namespace TransactionAuthorizer.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed class SemaphoreWrapper(int slots) : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(slots);
    private bool _disposed;

    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();
        await _semaphore.WaitAsync(cancellationToken);
    }

    public void Release()
    {
        CheckDisposed();
        _semaphore.Release();
    }

#pragma warning disable CA1513
    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SemaphoreWrapper));
    }
# pragma warning restore CA1513

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _semaphore.Dispose();

            _disposed = true;
        }
    }
}