using System.Collections.Generic;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Input;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        protected override void SetupBinding()
        {
     
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Header, x => x.HeaderNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Connectors, x => x.ConnectorsNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Input, x => x.InputNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Output, x => x.OutputNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Header.IsCollapse, x => x.ConnectorsNode.IsVisible, x=>!x).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Header.IsCollapse, x => x.OutputNode.IsVisible).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x=>x.Point1.X, x=>x.TranslateTransformNode.X).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Point1.Y, x => x.TranslateTransformNode.Y).DisposeWith(disposable);
            });
        }

        protected virtual void SetupEvents()
        {
            //var task = Test();
            //task.Wait();
            this.WhenViewModelAnyValue(disposable =>
            {
              
                //this.Events().PointerPressed.Subscribe(_=Mou.ca)
                //this.WhenAnyValue(x => x.IsMouseOver).Subscribe(value => OnEventMouseOver(value)).DisposeWith(disposable);
                //this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDowns(e)).DisposeWith(disposable);
                //this.Events().PointerPressed.Subscribe(_= this.ViewModel.Position = this.ViewModel.Position + new Point(10,10)).DisposeWith(disposable);
                //this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                //this.Events().MouseMove.Subscribe(e => OnMouseMove(e)).DisposeWith(disposable);

                //this.NodeHeaderElement.ButtonCollapse.Events().Click.Subscribe(_ => ViewModel.IsCollapse = !ViewModel.IsCollapse).DisposeWith(disposable);
                //this.NodeHeaderElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                //this.ViewModel.WhenAnyValue(x => x.IsCollapse).Subscribe(value => OnEventCollapse(value)).DisposeWith(disposable);
            });
        }

        public async Task Test()
        {
            Subject<bool> _canExecuteSubject = new Subject<bool>();
            IScheduler _scheduler = Scheduler.Immediate;
            int number = 0;
            
            ICommand command = ReactiveCommand.Create((int x) => number += x, canExecute: _canExecuteSubject, _scheduler); 
            
            List<bool> canExecuteChanged = new List<bool>();
            command.CanExecuteChanged += (s, e) => canExecuteChanged.Add(command.CanExecute(null));
            var count = 0;
            await _canExecuteSubject.FirstAsync();
            _canExecuteSubject.OnNext(true);
            _canExecuteSubject.OnCompleted();
            count = canExecuteChanged.Count;
            
            
            // var myTask = _canExecuteSubject.FirstAsync().ToTask();
            // _canExecuteSubject.OnNext(true);
            // var count = canExecuteChanged.Count;
            // var b = await myTask;
            // // _canExecuteSubject.OnCompleted();
            // count = canExecuteChanged.Count;
        }
    }
}
