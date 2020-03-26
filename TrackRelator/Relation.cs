using System.Collections.Generic;

namespace TrackRelator {
    public class Relation {
        public Track First_track { get; set; }
        public List<Track> Second_tracks { get; set; }
        public Relation() {
            Second_tracks = new List<Track>();
        }
    }
}
