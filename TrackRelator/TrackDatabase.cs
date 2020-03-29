using System;
using System.Collections.Generic;
using System.Text;

namespace TrackRelator {
    public class TrackDatabase {
        public List<string> List_artists { get; set; }
        public List<Release> List_releases { get; set; }
        public List<Track> List_tracks { get; set; }
        public List<string> List_labels { get; set; }
        public List<Relation> List_relations { get; set; }
        public TrackDatabase() {
            List_releases = new List<Release>();
            List_tracks = new List<Track>();
            List_artists = new List<string>();
            List_labels = new List<string>();
            LoadReleases();
            LoadRelations();
        }
        private void LoadReleases() {
            Import import = new Import("Releases");
            List_releases = import.GetReleases();
            foreach (Release rel in List_releases) {
                foreach (Track tr in rel.Tracks) {
                    List_tracks.Add(tr);
                }
                if (List_labels.Count == 0) {
                    List_labels.Add(rel.Label);
                } else {
                    var add = true;
                    foreach (string label in List_labels) {
                        if (label == rel.Label) {
                            add = false;
                            break;
                        }
                    }
                    if (add) List_labels.Add(rel.Label);
                }
            }
            foreach (Track tr in List_tracks) {
                if (List_artists.Count == 0) {
                    List_artists.Add(tr.Artist);
                } else {
                    var add = true;
                    foreach (string artist in List_artists) {
                        if (artist == tr.Artist) {
                            add = false;
                            break;
                        }
                    }
                    if (add) List_artists.Add(tr.Artist);
                }
            }
        }
        private void LoadRelations() {
            Import import = new Import("Relations");
            import.GetRelations(List_tracks);
        }
        public void SaveReleases() {
            Export export = new Export();
            export.SaveToFile(List_releases);
        }
        public void SaveRelations() {
            List_relations = new List<Relation>();
            foreach (Track tr in List_tracks) {
                foreach (Relation rel in tr.Relations) {
                    List_relations.Add(rel);
                }
            }
            Export export = new Export();
            export.SaveToFile(List_relations);
        }
    }
}
