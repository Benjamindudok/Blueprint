﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint
{
    class Program
    {
        public static string BlueprintAction = "manufacture";
        public static string SourceFolder = "O:\\_temp\\blueprint-test";
        public static string DestinationFolder = "O:\\_temp\\blueprint-test\\www\\";

        static void Main(string[] args)
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

            // check if destination folder exists
            if (DestinationFolder != "" && !Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }

            // check if source folder exists
            if (Directory.Exists(SourceFolder))
            {
                // start processing directies/files
                SourceBrowser.ProcessDirectory(SourceFolder, DestinationFolder);
            }

            Console.WriteLine("Process completed, press a key to exit");
            Console.ReadLine();
        }
    }
}
