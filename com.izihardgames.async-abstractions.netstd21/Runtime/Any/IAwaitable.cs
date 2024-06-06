using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IziHardGames.Libs.NonEngine.Async.Abstractions
{
    public interface IAwaiterWrap
    {

    }
    public interface IAwaitable
    {

    }

    public interface IGetAwaiter<T> where T : INotifyCompletion
    {

    }

    public class GetAwaiterAttribute : Attribute
    {

    }
}