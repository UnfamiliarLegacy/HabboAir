using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net.Packet;
using HabBridge.Server.Registers.Exceptions;
using Serilog;

namespace HabBridge.Server.Registers
{
    public class Registar
    {
        private static readonly ILogger Logger = Log.ForContext<Registar>();

        public static ReadOnlyDictionary<Release, Dictionary<Incoming, short>> HeadersIncoming { get; }
        public static ReadOnlyDictionary<Release, Dictionary<Outgoing, short>> HeadersOutgoing { get; }
        public static ReadOnlyDictionary<Release, Dictionary<Incoming, Type>> ModifiersIncoming { get; }
        public static ReadOnlyDictionary<Release, Dictionary<Outgoing, Type>> ModifiersOutgoing { get; }

        static Registar()
        {
            HeadersIncoming = new ReadOnlyDictionary<Release, Dictionary<Incoming, short>>(
                Enum.GetValues(typeof(Release))
                    .Cast<Release>()
                    .ToDictionary(key => key, value => new Dictionary<Incoming, short>())
            );

            HeadersOutgoing = new ReadOnlyDictionary<Release, Dictionary<Outgoing, short>>(
                Enum.GetValues(typeof(Release))
                    .Cast<Release>()
                    .ToDictionary(key => key, value => new Dictionary<Outgoing, short>())
            );

            ModifiersIncoming = new ReadOnlyDictionary<Release, Dictionary<Incoming, Type>>(
                Enum.GetValues(typeof(Release))
                    .Cast<Release>()
                    .ToDictionary(key => key, value => new Dictionary<Incoming, Type>())
            );

            ModifiersOutgoing = new ReadOnlyDictionary<Release, Dictionary<Outgoing, Type>>(
                Enum.GetValues(typeof(Release))
                    .Cast<Release>()
                    .ToDictionary(key => key, value => new Dictionary<Outgoing, Type>())
            );
        }

        public static async Task InitializeAsync()
        {
            Logger.Debug("Initializing registar");
            Logger.Debug("----------------------------------------------------");

            // Search for packet libraries.
            Logger.Debug("Detecting packet libraries");
            await RegisterPacketLibrariesAsync();

            // Search for IncomingPacketModifier(s)
            Logger.Debug("Detecting IncomingPacketModifier(s)");
            RegisterIncomingPacketModifiers();
            
            // Search for OutgoingPacketModifier(s)
            Logger.Debug("Detecting OutgoingPacketModifier(s)");
            RegisterOutgoingPacketModifiers();
            
            // Done.
            Logger.Debug("----------------------------------------------------");
        }
        
        private static void RegisterIncomingPacketModifiers()
        {
            var typesInfo = typeof(Registar).GetTypeInfo().Assembly.GetTypes()
                .Select(x => x.GetTypeInfo())
                .Where(x => x.GetCustomAttribute<DefineIncomingPacketModifier>() != null && typeof(PacketModifierBase).IsAssignableFrom(x.AsType()));

            foreach (var typeInfo in typesInfo)
            {
                var defineModifier = typeInfo.GetCustomAttribute<DefineIncomingPacketModifier>();

                Logger.Debug($"\t# {typeInfo.FullName}");

                foreach (var release in defineModifier.Releases)
                {
                    if (!ModifiersIncoming.ContainsKey(release))
                    {
                        continue;
                    }

                    if (ModifiersIncoming.ContainsKey(release) && ModifiersIncoming[release].ContainsKey(defineModifier.Header))
                    {
                        throw new Exception($"The incoming packet modifier of {release}:{defineModifier.Header} is already registered.");
                    }

                    ModifiersIncoming[release].Add(defineModifier.Header, typeInfo.AsType());
                }
            }
        }
        
        private static void RegisterOutgoingPacketModifiers()
        {
            var typesInfo = typeof(Registar).GetTypeInfo().Assembly.GetTypes()
                .Select(x => x.GetTypeInfo())
                .Where(x => x.GetCustomAttribute<DefineOutgoingPacketModifier>() != null && typeof(PacketModifierBase).IsAssignableFrom(x.AsType()));

            foreach (var typeInfo in typesInfo)
            {
                var defineModifier = typeInfo.GetCustomAttribute<DefineOutgoingPacketModifier>();

                Logger.Debug($"\t# {typeInfo.FullName}");

                foreach (var release in defineModifier.Releases)
                {
                    if (!ModifiersOutgoing.ContainsKey(release))
                    {
                        continue;
                    }

                    if (ModifiersOutgoing.ContainsKey(release) && ModifiersOutgoing[release].ContainsKey(defineModifier.Header))
                    {
                        throw new Exception($"The outgoing packet modifier of {release}:{defineModifier.Header} is already registered.");
                    }

                    ModifiersOutgoing[release].Add(defineModifier.Header, typeInfo.AsType());
                }
            }
        }

