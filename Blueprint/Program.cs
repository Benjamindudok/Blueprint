using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Models;
using Newtonsoft.Json;

namespace Blueprint
{
    public class Program
    {
        public static Config Config;
        public static string BlueprintAction;
        public static string SourceFolder;
        public static string DestinationFolder;

        static void Main(string[] args)
        {
            // use relative path in release builds if no arguments given.
            #if (DEBUG)
                const string relativePath = "O:\\_temp\\blueprint-test";
            #else
                string relativePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                relativePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            #endif

            // check program arguments
            if (args.Length < 1)
            {
                // set overrides for debug mode or defaults
                BlueprintAction = "manufacture";
                SourceFolder = relativePath;
                DestinationFolder = relativePath + "\\_www\\";
            } 
            else
            {
                // check which arguments are present
                foreach (string argument in args)
                {
                    int index = Array.IndexOf(args, argument);

                    // assign arguments to properties
                    if (index == 0)
                    {
                        BlueprintAction = argument;
                    }
                    else if (index == 1 && index != (args.Length - 1))
                    {
                        SourceFolder = argument;
                    }
                    else if (index == 1 && index == (args.Length - 1))
                    {
                        DestinationFolder = argument;
                    }
                    else if (index == 2)
                    {
                        DestinationFolder = argument;
                    }
                }
            }

            // Load config from json
            Config = new Config();
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(SourceFolder + "\\_config\\config.json"));

            // check if destination folder exists
            if (DestinationFolder != "" && !Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }

            // check if source folder exists
            if (Directory.Exists(SourceFolder))
            {
                // start processing directies/files
                SourceBrowser processor = new SourceBrowser();
                processor.ProcessDirectory(SourceFolder);

                foreach (SourceFile file in Config.Files)
                {
                    
                    if (file.Type == "post")
                    {
                        string destination = file.GetDirectoryStructureForPosts(file.FileName);
                        file.Render(file.Content, destination + file.FileName + ".html", true);
                    }
                    else
                    {
                        // TODO make destination folder keep source path in mind
                        string destination = file.DestinationPath;
                        file.Render(file.Content, destination + file.FileName + ".html", false);
                    }

                }

                Console.WriteLine("Process completed, press a key to exit");
            } else
            {
                Console.WriteLine("Source folder does not exist.");
                Console.WriteLine("Please specify the correct source folder.");
            }

            Console.ReadLine();
        }
    }
}
