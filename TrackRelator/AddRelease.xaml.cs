using System.Windows;

namespace TrackRelator {
    public partial class AddRelease : Window {
        private TrackDatabase track_database;
        private Release release;
        public AddRelease(MainWindow window, TrackDatabase track_database) {
            this.Owner = window;
            this.track_database = track_database;
            InitializeComponent();
        }
        private bool CheckValues() {
            if (text_title.Text == string.Empty || text_artist.Text == string.Empty || text_label.Text == string.Empty 
                || text_release.Text == string.Empty || text_side.Text == string.Empty) {
                return false;
            }
            return true;
        }
        private bool FoundRelease() {
            foreach (Release rel in track_database.List_releases) {
                if (text_release.Text == rel.Name) {
                    release = rel;
                    return true;
                }
            }
            return false;
        }
        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private void button_save_Click(object sender, RoutedEventArgs e) {
            if (CheckValues()) {
                Track track = new Track();
                track.Title = text_title.Text;
                track.Artist = text_artist.Text;
                track.Side = text_side.Text;
                if (!FoundRelease()) {
                    release = new Release();
                    release.Name = text_release.Text;
                    release.Label = text_label.Text;
                    track_database.List_labels.Add(release.Label);
                    track_database.List_releases.Add(release);
                }
                release.Tracks.Add(track);
                track.Release = release;
                track_database.List_tracks.Add(track);
                track_database.List_artists.Add(track.Artist);
                track_database.SaveReleases();
                this.Close();
                MessageBox.Show("Successfully saved.");
            } else {
                MessageBox.Show("All boxes have to be filled in!");
            }
        }
    }
}
