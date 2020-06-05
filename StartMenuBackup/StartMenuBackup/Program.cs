using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StartMenuBackup
{
    class Program
    {
        private static string FolderPath = "";
        private static string DestPath = "<BACKUP PATH HERE>";

        static void Main(string[] args)
        {
            if (!Directory.Exists(DestPath))
            {
                Directory.CreateDirectory(DestPath);
            }

            //https://www.c-sharpcorner.com/blogs/practical-approach-of-getting-special-folders-path-in-the-environment-using-c-sharp1
            //Getting all the Special folders
            string[] specialPaths = Enum.GetNames(typeof(Environment.SpecialFolder));

            //Iterating the Special folders to get paths of sepcial folders
            foreach (string special in specialPaths)
            {
                //Getting the Special folder enum by its name.
                Environment.SpecialFolder specialFolder = (Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), special);
                //Displaying the Special Folder.
                if (specialFolder.Equals(Environment.SpecialFolder.Programs))
                {
                    FolderPath = Environment.GetFolderPath(specialFolder);
                    break;
                }
            }

            DirectoryInfo d = new DirectoryInfo(@"" + FolderPath);

            foreach (DirectoryInfo folder in d.GetDirectories("Sombra - *"))
            {
                Console.WriteLine("Copying " + folder.FullName + "...");
                Console.WriteLine("----------------------------------");
                string destination = DestPath + Path.DirectorySeparatorChar + folder.Name;
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                else
                {
                    Console.WriteLine("Backup exists. Removing backup folder " + folder.FullName + "...");
                    Directory.Delete(destination,true);
                    Directory.CreateDirectory(destination);
                }
                Copy(folder.FullName, destination);
                Console.WriteLine("----------------------------------");
            }
        }

        //https://code.4noobz.net/c-copy-a-folder-its-content-and-the-subfolders/
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
