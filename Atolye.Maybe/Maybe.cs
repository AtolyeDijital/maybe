
using System.Diagnostics.CodeAnalysis;

namespace AtolyeDijital;

public class Maybe<T>
{
    private readonly T value;

    private readonly bool hasValue;

    private Maybe()
    {
        hasValue = false;
    }

    private Maybe(T value)
    {
        this.value = value;
        hasValue = true;
    }

    #region Private Methods

    private Maybe<TResult> GetValueOrThrow<TResult>(Maybe<TResult> val, string errorMessage)
    {
        if (val == null || !val.hasValue)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ApplicationException(errorMessage);
            }
            return new Maybe<TResult>();
        }
        return val;
    }

    #endregion

    public bool HasValue { get { return hasValue; } }


    public Maybe<T> CheckNull(string errorMessage = "")
    {
        if (!hasValue)
        {
            throw new ApplicationException(errorMessage);
        }
        return this;
    }

    public Maybe<T> Check(Func<T, bool> predicate)
    {
        if (!hasValue || !predicate(this.value))
        {
            return new Maybe<T>();
        }
        return this;
    }

    public Maybe<T> Check(Func<T, bool> predicate, string errorMessage)
    {
        if (!hasValue || !predicate(this.value))
        {
            throw new ApplicationException(errorMessage);
        }
        return this;
    }

    


    public static implicit operator Maybe<T>(T value)
    {
        if (value == null)
            return new Maybe<T>();

        return new Maybe<T>(value);
    }

    public static Maybe<T> from(T value)
    {
        if (value == null)
            return new Maybe<T>();

        return new Maybe<T>(value);
    }

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
    {
        if (!hasValue)
            return new Maybe<TResult>();


        var val = func(value);
        if (val == null || !val.hasValue)
        {
            return new Maybe<TResult>();
        }
        return val;
    }

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func, string errorMessage)
    {
        if (!hasValue)
            return new Maybe<TResult>();

        var val = func(value);
        return GetValueOrThrow(val, errorMessage);
    }

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func, string errorMessage, out TResult outVar)
    {
        outVar = default;
        if (!hasValue)
            return new Maybe<TResult>();

        var val = func(value);
        val = GetValueOrThrow(val, errorMessage);
        outVar = val.value;
        return val;
    }

    public Maybe<TResult> Bind<TResult>(Func<T, TResult> func, string errorMessage = "")
    {
        if (!hasValue)
            return new Maybe<TResult>();

        var val = func(value);
        if (val == null)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ApplicationException(errorMessage);
            }
            else
            {
                return new Maybe<TResult>();
            }

        }
        return val;
    }

    [return: NotNull]
    public T ValueOrThrow(string errorMessage = "Invalid entitiy!")
    {
        if (hasValue)
            return value!;

        throw new ApplicationException(errorMessage);
    }

    public Maybe<T> With(params Action<T>[] modifications)
    {

        if (!hasValue)
            return new Maybe<T>();

        var val = value;
        foreach (var modify in modifications)
        {
            modify(val);
        }
        return val;
    }

    #region Async
    public async Task<Maybe<TResult>> BindAsync<TResult>(Func<T, Task<Maybe<TResult>>> func)
    {
        if (!hasValue)
            return await Task.FromResult(new Maybe<TResult>());


        var val = await func(value);
        if (val == null || !val.hasValue)
        {
            return new Maybe<TResult>();
        }
        return val;
    }

    public async Task<Maybe<TResult>> BindAsync<TResult>(Func<T, Task<Maybe<TResult>>> func, string errorMessage)
    {
        if (!hasValue)
            return await Task.FromResult(new Maybe<TResult>());

        var val = await func(value);
        return GetValueOrThrow(val, errorMessage);
    }

    public async Task<Maybe<TResult>> BindAsync<TResult>(Func<T, Maybe<TResult>> func, string errorMessage)
    {
        if (!hasValue)
            return await Task.FromResult(new Maybe<TResult>());

        var val = await Task.FromResult(func(value));
        return GetValueOrThrow(val, errorMessage);
    }

    public async Task<Maybe<TResult>> BindAsync<TResult>(Func<T, Task<TResult>> func)
    {
        return await BindAsync(func, string.Empty);
    }

    public async Task<Maybe<TResult>> BindAsync<TResult>(Func<T, Task<TResult>> func, string errorMessage)
    {
        if (!hasValue)
            return await Task.FromResult(new Maybe<TResult>());

        var val = await func(value);
        if (val == null)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ApplicationException(errorMessage);
            }
            else
            {
                return new Maybe<TResult>();
            }

        }
        return val;
    }

    public async Task<Maybe<T>> CheckAsync(Func<T, Task<bool>> predicate, string errorMessage)
    {
        if (!hasValue)
        {
            return this;
        }

        if (await predicate(this.value))
        {
            return this;
        }

        throw new ApplicationException(errorMessage);
    }


    public async Task<T> OrElseAsync(Func<Task<T>> defaultValueProvider, string message = "Value provider returns null!")
    {
        if (hasValue)
        {
            return value;
        }

        var x = await defaultValueProvider();
        if (x != null)
        {
            return x;
        }
        
        throw new ArgumentNullException(message);
    }

    #endregion

    public T OrElse(T defaultValue)
    {
        return hasValue ? value : defaultValue;
    }

    public T OrElse(Func<T> defaultValueProvider)
    {
        return hasValue ? value : defaultValueProvider();
        
    }

}

