using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRxMVVM
{
    public class AsyncCommand<TParameter> : BaseCommand
    {
        private readonly Func<TParameter, Task> _executeDelgate;

        public AsyncCommand(Func<TParameter, Task> executeDelegate, IObservable<bool> canExecute) : base(canExecute)
        {
            _executeDelgate = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
        }

        public override async void Execute(object parameter)
        {
            await ExecuteAsync((TParameter)parameter);
        }

        public virtual Task ExecuteAsync(TParameter parameter) => _executeDelgate((TParameter)parameter);
    }

    public class AsyncCommand : BaseCommand
    {
        private readonly Func<Task> _executeDelgate;

        public AsyncCommand(Func<Task> executeDelegate, IObservable<bool> canExecute) : base(canExecute)
        {
            _executeDelgate = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
        }

        public override async void Execute(object parameter) => await ExecuteAsync();

        public void Execute() => Execute(null);

        public virtual Task ExecuteAsync() => _executeDelgate();
    }
}