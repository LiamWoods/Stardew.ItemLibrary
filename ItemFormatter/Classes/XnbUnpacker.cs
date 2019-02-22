using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemFormatter.Classes
{
    class XnbUnpacker
    {
        const string objInfoFileName = "ObjectInformation.xnb";

        public string SteamPath { get; private set; }
        public string XnbCliPath { get; private set; }

        public XnbUnpacker(string steamPath, string xnbCliPath)
        {
            SteamPath = steamPath;
            XnbCliPath = xnbCliPath;
        }

        public bool Unpack()
        {
            var outputPath = Path.Combine(XnbCliPath, "packed", objInfoFileName);
            File.Delete(outputPath);
            File.Copy(Path.Combine(SteamPath, objInfoFileName), outputPath, true);

            var packedDir = Path.Combine(XnbCliPath, "packed");
            var unpackedDir = Path.Combine(XnbCliPath, "unpacked");

            var xnbExe = Path.Combine(XnbCliPath, "xnbcli.exe");
            var processInfo = new ProcessStartInfo(xnbExe);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.Arguments = $@"unpack ""{packedDir}"" ""{unpackedDir}""";
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var unpacker = Process.Start(processInfo);

            unpacker.Start();
            unpacker.WaitForExit();

            string output = unpacker.StandardOutput.ReadToEnd();
            string error = unpacker.StandardError.ReadToEnd();

            unpacker.Close();

            return output.IndexOf("Success 1") > -1 && string.IsNullOrEmpty(error);
        }
    }
}
