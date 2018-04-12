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
  --voiceroid          (Default: 結月ゆかり) 読み上げ VOICEROID 名
  -o, --output-file    Required. 出力ファイルパス
  -i, --input-file     Required. 入力ファイルパス
  -l, --list           読み上げ可能 VOICEROID 名一覧表示
  --split-size         (Default: 2000) 読み上げ文字列を分割する目安のサイズ
  --help               ヘルプを表示
```

| オプション | 説明 |
|:-----------|:-----|
| voicerid   | 読み上げ VOICEROID 名。 |
| output-file| 出力先ファイルパス。</br>ファイル名末尾に `_000` のように連番が挿入される。</br>指定した拡張子は無視され、 `wav` で保存される。 |
| input-file | 入力ファイルパス</br>UTF-8 で書かれたテキストファイル。 |
| list       | 読み上げ可能 VOICEROID 名一覧表示。 |
| split-size | 読み上げ文字列を分割する目安のサイズ。</br>大体この文字数毎に音声ファイルが出力される。 |


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

