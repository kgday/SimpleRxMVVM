using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRxMVVM
{
    public class Command<TParameter, TResult> : BaseCommand
    {
        private readonly Func<TParameter, TResult> _executeDelgate;
        private readonly Action<TResult> _executionDone;

        public Command(Func<TParameter, TResult> executeDelegate, Action<TResult> executionDone, IObservable<bool> canExecute) : base(canExecute)
        {
            _executeDelgate = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
            _executionDone = executionDone;
        }

        public override void Execute(object parameter)
        {
            var result = _executeDelgate((TParameter)parameter);
            _executionDone?.Invoke(result);
        }
    }
}