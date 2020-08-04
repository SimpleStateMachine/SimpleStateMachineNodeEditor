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

        public static IReactiveBinding<TView, TViewModel, TVProp> OneWayBind2<TViewModel, TView, TVMProp, TVProp>(this TView view, Expression<Func<TViewModel, TVMProp>> vmProperty, Expression<Func<TView, TVProp>> viewProperty, object conversionHint = null, IBindingTypeConverter vmToViewConverterOverride = null)
            where TViewModel : class
            where TView : class, IViewFor<TViewModel>
        {
            return view.OneWayBind<TViewModel, TView, TVMProp, TVProp>(view.ViewModel, vmProperty, viewProperty, conversionHint, vmToViewConverterOverride);
        }

        public static IReactiveBinding<TView, TViewModel, TOut> OneWayBind5<TViewModel, TView, TProp, TOut>(this TView view, Expression<Func<TViewModel, TProp>> vmProperty, Expression<Func<TView, TOut>> viewProperty)
            where TViewModel : class
            where TView : class, IViewFor<TViewModel>
        {
            return view.OneWayBind(view.ViewModel, vmProperty, viewProperty, null);
        }
    }
}
