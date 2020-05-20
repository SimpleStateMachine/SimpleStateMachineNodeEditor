using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class IObservableExtension
    {
        public static IObservable<Unit> WithoutParameter<TDontCare>(this IObservable<TDontCare> source)
        {
            return source.Select(_ => Unit.Default);
        }
    }
}
