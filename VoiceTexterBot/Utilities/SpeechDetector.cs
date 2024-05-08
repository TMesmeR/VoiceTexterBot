﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTexterBot.Extensions;
using Vosk;

namespace VoiceTexterBot.Utilities
{
    internal class SpeechDetector
    {

        public static string DetectSpeech (string audioPath, float inputBitrate,
            string languageCode)
        {
            Vosk.Vosk.SetLogLevel(0);

            var modelPath = Path.Combine(DirectoryExtensions.GetSolutionRoot(),
                "Speech-models", $"vosk-model-small-{languageCode.ToLower()}");
            Model model = new Model(modelPath);
            return  GetWords(model,audioPath, inputBitrate);
        }

        private static string GetWords(Model model, string audioPath, float inputBitrate)
        {
            VoskRecognizer rec = new VoskRecognizer(model, inputBitrate);
            rec.SetMaxAlternatives(0);
            rec.SetWords(true);

            StringBuilder textBuffer = new ();

            using (Stream source = File.OpenRead(audioPath))
            {
                byte[] buffer = new byte[4096];
                int bytesRead ;

                while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (rec.AcceptWaveform(buffer, bytesRead))
                    {
                        var sentenceJson = rec.Result();

                        JObject sentenceObj = JObject.Parse(sentenceJson);
                        string sentence = (string)sentenceObj["text"];

                        textBuffer.Append(StringExtensions.UppercaseFirst(sentence) + ". ");
                    }
                }
            }

            var finalSentence = rec.FinalResult();
            JObject finalSentenceObj = JObject.Parse(finalSentence);
            textBuffer.Append((string)finalSentenceObj["text"]);
            return textBuffer.ToString();

        }
    }
}
