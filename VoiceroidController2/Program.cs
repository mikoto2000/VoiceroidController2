using CommandLine;
using CommandLine.Text;
using Speech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CommandLineParserLibrary
{
    class Options
    {
        [Option("voiceroid", HelpText = "読み上げ VOICEROID 名", DefaultValue = "結月ゆかり")]
        public string Voiceroid { get; set; }

        [Option('o', "output-file", HelpText = "出力ファイルパス")]
        public string OutputFile { get; set; }

        [Option('i', "input-file", HelpText = "入力ファイルパス")]
        public string InputFile { get; set; }

        [Option('l', "list", HelpText = "読み上げ可能 VOICEROID 名一覧表示")]
        public bool IsPrintList { get; set; }

        [Option("split-size", HelpText = "読み上げ文字列を分割する目安のサイズ", DefaultValue = 2000)]
        public int SplitSize { get; set; }

        [Option("linebreak-to-period", HelpText = "改行を句点に置換")]
        public bool IsLinebreakToPeriod { get; set; }

        [Option('p', "play", HelpText = "音声を再生する")]
        public bool IsPlay { get; set; }

        [HelpOption(HelpText = "ヘルプを表示")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public override string ToString()
        {
            return $@"Options {{
    Voiceroid: {this.Voiceroid},
    OutputFile: {this.OutputFile},
    InputFile: {this.InputFile},
    IsPrintList: {this.IsPrintList},
    SplitSize: {this.SplitSize},
    IsLinebreakToPeriod: {this.IsLinebreakToPeriod},
    IsPlay: {this.IsPlay},
}}";
        }
    }

    class Program
    {
        static readonly string[] DELIMITERS = { ".", "。" };
        static readonly string[] DELETE_CHARS = { "\r\n", "\n" };

        static void Main(string[] args)
        {
            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            if (args.Length == 0)
            {
                Console.Error.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            if (options.IsPrintList)
            {
                var engines = SpeechController.GetVoiceroid2SpeechEngine();
                foreach (var c in engines)
                {
                    Console.WriteLine($"{c.LibraryName},{c.EngineName},{c.EnginePath}");
                }
                return;
            }

            if (options.InputFile == null)
            {
                Console.Error.WriteLine("入力ファイルパス(--input-file)は必須です。");
                Console.Error.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            if (!options.IsPlay && options.OutputFile == null)
            {
                Console.Error.WriteLine("出力ファイルパス(--output-file) または --play の指定が必須です。");
                Console.Error.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            var engine = SpeechController.GetVoiceroid2Instance(options.Voiceroid);
            if (engine == null)
            {
                Console.WriteLine($"{options.Voiceroid} を起動できませんでした。");
                Console.ReadKey();
                return;
            }
            engine.Activate();

            Encoding enc = Encoding.GetEncoding("utf-8");
            string text = File.ReadAllText(options.InputFile, enc);

            string outputFileBase = null;
            if (!string.IsNullOrEmpty(options.OutputFile))
            {
                string outputDir = Path.GetDirectoryName(options.OutputFile);
                if (string.IsNullOrEmpty(outputDir))
                {
                    outputDir = ".";
                }
                string outputDirFullPath = Path.GetFullPath(outputDir);
                outputFileBase = outputDirFullPath + "\\" + Path.GetFileNameWithoutExtension(options.OutputFile);
            }

            string replaceString = options.IsLinebreakToPeriod ? "。" : "";
            foreach (string deleteChar in DELETE_CHARS)
            {
                text = text.Replace(deleteChar, replaceString);
            }

            string[] splitText = text.Split(DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            var chunks = new List<string>();

            foreach (string sentence in splitText)
            {
                sb.Append(sentence);
                sb.Append("。");

                if (sb.Length > options.SplitSize)
                {
                    chunks.Add(sb.ToString().Normalize(NormalizationForm.FormKC));
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
            {
                chunks.Add(sb.ToString().Normalize(NormalizationForm.FormKC));
            }

            if (!string.IsNullOrEmpty(outputFileBase))
            {
                short count = 1;
                foreach (string chunk in chunks)
                {
                    string fileName = string.Format("{0}_{1:D3}.wav", outputFileBase, count);
                    count++;
                    engine.Save(fileName, chunk);
                }
            }

            if (options.IsPlay && chunks.Count > 0)
            {
                using (var finishedEvent = new ManualResetEventSlim(false))
                {
                    EventHandler<EventArgs> onFinished = (sender, e) => finishedEvent.Set();
                    engine.Finished += onFinished;
                    try
                    {
                        foreach (string chunk in chunks)
                        {
                            finishedEvent.Reset();
                            engine.Play(chunk);
                            finishedEvent.Wait();
                        }
                    }
                    finally
                    {
                        engine.Finished -= onFinished;
                    }
                }
            }
        }
    }
}
