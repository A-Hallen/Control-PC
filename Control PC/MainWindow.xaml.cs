using System;
using System.Windows;
using InTheHand.Net.Sockets;
using VentanaBase;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Control_PC
{
    public partial class MainWindow : WindowBase 
    {
        private Connection connection;
        private VisualControl visualControl;

        private ObservableCollection<DeviceItem> deviceItems = new ObservableCollection<DeviceItem>();

       public MainWindow() {
            InitializeComponent();
            visualControl = new VisualControl(
                BorderListView,
                SelectorPanel,
                ResultGrid, 
                ConnectedBtn, 
                ResultProgress,
                Dispatcher);
            connection = new Connection(visualControl);
            DevicesListView.ItemsSource = deviceItems;
        }

        private void Selector_Checked(object sender, RoutedEventArgs e)
        {
            if(Selector.IsChecked== true)
            {
                connection.StartSearch((BluetoothDeviceInfo[] devices) =>
                {
                    BorderListView.Visibility = Visibility.Visible;
                    addDevicesToList(devices);
                });
            } else {
                BorderListView.Visibility = Visibility.Collapsed;
                connection.StopSearch();
                deviceItems.Clear();
            }
        }

        private void addDevicesToList(BluetoothDeviceInfo[] devices)
        {
            deviceItems.Clear();

            foreach(BluetoothDeviceInfo device in devices) {
                deviceItems.Add(new DeviceItem(device.DeviceName, device.DeviceAddress));
            }
            
        }
        

        private void DevicesListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Collections.IList items = e.AddedItems;
            if(items.Count > 0) {
                connection.StopSearch();
                visualControl.SetLoading();
                connection.Connect((DeviceItem)items[0]);
            }
        }

        private void ConnectedBtn_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            connection.Disconnect();
        }
    }
}
