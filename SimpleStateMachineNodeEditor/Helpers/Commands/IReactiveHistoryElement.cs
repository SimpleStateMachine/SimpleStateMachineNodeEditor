using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public interface IReactiveHistoryElement<TParameter, TResult>: IReactiveCommandWithUndoRedo
    {
        TParameter Parameter { get; set; }
        TResult Result { get; set; }
    }
}

