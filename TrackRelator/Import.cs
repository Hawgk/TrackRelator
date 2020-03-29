using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TrackRelator {
    class Import {
        private XDocument doc;
        public Import(string type) {
            if (type == "Releases") {
                if (File.Exists(Environment.CurrentDirectory + @"\resources\Releases.xml")) {
                    doc = XDocument.Load(Environment.CurrentDirectory + @"\resources\Releases.xml");
                } else {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\resources\"))
                        Directory.CreateDirectory(Environment.CurrentDirectory + @"\resources\");
                    Export export = new Export();
                    export.SaveToFile(new List<Release>());
                }
            } else {
                if (File.Exists(Environment.CurrentDirectory + @"\resources\Relations.xml")) {
                    doc = XDocument.Load(Environment.CurrentDirectory + @"\resources\Relations.xml");
                } else {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\resources\"))
                        Directory.CreateDirectory(Environment.CurrentDirectory + @"\resources\");
                    Export export = new Export();
                    export.SaveToFile(new List<Relation>());
                }
            }
        }
        public List<Release> GetReleases() {
            List<Release> releases = new List<Release>();
            if (doc != null) {
                IEnumerable<XElement> contents = doc.Descendants("Release");
                int i = 0;
                foreach (var content in contents) {
                    Release rel = new Release {
                        Name = content.Element("Name").Value,
                        Label = content.Element("Label").Value
                    };
                    IEnumerable<XElement> tracks = content.Descendants("Track");
                    foreach (var track in tracks) {
                        Track tr = new Track {
                            Id = i,
                            Title = track.Element("Title").Value,
                            Artist = track.Element("Artist").Value,
                            Side = track.Element("Side").Value,
                            Release = rel
                        };
                        rel.Tracks.Add(tr);
                        i++;
                    }
                    releases.Add(rel);
                }
            }
            return releases;
        }
        public void GetRelations(List<Track> tracks) {
            if (doc != null) {
                IEnumerable<XElement> contents = doc.Descendants("Relation");
                foreach (var content in contents) {
                    int.TryParse(content.Element("First_track").Value, out int Id_0);
                    int.TryParse(content.Element("Second_track").Value, out int Id_1);
                    Relation relation = new Relation();
                    relation.First_track = tracks.Where(track => track.Id == Id_0).ToArray()[0];
                    relation.Second_track = tracks.Where(track => track.Id == Id_1).ToArray()[0];
                    relation.First_track.Relations.Add(relation);
                }
            }
        }
    }
}
