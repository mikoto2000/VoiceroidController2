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
  --voiceroid              (Default: 結月ゆかり) 読み上げ VOICEROID 名
  -o, --output-file        出力ファイルパス（--play のみの場合は不要）
  -i, --input-file         入力ファイルパス（UTF-8）
  -t, --text               読み上げテキスト（--input-file より優先）
  -p, --play               音声を再生する
  -l, --list               読み上げ可能 VOICEROID 名一覧表示
  --split-size             (Default: 2000) 読み上げ文字列を分割する目安のサイズ
  --linebreak-to-period    改行を句点に置換
  --help                   ヘルプを表示
```

| オプション | 説明 |
|:--|:--|
| voiceroid | 読み上げに使用する VOICEROID 名。 |
| output-file | 出力先ファイルパス。ファイル名末尾に `_000` のように連番が付与される。拡張子は無視され `wav` で保存される。`--play` のみ利用時は省略可。 |
| input-file | 入力ファイルパス。UTF-8 テキストファイル。`--text` が指定された場合は使用されない。 |
| text | 読み上げるテキスト。`--input-file` と同時指定された場合はこちらを優先する。 |
| play | 音声を再生する。保存と併用可能。 |
| list | 読み上げ可能 VOICEROID 名一覧を表示する。 |
| split-size | 読み上げ文字列を分割する目安サイズ。概ねこの文字数ごとに分割して処理する。 |
| linebreak-to-period | 入力テキスト内の改行を句点に置換する。 |

以下のような使い方を想定しています。
```bat
rem 入力ファイルから保存のみ
VoiceroidController2.exe --voiceroid 結月ゆかり -i input.txt -o output.wav

rem テキスト直接指定で再生のみ
VoiceroidController2.exe --voiceroid 結月ゆかり --text "こんにちは。テストです。" --play

rem 保存しつつ再生（--text が --input-file より優先）
VoiceroidController2.exe --voiceroid 結月ゆかり --text "優先されるテキスト" -i input.txt -o output.wav --play
```

build:
------

ビルドには、 Visual Studio 2022 が必要です。

### ソース一式取得

```sh
git clone https://github.com/mikoto2000/VoiceroidController2.git
cd VoiceroidController2
git submodule init
git submodule update
git submodule sync
```

License:
--------

Copyright (C) 2018-2026 mikoto2000

This software is released under the Apache 2.0 License, see LICENSE

このソフトウェアは Apache 2.0 ライセンスの下で公開されています。 LICENSE を参照してください。


Author:
-------

mikoto2000 <mikoto2000@gmail.com>

