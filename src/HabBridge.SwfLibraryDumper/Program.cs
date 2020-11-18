using System;
using System.Collections.Generic;
using System.Linq;
using Flazzy.ABC.AVM2.Instructions;
using HabBridge.SwfPatcher.Air;
using HabBridge.SwfPatcher.Air.Messages;

namespace HabBridge.SwfLibraryDumper
{
    internal class Program
    {
        private const string TargetSwf = "HabboTablet.deob.swf";
        
        private static void Main(string[] args)
        {
            using (var game = new AirGame(TargetSwf))
            {
                game.Initialize();

                DumpIncoming(game.MessagesIncoming); // Parsers
                DumpOutgoing(game.MessagesOutgoing); // MessageComposers
            }
        }

        private static void DumpIncoming(SortedDictionary<ushort, MessageItem> messages)
        {
            var packets = new List<PacketInfo>();
            
            // Find info.
            foreach (var (id, message) in messages)
            {
                var packet = new PacketInfo
                {
                    Id = id,
                    Category = "Unknown",
                    Name = $"Unknown_{id}"
                };
                
                // First category attempt.
                var package = message.Clazz.QName.Namespace.Name;
                if (package.StartsWith("com.sulake.habbo.communication.messages.incoming"))
                {
                    packet.Category = string.Join('.', package
                        .Replace("com.sulake.habbo.communication.messages.incoming.", string.Empty)
                        .Split('.')
                        .Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                    );
                }

                // Parse constructor.
                GetLexIns parserIns = null;
                
                if (message.Clazz.Instance.Constructor.Parameters.Count == 1)
                {
                    // Find parser inside constructor.
                    var code = message.Clazz.Instance.Constructor.Body.ParseCode();
                    var parser = (GetLexIns) code.First(x => x.OP == OPCode.GetLex);

                    parserIns = parser;
                } 
                else if (message.Clazz.Instance.Constructor.Parameters.Count == 2)
                {
                    // Attempt to find parser somewhere else.
                    var abc = message.Clazz.GetABC();
                    
                    foreach (var instance in abc.Instances)
                    {
                        if (instance.Constructor?.Body == null)
                        {
                            continue;
                        }
                        
                        var instCode = instance.Constructor.Body.ParseCode();
                        var instConstruct = instCode.FirstOrDefault(x => x.OP == OPCode.ConstructProp && 
                                                                         ((ConstructPropIns) x).PropertyName == message.Clazz.QName &&
                                                                         ((ConstructPropIns) x).ArgCount == 2);
                        if (instConstruct != null)
                        {
                            parserIns = (GetLexIns) instCode[instCode.IndexOf(instConstruct) - 1];
                            break;
                        }
                    }
                }
                else
                {
                    throw new Exception($"Unknown parameters count {message.Clazz.Instance.Constructor.Parameters.Count}.");
                }
                
                if (parserIns != null)
                {
                    var parserName = parserIns.TypeName.Name;
                    if (parserName.EndsWith("MessageParser"))
                    {
                        packet.Name = parserName.Substring(0, parserName.Length - 13);
                    } 
                    else if (parserName.EndsWith("Parser"))
                    {
                        packet.Name = parserName.Substring(0, parserName.Length - 6);
                    }

                    // Might give a better result than previous category.
                    var parserPackage = parserIns.TypeName.Namespace.Name;
                    if (parserPackage.StartsWith("com.sulake.habbo.communication.messages.parser"))
                    {
                        packet.Category = string.Join('.', parserPackage
                            .Replace("com.sulake.habbo.communication.messages.parser.", string.Empty)
                            .Split('.')
                            .Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                        );
                    }
                }

                packets.Add(packet);
            }
            
            // Print out.
            Console.WriteLine("# INCOMING");
            
            foreach (var packet in packets.OrderBy(x => x.Category).ThenBy(x => x.Name))
            {
                Console.WriteLine($"[{packet.Id,4}] {packet.Category} - {packet.Name}");
            }
        }

        private static void DumpOutgoing(SortedDictionary<ushort, MessageItem> messages)
        {
            var packets = new List<PacketInfo>();
            
            // Find info.
            foreach (var (id, message) in messages)
            {
                var packet = new PacketInfo
                {
                    Id = id,
                    Category = "Unknown",
                    Name = $"Unknown_{id}"
                };
                
                var package = message.Clazz.QName.Namespace.Name;
                if (package.StartsWith("com.sulake.habbo.communication.messages.outgoing"))
                {
                    packet.Category = string.Join('.', package
                        .Replace("com.sulake.habbo.communication.messages.outgoing.", string.Empty)
                        .Split('.')
                        .Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                    );
                }

                // Check class itself.
                var name = message.Clazz.QName.Name;
                if (name.EndsWith("MessageComposer"))
                {
                    packet.Name = name.Substring(0, name.Length - 15);
                } 
                else if (name.EndsWith("Composer"))
                {
                    packet.Name = name.Substring(0, name.Length - 8);
                }
                else if (message.Clazz.Instance?.Super?.Name != null)
                {
                    // Check super.
                    name = message.Clazz.Instance.Super.Name;
                    
                    if (name.EndsWith("MessageComposer"))
                    {
                        packet.Name = name.Substring(0, name.Length - 15) + "_PARENT";
                    } 
                    else if (name.EndsWith("Composer"))
                    {
                        packet.Name = name.Substring(0, name.Length - 8) + "_PARENT";
                    }
                }

                packets.Add(packet);
            }
            
            // Print out.
            Console.WriteLine("# OUTGOING");
            
            foreach (var packet in packets.OrderBy(x => x.Category).ThenBy(x => x.Name))
            {
                Console.WriteLine($"[{packet.Id,4}] {packet.Category} - {packet.Name}");
            }
        }
    }
}