using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleRxMVVM
{
    public class Command<TParameter> : BaseCommand
    {
        private readonly Action<TParameter> _executeDelgate;

        public Command(Action<TParameter> executeDelegate, IObservable<bool> canExecute) : base(canExecute)
        {
            _executeDelgate = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
        }

        public override void Execute(object parameter)
        {
            _executeDelgate((TParameter)parameter);
        }

        public void Execute() => Execute(default(TParameter));
    }

    public class Command : BaseCommand
    {
        private readonly Action _executeDelgate;

        public Command(Action executeDelegate, IObservable<bool> canExecute) : base(canExecute)
        {
            _executeDelgate = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
        }

        public override void Execute(object parameter)
        {
            _executeDelgate();
        }

        public void Execute() => Execute(null);
    }
}