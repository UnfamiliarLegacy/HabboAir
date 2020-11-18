using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Flazzy;
using Flazzy.ABC;
using Flazzy.ABC.AVM2;
using Flazzy.ABC.AVM2.Instructions;
using Flazzy.Extensions;
using Flazzy.IO;
using Flazzy.Tags;
using HabBridge.SwfPatcher.Air.ClassLookup;
using HabBridge.SwfPatcher.Air.Messages;
using HabBridge.SwfPatcher.Config;

namespace HabBridge.SwfPatcher.Air
{
    public class AirGame : IDisposable
    {
        private readonly AirGameFlash _flash;
        private readonly AirClassLookup _lookup;

        public AirGame(string path)
        {
            _flash = new AirGameFlash(path);
            _lookup = new AirClassLookup(_flash);

            MessagesIncoming = new SortedDictionary<ushort, MessageItem>();
            MessagesOutgoing = new SortedDictionary<ushort, MessageItem>();
        }

        public string Revision { get; private set; }
        public string RevisionProtocol { get; private set; }

        public SortedDictionary<ushort, MessageItem> MessagesIncoming { get; }
        public SortedDictionary<ushort, MessageItem> MessagesOutgoing { get; }

        /// <summary>
        ///     Disassembles the flash file and stores the abc files.
        /// </summary>
        public void Initialize()
        {
            _flash.Disassemble();

            LoadMessages();
            LoadRevision();
        }

        public void Reassemble(string destination = null)
        {
            if (destination == null)
            {
                destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Revision}.swf");
            }

            using (var fileStream = File.Open(destination, FileMode.Create))
            using (var fileWriter = new FlashWriter(fileStream))
            {
                _flash.Assemble(fileWriter, CompressionKind.ZLIB);
            }
        }

        /// <summary>
        ///     Reads the incoming and outcoming classes.
        /// </summary>
        private void LoadMessages()
        {
            var abc = _flash.AbcGame;

            // Find HabboMessages class.
            var habboMessagesClass = abc.Classes
                .AsParallel()
                .FirstOrDefault(x => x.Traits.Count == 2 &&
                                     x.Instance.Traits.Count == 2 &&
                                     x.Traits.All(y => y.Type.Name.Equals("Map")));

            if (habboMessagesClass == null)
            {
                throw new ApplicationException("Unable to find HabboMessages class.");
            }

            // Parse code.
            var code = habboMessagesClass.Constructor.Body.ParseCode();
            var instructions = code
                .Where(x => x.OP == OPCode.GetLex ||
                            x.OP == OPCode.PushShort ||
                            x.OP == OPCode.PushByte)
                .ToArray();

            var messageMapIn = habboMessagesClass.Traits[0].QNameIndex;
            var messageMapOut = habboMessagesClass.Traits[1].QNameIndex;

            // Walk through the instructions.
            for (var i = 0; i < instructions.Length; i += 3)
            {
                // Get instructions.
                var InsMessageMap = (GetLexIns) instructions[i];
                var InsMessageId = (Primitive) instructions[i + 1];
                var InsMessageClass = (GetLexIns) instructions[i + 2];

                // Get message data.
                var messageOut = messageMapOut == InsMessageMap.TypeNameIndex;
                var messageId = Convert.ToUInt16(InsMessageId.Value);
                var messageClass = abc.GetClass(InsMessageClass.TypeName);

                // Store data.
                var message = new MessageItem(messageId, messageOut ? MessageType.Outgoing : MessageType.Incoming, messageClass);
                if (message.Type == MessageType.Incoming)
                {
                    MessagesIncoming.Add(message.Id, message);
                }
                else
                {
                    MessagesOutgoing.Add(message.Id, message);
                }
            }
        }

