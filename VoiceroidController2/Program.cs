using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Speech;

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

        [HelpOption(HelpText= "ヘルプを表示")]
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
}}";
        }
    }

    class Program
    {
        static string[] DELIMITERS = { ".", "。", "\r\n", "\n" };

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

            if (options.IsPrintList) {
                // -l, --list オプションが指定されている場合、
                // 使用可能 VOICEROID 一覧を表示する。
                var engines = SpeechController.GetVoiceroid2SpeechEngine();
                foreach (var c in engines)
                {
                    Console.WriteLine($"{c.LibraryName},{c.EngineName},{c.EnginePath}");
                }
                return;
            }

            // 入力ファイル、出力ファイルの指定確認
            if (options.InputFile == null)
            {
                Console.Error.WriteLine("入力ファイルパス(--input-file)は必須です。");
                Console.Error.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            if (options.OutputFile == null)
            {
                Console.Error.WriteLine("出力ファイルパス(--output-file)は必須です。");
                Console.Error.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            // Voiceroid2Engine 作成
            var engine = SpeechController.GetVoiceroid2Instance(options.Voiceroid);
            if (engine == null)
            {
                Console.WriteLine($"{options.Voiceroid} を起動できませんでした。");
                Console.ReadKey();
                return;
            }
            engine.Activate();

            // ファイル読み込み
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            string text = System.IO.File.ReadAllText(options.InputFile, enc);

            // 出力ファイルのベースを取得
            string outputDir = Path.GetDirectoryName(options.OutputFile);
            string outputFileBase = outputDir + "\\" + Path.GetFileNameWithoutExtension(options.OutputFile);

            // テキストを句点、改行で分割。
            string[] splitedText = text.Split(DELIMITERS, System.StringSplitOptions.RemoveEmptyEntries);

            // 音声保存 1 回分の文字列を記録するバッファを作成
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // 大体 --split-size 文字毎にひとまとめにして音声保存していく。
            short count= 1;
            foreach (string sentence in splitedText) {
                sb.Append(sentence);
                sb.Append("。");

                // 指定文字数を超える場合、
                // 今までため込んでいたものを読み上げる。
                // ただし、初回から超えていた場合はあきらめる。
                if (sb.Length > options.SplitSize) {
                    // ファイル名組み立て
                    string fileName = String.Format("{0}_{1:D3}.wav", outputFileBase, count);
                    count++;
                    
                    engine.Save(fileName, sb.ToString());

                    // sb リセット
                    sb.Clear();
                }
            }

            // 最後に残った文字列があれば音声保存
            if (sb.Length > 0)
            {
                // ファイル名組み立て
                string fileName = String.Format("{0}_{1:D3}.wav", outputFileBase, count);
                engine.Save(fileName, sb.ToString());
            }

            return;
        }
    }
}

