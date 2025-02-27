﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFMpegCore;
using VoiceTexterBot.Extensions;

namespace VoiceTexterBot.Utilities
{
    internal class AudioConverter
    {
        public static void TryConvert(string inputFile, string outputFile)
        {
            GlobalFFOptions.Configure(options => options.BinaryFolder =
            Path.Combine(DirectoryExtensions.GetSolutionRoot(), "ffmpeg-win64", "bin"));

            FFMpegArguments.FromFileInput(inputFile).OutputToFile(outputFile,true, 
                options => options.WithFastStart()).ProcessSynchronously();
        }

        private static string GetSolutionRoot()
        {
            var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            var fullname = Directory.GetParent(dir).FullName;
            var projectRoot = fullname.Substring(0, fullname.Length - 4);
            return Directory.GetParent(projectRoot)?.FullName;
        }
    }
}
