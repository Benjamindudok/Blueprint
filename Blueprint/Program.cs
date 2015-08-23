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
        public static Config Variable;
        public static string BlueprintAction = "manufacture";
        public static string SourceFolder = "D:\\Projects\\www\\blueprint-test";
        public static string DestinationFolder = "D:\\Projects\\www\\blueprint-test\\_www\\";

        static void Main(string[] args)
        {
            // Load config from json
            Variable = new Config();
            Variable = JsonConvert.DeserializeObject<Config>(File.ReadAllText(SourceFolder + "\\_config\\config.json"));

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

            // check if destination folder exists
            if (DestinationFolder != "" && !Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }

            // check if source folder exists
            if (Directory.Exists(SourceFolder))
            {
                // start analyzing directies/files
                SourceBrowser.AnalyzeDirectory(SourceFolder);

                Console.WriteLine("Currently stored posts");
                foreach (Post post in Variable.Posts)
                {
                    Console.WriteLine(post.Title);
                }

                Console.WriteLine("Currently stored pages");
                foreach (Page page in Variable.Pages)
                {
                    Console.WriteLine(page.Title);
                }

                // start processing directies/files
                SourceBrowser.ProcessDirectory(SourceFolder);

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
