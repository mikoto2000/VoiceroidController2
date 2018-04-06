VoiceroidController2
====================

VOICEROID2 を使用し、コマンドプロンプトから
バッチ処理的に音声保存を実行するためのツール。

VOICEROID+, VOICEROID+ EX は、
[VoiceroidController](https://github.com/mikoto2000/VoiceroidController)
を使用してください。


Usage:
------

```bat
Usage: VoiceroidController2.exe [options]
オプション:
  -h [ --help ]                        ヘルプを表示
  -l [ --list ]                        読み上げ可能 VOICEROID 名一覧表示
  --voiceroid VOICEROID (=結月ゆかり)  読み上げ VOICEROID 名(結月ゆかり, 言葉葵)
  -o [ --output-file ] OUTPUT_FILE     出力ファイルパス
  -i [ --input-file ] INPUT_FILE       入力ファイルパス
  --split-size SPLIT_SIZE (=20000)     読み上げ文字列を分割する目安のサイズ
```

オプション  説明
----------  ----
VOICERID    読み上げ VOICEROID 名。
INPUT_FILE  入力ファイルパス</br>UTF-8 で書かれたテキストファイル。
OUTPUT_FILE 出力先ファイルパス。</br>ファイル名末尾に `_000` のように連番が挿入される。</br>指定した拡張子は無視され、 `wav` で保存される。
SPLIT_SIZE  読み上げ文字列を分割する目安のサイズ。</br>大体この文字数毎に音声ファイルが出力される。


下記のような使い方を想定しています。

```bat
rem 「VOICEROID2 結月ゆかり」で音声保存する。
rem 「文字列ファイル.txt」を「OUTPUT_FILE_xxx.wav」に保存(xxx は 000 からの連番)
rem 1 ファイル毎の読み上げ文字列が 100 文字を超えないように分割してファイル出力する
VoiceroidController.exe --voiceroid 結月ゆかり --split-size 100 -i 文字列ファイル.txt -o OUTPUT_FILE.wav
```

limitation:
-----------

VoiceroidController2 は、音声保存に特化しています。
VoiceroidController のように音声再生はできないので注意してください。


License:
--------

Copyright (C) 2018 mikoto2000

This software is released under the Apache 2.0 License, see LICENSE

このソフトウェアは Apache 2.0 ライセンスの下で公開されています。 LICENSE を参照してください。


Author:
-------

mikoto2000 <mikoto2000@gmail.com>

