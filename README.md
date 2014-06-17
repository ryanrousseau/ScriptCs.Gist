ScriptCs.Gist
=============

ScriptCs module that adds a #gist line processor.

Inspired by
* http://stackoverflow.com/questions/18406109/scriptcs-hosting-advantages-over-roslyn/18442295#18442295
* https://github.com/scriptcs/scriptcs/issues/109
* https://github.com/mmbot/mmbot.scripts/blob/master/scripts/Scripting/scriptgist.csx

Installing
==========
Install via the scriptcs install command `scriptcs -install ScriptCs.Gist -g`

This module requires the nightly build of scriptcs (until 0.10 is released) `cinst scriptcs -pre -source https://www.myget.org/F/scriptcsnightly/`

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

