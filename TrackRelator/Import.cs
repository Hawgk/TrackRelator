using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TrackRelator {
    class Import {
        private XDocument doc;
        public Import(string type) {
            if (type == "Releases")
                doc = XDocument.Load(Environment.CurrentDirectory + @"\resources\Releases.xml");
            else
                doc = XDocument.Load(Environment.CurrentDirectory + @"\resources\Relations.xml");

        }
        public List<Release> GetReleases() {
            List<Release> releases = new List<Release>();
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
            return releases;
        }
        public List<Relation> GetRelations(List<Track> tracks) {
            List<Relation> relations = new List<Relation>();
            IEnumerable<XElement> contents = doc.Descendants("Relation");
            foreach (var content in contents) {
                int.TryParse(content.Element("First_track").Value, out int Id_0);
                Relation relation = new Relation();
                relation.First_track = tracks.Where(track => track.Id == Id_0).ToArray()[0];
                
                IEnumerable<XElement> Second_tracks = content.Descendants("Track");
                List<Track> lst = new List<Track>();
                foreach (var track in Second_tracks) {
                    int.TryParse(track.Value, out int _Id);
                    lst.Add(tracks.Where(track => track.Id == _Id).ToArray()[0]);
                }
                relation.Second_tracks = lst;
                relation.First_track.Relation = relation;
                relations.Add(relation);
            }
            return relations;
        }
    }
}