        /// <summary>
        ///     Figures out the revision.
        /// </summary>
        private void LoadRevision()
        {
            // Find the message containing revision info.
            if (!MessagesOutgoing.ContainsKey(AirConstants.ClientHelloMessageComposer))
            {
                throw new ApplicationException($"Cannot find the {nameof(AirConstants.ClientHelloMessageComposer)} message.");
            }

            var messageClientHello = MessagesOutgoing[AirConstants.ClientHelloMessageComposer].Clazz;
            var messageInstance = messageClientHello.Instance;

            // Find the toArray method.
            var toArrayMethod = messageInstance.GetMethods()
                .FirstOrDefault(x => x.Parameters.Count == 0 &&
                                     x.ReturnType.Name.Equals("Array"));

            if (toArrayMethod == null)
            {
                throw new ApplicationException($"Can not find the {nameof(AirConstants.ClientHelloMessageComposer)} toArrayMethod.");
            }

            // Find the client revision.
            var toArrayCode = toArrayMethod.Body.ParseCode();
            var revisionInst = (PushStringIns) toArrayCode.First(x => x.OP == OPCode.PushString);

            Revision = revisionInst.Value;

            // Find the protocol revision.
            var revisionProtocolTrait = messageInstance.GetSlotTraits("String").First();

            RevisionProtocol = (string) revisionProtocolTrait.Value;
        }

        /// <summary>
        ///     Replaces a the data of a <see cref="DefineBinaryDataTag"/>.
        /// </summary>
        /// <param name="symbolName">A name that identifies the BinaryDataTag.</param>
        /// <param name="data">The data.</param>
        public void ReplaceBinaryData(string symbolName, byte[] data)
        {
            var symbols = (SymbolClassTag)_flash.Tags.Last(x => x.Kind == TagKind.SymbolClass);
            var binaryDataId = symbols.Entries.First(x => x.Item2.Contains(symbolName));
            var binaryData = (DefineBinaryDataTag)_flash.Tags.First(x => x.Kind == TagKind.DefineBinaryData && ((DefineBinaryDataTag)x).Id == binaryDataId.Item1);

            binaryData.Data = data;
        }

