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
                Release rel = new Release();
                rel.Name = content.Element("Name").Value;
                rel.Label = content.Element("Label").Value;
                IEnumerable<XElement> tracks = content.Descendants("Track");
                foreach (var track in tracks) {
                    Track tr = new Track();
                    tr.Title = track.Element("Title").Value;
                    tr.Artist = track.Element("Artist").Value;
                    tr.Side = track.Element("Side").Value;
                    rel.Tracks.Add(tr);
                }
                releases.Add(rel);
            }
            return releases;
        }
    }
}
