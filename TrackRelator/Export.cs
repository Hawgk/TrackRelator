using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TrackRelator {
    class Export {
        private XDocument doc;
        private List<XElement> contents, tracks;
        public Export() { }
        public void SaveToFile(List<Release> export_list) {
            contents = new List<XElement>();
            foreach (Release rel in export_list) {
                tracks = new List<XElement>();
                foreach (Track tr in rel.Tracks) {
                    var track = new XElement("Track",
                        new XElement("Title", tr.Title),
                        new XElement("Artist", tr.Artist),
                        new XElement("Side", tr.Side));
                    tracks.Add(track);
                }
                var content = new XElement("Release",
                    new XElement("Name", rel.Name),
                    new XElement("Label", rel.Label),
                    new XElement("Tracks", tracks));
                contents.Add(content);
            }
            doc = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty),
                    new XElement("Releases", contents));
            doc.Save(Environment.CurrentDirectory + @"\resources\Releases.xml");
        }
        public void SaveToFile(List<Relation> export_list) {
            contents = new List<XElement>();
            foreach (Relation rel in export_list) {
                var content = new XElement("Relation",
                    new XElement("First_track", rel.First_track.Id),
                    new XElement("Second_track", rel.Second_track.Id));
                contents.Add(content);
            }
            doc = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty),
                    new XElement("Releases", contents));
            doc.Save(Environment.CurrentDirectory + @"\resources\Relations.xml");
        }
    }
}
