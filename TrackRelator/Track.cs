using System;
using System.Collections.Generic;
using System.Text;

namespace TrackRelator {
    class Track {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Side { get; set; }
        public Release Release { get; set; }
    }
}
