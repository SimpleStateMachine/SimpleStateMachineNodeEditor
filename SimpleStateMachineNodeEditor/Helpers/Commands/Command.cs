using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public class Command<TParameter, TResult> : ICommandWithUndoRedo, ICommand, ICloneable 
    {

        private readonly Func<TParameter, TResult, TResult> _execute;

        /// <summary>
        /// Функция отмены команды
        /// </summary>
        private readonly Func<TParameter, TResult, TResult> _unExecute;

        public Action OnExecute { get; set; }

        /// <summary>
        /// Параметр, который был передан в команду при выполнении
        /// </summary>
        public TParameter Parameters { get; set; }

        /// <summary>
        /// Результат выполнения команды
        /// Например здесь может храниться список объектов, которые были изменены
        public TResult Result { get; set; }
        /// </summary>


        /// <summary>
        /// Клонирование текущей команды, для записи в стек выполненных или отмененных команд
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
          
            return new Command<TParameter, TResult>(_execute, _unExecute, OnExecute)
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
            Parameters = parameter.Cast<TParameter>();

            //Выполняем команду и запоминаем результат ( чтобы можно было выполнить отмену именно для этого результата)
            Result = this._execute(Parameters, Result).Cast<TResult>();

            //Добавляем копию команды в стек команд, которые можно отменить
            ICommandWithUndoRedo.AddInUndo(this.Clone() as ICommandWithUndoRedo);

            //Очищаем список отмененнных команд ( началась новая ветка изменений)
            ICommandWithUndoRedo.StackRedo.Clear();

            //Очищаем результат ( чтобы не передавать его при повторном выполнении)
            Result = default(TResult);

            //Очищаем параметр ( чтобы не передавать его при повторном выполнении)
            Parameters = default(TParameter);

            OnExecute?.Invoke();
        }

        /// <summary>
        /// Отмена команды
        /// </summary>
        public  void UnExecute()
        {         
            //Выполняем отмену команду
            this._unExecute(Parameters, Result);

            //Добавляем копию команды в стек команд, которые можно выполнить повторно
            ICommandWithUndoRedo.AddInRedo(this.Clone() as ICommandWithUndoRedo);
        }

        /// <summary>
        /// Повторное выполнения команды
        /// </summary>
        public  void ExecuteWithSubscribe()
        {
            //Выполянем команду
            this.Result = this._execute(this.Parameters, this.Result);

            //Добавляем копию команды в стек команд, которые можно отменить
            ICommandWithUndoRedo.AddInUndo(this.Clone() as ICommandWithUndoRedo);
        }

        /// <summary>
        /// Создать отменяемую команду
        /// </summary>
        /// <param name="owner">Объкт, которому принадлежит команда</param>
        /// <param name="ExecuteWithSubscribe">Функция, которая будет вызвана при выполнении команды</param>
        /// <param name="unExecute">Функция, которая будет вызвана при отмене команды</param>
        public Command(Func<TParameter, TResult, TResult> ExecuteWithSubscribe, Func<TParameter, TResult, TResult> unExecute, Action onExecute = null)
        {
            _execute = ExecuteWithSubscribe;

            _unExecute = unExecute;

            OnExecute += onExecute;

            
        }
    }
}