        public void ReplaceLocalizationDefaults(RetroLocalizationConfig[] localizationConfigs)
        {
            var abc = _flash.AbcGame;
            var symbols = (SymbolClassTag)_flash.Tags.Last(x => x.Kind == TagKind.SymbolClass);
            var localizationClass = abc.GetClass("HabboLocalizationCom");

            // Save all old localizations.
            var localizations = localizationConfigs
                .Select(x => x.HotelCodeBase).Distinct()
                .ToDictionary(x => x, y =>
                {
                    var binaryDataId = symbols.Entries.First(x => x.Item2.Contains(y));
                    var binaryData = (DefineBinaryDataTag)_flash.Tags.First(x => x.Kind == TagKind.DefineBinaryData && ((DefineBinaryDataTag)x).Id == binaryDataId.Item1);

                    return binaryData.Data;
                });

            // Find all old localizations.
            var localizationSymbols = symbols.Entries.Where(x => x.Item2.Contains("default_localizations_")).ToArray();
            var localizationTraits = localizationClass.Traits.Where(x => x.IsStatic && x.QName.Name.StartsWith("default_localizations_")).ToArray();
            var localizationClasses = abc.Classes.Where(x => x.QName.Name.StartsWith("default_localizations_")).ToArray();
            
            if (localizationSymbols.Length != localizationTraits.Length ||
                localizationTraits.Length != localizationClasses.Length)
            {
                throw new ApplicationException("Localization result amount mismatch.");
            }

            // Remove all old localization references.

            // - Remove symbols
            foreach (var symbol in localizationSymbols)
            {
                _flash.Tags.RemoveAll(x => x.Kind == TagKind.DefineBinaryData && ((DefineBinaryDataTag)x).Id == symbol.Item1);
                symbols.Entries.RemoveAll(x => x.Item1 == symbol.Item1);
            }
            
            // - Remove "public static var default_localizations_*"
            localizationClass.Traits.RemoveAll(trait => trait.QName.Name.StartsWith("default_localizations_"));
            
            var body = localizationClass.Constructor.Body.ParseCode();
            
            var getLex = body.First(x => x.OP == OPCode.GetLex && ((GetLexIns)x).TypeName.Name.StartsWith("default_localizations_"));
            var getLexIndex = body.IndexOf(getLex);
            var setProp = body.LastIndexOf(body.Count - 1, OPCode.SetProperty);
            
            body.RemoveRange(getLexIndex, setProp - getLexIndex + 1);
            
            localizationClass.Constructor.Body.Code = body.ToArray();

            // - Remove "default_localizations_" classes / instances / scripts
            // TODO: Figure out how to fix all index based stuff, because this messes it up. DO NOT USE YET.
            // _flash.AbcGame.Classes.RemoveAll(x => x.QName.Name.StartsWith("default_localizations_"));
            // _flash.AbcGame.Instances.RemoveAll(x => x.QName.Name.StartsWith("default_localizations_"));
            // _flash.AbcGame.Scripts.RemoveAll(x => x.QName.Name.Contains("default_localizations_"));

            // Add localizations.
            foreach (var config in localizationConfigs)
            {
                var localizationName = $"custom_localizations_{config.HotelCode}";
                var defLocalizationName = $"own_default_localizations_{config.HotelCode}";

                var localizationMultiNameIndex = abc.Pool.AddConstant(new ASMultiname(abc.Pool)
                {
                    NameIndex = abc.Pool.AddConstant(localizationName),
                    Kind = MultinameKind.QName,
                    NamespaceIndex = 1
                });

                #region Create localization class

                // Create instance.
                var instanceConstructor = new ASMethod(abc)
                {
                    ReturnTypeIndex = 0
                };
                
                var instanceConstructorIndex = abc.AddMethod(instanceConstructor);
                
                var instanceConstructorBody = new ASMethodBody(abc)
                {
                    MethodIndex = instanceConstructorIndex,
                    MaxStack = 1,
                    LocalCount = 1,
                    InitialScopeDepth = 0,
                    MaxScopeDepth = 1,
                    Code = new byte[] {0xD0, 0x30, 0xD0, 0x49, 0x00, 0x47}
                };
                
                abc.AddMethodBody(instanceConstructorBody);
                
                // Create class.
                var classConstructor = new ASMethod(abc)
                {
                    ReturnTypeIndex = 0
                };
                
                var classConstructorIndex = abc.AddMethod(classConstructor);
                
                var classConstructorBody = new ASMethodBody(abc)
                {
                    MethodIndex = classConstructorIndex,
                    MaxStack = 0,
                    LocalCount = 1,
                    InitialScopeDepth = 0,
                    MaxScopeDepth = 0,
                    Code = new byte[] {0x47}
                };
                
                abc.AddMethodBody(classConstructorBody);
                
                // Add class to SWF.
                var classIndex = abc.AddClass(
                    new ASClass(abc)
                    {
                        ConstructorIndex = classConstructorIndex
                    },
                    new ASInstance(abc)
                    {
                        ConstructorIndex = instanceConstructorIndex,
                        QNameIndex = localizationMultiNameIndex,
                        SuperIndex = abc.Pool.GetMultinameIndex("ByteArray"),
                        Flags = ClassFlags.Sealed
                    }
                );
                
                // Create script.
                var initializerMethod = new ASMethod(abc)
                {
                    ReturnTypeIndex = 0
                };
                
                var initializerMethodIndex = abc.AddMethod(initializerMethod);
                
                var initializerMethodBody = new ASMethodBody(abc)
                {
                    MethodIndex = initializerMethodIndex,
                    MaxStack = 3,
                    LocalCount = 1,
                    InitialScopeDepth = 0,
                    MaxScopeDepth = 3,
                    Code = new byte[0]
                };

                abc.AddMethodBody(initializerMethodBody);
                
                var initializerMethodBodyCode = new ASCode(abc, initializerMethodBody);
                
                initializerMethodBodyCode.AddRange(new ASInstruction[]
                {
                    new GetLocal0Ins(),
                    new PushScopeIns(),
                    new GetScopeObjectIns(0),
                    new GetLexIns(abc, abc.Pool.GetMultinameIndex("Object")),
                    new PushScopeIns(),
                    new GetLexIns(abc, abc.Pool.GetMultinameIndex("ByteArray")),
                    new DupIns(),
                    new PushScopeIns(),
                    new NewClassIns(abc, classIndex),
                    new PopScopeIns(),
                    new PopScopeIns(),
                    new InitPropertyIns(abc, localizationMultiNameIndex),
                    new ReturnVoidIns()
                });
                
                initializerMethodBody.Code = initializerMethodBodyCode.ToArray();
                
                var initScript = new ASScript(abc)
                {
                    InitializerIndex = initializerMethodIndex
                };
                
                initScript.Traits.Add(new ASTrait(abc)
                {
                    ClassIndex = classIndex,
                    QNameIndex = localizationMultiNameIndex,
                    Kind = TraitKind.Class
                });
                
                _flash.AbcGame.AddScript(initScript);

                #endregion

                #region Create binary data symbol

                var symbolId = symbols.AddSymbol(localizationName);
                var data = localizations[config.HotelCodeBase];
                
                _flash.Tags.Insert(_flash.Tags.IndexOf(symbols), new DefineBinaryDataTag(symbolId, data));

                #endregion

                #region Add to HabboLocalizationCom

                var defLocalizationMultiNameIndex = abc.Pool.AddConstant(new ASMultiname(abc.Pool)
                {
                    NameIndex = abc.Pool.AddConstant(defLocalizationName),
                    Kind = MultinameKind.QName,
                    NamespaceIndex = 1
                });
                
                localizationClass.Traits.Add(new ASTrait(abc)
                {
                    QNameIndex = defLocalizationMultiNameIndex,
                    TypeIndex = abc.Pool.GetMultinameIndex("Class")
                });
                
                var constructorBody = localizationClass.Constructor.Body.ParseCode();
                
                constructorBody.InsertRange(constructorBody.Count - 1, new ASInstruction[]
                {
                    new GetLexIns(abc, localizationMultiNameIndex), 
                    new FindPropertyIns(abc, defLocalizationMultiNameIndex), 
                    new SwapIns(), 
                    new SetPropertyIns(abc, defLocalizationMultiNameIndex)
                });
                
                localizationClass.Constructor.Body.Code = constructorBody.ToArray();

                #endregion
            }

            // Swap localization to load.
            var stringId = abc.Pool.Strings.IndexOf("default_localizations_");
            abc.Pool.Strings[stringId] = "own_default_localizations_";

            // Replace localization_configuration_txt.
            var locConfigBuilder = new StringBuilder();

            for (var i = 0; i < localizationConfigs.Length; i++)
            {
                var config = localizationConfigs[i];
                var currentId = i + 1;

                locConfigBuilder.AppendLine($"localization.{currentId}={config.HotelCode}");
                locConfigBuilder.AppendLine($"localization.{currentId}.code={config.Code}");
                locConfigBuilder.AppendLine($"localization.{currentId}.name={config.Name}");
                locConfigBuilder.AppendLine($"localization.{currentId}.url={config.Url}");
            }

            var locConfigId = symbols.Entries.First(x => x.Item2.Contains("localization_configuration_txt"));
            var locConfig = (DefineBinaryDataTag)_flash.Tags.First(x => x.Kind == TagKind.DefineBinaryData && ((DefineBinaryDataTag)x).Id == locConfigId.Item1);

            locConfig.Data = Encoding.UTF8.GetBytes(locConfigBuilder.ToString());
            
            // Replace localizations manifest.
            string localizationManifestStr = null;
            DefineBinaryDataTag localizationManifest = null; 
            
            foreach (var (entryId, _) in symbols.Entries)
            {
                var binaryData = (DefineBinaryDataTag) _flash.Tags.FirstOrDefault(x => x.Kind == TagKind.DefineBinaryData && ((DefineBinaryDataTag) x).Id == entryId);
                if (binaryData != null)
                {
                    var dataString = Encoding.UTF8.GetString(binaryData.Data);
                    if (dataString.Contains("<asset mimeType=\"text/plain\" name=\"default_localizations_en\" />"))
                    {
                        localizationManifestStr = dataString;
                        localizationManifest = binaryData;
                        break;
                    }
                }
            }

            if (localizationManifest == null || localizationManifestStr == null)
            {
                throw new ApplicationException("Could not find localizationManifest or localizationManifestStr.");
            }
            
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(localizationManifestStr);

            var defaultLoc = new string[] {"development_localizations"};
            var manifestAssets = xmlDocument.GetElementsByTagName("assets")[0];
            manifestAssets.RemoveAll();

            foreach (var name in defaultLoc.Union(localizationConfigs.Select(x => "own_default_localizations_" + x.HotelCode)))
            {
                var child = xmlDocument.CreateElement("asset");

                child.SetAttribute("mimeType", "text/plain");
                child.SetAttribute("name", name);
                
                manifestAssets.AppendChild(child);
            }
            
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
            {
                Indent = true
            }))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();

                localizationManifest.Data = Encoding.UTF8.GetBytes(stringWriter.GetStringBuilder().ToString());
            }
        }

        /// <summary>
        ///     Removes the host check.
        /// </summary>
        public void PatchHostCheck()
        {
            var abc = _flash.AbcGame;

            // Find HabboCommunicationManager.
            var communicationManager = _lookup.Get(HabboClass.HabboCommunicationManager);

            // Find connect method.
            var connectMethodPossible = communicationManager.Instance.Traits
                .AsParallel()
                .Where(x => x.IsStatic == false &&
                            x.Kind == TraitKind.Method &&
                            x.QName.Namespace.Kind == NamespaceKind.Private &&
                            x.Method.Parameters.Count == 0 &&
                            x.Method.ReturnType.Name.Equals("void"))
                .Select(x => x.Method)
                .ToArray();

            var connectMethod = connectMethodPossible
                .AsParallel()
                .FirstOrDefault(x =>
                {
                    var code = x.Body.ParseCode();
                    var correctCount = 0;

                    foreach (var instruction in code)
                    {
                        if (instruction.OP == OPCode.PushInt)
                        {
                            var pushIntIns = (PushIntIns) instruction;

                            // We want to atleast see 3 correct values before assuming
                            // that we have the correct method.
                            if (correctCount == 0 && pushIntIns.Value == 65244)
                            {
                                correctCount++;
                            }
                            else if (correctCount == 1 && pushIntIns.Value == 65185)
                            {
                                correctCount++;
                            }
                            else if (correctCount == 2 && pushIntIns.Value == 65191)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // Reset.
                            correctCount = 0;
                        }
                    }

                    return false;
                });

            if (connectMethod == null)
            {
                throw new ApplicationException("Unable to find the connect method of the HabboCommunicationManager.");
            }

            // Patch.
            var connectCode = connectMethod.Body.ParseCode();
            var currentIndex = 0;

            while (true)
            {
                var patchLocation = connectCode
                    .Skip(currentIndex)
                    .FirstOrDefault(x => x.OP == OPCode.PushInt && ((PushIntIns) x).Value == 65244);

                if (patchLocation == null)
                {
                    break;
                }

                // Find location where ".habbo.com" obfuscation starts.
                var patchIndex = connectCode.IndexOf(patchLocation);

                // Find start and end position of the obfuscated code.
                var patchStart = connectCode.LastIndexOf(patchIndex, OPCode.GetLex);
                var patchEnd = connectCode.IndexOf(patchIndex, OPCode.Add);

                if (patchStart == -1 || patchEnd == -1)
                {
                    throw new ApplicationException("Could not find patch start and/or end for the HabboCommunicationManager connect method.");
                }

                // Remove the instructions.
                connectCode.RemoveRange(patchStart, patchEnd - patchStart + 1);

                // Place patch.
                connectCode.InsertRange(patchStart, new ASInstruction[]
                {
                    new FindPropStrictIns(abc, abc.Pool.GetMultinameIndex("getProperty")), 
                    new PushStringIns(abc, "connection.info.host"),
                    new CallPropertyIns(abc, abc.Pool.GetMultinameIndex("getProperty"), 1)
                });
            }

            connectMethod.Body.Code = connectCode.ToArray();
        }

        /// <summary>
        ///     Replaces the RSA keys with your own.
        /// </summary>
        /// <param name="modulus">An modulus.</param>
        /// <param name="exponent">An exponent. (Make sure it is in hex, so 10001 instead of 65537)</param>
        public void PatchCrypto(string modulus, string exponent)
        {
            var abc = _flash.AbcGame;

            // Find KeyObfuscator class.
            var keyObfuscatorClass = _lookup.Get(HabboClass.KeyObfuscator);

            // Find the methods to patch.
            var patchMethods = keyObfuscatorClass.Traits
                .Where(x => x.IsStatic &&
                            x.Kind == TraitKind.Method &&
                            x.Method.Parameters.Count == 0 &&
                            x.Method.ReturnType.Name.Equals("String"))
                .ToArray();

            if (patchMethods.Length != 2)
            {
                throw new ApplicationException("Could not find the correct methods to patch in the KeyObfuscator.");
            }

            // Patch the methods.
            for (var i = 0; i < patchMethods.Length; i++)
            {
                var method = patchMethods[i].Method;
                var methodCode = method.Body.ParseCode();

                methodCode.Clear();
                methodCode.AddRange(new ASInstruction[]
                {
                    new GetLocal0Ins(), 
                    new PushScopeIns(), 
                    new PushStringIns(abc, i == 0 ? modulus : exponent),
                    new ReturnValueIns()
                });

                method.Body.Code = methodCode.ToArray();
            }
        }

        /// <summary>
        ///     Disables various tracking methods that
        ///     we don't need for retro hotels.
        /// </summary>
        public void PatchHabboTracking()
        {
            var abc = _flash.AbcGame;

            var patchMethods = new[]
            {
                "legacyTrackGoogle",
                "trackGoogle"
            };

            var trackingClass = abc.GetClass("HabboTracking");

            foreach (var method in patchMethods.Select(x => trackingClass.Instance.GetMethod(x)))
            {
                var code = method.Body.ParseCode();

                code.Clear();
                code.Add(new ReturnVoidIns());

                method.Body.Code = code.ToArray();
                method.Body.Exceptions.Clear();
                method.Body.MaxStack = 1;
                method.Body.MaxScopeDepth = 1;
                method.Body.LocalCount = method.Parameters.Count + 1;
            }
        }

        /// <summary>
        ///     Modifies the "ClientHelloMessageComposer" (Packet 4000) class
        ///     to send the current environment instead of the protocol revision.
        /// </summary>
        public void PatchClientHelloMessageComposer()
        {
            var abc = _flash.AbcGame;

            // Grab ClientHelloMessageComposer.
            var composerClass = MessagesOutgoing[4000].Clazz;

            // Find CommunicationUtils.
            var communicationUtilsClass = _lookup.Get(HabboClass.CommunicationUtils);
            var communicationUtilsMethod = communicationUtilsClass.Traits
                .AsParallel()
                .FirstOrDefault(x => x.IsStatic &&
                            x.Kind == TraitKind.Method &&
                            x.Method.Parameters.Count == 2 &&
                            x.Method.Parameters[1].IsOptional &&
                            x.Method.ReturnType.Name.Equals("String"));

            if (communicationUtilsMethod == null)
            {
                throw new ApplicationException("Unable to find communicationUtilsMethod.");
            }

            // Find com.sulake.habbo.communication.demo:IncomingMessages.
            var incomingMessagesClass = _lookup.Get(HabboClass.IncomingMessages);
            var incomingMessagesMethod = incomingMessagesClass.Instance.Traits
                .AsParallel()
                .Where(x => x.Kind == TraitKind.Method &&
                            x.Method.Parameters.Count == 1 &&
                            x.Method.Parameters[0].Type.Name.Equals("Event") &&
                            x.Method.Parameters[0].IsOptional &&
                            x.Method.ReturnType.Name.Equals("void"))
                .Select(x => x.Method)
                .FirstOrDefault();

            if (incomingMessagesMethod == null)
            {
                throw new ApplicationException("Unable to find incomingMessagesMethod.");
            }

            var incomingMessagesCode = incomingMessagesMethod.Body.ParseCode();

            // Modify the method of IncomingMessages.
            var constructComposer = incomingMessagesCode
                .Where(x => x.OP == OPCode.ConstructProp && ((ConstructPropIns)x).PropertyName.NameIndex == composerClass.QName.NameIndex)
                .Cast<ConstructPropIns>()
                .First();

            // IncomingMessages: Add an argument when constructing the message.
            constructComposer.ArgCount = 1;

            // IncomingMessages: Add the argument value.
            incomingMessagesCode.InsertRange(incomingMessagesCode.IndexOf(constructComposer), new ASInstruction[]
            {
                new GetLexIns(abc, abc.Pool.Multinames.IndexOf(communicationUtilsClass.QName)), 
                new PushStringIns(abc, "environment"),
                new CallPropertyIns(abc, communicationUtilsMethod.QNameIndex, 1),
                new CoerceSIns()
            });

            // IncomingMessages: Increase stack.
            incomingMessagesMethod.Body.MaxStack += 4;
            incomingMessagesMethod.Body.Code = incomingMessagesCode.ToArray();

            // Modify the constructor of ClientHelloMessageComposer.
            var composerConstructor = composerClass.Instance.Constructor;
            var composerCode = composerConstructor.Body.ParseCode();

            // ClientHelloMessageComposer: Add "String" parameter.
            composerConstructor.Parameters.Add(new ASParameter(abc.Pool, composerConstructor)
            {
                TypeIndex = abc.Pool.GetMultinameIndex("String")
            });

            // ClientHelloMessageComposer: Add "environmentId" variable.
            var composerSlotQName = new ASMultiname(abc.Pool)
            {
                NameIndex = abc.Pool.AddConstant("environmentId"),
                Kind = MultinameKind.QName,
                NamespaceIndex = 1
            };
            
            var composerSlot = new ASTrait(abc)
            {
                Kind = TraitKind.Slot,
                QNameIndex = abc.Pool.AddConstant(composerSlotQName),
                TypeIndex = abc.Pool.GetMultinameIndex("String")
            };

            composerClass.Instance.Traits.Add(composerSlot);

            // ClientHelloMessageComposer: Set parameter of constructor to "environmentId".
            composerCode.InsertRange(composerCode.Count - 1, new ASInstruction[]
            {
                new GetLocal1Ins(),
                new FindPropertyIns(abc, composerSlot.QNameIndex), 
                new SwapIns(), 
                new SetPropertyIns(abc, composerSlot.QNameIndex), 
            });

            composerConstructor.Body.LocalCount = 2;
            composerConstructor.Body.MaxStack = 2;
            composerConstructor.Body.Code = composerCode.ToArray();

            // Modify the method that returns packet as array.
            var getArrayMethod = composerClass.Instance.Traits
                .AsParallel()
                .First(x => x.Kind == TraitKind.Method &&
                            x.Method.Parameters.Count == 0 &&
                            x.Method.ReturnType.Name.Equals("Array"));

            var getArrayCode = getArrayMethod.Method.Body.ParseCode();

            // getArray: Remove everything until constructing array.
            getArrayCode.Clear();
            getArrayCode.AddRange(new ASInstruction[]
            {
                new GetLocal0Ins(),
                new PushScopeIns(),

                new PushStringIns(abc, Revision),
                new PushStringIns(abc, RevisionProtocol), 
                new PushIntIns(abc, 3), // Android.
                new PushIntIns(abc, 1),
                new GetLexIns(abc, composerSlot.QNameIndex),

                new NewArrayIns(5), 
                new ReturnValueIns(), 
            });

            // getArray: Apply patch.
            getArrayMethod.Method.Body.MaxStack = 5;
            getArrayMethod.Method.Body.LocalCount = 1;
            getArrayMethod.Method.Body.Code = getArrayCode.ToArray();

            // ClientHelloMessageComposer: Clean class.
            composerClass.Instance.Traits.Remove(composerClass.Instance.Traits.First(x => x.Kind == TraitKind.Slot && x.QNameIndex != composerSlot.QNameIndex));
        }

        /// <summary>
        ///     Patches the "Logger" so that it writes to a local directory.
        /// </summary>
        public void PatchLogger()
        {
            var abc = _flash.AbcSplash;

            var loggerClass = abc.GetClass("Logger");

            var logMethod = loggerClass.GetMethod("log");
            var logBody = logMethod.Body;
            var logCode = logBody.ParseCode();

            var toStringMultiname = abc.Pool
                .GetMultinames("toString")
                .First(x => x.NamespaceSet != null && x.NamespaceSet.NamespaceIndices.Count == 7);

            var toStringIndex = abc.Pool.Multinames.IndexOf(toStringMultiname);

            logCode.Clear();
            logCode.InsertRange(0, new ASInstruction[]
            {
                new GetLocal0Ins(),
                new PushScopeIns(),
                new GetLocal1Ins(),
                new CallPropertyIns(abc, toStringIndex, 0),
                new CoerceSIns(),
                new SetLocal2Ins(),
                new GetLexIns(abc, abc.Pool.GetMultinameIndex("File")),
                new GetPropertyIns(abc, abc.Pool.GetMultinameIndex("cacheDirectory")),
                new PushStringIns(abc, "com.sulake.habbo/logger.txt"),
                new CallPropertyIns(abc, abc.Pool.GetMultinameIndex("resolvePath"), 1),
                new CoerceIns(abc, abc.Pool.GetMultinameIndex("File")),
                new SetLocal3Ins(),
                new FindPropStrictIns(abc, abc.Pool.GetMultinameIndex("FileStream")),
                new ConstructPropIns(abc, abc.Pool.GetMultinameIndex("FileStream"), 0),
                new CoerceIns(abc, abc.Pool.GetMultinameIndex("FileStream")),
                new SetLocalIns(4),
                new GetLocalIns(4),
                new GetLocal3Ins(),
                new PushStringIns(abc, "append"),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("open"), 2),
                new GetLocalIns(4),
                new GetLocal2Ins(),
                new PushStringIns(abc, "\n"),
                new AddIns(),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("writeUTFBytes"), 1),
                new GetLocalIns(4),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("close"), 0),
                new ReturnVoidIns()
            });

            logBody.Code = logCode.ToArray();
            logBody.MaxStack = 5;
            logBody.LocalCount = 5;
            logBody.InitialScopeDepth = 0;
            logBody.MaxScopeDepth = 1;
        }

        public void Dispose()
        {
            _flash?.Dispose();
        }

    }
}