namespace TransactionAuthorizer.Infrastructure;

public sealed class Semaphore
{
    private readonly SemaphoreSlim _semaphore;

    public Semaphore(int slots)
    {
        _semaphore = new SemaphoreSlim(slots);
    }

    public Task WaitAsync(CancellationToken cancellationToken = default)
    {
        return _semaphore.WaitAsync(cancellationToken);
    }

    public void Release()
    {
        _semaphore.Release();
    }
}