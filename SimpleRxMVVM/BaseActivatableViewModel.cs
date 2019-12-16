using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleRxMVVM
{
    public abstract class BaseActivatableViewModel : BaseViewModel
    {
        private CompositeDisposable _activationDisposables;

        public void Activate()
        {
            _activationDisposables = new CompositeDisposable();
            DoActivate(_activationDisposables);
        }

        public void Deactivate()
        {
            _activationDisposables.Dispose();
        }

        protected abstract void DoActivate(CompositeDisposable disposables);
    }
}