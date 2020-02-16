using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using StateMachineNodeEditorNerCore.Helpers;
using StateMachineNodeEditorNerCore.Helpers.Commands;

namespace StateMachineNodeEditorNerCore.ViewModel
{
    public class ViewModelCutter : ReactiveObject
    {
        /// <summary>
        /// Отображается ли линия среза
        /// </summary>
        [Reactive] public bool? Visible { get; set; } = false;

        /// <summary>
        /// Точка из которой выходит линия среза
        /// </summary>
        [Reactive] public MyPoint StartPoint { get; set; } = new MyPoint();

        /// <summary>
        /// Точка в которую приходит линия среза
        /// </summary>
        [Reactive] public MyPoint EndPoint { get; set; } = new MyPoint();

        public ViewModelCutter()
        {
            SetupCommands();
        }
        #region Setup Commands
        public SimpleCommandWithParameter<MyPoint> CommandStartCut { get; set; }
        public SimpleCommand CommandEndCut { get; set; }

        private void SetupCommands()
        {
            CommandStartCut = new SimpleCommandWithParameter<MyPoint>(this, StartCut);
            CommandEndCut = new SimpleCommand(this, EndCut);
        }

        private void StartCut(MyPoint point)
        {
            Visible = true;
            StartPoint.Set(point);
        }
        private void EndCut()
        {

        }

        #endregion Setup Commands
    }
}
