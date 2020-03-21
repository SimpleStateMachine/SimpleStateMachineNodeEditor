using System.Collections.Generic;


namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public abstract class CommandUndoRedo
    {
        /// <summary>
        /// Стек отмененных команд, которые можно выполнить повторно
        /// </summary>
        public static Stack<CommandUndoRedo> StackRedo { get; set; } = new Stack<CommandUndoRedo>();

        /// <summary>
        /// Стек выполненных команд, которые можно отменить 
        /// </summary>
        public static Stack<CommandUndoRedo> StackUndo { get; set; } = new Stack<CommandUndoRedo>();

        /// <summary>
        /// Функция для команды повторного выполнения
        /// </summary>
        public static void Redo()
        {
            if (CommandUndoRedo.StackRedo.Count > 0)
            {
                CommandUndoRedo last = CommandUndoRedo.StackRedo.Pop();
                last.Execute();
            }
        }

        /// <summary>
        /// Функция для команды отмены 
        /// </summary>
        public static void Undo()
        {
            if (CommandUndoRedo.StackUndo.Count > 0)
            {
                CommandUndoRedo last = CommandUndoRedo.StackUndo.Pop();
                last.UnExecute();
            }
        }

        public abstract void Execute();
        public abstract void UnExecute();

        /// <summary>
        /// Добавить команду в стек команд, которые можно выполнить повторно
        /// </summary>
        public void AddInRedo(CommandUndoRedo command)
        {
            StackRedo.Push(command);
        }

        /// <summary>
        /// Добавить команду в стек команд, которые можно отменить
        /// </summary>
        public void AddInUndo(CommandUndoRedo command)
        {
            StackUndo.Push(command);
        }
    }
}
