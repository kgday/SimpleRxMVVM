using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Components;
using SimpleRxMVVM;

namespace BlazorApp4.Client.Shared
{
    //much of the code copied from the source from ReactiveUI.Blazor - ReactiveComponentBase
    //use this class for when views are a part of modals so that activation doesn't occur on initialization but rather when the modal is being shown
    //It also has the viewmodel automatically injected via the OwningComponentBase service property
    //Activation doesn't occur unless Activate() is called.  Likewise Deactivate() must also be called from the inherited view.
    public class BaseMVVMComponentBase<T> : ComponentBase, INotifyPropertyChanged where T : class, INotifyPropertyChanged
    {
        private readonly BehaviorSubject<T> _viewModelSubject = new BehaviorSubject<T>(null);

        public BaseMVVMComponentBase()
        {
            ViewModelPropertyChanged = this.WhenProperty(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => Observable.FromEvent<PropertyChangedEventHandler, Unit>(
                    eventHandler =>
                    {
                        void Handler(object sender, PropertyChangedEventArgs e) => eventHandler(Unit.Default);

                        return Handler;
                    },
                    eh => x.PropertyChanged += eh,
                    eh => x.PropertyChanged -= eh))
                .Switch();

            ViewModelPropertyChanged.Subscribe(_ => DoOnViewModelPropertyChanged());
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public T ViewModel
        {
            get => _viewModelSubject.Value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_viewModelSubject.Value, value))
                {
                    return;
                }

                _viewModelSubject.OnNext(value);
                OnPropertyChanged();
            }
        }

        public IObservable<T> ViewModelChanged => _viewModelSubject.AsObservable();

        public IObservable<Unit> ViewModelPropertyChanged { get; }

        /// <summary>
        /// Invokes the property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DoOnViewModelPropertyChanged()
        {
            InvokeAsync(() => StateHasChanged());
        }
    }
}