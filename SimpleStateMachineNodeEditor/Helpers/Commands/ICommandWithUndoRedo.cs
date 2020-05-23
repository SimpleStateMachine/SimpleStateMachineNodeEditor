using System.Collections.Generic;


namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public interface ICommandWithUndoRedo
    {
        public static Stack<ICommandWithUndoRedo> StackRedo { get; set; } = new Stack<ICommandWithUndoRedo>();

        public static Stack<ICommandWithUndoRedo> StackUndo { get; set; } = new Stack<ICommandWithUndoRedo>();

        public static void Redo()
        {
            if (StackRedo.Count > 0)
            {
                ICommandWithUndoRedo last = StackRedo.Pop();
                last.ExecuteWithSubscribe();
            }
        }

        public static void Undo()
        {
            if (StackUndo.Count > 0)
            {
                ICommandWithUndoRedo last = StackUndo.Pop();
                last.UnExecute();
            }
        }

        public static ICommandWithUndoRedo AddInRedo(ICommandWithUndoRedo command)
        {
            StackRedo.Push(command);

            return command;
        }

        public static ICommandWithUndoRedo AddInUndo(ICommandWithUndoRedo command)
        {
            StackUndo.Push(command);

            return command;
        }

        public void ExecuteWithSubscribe();
        public void UnExecute();

    }
}
