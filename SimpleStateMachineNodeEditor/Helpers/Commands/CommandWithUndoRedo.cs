using System.Collections.Generic;


namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public interface CommandWithUndoRedo
    {
        public static Stack<CommandWithUndoRedo> StackRedo { get; set; } = new Stack<CommandWithUndoRedo>();

        public static Stack<CommandWithUndoRedo> StackUndo { get; set; } = new Stack<CommandWithUndoRedo>();

        public static void Redo()
        {
            if (StackRedo.Count > 0)
            {
                CommandWithUndoRedo last = StackRedo.Pop();
                last.Execute();
            }
        }

        public static void Undo()
        {
            if (StackUndo.Count > 0)
            {
                CommandWithUndoRedo last = StackUndo.Pop();
                last.UnExecute();
            }
        }

        public static CommandWithUndoRedo AddInRedo(CommandWithUndoRedo command)
        {
            StackRedo.Push(command);

            return command;
        }

        public static CommandWithUndoRedo AddInUndo(CommandWithUndoRedo command)
        {
            StackUndo.Push(command);

            return command;
        }

        public void Execute();
        public void UnExecute();

    }
}
