using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRxMVVM
{
    public class AsyncCommand<TParameter, TResult> : BaseCommand
    {
        private readonly Func<TParameter, Task<TResult>> _executeDelgate;
        private readonly Action<TResult> _executionDone;

        public AsyncCommand(Func<TParameter, Task<TResult>> executeDelegate, Action<TResult> executionDone, IObservable<bool> canExecute) : base(canExecute)
        {
            _executeDelgate = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
            _executionDone = executionDone;
        }

        public override async void Execute(object parameter)
        {
            var result = await _executeDelgate((TParameter)parameter);
            _executionDone?.Invoke(result);
        }

        public virtual Task ExecuteAsync(TParameter parameter) => _executeDelgate(parameter).ContinueWith(result => _executionDone?.Invoke(result.Result));
    }
}