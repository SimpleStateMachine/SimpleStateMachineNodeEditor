using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.DirectoryServices;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    /// <summary>
    /// Команда без Undo/Redo
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public class SimpleCommandWithParameter<TParameter> : ICommand
    {
        /// <summary>
        /// Функция с параметром, которая будет вызвана при выполнении команды
        /// </summary>
        private Action<TParameter> _execute;

        public Action OnExecute { get; set; }

        /// <summary>
        /// Требуется  интерфейсом ICloneable, не используется
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Требуется  интерфейсом ICloneable, не используется
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Всегда возвращает true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="parameter"> Параметр команды </param>
        public void Execute(object parameter)
        {

            this._execute(parameter.Cast<TParameter>());

            OnExecute?.Invoke();
        }

        /// <summary>
        /// Создать команду
        /// </summary>
        /// <param name="owner">Объкт, которому принадлежит команда</param>
        /// <param name="execute">Функция, которая будет вызвана при выполнении команды</param>
        public SimpleCommandWithParameter(Action<TParameter> execute, Action onExecute = null)
        {
            _execute = execute;
            OnExecute += onExecute;
        }

        public static SimpleCommandWithParameter<TParameter> Create<TParameter>(Action<TParameter> execute, Action onExecute = null)
        {
            return new SimpleCommandWithParameter<TParameter>(execute, onExecute);
    }
    }
}
