using System;
using System.IO;
using System.Text;
using System.Reflection;

using ICSharpCode.SharpZipLib.Zip;

namespace CKAN
{
    public class CrashInformation
    {
        public static void DumpToFile(Exception ex)
        {
            // Get the executing assembly.
            string codebase = Assembly.GetExecutingAssembly().CodeBase;

            // Get the directory.
            UriBuilder assembly_uri = new UriBuilder(codebase);
            string assembly_path = Uri.UnescapeDataString(assembly_uri.Path);
            string assembly_dir = Path.GetDirectoryName(assembly_path);

            string format = "dd-MM-yyyy-HH-mm";
            string file_name = DateTime.Now.ToString(format) + ".zip";
            string output_file = Path.Combine(assembly_dir, file_name);

            // Check if the exception contains the KSP instance.
            KSP ksp = null;

            if (ex is Kraken)
            {
                ksp = ((Kraken)ex).ksp;
            }

            System.Console.WriteLine(output_file);

            // Create a new zipfile.
            using(ZipOutputStream stream = new ZipOutputStream(File.Create(output_file)))
            {
                byte[] buffer;

                // Store the exception data.
                ZipEntry exception_entry = new ZipEntry("Exception.txt");
                exception_entry.DateTime = DateTime.Now;
                buffer = Encoding.UTF8.GetBytes(ex.ToString());

                stream.PutNextEntry(exception_entry);
                stream.Write(buffer, 0, buffer.Length);
                stream.CloseEntry();

                if (ksp != null)
                {
                    // Store the registry.
                    ZipEntry registry_entry = new ZipEntry("Registry.json");
                    registry_entry.DateTime = DateTime.Now;
                    string serialized_registry = RegistryManager.Instance(ksp).Serialize();
                    buffer = Encoding.UTF8.GetBytes(serialized_registry);

                    stream.PutNextEntry(registry_entry);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.CloseEntry();
                }

                // Store the system registry.
                ZipEntry system_registry_entry = new ZipEntry("SystemRegistry.txt");
                system_registry_entry.DateTime = DateTime.Now;
                string serialized_system_registry = Win32Registry.Serialize();
                buffer = Encoding.UTF8.GetBytes(serialized_system_registry);

                stream.PutNextEntry(system_registry_entry);
                stream.Write(buffer, 0, buffer.Length);
                stream.CloseEntry();
            }
        }
    }
}
