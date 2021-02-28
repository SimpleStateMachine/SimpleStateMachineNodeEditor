using ReactiveUI;
using System;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class ReactiveCommandExtension
    {
        public static IDisposable ExecuteWithSubscribe<TParam, TResult>(this ReactiveCommand<TParam, TResult> reactiveCommand, TParam parameter = default)
        {
            return reactiveCommand.Execute(parameter).Subscribe();
        }
    }
}
