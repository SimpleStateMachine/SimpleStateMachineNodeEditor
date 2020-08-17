using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    //public class ReactiveCommandWithUndoRedo<TParam, TResult> : ReactiveCommandBase<TParam, TResult>
    //{
    //    private readonly Func<TParam, IObservable<TResult>> _execute;
    //    private readonly IScheduler _outputScheduler;
    //    private readonly Subject<ExecutionInfo> _executionInfo;
    //    private readonly ISubject<ExecutionInfo, ExecutionInfo> _synchronizedExecutionInfo;
    //    private readonly IObservable<bool> _isExecuting;
    //    private readonly IObservable<bool> _canExecute;
    //    private readonly IObservable<TResult> _results;
    //    private readonly ScheduledSubject<Exception> _exceptions;
    //    private readonly IDisposable _canExecuteSubscription;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ReactiveCommand{TParam, TResult}"/> class.
    //    /// </summary>
    //    /// <param name="execute">The Func to perform when the command is executed.</param>
    //    /// <param name="canExecute">A observable which has a value if the command can execute.</param>
    //    /// <param name="outputScheduler">The scheduler where to send output after the main execution.</param>
    //    /// <exception cref="ArgumentNullException">Thrown if any dependent parameters are null.</exception>
    //    protected internal ReactiveCommand(
    //        Func<TParam, IObservable<TResult>> execute,
    //        IObservable<bool>? canExecute,
    //        IScheduler? outputScheduler)
    //    {
    //        if (execute == null)
    //        {
    //            throw new ArgumentNullException(nameof(execute));
    //        }

    //        if (canExecute == null)
    //        {
    //            throw new ArgumentNullException(nameof(canExecute));
    //        }

    //        if (outputScheduler == null)
    //        {
    //            throw new ArgumentNullException(nameof(outputScheduler));
    //        }

    //        _execute = execute;
    //        _outputScheduler = outputScheduler;
    //        _executionInfo = new Subject<ExecutionInfo>();
    //        _synchronizedExecutionInfo = Subject.Synchronize(_executionInfo, outputScheduler);
    //        _isExecuting = _synchronizedExecutionInfo
    //            .Scan(
    //                0,
    //                (acc, next) =>
    //                {
    //                    if (next.Demarcation == ExecutionDemarcation.Begin)
    //                    {
    //                        return acc + 1;
    //                    }

    //                    if (next.Demarcation == ExecutionDemarcation.End)
    //                    {
    //                        return acc - 1;
    //                    }

    //                    return acc;
    //                })
    //            .Select(inFlightCount => inFlightCount > 0)
    //            .StartWith(false)
    //            .DistinctUntilChanged()
    //            .Replay(1)
    //            .RefCount();
    //        _canExecute = canExecute
    //            .Catch<bool, Exception>(ex =>
    //            {
    //                _exceptions.OnNext(ex);
    //                return Observables.False;
    //            })
    //            .StartWith(false)
    //            .CombineLatest(_isExecuting, (canEx, isEx) => canEx && !isEx)
    //            .DistinctUntilChanged()
    //            .Replay(1)
    //            .RefCount();
    //        _results = _synchronizedExecutionInfo
    //            .Where(x => x.Demarcation == ExecutionDemarcation.Result)
    //            .Select(x => x.Result);

    //        _exceptions = new ScheduledSubject<Exception>(outputScheduler, RxApp.DefaultExceptionHandler);

    //        _canExecuteSubscription = _canExecute.Subscribe(OnCanExecuteChanged);
    //    }

    //    private enum ExecutionDemarcation
    //    {
    //        Begin,
    //        Result,
    //        End
    //    }

    //    /// <inheritdoc/>
    //    public override IObservable<bool> CanExecute => _canExecute;

    //    /// <inheritdoc/>
    //    public override IObservable<bool> IsExecuting => _isExecuting;

    //    /// <inheritdoc/>
    //    public override IObservable<Exception> ThrownExceptions => _exceptions.AsObservable();

    //    /// <inheritdoc/>
    //    public override IDisposable Subscribe(IObserver<TResult> observer)
    //    {
    //        return _results.Subscribe(observer);
    //    }

    //    /// <inheritdoc/>
    //    public override IObservable<TResult> Execute(TParam parameter = default(TParam))
    //    {
    //        try
    //        {
    //            return Observable
    //                .Defer(
    //                    () =>
    //                    {
    //                        _synchronizedExecutionInfo.OnNext(ExecutionInfo.CreateBegin());
    //                        return Observable<TResult>.Empty;
    //                    })
    //                .Concat(_execute(parameter))
    //                .Do(result => _synchronizedExecutionInfo.OnNext(ExecutionInfo.CreateResult(result)))
    //                .Catch<TResult, Exception>(
    //                    ex =>
    //                    {
    //                        _exceptions.OnNext(ex);
    //                        return Observable.Throw<TResult>(ex);
    //                    })
    //                .Finally(() => _synchronizedExecutionInfo.OnNext(ExecutionInfo.CreateEnd()))
    //                .PublishLast()
    //                .RefCount()
    //                .ObserveOn(_outputScheduler);
    //        }
    //        catch (Exception ex)
    //        {
    //            _exceptions.OnNext(ex);
    //            return Observable.Throw<TResult>(ex);
    //        }
    //    }

    //    /// <inheritdoc/>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            _executionInfo?.Dispose();
    //            _exceptions?.Dispose();
    //            _canExecuteSubscription?.Dispose();
    //        }
    //    }

    //    private struct ExecutionInfo
    //    {
    //        private readonly ExecutionDemarcation _demarcation;
    //        private readonly TResult _result;

    //        private ExecutionInfo(ExecutionDemarcation demarcation, TResult result)
    //        {
    //            _demarcation = demarcation;
    //            _result = result;
    //        }

    //        public ExecutionDemarcation Demarcation => _demarcation;

    //        public TResult Result => _result;

    //        public static ExecutionInfo CreateBegin() =>
    //            new ExecutionInfo(ExecutionDemarcation.Begin, default!);

    //        public static ExecutionInfo CreateResult(TResult result) =>
    //            new ExecutionInfo(ExecutionDemarcation.Result, result);

    //        public static ExecutionInfo CreateEnd() =>
    //            new ExecutionInfo(ExecutionDemarcation.End, default!);
    //    }
    //}
}
