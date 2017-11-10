﻿using System.Diagnostics;
using System.IO;

namespace QuickOverTool_WPF
{
    class Validation
    {
        // Validate Overwatch and return its version and branch
        public static string[] Overwatch(string path)
        {
            string version;
            string branch = "Live";
            try
            {
                string pdbPath = Path.Combine(path, ".product.db");
                StreamReader pdbStream = new StreamReader(pdbPath);
                using (var reader = File.OpenText(pdbPath))
                {
                    string pdbRead = pdbStream.ReadLine();
                    while (pdbStream.Peek() >= 0)
                    {
                        pdbRead = pdbStream.ReadLine();
                        if (pdbRead.Contains("prometheus_test")) branch = "PTR";
                        if (pdbRead.EndsWith("B&")) break;
                    }
                    version = pdbRead.Substring(pdbRead.IndexOf("1."), 14);
                    /*
                    pdbStream.BaseStream.Position = 0;
                    pdbStream.DiscardBufferedData();
                    while (pdbStream.Peek() >= 0)
                    {
                        pdbRead = pdbStream.ReadLine();
                        if (pdbRead.Contains("prometheus"))
                        {
                            labelOverwatchBranch.Content = "正式服";
                            pdbStream.Close();
                            break;
                        }
                    }
                    */
                }
                return new string[] { version, branch };
            }
            catch
            {
                // If .product.db does not even exist
                return null;
            }
        }
        // Validate DataTool and ow.keys
        public static string[] DataTool(string path)
        {
            string version;
            string keys = null;
            try
            {
                version = FileVersionInfo.GetVersionInfo(
                    Path.Combine(path, "DataTool.exe"))
                    .FileVersion;
            }
            catch
            {
                version = null;
            }
            if (File.Exists(
                Path.Combine(path, "ow.keys")))
                keys = "ok";
            return new string[] { version, keys };
        }
    }
}
