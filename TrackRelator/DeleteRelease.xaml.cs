using System.Windows;
using System.Windows.Controls;

namespace TrackRelator {
    public partial class DeleteRelease : Window {
        private TrackDatabase track_database;
        private TrackInputHandler track_input_handler;
        public DeleteRelease(MainWindow window, TrackDatabase track_database) {
            this.Owner = window;
            this.track_database = track_database;
            InitializeComponent();
            track_input_handler = new TrackInputHandler(combo_title, combo_artist, combo_label, combo_release, combo_side, button_reset, track_database);
        }
        private void combo_title_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.TitleChanged();
        }
        private void combo_artist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.ArtistChanged();
        }
        private void combo_side_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.SideChanged();
        }
        private void combo_label_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.LabelChanged();
        }
        private void combo_release_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.ReleaseChanged();
        }
        private void button_reset_Click(object sender, RoutedEventArgs e) {
            track_input_handler.Reset();
        }
        private void button_delete_Click(object sender, RoutedEventArgs e) {
            if (track_input_handler.Current_track != null && track_input_handler.Current_artist != null) {
                track_database.DeleteTrack(track_input_handler.Current_track);
                this.Close();
            } else if (track_input_handler.Current_track == null && track_input_handler.Current_artist == null) {
                track_database.DeleteRelease(track_input_handler.Current_release);
                this.Close();
            } else {
                MessageBox.Show("Please fill out all fields.");
            }
        }
        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
