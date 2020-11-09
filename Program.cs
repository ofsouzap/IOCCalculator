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
            int readOffset;

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

            Console.Write("Letter Reading Offset> ");
            readOffset = int.Parse(Console.ReadLine());

            if (readOffset > 1) text = GetTextFromStringByOffset(text, readOffset);

            Console.WriteLine($"IOC: {CalculateIOC(text, onlyUseEnglishChars)}");

            Console.WriteLine();
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

            for (int i = 0; oldText.Length - 1 > i; i += offset)
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

    }

}
