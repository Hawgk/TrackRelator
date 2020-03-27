using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace TrackRelator {
    class Import {
        private XDocument doc;
        public Import() {
            Uri uri = new Uri("/resources/Releases.xml", UriKind.Relative);
            doc = XDocument.Load(Environment.CurrentDirectory + @"\resources\Releases.xml");
        }
        public List<Release> GetReleases() {
            List<Release> releases = new List<Release>();
            IEnumerable<XElement> contents = doc.Descendants("Release");
            foreach (var content in contents) {
                Release rel = new Release {
                    Name = content.Element("Name").Value,
                    Label = content.Element("Label").Value
                };
                IEnumerable<XElement> tracks = content.Descendants("Track");
                int i = 0;
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
    }
}
