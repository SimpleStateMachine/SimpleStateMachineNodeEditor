using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Windows;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelMainWindow : ReactiveObject
    {
        public ObservableCollectionExtended<ViewModelMessage> Messages { get; set; } = new ObservableCollectionExtended<ViewModelMessage>();

        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public TypeMessage DisplayMessageType { get; set; }
        [Reactive] public bool? DebugEnable { get; set; } = true;

        [Reactive] public int CountError { get; set; }
        [Reactive] public int CountWarning { get; set; }
        [Reactive] public int CountInformation { get; set; }
        [Reactive] public int CountDebug { get; set; }

        public Theme CurrentTheme { get; set; } = Theme.Dark;
        static Dictionary<Theme, string> themesPaths = new Dictionary<Theme, string>()
        {
            {Theme.Dark, @"Styles\Themes\Dark.xaml" },
            {Theme.Light, @"Styles\Themes\Light.xaml"},
        };

        private IDisposable ConnectToMessages;
        public double MaxHeightMessagePanel = 150;



        public ViewModelMainWindow(ViewModelNodesCanvas viewModelNodesCanvas)
        {
            NodesCanvas = viewModelNodesCanvas;
            SetupCommands();
            SetupSubscriptions();
        }

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.NodesCanvas.DisplayMessageType).Subscribe(_ => UpdateMessages());
            this.WhenAnyValue(x => x.NodesCanvas.Messages.Count).Subscribe(_ => UpdateCountMessages());

        }
        private void UpdateMessages()
        {
            ConnectToMessages?.Dispose();
            Messages.Clear();

            bool debugEnable = DebugEnable.HasValue && DebugEnable.Value;
            bool displayAll = this.NodesCanvas.DisplayMessageType == TypeMessage.All;

            ConnectToMessages = this.NodesCanvas.Messages.ToObservableChangeSet().Filter(x => CheckForDisplay(x.TypeMessage)).ObserveOnDispatcher().Bind(Messages).DisposeMany().Subscribe();

            bool CheckForDisplay(TypeMessage typeMessage)
            {
                if (typeMessage == this.NodesCanvas.DisplayMessageType)
                {
                    return true;
                }
                else if (typeMessage == TypeMessage.Debug)
                {
                    return debugEnable && displayAll;
                }

                return displayAll;
            }
        }
        #endregion Setup Subscriptions
        #region Setup Commands

        public ReactiveCommand<string, Unit> CommandCopyError { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCopySchemeName{ get; set; }
        public ReactiveCommand<Unit, Unit> CommandChangeTheme { get; set; }
        private void SetupCommands()
        {
            CommandCopyError = ReactiveCommand.Create<string>(CopyError);
            CommandCopySchemeName = ReactiveCommand.Create(CopySchemeName);
            CommandChangeTheme = ReactiveCommand.Create(ChangeTheme);
        }

        private void CopyError(string errrorText)
        {
            Clipboard.SetText(errrorText);
        }
        private void CopySchemeName()
        {
            Clipboard.SetText(this.NodesCanvas.SchemePath);
        }
        private void ChangeTheme()
        {
            if (CurrentTheme == Theme.Dark)
            {
                SetTheme(Theme.Light);
            }
            else if (CurrentTheme == Theme.Light)
            {
                SetTheme(Theme.Dark);
            }
           
        }
        private void SetTheme(Theme theme)
        {
            Application.Current.Resources.Clear();
            var uri = new Uri(themesPaths[theme], UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;         
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            CurrentTheme = theme;
            LoadIcons();
        }
        private void LoadIcons()
        {
            string path = @"Icons\Icons.xaml";
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
        private void UpdateCountMessages()
        {          
            var counts =  NodesCanvas.Messages.GroupBy(x => x.TypeMessage).ToDictionary(x => x.Key, x => x.Count());
            CountError = counts.Keys.Contains(TypeMessage.Error) ? counts[TypeMessage.Error] : 0;
            CountWarning = counts.Keys.Contains(TypeMessage.Warning) ? counts[TypeMessage.Warning] : 0;
            CountInformation = counts.Keys.Contains(TypeMessage.Information) ? counts[TypeMessage.Information] : 0;
            CountDebug = counts.Keys.Contains(TypeMessage.Debug) ? counts[TypeMessage.Debug] : 0;
        }

        #endregion Setup Commands

    }
}
