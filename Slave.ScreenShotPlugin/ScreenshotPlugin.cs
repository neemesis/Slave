using System;
using System.Windows.Forms;
using Slave.Framework.Components;
using Slave.Framework.Interfaces;

namespace Slave.ScreenShotPlugin {
    public class ScreenshotPlugin : IMaster {
        private string _alias;
        private Shortcut _hotKey;

        public ScreenshotPlugin() {
            _alias = "screenshot";
            _hotKey = Shortcut.ShiftF1;
        }

        void IMaster.Initialize() {

        }

        void IMaster.Execute(string[] args, Action<string> display) {
            var hwnd = (IntPtr)NativeWin32.GetForegroundWindow();


            var image = Capture.GrabWindow(hwnd);
            var dialog = new SaveFileDialog {
                AddExtension = true,
                DefaultExt = "*.png",
                Filter = "Png files (*.png)|*.png",
                Title = "Save the screenshot..."
            };

            if (dialog.ShowDialog() == DialogResult.OK) {
                image.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        string IMaster.Alias {
            get => _alias;
            set => _alias = value;
        }

        Shortcut IMaster.HotKey {
            get => _hotKey;
            set => _hotKey = value;
        }
        public IParse Parser { get; set; }
        string IMaster.Name => "Screenshot maker";

        string IMaster.Description => "Take a screenshot of the current windows and save it as a PNG file";

        string IMaster.Author => "John Roland";

        string IMaster.Version => "1.0";
        public string HelpUrl => "https://github.com/neemesis/Slave/blob/master/Slave.ScreenShotPlugin/README.MD";
    }
}
