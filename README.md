# HabboAir

Flash is dying and I am quitting. What better time to release all my hard work for you to create a bad Habbo mobile retro. Have fun.

I have been sitting on this since August 2, 2018 and sometimes updated it to newer versions.

Tested versions:
- AIR63-201708251331-359388093
- AIR63-201805250931-867387450
- AIR63-201911271159-623255659

Other versions will work as well, the tools in this repository have been pretty reliable when updating to newer releases.

## Projects

Only listing the most important projects in this repository. The others are self-explanatory or libraries.

### src-frida

This contains work on the iOS Habbo application version **2.21.0**. Dealing with native AVM2 code is pretty annoying thus this part is unfinished. I still decided to add it because it may help someone else.

### src/HabBridge.Logger

This project contains code from [iamdroppy/AirLogger](https://github.com/iamdroppy/AirLogger), which contains code from [ArachisH/Tanji](https://github.com/ArachisH/Tanji). It basically implements some stuff from the [Sulakore](https://github.com/ArachisH/Sulakore) library to proxy Habbo packets.

### src/HabBridge.Logger.Air

This project contains an actual packet logger that can be used with the habbo.com hotel on the HabboAir app. You might need to update the `handshakeIds` because those currently in there belong to `AIR63-201805250931-867387450`.

In order to use this logger, you need to use `src/HabBridge.SwfPatcher` to patch the HabboTablet.swf so it connects to your packet logger instead of the habbo.com hotel.

### src/HabBridge.Server

This project is so dumb it works. Code might be messy since I always looked at it as a proto type.

**What is it?** It is a server that sits in between your server and the HabboAir application. It decrypts, translates and encrypts packets on the fly to make any emulator compatible with the HabboAir application.

**What is wrong with you?** I wanted to add mobile support to a few dutch Habbo private servers and I did not have access to their emulators, which is why I chose for this method. To my surprise, it worked.

Ideally you should add support for multiple revisions to your emulator.

Tested versions:
- AIR63-201708251331-359388093
- AIR63-201805250931-867387450
- AIR63-201911271159-623255659

Tested target versions:
- PRODUCTION-201607262204-86871104
- PRODUCTION-201704051204-452050219
- PRODUCTION-201701242205-837386173

### src/HabBridge.SwfDeobfuscator

Takes advantage of a mistake (?) done by SecureSWF to deobfuscate a lot of class names. This makes it way easier to look at the `HabboTablet.swf` in a decompiler such as [JPEXS](https://github.com/jindrapetrik/jpexs-decompiler).

### src/HabBridge.SwfLibraryDumper

Creates incoming and outgoing header files. Recommended to use with a deobfuscated SWF to get better results.

### src/HabBridge.SwfPatcher

This is an automated `HabboTablet.swf` patcher, heavily inspired by [HabKit](https://github.com/ArachisH/HabKit) which is an awesome tool. The methods used by HabKit had to be pretty much rewritten for HabboAir and I did it all by myself.

The patcher does the following:
- Replaces binary data `common_configuration_txt`.
- Replaces localization languages.
- Patches host check.
- Patches RSA keys with your own.
- Patches ClientHelloMessageComposer to be more useful for private hotels.
- Disables HabboTracking.
- Patches the logger so that it writes to `/data/data/..` cache directory of the HabboAir app, useful for debugging.

## APK

To unpack and repack an APK I recommend [apktool](https://ibotpeaches.github.io/Apktool/). Just make sure you configure it so that it does not touch any APK code since we are only interested in the resources (HabboTablet.swf). 

To redistribute your modified APK, you need to resign it (jarsigner) and I recommend you to realign it (zipalign) too.

## License

The license does NOT apply to the files in:
- `./submodules/Flazzy/`
- `./submodules/Sulakore/`
- `./src/HabBridge.Logger/`

These are slightly modified forks by me and are included for preservation reasons. Their original repositories can be found here: [Flazzy](https://github.com/ArachisH/Flazzy) / [Sulakore](https://github.com/ArachisH/Sulakore). It is amazing work by [ArachisH](https://github.com/ArachisH).

The AGPL 3.0 license applies to all other files in this repository.  
If you are unfamiliar with AGPL 3.0, I suggest you to [read this](https://choosealicense.com/licenses/agpl-3.0/#).