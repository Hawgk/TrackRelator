using System.Collections.Generic;

namespace TrackRelator {
    public class Release {
        public string Name { get; set; }
        public string Label { get; set; }
        public List<Track> Tracks { get; set; }
        public Release() {
            Tracks = new List<Track>();
        }
        public void AddTrack(Track track) {
            Tracks.Add(track);
        }
        public Track GetTrack(string title) {
            for (int i = 0; i < Tracks.Count; i++) {
                if (Tracks.ToArray()[i].Title == title) return Tracks.ToArray()[i];
            }
            return null;
        }
        public List<string> GetArtists() {
            List<string> artists = new List<string>();
            foreach (Track tr in Tracks) {
                if (artists.Count == 0) {
                    artists.Add(tr.Artist);
                } else {
                    var add = true;
                    foreach (string artist in artists) {
                        if (artist == tr.Artist) add = false;
                    }
                    if (add) artists.Add(tr.Artist);
                }
            }
            return artists;
        }
        public bool ContainsArtist(string artist) {
            foreach (Track tr in Tracks) {
                if (tr.Artist == artist) return true;
            }
            return false;
        }
        public List<string> GetSides() {
            var sides = new List<string>();
            foreach (Track tr in Tracks) {
                sides.Add(tr.Side);
            }
            return sides;
        }
    }
}
