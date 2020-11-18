using System;
using System.IO;
using System.Linq;
using Flazzy;
using Flazzy.ABC;
using Flazzy.IO;
using HabBridge.SwfDeobfuscator.Flash;

namespace HabBridge.SwfDeobfuscator
{
    internal class Program
    {
        private const string TargetSwf = "HabboTablet.swf";

        private static void Main(string[] args)
        {
            var flash = new FlashFile(TargetSwf);

            // Disassemble.
            flash.Disassemble();

            // Iterate through all abc tags.
            foreach (var abc in flash.AbcFiles)
            {
                // Iterate through all classes.
                foreach (var clazz in abc.Classes)
                {
                    // We don't care about these ok.
                    if (clazz.QName.Namespace.Name.StartsWith("FilePrivateNS"))
                    {
                        continue;
                    }

                    // Find a private trait.
                    var trait = clazz.Traits
                            .Concat(clazz.Instance.Traits)
                            .FirstOrDefault(x => x.QName.Namespace.Kind == NamespaceKind.Private);

                    if (trait == null)
                    {
                        continue;
                    }

                    // Parse original names.
                    var names = trait.QName.Namespace.Name.Split(new []{':'}, 2, StringSplitOptions.None);

                    var privNamespace = names.Length == 2 ? names[0] : string.Empty;
                    var privClass = names.Length == 2 ? names[1] : names[0];

                    var oldNamespace = clazz.QName.Namespace.Name;
                    var oldClass = clazz.QName.Name;

                    var wasModified = false;

                    // Fix namespace.
                    if (!privNamespace.Equals(oldNamespace))
                    {
                        clazz.QName.Namespace.NameIndex = abc.Pool.AddConstant(privNamespace);
                        wasModified = true;
                    }

                    // Fix class name.
                    if (!privClass.Equals(oldClass))
                    {
                        clazz.QName.NameIndex = abc.Pool.AddConstant(privClass);
                        wasModified = true;
                    }

                    if (!wasModified) continue;

                    // Apply fix.
                    Console.WriteLine($"Patch {oldNamespace,-6} {oldClass, -6} => {privNamespace} {privClass}");
                }
            }

            // Reassemble.
            var destination = TargetSwf.Replace(".swf", ".deob.swf");

            using (var fileStream = File.Open(destination, FileMode.Create))
            using (var fileWriter = new FlashWriter(fileStream))
            {
                flash.Assemble(fileWriter, CompressionKind.ZLIB);
            }
        }
    }
}
