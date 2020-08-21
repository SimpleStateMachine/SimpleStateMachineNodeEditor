using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{

    public class ReactiveCommandWithUndoRedo<TParam, TResult> : ReactiveCommandWithUndoRedoBase<TParam, TResult>
    {
        private readonly ReactiveCommand<TParam, TResult> _command;
        private readonly IDisposable _canUnExecuteSubscription;
        private readonly ScheduledSubject<Exception> _exceptions;

        private readonly Subject<ExecutionInfo<TResult>> _unExecutionInfo;
        private readonly ISubject<ExecutionInfo<TResult>, ExecutionInfo<TResult>> _synchronizedUnExecutionInfo;
        private readonly IScheduler _outputScheduler;

        private readonly Func<TParam, IObservable<TResult>, IObservable<TResult>> _unExecute;
        private readonly IObservable<bool> _canUnExecute; 
        private readonly IObservable<bool> _isUnExecuting;
        private TParam _parameter;
        private IObservable<TResult> _result;

        protected internal ReactiveCommandWithUndoRedo(
            Func<TParam, IObservable<TResult>> execute,
            Func<TParam, IObservable<TResult>, IObservable<TResult>> unExecute,
            IObservable<bool>? canExecute,
            IObservable<bool>? canUnExecute,
            IScheduler? outputScheduler)
        {
            _outputScheduler = outputScheduler;
            if (unExecute == null)
            {
                throw new ArgumentNullException(nameof(unExecute));
            }

            if (canUnExecute == null)
            {
                throw new ArgumentNullException(nameof(canUnExecute));
            }

            //_command = ReactiveCommand.CreateFromObservable<TParam, IList<TResult>>(execute, canExecute, outputScheduler);

            _unExecute = unExecute;

            _unExecutionInfo = new Subject<ExecutionInfo<TResult>>();
            _synchronizedUnExecutionInfo = Subject.Synchronize(_unExecutionInfo, outputScheduler);
            _isUnExecuting = _synchronizedUnExecutionInfo
                .Scan(
                    0,
                    (acc, next) =>
                    {
                        if (next.Demarcation == ExecutionDemarcation.Begin)
                        {
                            return acc + 1;
                        }

                        if (next.Demarcation == ExecutionDemarcation.End)
                        {
                            return acc - 1;
                        }

                        return acc;
                    })
                .Select(inFlightCount => inFlightCount > 0)
                .StartWith(false)
                .DistinctUntilChanged()
                .Replay(1)
                .RefCount();

            _canUnExecute = canUnExecute
               .Catch<bool, Exception>(ex =>
               {
                   _exceptions.OnNext(ex);
                   return Observables.False;
               })
               .StartWith(false)
               .CombineLatest(_isUnExecuting, (canEx, isEx) => canEx && !isEx)
               .DistinctUntilChanged()
               .Replay(1)
               .RefCount();

            _command
              .ThrownExceptions
              .Subscribe();
            _exceptions = new ScheduledSubject<Exception>(outputScheduler, RxApp.DefaultExceptionHandler);

            _canUnExecuteSubscription = _canUnExecute.Subscribe(OnCanUnExecuteChanged);
        }


        public override IObservable<bool> CanExecute => _command.CanExecute;

        public override IObservable<bool> IsExecuting => _command.IsExecuting;
        public override IObservable<Exception> ThrownExceptions => _command.ThrownExceptions;

        public override IObservable<bool> CanUnExecute => _command.CanExecute.Where(x => !x);

        public override IObservable<bool> IsUnExecuting => _isUnExecuting;

        public override IObservable<TResult> Execute(TParam parameter = default(TParam))
        {
            _parameter = parameter;
            return _command.Execute(parameter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _command.Dispose();
                _unExecutionInfo?.Dispose();
                _exceptions?.Dispose();
                _canUnExecuteSubscription?.Dispose();
            }
        }
        /// <inheritdoc/>
        public  IObservable<TResult> UnExecute()
        {

            try
            {
                return Observable
                    .Defer(
                        () =>
                        {
                            _synchronizedUnExecutionInfo.OnNext(ExecutionInfo<TResult>.CreateBegin());
                            return Observable<TResult>.Empty;
                        })
                    .Concat(_unExecute(_parameter, _result))
                    .Do(result => _synchronizedUnExecutionInfo.OnNext(ExecutionInfo<TResult>.CreateResult(result)))
                    .Catch<TResult, Exception>(
                        ex =>
                        {
                            _exceptions.OnNext(ex);
                            return Observable.Throw<TResult>(ex);
                        })
                    .Finally(() => _synchronizedUnExecutionInfo.OnNext(ExecutionInfo<TResult>.CreateEnd()))
                    .PublishLast()
                    .RefCount()
                    .ObserveOn(_outputScheduler);
            }
            catch (Exception ex)
            {
                _exceptions.OnNext(ex);
                return Observable.Throw<TResult>(ex);
            }

        }
        public override IDisposable Subscribe(IObserver<TResult> observer)
        {
            return _command.Subscribe(observer);
        }

    }
}
