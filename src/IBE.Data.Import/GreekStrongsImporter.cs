using DevExpress.Xpo;
using IBE.Data.Import.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace IBE.Data.Import {
    /// <summary>
    /// Importer słownika greckiego Stronga z pliku XML.
    /// </summary>
    public class GreekStrongsImporter : BaseImporter {
        public override void Import(string filePath, UnitOfWork uow) {
            if (String.IsNullOrEmpty(filePath)) { throw new ArgumentNullException("filePath"); }
            if (uow == null) { throw new ArgumentNullException("uow"); }
            if (File.Exists(filePath)) {
                var xml = XElement.Load(filePath);
                var entries = xml.GetElement("entries");
                if (entries != null) {
                    foreach (var entry in entries.Elements()) {
                        if (entry.Value().Contains("Not Used")) {
                            var emptyStrongCode = new StrongCode(uow) {
                                Code = entry.GetElement("strongs").Value().ToInt(),
                                Derivation = String.Empty,
                                DescriptionText = String.Empty,
                                KjvDef = String.Empty,
                                Lang = Language.Greek,
                                Pronunciation = String.Empty,
                                SourceWord = String.Empty,
                                StrongsDef = String.Empty,
                                Transliteration = String.Empty
                            };

                            emptyStrongCode.Save();
                            continue;
                        }
                        var refs = new List<StrongCodeReferences>();
                        var sourceWord = entry.GetElement("greek").Attribute("unicode").Value();
                        var translit = entry.GetElement("greek").Attribute("translit").Value();
                        var pronunciation = entry.GetElement("pronunciation").Attribute("strongs").Value();
                        var derivation = String.Empty;
                        var text = String.Empty;
                        var definition = entry.GetElement("strongs_def").Value();
                        var strongsDeravationElement = entry.GetElement("strongs_derivation");
                        if (strongsDeravationElement.IsNotNull()) {
                            foreach (var node in strongsDeravationElement.Nodes()) {
                                if (node.NodeType == System.Xml.XmlNodeType.Text) {
                                    derivation += (node as XText).Value;
                                }
                                else if (node.NodeType == System.Xml.XmlNodeType.Element) {
                                    var e = node as XElement;
                                    if (e.Name.LocalName == "strongsref") {
                                        var prefix = "G";
                                        if (e.Attribute("language").Value() == "HEBREW") { prefix = "H"; }
                                        var code = e.Attribute("strongs").Value().ToInt();
                                        derivation += $@"<a href=""Strongs/{prefix}/{code}"">{prefix}{code}</a>";
                                    }
                                }
                            }
                        }

                        var start = false;
                        foreach (var node in entry.Nodes()) {
                            if (node.NodeType == System.Xml.XmlNodeType.Text) {
                                text += (node as XText).Value;
                            }
                            else if (node.NodeType == System.Xml.XmlNodeType.Element) {
                                var e = node as XElement;

                                if (e.Name.LocalName == "kjv_def") { start = true; continue; }
                                if (!start) { continue; }

                                if (e.Name.LocalName == "strongsref") {
                                    var prefix = "G";
                                    if (e.Attribute("language").Value() == "HEBREW") { prefix = "H"; }
                                    var code = e.Attribute("strongs").Value().ToInt();
                                    text += $@"<a href=""Strongs/{prefix}/{code}"">{prefix}{code}</a>";
                                }
                                else if (e.Name.LocalName == "greek") {
                                    text += e.Attribute("unicode").Value() + " " + e.Attribute("translit").Value();
                                }
                                else if (e.Name.LocalName == "see") {
                                    var reference = new StrongCodeReferences(uow) {
                                         Lang = e.Attribute("language").Value() == "HEBREW" ? Language.Hebrew : Language.Greek,
                                          Code = e.Attribute("strongs").Value().ToInt()
                                    };
                                    refs.Add(reference);
                                }
                            }
                        }

                        var strongCode = new StrongCode(uow) {
                            Code = entry.GetElement("strongs").Value().ToInt(),
                            Derivation = derivation.Replace("\n", " ").Trim(),
                            KjvDef = entry.GetElement("kjv_def").Value(),
                            Lang = Language.Greek,
                            Pronunciation = pronunciation,
                            SourceWord = sourceWord,
                            Transliteration = translit,
                            StrongsDef = definition,
                            DescriptionText = text.Replace("\n", " ").Trim()
                        };

                        strongCode.Save();

                        if (refs.Count > 0) {
                            foreach (var item in refs) {
                                item.Parent = strongCode;
                                item.Save();
                            }
                        }
                    }
                }
            }
        }
    }
}
