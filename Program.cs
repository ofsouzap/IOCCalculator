using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace IOCCalculator
{

    class Program
    {

        public static readonly char[] englishCharacters = new char[]
        {
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z'
        };

        static void Main(string[] args)
        {

            string text;
            bool normalizeCase;
            bool onlyUseEnglishChars;
            List<SampleReadingOptions> readingOptions = new List<SampleReadingOptions>();
            List<string> textSamples = new List<string>();

            Console.WriteLine("Text:");
            string input = Console.ReadLine();

            if (input[0] == '\\')
            {
                text = GetTextFromFile(input.Substring(1));
            }
            else
            {
                text = input;
            }

            Console.Write("Normalize Case (0/1)?> ");
            normalizeCase = int.Parse(Console.ReadLine()) != 0;

            if (normalizeCase) text = text.ToLower();
            
            Console.Write("Only use English Characters (0/1)?> ");
            onlyUseEnglishChars = int.Parse(Console.ReadLine()) != 0;

            Console.Write("Letter Reading Samples ({startIndex1}:{interval1},{startIndex2}:{interval2} etc.)> ");
            string sampleOptionsInput = Console.ReadLine();

            foreach (string sampleOption in sampleOptionsInput.Split(','))
            {

                string[] parts = sampleOption.Split(':');
                int startIndex = int.Parse(parts[0]);
                int interval = int.Parse(parts[1]);

                SampleReadingOptions newOption = new SampleReadingOptions(startIndex, interval);

                readingOptions.Add(newOption);

            }

            foreach (SampleReadingOptions option in readingOptions)
            {

                decimal ioc = CalculateIOC(
                    GetTextFromStringByOffset(text.Substring(option.startIndex), option.interval),
                    onlyUseEnglishChars
                );

                Console.WriteLine($"{option.startIndex}:{option.interval}");
                Console.WriteLine($"IOC: {ioc}");
                Console.WriteLine();

            }

            Console.WriteLine("Program Finished");
            Console.ReadKey();

        }

        public static string GetTextFromFile(string filename)
        {
            Debug.Assert(File.Exists(filename));
            return File.ReadAllText(filename);
        }

        public static string GetTextFromStringByOffset(string oldText, int offset)
        {

            string output = "";

            for (int i = 0; oldText.Length > i; i += offset)
            {

                output = output + oldText[i];

            }

            return output;

        }

        public static decimal CalculateIOC(string text, bool onlyUseEnglishChars)
        {

            Dictionary<char, int> charFrequencies = new Dictionary<char, int>();
            int includedCharCount = 0;

            foreach (char c in text)
            {

                if (onlyUseEnglishChars && !englishCharacters.Contains(c)) continue;

                if (charFrequencies.ContainsKey(c)) charFrequencies[c]++;
                else charFrequencies.Add(c, 1);

                includedCharCount++;

            }

            decimal denominator = (includedCharCount - 1) * includedCharCount;

            decimal total = 0;
            foreach (char c in charFrequencies.Keys)
            {

                total += (decimal)((charFrequencies[c] - 1) * charFrequencies[c]) / denominator;

            }

            return total;

        }

        private struct SampleReadingOptions
        {

            public int startIndex;
            public int interval;

            public SampleReadingOptions(int startIndex, int interval)
            {
                this.startIndex = startIndex;
                this.interval = interval;
            }
        }

    }

}
