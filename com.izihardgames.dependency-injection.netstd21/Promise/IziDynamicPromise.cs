using System;
using System.Collections.Generic;
using System.Text;

namespace IziHardGames.DependencyInjection.Promises
{
    public class IziDynamicPromise<T> : IDisposable where T : class
    {
        private T? value;
        public void SetValueWithoutNotify()
        {
            throw new System.NotImplementedException();
        }
        public void SetValue()
        {
            throw new System.NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
