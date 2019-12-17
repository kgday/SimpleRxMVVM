using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRxMVVM.Blazor
{
    public class PageMVVMComponentBase<T> : ActivatableMVVMComponentBase<T> where T : BaseActivatableViewModel
    {
        private bool disposedValue = false;

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected override void OnInitialized()
        {
            Activate();
            base.OnInitialized();
        }

        #region IDisposable Support

        // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Deactivate();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PageMVVMComponentBase()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        #endregion IDisposable Support
    }
}