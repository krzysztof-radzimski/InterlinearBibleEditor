using DevExpress.LookAndFeel;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace ChurchServices.WinApp {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            UserLookAndFeel.Default.SetSkinStyle(SkinSvgPalette.WXICompact.Darkness);
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