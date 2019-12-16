using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace SimpleRxMVVM
{
    public static class PropertyChangedExtensions
    {
        public static IObservable<T> WhenProperty<TViewModel, T>(this TViewModel viewModel, Expression<Func<TViewModel, T>> fieldExpression) where TViewModel : INotifyPropertyChanged
        {
            if (fieldExpression == null)
                throw new ArgumentNullException(nameof(fieldExpression));
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            IObservable<string> propertyChangedObservable;
            if (viewModel is BaseViewModel)
                propertyChangedObservable = (viewModel as BaseViewModel).PropertyChangedObservable;

            propertyChangedObservable = Observable.FromEvent<PropertyChangedEventHandler, string>(
                eventHandler => (sender, e) => eventHandler(e.PropertyName),
                    eh => viewModel.PropertyChanged += eh,
                    eh => viewModel.PropertyChanged -= eh);

            var expr = (MemberExpression)fieldExpression.Body;
            var propertyName = expr.Member.Name;
            return propertyChangedObservable
                .Where(name => name == propertyName)
                .Select(_ => fieldExpression.Compile()(viewModel));
        }
    }
}