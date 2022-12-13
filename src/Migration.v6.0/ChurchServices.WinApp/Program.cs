using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace ChurchServices.WinApp {
    internal static class Program {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        public static byte[] GetResource(string name) {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resourcePath in assembly.GetManifestResourceNames()) {
                if (resourcePath == $"ChurchServices.WinApp.Resources.{name}") {
                    using var stream = assembly.GetManifestResourceStream(resourcePath);
                    using BinaryReader br = new(stream);
                    return br.ReadBytes((int)stream.Length);
                }
            }
            return default;
        }

        public static void SafeInvoke<T>(this T isi, Action<T> call) where T : ISynchronizeInvoke {
            if (isi.InvokeRequired) isi.BeginInvoke(call, new object[] { isi });
            else
                call(isi);
        }
    }
}