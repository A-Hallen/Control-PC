using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;

namespace Control_PC
{
    internal class VisualControl
    {
        private Border borderListView;
        private StackPanel selectorPanel;
        private Card resultGrid;
        private Card connectedBtn;
        private ProgressBar resultProgress;
        Dispatcher dispacher;


        public VisualControl(Border border, StackPanel stack, Card card1, Card card2, ProgressBar progress, Dispatcher dispacher)
        {
            this.borderListView = border;
            this.selectorPanel = stack;
            this.resultGrid = card1;
            this.connectedBtn = card2;
            this.resultProgress = progress;
            this.dispacher = dispacher;
        }

        public void SetLoading()
        {
            borderListView.Visibility = Visibility.Collapsed;
            selectorPanel.Visibility = Visibility.Collapsed;
            resultGrid.Visibility = Visibility.Visible;
            resultProgress.Visibility = Visibility.Visible;
        }

        public void SetVisible()
        {
            dispacher.Invoke(new Action(() => {

                resultProgress.Visibility = Visibility.Collapsed;
                resultGrid.Visibility = Visibility.Collapsed;
                connectedBtn.Visibility = Visibility.Collapsed;
                borderListView.Visibility = Visibility.Visible;
                selectorPanel.Visibility = Visibility.Visible;
            }));
        }

        public void IsConnected()
        {
            dispacher.Invoke(new Action(() => {
                resultGrid.Visibility = Visibility.Collapsed;
                resultProgress.Visibility = Visibility.Collapsed;
                connectedBtn.Visibility = Visibility.Visible;
            }));
        }
    }
}
