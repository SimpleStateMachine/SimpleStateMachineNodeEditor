using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;


using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleStateMachineNodeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();



            Header.PreviewMouseLeftButtonDown += HeaderClick;
            ButtonClose.Click += ButtonCloseClick;
            ButtonMin.Click += ButtonMinClick;
            ButtonMax.Click += ButtonMaxClick;


            //var str = new StringBuilder();
            //using (var writer = new StringWriter(str))
            //    XamlWriter.Save(MI1.Template, writer);
            //Debug.Write(str);

        }
        void StateNormalMaximaze()
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
        void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void ButtonMinClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        void ButtonMaxClick(object sender, RoutedEventArgs e)
        {
            StateNormalMaximaze();
        }

        private void HeaderClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DockPanel)
            {
                if (e.ClickCount == 1)
                {

                    if (this.WindowState == WindowState.Maximized)
                    {
                        var point = PointToScreen(e.MouseDevice.GetPosition(this));

                        if (point.X <= RestoreBounds.Width / 2)
                            Left = 0;
                        else if (point.X >= RestoreBounds.Width)
                            Left = point.X - (RestoreBounds.Width - (this.ActualWidth - point.X));
                        else
                            Left = point.X - (RestoreBounds.Width / 2);

                        Top = point.Y - (((FrameworkElement)sender).ActualHeight / 2);
                        WindowState = WindowState.Normal;
                    }

                    this.DragMove();
                }
                else
                {
                    StateNormalMaximaze();
                }
                e.Handled = true;
            }
        }
    }
}
