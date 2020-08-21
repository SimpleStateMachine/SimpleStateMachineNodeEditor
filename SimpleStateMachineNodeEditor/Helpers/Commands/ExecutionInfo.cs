using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public enum ExecutionDemarcation
    {
        Begin,
        Result,
        End
    }

    public struct ExecutionInfo<TResult>
    {
        private readonly ExecutionDemarcation _demarcation;
        private readonly TResult _result;

        private ExecutionInfo(ExecutionDemarcation demarcation, TResult result)
        {
            _demarcation = demarcation;
            _result = result;
        }

        public ExecutionDemarcation Demarcation => _demarcation;

        public TResult Result => _result;

        public static ExecutionInfo<TResult> CreateBegin() =>
            new ExecutionInfo<TResult>(ExecutionDemarcation.Begin, default!);

        public static ExecutionInfo<TResult> CreateResult(TResult result) =>
            new ExecutionInfo<TResult>(ExecutionDemarcation.Result, result);

        public static ExecutionInfo<TResult> CreateEnd() =>
            new ExecutionInfo<TResult>(ExecutionDemarcation.End, default!);
    }
}
