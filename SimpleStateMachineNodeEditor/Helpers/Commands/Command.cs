
using System;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    /// <summary>
    /// Команда с Undo/Redo
    /// </summary>
    /// <typeparam name="TParameter">Тип параметра, передаваемого для выполнения</typeparam>
    /// <typeparam name="TResult">Тип результата выполнения</typeparam>
    public class Command<TParameter, TResult> : CommandUndoRedo, ICommand, ICloneable where TParameter : class where TResult : class
    {

        /// <summary>
        /// Функция, которая будет вызвана при выполнении команды
        /// </summary>
        private readonly Func<TParameter, TResult, TResult> _execute;

        /// <summary>
        /// Функция отмены команды
        /// </summary>
        private readonly Func<TParameter, TResult, TResult> _unExecute;

        /// <summary>
        /// Параметр, который был передан в команду при выполнении
        /// </summary>
        public TParameter Parameters { get; set; }

        /// <summary>
        /// Результат выполнения команды
        /// </summary>
        /// Например здесь может храниться список объектов, которые были изменены
        public TResult Result { get; set; }

        /// <summary>
        /// Объкт, которому принадлежит команда
        /// </summary>
        public object Owner { get; protected set; }

        /// <summary>
        /// Флаг того, является ли команда отменяемой 
        /// </summary>
        private bool CanUnExecute
        {
            get { return _unExecute != null; }
        }

        /// <summary>
        /// Клонирование текущей команды, для записи в стек выполненных или отмененных команд
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Command<TParameter, TResult>(Owner, _execute, _unExecute)
            {
                Parameters = this.Parameters,
                Result = this.Result
            };
        }

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

            //Запоминаем параметр ( чтобы можно было егоже передать в отмену)
            Parameters = (parameter as TParameter);

            //Выполняем команду и запоминаем результат ( чтобы можно было выполнить отмену именно для этого результата)
            Result = this._execute(Parameters, Result) as TResult;

            //Если команду можно отменить
            if (CanUnExecute)
            {
                //Добавляем копию команды в стек команд, которые можно отменить
                AddInUndo(this.Clone() as CommandUndoRedo);

                //Очищаем список отмененнных команд ( началась новая ветка изменений)
                StackRedo.Clear();
            }

            //Очищаем результат ( чтобы не передавать его при повторном выполнении)
            Result = null;

            //Очищаем параметр ( чтобы не передавать его при повторном выполнении)
            Parameters = null;
        }

        /// <summary>
        /// Отмена команды
        /// </summary>
        public override void UnExecute()
        {
            //Выполняем отмену команду
            this._unExecute(Parameters, Result);

            //Добавляем копию команды в стек команд, которые можно выполнить повторно
            AddInRedo(this.Clone() as CommandUndoRedo);
        }

        /// <summary>
        /// Повторное выполнения команды
        /// </summary>
        public override void Execute()
        {
            //Выполянем команду
            this.Result = this._execute(this.Parameters, this.Result);

            //Добавляем копию команды в стек команд, которые можно отменить
            AddInUndo(this.Clone() as CommandUndoRedo);
        }

        /// <summary>
        /// Создать неотменяемую команду ( Для создания отменяемой команды, добавьте функцию, которая будет вызвана при отмене)
        /// </summary>
        /// <param name="owner">Объкт, которому принадлежит команда</param>
        /// <param name="action">Функция, которая будет вызвана при выполнении команды</param>
        private Command(object owner, Func<TParameter, TResult, TResult> execute)
        {
            Owner = owner;
            _execute = execute;
        }

        /// <summary>
        /// Создать отменяемую команду
        /// </summary>
        /// <param name="owner">Объкт, которому принадлежит команда</param>
        /// <param name="execute">Функция, которая будет вызвана при выполнении команды</param>
        /// <param name="unExecute">Функция, которая будет вызвана при отмене команды</param>
        public Command(object owner, Func<TParameter, TResult, TResult> execute, Func<TParameter, TResult, TResult> unExecute) : this(owner, execute)
        {
            _unExecute = unExecute;
        }
    }
}
