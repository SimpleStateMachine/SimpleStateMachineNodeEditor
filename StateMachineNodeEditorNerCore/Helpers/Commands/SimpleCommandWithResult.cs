using System;
using System.Windows.Input;


namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public class SimpleCommandWithResul<TResult> : ICommand
    {
        /// <summary>
        /// Функция с параметром, которая будет вызвана при выполнении команды
        /// </summary>
        private Func<TResult> _execute;

        /// <summary>
        /// Объкт, которому принадлежит команда
        /// </summary>
        public object Owner { get; protected set; }

        /// <summary>
        /// Требуется  интерфейсом ICloneable, не используется
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Результат выполнения команды
        /// </summary>
        /// Например здесь может храниться список объектов, которые были изменены
        public TResult Result { get; set; }

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
        public void Execute(object parameter = null)
        {
            //Result = this._execute(Parameters, Result) as TResult;
            this._execute();
        }

        /// <summary>
        /// Создать команду
        /// </summary>
        /// <param name="owner">Объкт, которому принадлежит команда</param>
        /// <param name="execute">Функция, которая будет вызвана при выполнении команды</param>
        public SimpleCommandWithResul(object owner, Func<TResult> execute)
        {
            Owner = owner;
            _execute = execute;
        }
    }
}
