using Moq;
using DeviceRepository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using DeviceRepository;

namespace DeviceRepositoryTests;

public class DeviceRepositoryTests
{
    [Fact]
    public async void GetAsync_GetNotExistsModel_IsNull()
    {
        var data = Enumerable.Empty<Device>().AsQueryable();

        var mockSet = new Mock<DbSet<Device>>();
        mockSet.As<IDbAsyncEnumerable<Device>>()
            .Setup(m => m.GetAsyncEnumerator())
            .Returns(new TestDbAsyncEnumerator<Device>(data.GetEnumerator()));

        mockSet.As<IQueryable<Device>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<Device>(data.Provider));

        mockSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        var mockContext = new Mock<DeviceContext>();
        mockContext
            .Setup(c => c.Devices)
            .Returns(mockSet.Object);

        var sut = new DeviceRepository.Repositories.DeviceRepository(mockContext.Object);
        var actual = await sut.GetAsync(1).ConfigureAwait(false);
        Assert.Null(actual);
    }

    [Fact]
    public async void GetAsync_GetExistingModel_IsNotNull()
    {
        var data = new List<Device> { new Device { Id = 1 } }.AsQueryable();

        var mockSet = new Mock<DbSet<Device>>();
        mockSet.As<IDbAsyncEnumerable<Device>>()
            .Setup(m => m.GetAsyncEnumerator())
            .Returns(new TestDbAsyncEnumerator<Device>(data.GetEnumerator()));

        mockSet.As<IQueryable<Device>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<Device>(data.Provider));

        mockSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        var mockContext = new Mock<DeviceContext>();
        mockContext
            .Setup(c => c.Devices)
            .Returns(mockSet.Object);

        var sut = new DeviceRepository.Repositories.DeviceRepository(mockContext.Object);
        var actual = await sut.GetAsync(1).ConfigureAwait(false);
        Assert.NotNull(actual);
    }
}

internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider, IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestDbAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestDbAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestDbAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(expression));
    }

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute<TResult>(expression));
    }

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Execute<TResult>(expression);
    }
}

internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
{
    public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    { }

    public TestDbAsyncEnumerable(Expression expression)
        : base(expression)
    { }

    public IDbAsyncEnumerator<T> GetAsyncEnumerator()
    {
        return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
    {
        return GetAsyncEnumerator();
    }

    IQueryProvider IQueryable.Provider
    {
        get { return new TestDbAsyncQueryProvider<T>(this); }
    }
}

internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestDbAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public void Dispose()
    {
        _inner.Dispose();
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_inner.MoveNext());
    }

    public T Current
    {
        get { return _inner.Current; }
    }

    object IDbAsyncEnumerator.Current
    {
        get { return Current; }
    }
}