        private static async Task RegisterPacketLibrariesAsync()
        {
            var pattern = new Regex("(?<name>[a-zA-Z0-9]+)=(?<id>-?\\d+)", RegexOptions.Compiled);
            var directory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Libraries");
            var files = Directory.GetFiles(directory, "*.txt");

            foreach (var file in files)
            {
                string releaseStr;
                Release release;
                PacketType packetType;

                // Detect packet type.
                if (file.EndsWith("incoming.txt"))
                {
                    releaseStr = Path.GetFileName(file).Replace(".incoming.txt", "");
                    packetType = PacketType.Incoming;
                }
                else if (file.EndsWith("outgoing.txt"))
                {
                    releaseStr = Path.GetFileName(file).Replace(".outgoing.txt", "");
                    packetType = PacketType.Outgoing;
                }
                else
                {
                    continue;
                }

                Logger.Debug($"\t# {Path.GetFileName(file)}");

                // Detect release.
                if (!Enum.TryParse(releaseStr, true, out release))
                {
                    Logger.Debug($"\t\t# Invalid release '{releaseStr}'");
                    continue;
                }

                // Detect packet entries.
                var lines = await File.ReadAllLinesAsync(file);
                var data = new Dictionary<string, short>();

                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("[") || line.StartsWith("//"))
                    {
                        continue;
                    }

                    var match = pattern.Match(line);
                    if (match.Success)
                    {
                        var header = short.Parse(match.Groups["id"].Value);
                        if (header > 0)
                        {
                            data.Add(match.Groups["name"].Value, header);
                        }
                    }
                    else
                    {
                        Logger.Debug($"\t\t# Invalid library entry '{line}'");
                    }
                }

                foreach (var pair in data)
                {
                    switch (packetType)
                    {
                        case PacketType.Incoming when Enum.TryParse(pair.Key, true, out Incoming incomingValue):
                            HeadersIncoming[release].Add(incomingValue, pair.Value);
                            break;

                        case PacketType.Outgoing when Enum.TryParse(pair.Key, true, out Outgoing outgoingValue):
                            HeadersOutgoing[release].Add(outgoingValue, pair.Value);
                            break;

                        default:
                            Logger.Debug($"\t\t# Unknown packet {packetType} {pair.Key} {pair.Value}");
                            break;
                    }
                }
            }
        }

        // Utilities
        public static short GetPacketIdModified(short originalHeader, PacketType type, Release releaseFrom, Release releaseTarget)
        {
            if (releaseTarget == releaseFrom)
            {
                return originalHeader;
            }

            if (type == PacketType.Incoming)
            {
                var entry = HeadersIncoming[releaseFrom].FirstOrDefault(x => x.Value == originalHeader);
                if (entry.Key == Incoming.Unknown)
                {
                    throw new UnknownPacketHeaderException($"Packet Id {originalHeader} incoming could not be found for release {releaseFrom}.");
                }

                if (!HeadersIncoming[releaseTarget].ContainsKey(entry.Key))
                {
                    throw new UnknownPacketHeaderException($"Packet Id {originalHeader} ({entry.Key.ToString()}) incoming could not be found for release {releaseTarget}.");
                }

                return HeadersIncoming[releaseTarget][entry.Key];
            }
            else
            {
                var entry = HeadersOutgoing[releaseFrom].FirstOrDefault(x => x.Value == originalHeader);
                if (entry.Key == Outgoing.Unknown)
                {
                    throw new UnknownPacketHeaderException($"Packet Id {originalHeader} outgoing could not be found for release {releaseFrom}.");
                }
                
                if (!HeadersOutgoing[releaseFrom].ContainsKey(entry.Key))
                {
                    throw new UnknownPacketHeaderException($"Packet Id {originalHeader} ({entry.Key.ToString()}) outgoing could not be found for release {releaseTarget}.");
                }

                return HeadersOutgoing[releaseTarget][entry.Key];
            }
        }
    }
}
