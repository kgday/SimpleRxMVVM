using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRxMVVM
{
    public abstract class BaseCommand : ICommand, IDisposable
    {
        private readonly IDisposable _canExecuteSubscription;
        private bool _canExecute;
        private bool _isExecuting;

        private bool _disposedValue = false;

        public BaseCommand(IObservable<bool> canExecute)
        {
            if (canExecute != null)
                _canExecuteSubscription = canExecute.Subscribe(canExec =>
                {
                    _canExecute = canExec;
                    OnCanExecutedChanged();
                });
            else
                _canExecute = true;
        }

        public event EventHandler CanExecuteChanged;

        public bool IsExecuting
        {
            get => _isExecuting;
            protected set
            {
                _isExecuting = value;
                OnCanExecutedChanged();
            }
        }

        public bool CanExecute(object parameter) => !_isExecuting && _canExecute;

        public abstract void Execute(object parameter);

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        protected void OnCanExecutedChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());

        #region IDisposable Support

        // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _canExecuteSubscription?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseCommand()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        #endregion IDisposable Support
    }
}