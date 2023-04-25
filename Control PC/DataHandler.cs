using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Control_PC
{
    internal class DataHandler
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private MouseSimulator mouseSimulator;

        const byte LEFT_ARROW_KEY = 0x25; // Código de la tecla de la flecha izquierda
        const byte RIGHT_ARROW_KEY = 0x27; // Código de la tecla de la flecha derecha
        const int KEY_DOWN_EVENT = 0x0000; // Evento de pulsación de tecla
        const int KEY_UP_EVENT = 0x0002; // Evento de liberación de tecla

        // Ancho y alto de la pantalla principal
        private int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
        private int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;

        

        private Point cursorPosition;

        public DataHandler()
        {
            mouseSimulator = new MouseSimulator();
        }

        private void down() {
            cursorPosition = Cursor.Position;
        }
        private void motion(string[] split) {
            float x = float.Parse(split[1]);
            float y = float.Parse(split[2]);
            if (x == 0 || y == 0) return;
            int positionx = cursorPosition.X;
            int positiony = cursorPosition.Y;
            double x_move = x * screenWidth;
            double y_move = y * screenHeight;
            Point newPos = new Point(positionx + (int)x_move, positiony + (int)y_move);
            Cursor.Position = newPos;
        }
        private void click() {
            mouseSimulator.click();
        }
        private void doubleClick() {
            mouseSimulator.doubleClick();
        }
        private void scroll(int scrollAmount) {
            mouseSimulator.scroll(scrollAmount);
        }
        private void startDrag() {
            mouseSimulator.pressLeft();
        }
        private void endDrag() {
            mouseSimulator.releaseLeft();
        }
        private void left() {
            // Envía la tecla de la flecha izquierda a la aplicación activa
            keybd_event(LEFT_ARROW_KEY, 0, KEY_DOWN_EVENT, UIntPtr.Zero);
            keybd_event(LEFT_ARROW_KEY, 0, KEY_UP_EVENT, UIntPtr.Zero);
        }
        private void right() {
            // Envía la tecla de la flecha derecha a la aplicación activa
            keybd_event(RIGHT_ARROW_KEY, 0, KEY_DOWN_EVENT, UIntPtr.Zero);
            keybd_event(RIGHT_ARROW_KEY, 0, KEY_UP_EVENT, UIntPtr.Zero);
        }
        private void manageData(string data)
        {
            string[] split = data.Split(',');
            if (split.Length != 3) return; // si el indice es distinto de 3 no es un comando
            switch (split[0])
            {
                case "DOWN":
                    down();
                    break;
                case "MOTION":
                    motion(split);
                    break;
                case "CLICK":
                    click();
                    break;
                case "DOUBLE_CLICK":
                    doubleClick();
                    break;
                case "SCROLL":
                    scroll(int.Parse(split[1]));
                    break;
                case "START_DRAG":
                    startDrag();
                    break;
                case "END_DRAG":
                    endDrag();
                    break;
            }
        }
        public void ProcessData(string data)
        {
            switch (data)
            {
                case "left":
                    left();
                    break;
                case "right":
                    right();
                    break;
                default:
                    manageData(data);
                    break;
            }
        }

        
    }
}
