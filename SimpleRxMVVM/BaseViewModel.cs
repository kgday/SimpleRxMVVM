using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleRxMVVM
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Subject<string> _propertyChanged;
        private bool _disposedValue = false;

        public BaseViewModel()
        {
            Disposables = new CompositeDisposable();
            _propertyChanged = new Subject<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal IObservable<string> PropertyChangedObservable => _propertyChanged.AsObservable();

        // To detect redundant calls
        protected CompositeDisposable Disposables { get; }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            _propertyChanged.OnNext(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        //protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        //{
        //    if (expression == null)
        //        throw new ArgumentNullException(nameof(expression));

        //    if (!(expression.Body is MemberExpression body))
        //        body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
        //    var propertyName = body.Member.Name;
        //    OnPropertyChanged(propertyName);
        //}

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Disposables.Dispose();
                    _propertyChanged.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseViewModel()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        #endregion IDisposable Support
    }
}