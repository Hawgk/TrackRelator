using System.Collections.Generic;

namespace TrackRelator {
    public class Track {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public Release Release { get; set; }
        public string Side { get; set; }
        public Relation Relation { get; set; }
    }
}
