using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class BindingExtensions
    {
        public static IDisposable WhenViewModelAnyValue(this IViewFor view, Action<CompositeDisposable> block)
        {
            return view.WhenActivated(disposable =>
            {
                view.WhenAnyValue(x => x.ViewModel).Where(x=>x!=null).Subscribe(_=>block.Invoke(disposable)).DisposeWith(disposable);
            });
        }
    }
}
