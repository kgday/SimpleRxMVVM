using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace SimpleRxMVVM
{
    public static class CompositeDisposabeExtensions
    {
        public static void DisposeWith(this IDisposable @this, CompositeDisposable disposeWith)
        {
            disposeWith.Add(@this);
        }
    }
}