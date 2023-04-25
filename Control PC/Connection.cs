using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using InTheHand.Net;
using InTheHand.Net.Sockets;

namespace Control_PC
{
    internal class Connection
    {
        private const string uuid = "00001101-0000-1000-8000-00805F9B34FB";

        // decide si se deberia detener el escaneo de dispositivos
        private Boolean stopScan = false;
        // decide si se deberia parar de escuchar
        private Boolean stopListen = false;
        // Cliente bluetooth
        private BluetoothClient client;
        // Socket Bluetooth
        private Socket socket;
        // El que maneja los datos que llegan desde el movil
        private readonly DataHandler dataHandler;

        // Clase que maneja las vistas
        private readonly VisualControl visualControl;
        

        public Connection(VisualControl visualControl)
        {
            this.visualControl = visualControl;
            dataHandler = new DataHandler();
        }


        // Comenzar a escuchar las conexiones bluetooth
        public delegate void SearchCallback(BluetoothDeviceInfo[] devices);

        private async Task ScanDevicesAsync(SearchCallback callbakc)
        {
            while (!stopScan)
            {
                var client = new BluetoothClient();
                var ar = client.BeginDiscoverDevices(255, true, false, false, false, null, null);
                var devices = await Task.Factory.FromAsync(ar, client.EndDiscoverDevices);
                callbakc(devices);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        public void StartSearch(SearchCallback callback)
        {
            stopScan = false;
            _ = ScanDevicesAsync(callback);
        }


        // Parar las conexiones bluetooth
        public void StopSearch() 
        {
            stopScan = true;
        }

        internal void Connect(DeviceItem device)
        {
            var hilo = new Thread(new ParameterizedThreadStart(StartThread))
            {
                IsBackground = true
            };
            hilo.Start(device);
            //Task task = StartConnection(device.DeviceAdress);
        }

        private void StartThread(object argumentos)
        {
            DeviceItem device = (DeviceItem)argumentos;
            Start_Connection(device.DeviceAdress);
        }

        public void Disconnect()
        {
            stopListen = true;
            client?.Close();
            socket?.Close();
            visualControl.SetVisible();
        }

        private void Start_Connection(BluetoothAddress address)
        {

            client = new BluetoothClient();
            Guid serviceUuid = new Guid(uuid);

            BluetoothEndPoint endPoint = new BluetoothEndPoint(address, serviceUuid);

            try
            {
                client.Connect(endPoint);
                socket = client.Client;
                visualControl.IsConnected();
                while (!stopListen)
                {
                    byte[] buffer = new byte[client.Available];
                    socket.Receive(buffer);
                    string data = Encoding.ASCII.GetString(buffer);
                    // Procesar los datos recibidos...
                    ProcessData(data);
                }
            } catch (SocketException)
            {

            } catch (Exception ex) { 
                Disconnect();
                
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void ProcessData(string data)
        {
            if (data == "close") Disconnect();
            dataHandler.ProcessData(data);
        }
    }
}
