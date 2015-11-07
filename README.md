ScriptCs.Gist
=============

[![Build status](https://ci.appveyor.com/api/projects/status/ldgbvtkxc4lldafw?svg=true)](https://ci.appveyor.com/project/ryanrousseau/scriptcs-gist)

ScriptCs module that adds a #gist line processor.

Inspired by
* http://stackoverflow.com/questions/18406109/scriptcs-hosting-advantages-over-roslyn/18442295#18442295
* https://github.com/scriptcs/scriptcs/issues/109
* https://github.com/mmbot/mmbot.scripts/blob/master/scripts/Scripting/scriptgist.csx
* https://twitter.com/filip_woj/status/478677382598250496

Installing
==========
Install via the scriptcs install command `scriptcs -install ScriptCs.Gist -g`

The current version is built using ScriptCs.Contracts version 0.15.0

Using in a script
=================

Specify the gist module when executing your script `scriptcs script.csx -modules gist`

The #gist line processor takes a gist id as an argument.  The processor will download any files in the gist ending with .csx to a folder named gists/{gistId}.  Subsequent runs will load the existing file on disk.  To redownload the scripts, simply delete the downloaded files before running again.

Here's the script that I used for testing.  Test gist found here: https://gist.github.com/ryanrousseau/0dca8b3a74958f82406a

    #gist 0dca8b3a74958f82406a

    var pinger = new Pinger();
    pinger.Ping();

    Console.WriteLine("done!");

Using in the repl
=================

Specify the gist module when executing your script `scriptcs -repl -modules gist`

Use the :gist repl command

    > :gist "0dca8b3a74958f82406a"
    Hello from gist!
    Tot ziens van gist!
    > var pinger = new Pinger();
    > pinger.Ping()
    Pong!
    >

Executing specific scripts in a gist
====================================

List script names after the gist id in either the line processor or the repl command to execute those scripts in that order.

From a script
    #gist 0dca8b3a74958f82406a hello.csx

From the repl
    > :gist "0dca8b3a74958f82406a" "tot-ziens.csx" "hello.csx"
    Tot ziens van gist!
    Hello from gist!
    
A script in the gist can also load a script from the gist. Using https://gist.github.com/ryanrousseau/392d533adc3ce49f7029 as an example.

    > :gist "392d533adc3ce49f7029" "main.csx"
    Greetings from gist!

More to come!
=============

Have ideas? Open an issue!
