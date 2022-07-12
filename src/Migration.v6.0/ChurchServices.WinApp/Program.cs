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
    }
}