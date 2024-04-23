using AtolyeDijital;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtolyeDijital;
public static class TaskExtensions
{
    public static async Task<Maybe<TResult>> Bind<T, TResult>(
        this Task<Maybe<T>> task, Func<T, Maybe<TResult>> func)
        => (await task).Bind(func);

    public static async Task<Maybe<TResult>> Bind<T, TResult>(
        this Task<Maybe<T>> task, Func<T, Maybe<TResult>> func, string errorMessage)
        => (await task).Bind(func, errorMessage);

    public static async Task<Maybe<TResult>> BindAsync<T, TResult>(
        this Task<Maybe<T>> task, Func<T, Task<Maybe<TResult>>> func)
        => await (await task).BindAsync(func);

    public static async Task<Maybe<TResult>> BindAsync<T, TResult>(
        this Task<Maybe<T>> task, Func<T, Task<Maybe<TResult>>> func, string errorMessage)
        => await (await task).BindAsync(func, errorMessage);

    public static async Task<Maybe<TResult>> BindAsync<T, TResult>(
        this Task<Maybe<T>> task, Func<T, Task<TResult>> func)
        => await (await task).BindAsync(func);

    public static async Task<Maybe<TResult>> BindAsync<T, TResult>(
        this Task<Maybe<T>> task, Func<T, Task<TResult>> func, string errorMessage)
        => await (await task).BindAsync(func, errorMessage);

    [return: NotNull]
    public async static Task<T> ValueOrThrow<T>(
        this Task<Maybe<T>> task, string errorMessage)
        => (await task).ValueOrThrow(errorMessage);

    public async static Task<Maybe<T>> Check<T>(
        this Task<Maybe<T>> task, Func<T, bool> predicate)
        => (await task).Check(predicate);

    public async static Task<Maybe<T>> Check<T>(
        this Task<Maybe<T>> task, Func<T, bool> predicate, string errorMessage)
        => (await task).Check(predicate, errorMessage);
    public async static Task<Maybe<T>> CheckAsync<T>(
        this Task<Maybe<T>> task, Func<T, Task<bool>> predicate, string errorMessage)
    {
        var maybe = await task;
        return await maybe.CheckAsync(predicate, errorMessage);
    }

    public async static Task<Maybe<T>> With<T>(
        this Task<Maybe<T>> task, params Action<T>[] modifications)
        => (await task).With(modifications);

}
