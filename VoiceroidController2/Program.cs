using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLineParserLibrary
{
    class Options
    {
        [Option("voiceroid", HelpText = "読み上げ VOICEROID(Yukari, YukariEx, Aoi)", DefaultValue = "結月ゆかり")]
        public string Voiceroid { get; set; }

        [Option('o', "output-file", Required = true, HelpText = "出力ファイルパス")]
        public string OutputFile { get; set; }

        [Option('i', "input-file", Required = true, HelpText = "入力ファイルパス")]
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
        static void Main(string[] args)
        {
            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }
        }
    }
}
