using System;
using System.Collections.Generic;
using System.Text;

namespace TrackRelator {
    class Release {
        public string Name { get; set; }
        public string Label { get; set; }
        public Track[] Tracks { get; set; }
        public Track GetTrack(string title) {
            for (int i = 0; i < Tracks.Length; i++) {
                if (Tracks[i].Title == title) return Tracks[i];
            }
            return null;
        }
    }
}
