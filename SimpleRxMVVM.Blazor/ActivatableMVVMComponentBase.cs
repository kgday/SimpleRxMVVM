using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SimpleRxMVVM;

namespace BlazorApp4.Client.Shared
{
    public class ActivatableMVVMComponentBase<T> : BaseMVVMComponentBase<T> where T : BaseActivatableViewModel
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

        protected virtual void DoActivate(CompositeDisposable disposables)
        {
            ViewModelChanged
                .DistinctUntilChanged()
                .Where(viewModel => viewModel != null)
                .Subscribe(viewModel => viewModel.Activate())
                .DisposeWith(disposables);
        }
    }
}