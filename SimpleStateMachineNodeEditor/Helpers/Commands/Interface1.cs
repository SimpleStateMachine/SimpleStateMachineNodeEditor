using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public interface Interface1
    {
        /// <summary>
        /// Стек отмененных команд, которые можно выполнить повторно
        /// </summary>
        public static Stack<Interface1> StackRedo { get; set; } = new Stack<Interface1>();

        /// <summary>
        /// Стек выполненных команд, которые можно отменить 
        /// </summary>
        public static Stack<Interface1> StackUndo { get; set; } = new Stack<Interface1>();

        /// <summary>
        /// Функция для команды повторного выполнения
        /// </summary>
        public static void Redo()
        {
            if (CommandWithUndoRedo.StackRedo.Count > 0)
            {
                CommandWithUndoRedo last = CommandWithUndoRedo.StackRedo.Pop();
                last.Execute();
            }
        }

        /// <summary>
        /// Функция для команды отмены 
        /// </summary>
        public static void Undo()
        {
            if (CommandWithUndoRedo.StackUndo.Count > 0)
            {
                CommandWithUndoRedo last = CommandWithUndoRedo.StackUndo.Pop();
                last.UnExecute();
            }
        }

        public abstract void Execute();
        public abstract void UnExecute();

        /// <summary>
        /// Добавить команду в стек команд, которые можно выполнить повторно
        /// </summary>
        public void AddInRedo(Interface1 command)
        {
            StackRedo.Push(command);
        }

        /// <summary>
        /// Добавить команду в стек команд, которые можно отменить
        /// </summary>
        public void AddInUndo(Interface1 command)
        {
            StackUndo.Push(command);
        }
    }
}
