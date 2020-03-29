using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TrackRelator {
    public partial class MainWindow : Window {
        private TrackInputHandler track_input_handler;

        private TrackDatabase track_database;
        private List<Track> related_tracks;
        private List<Relation> current_relations;

        public MainWindow() {
            track_database = new TrackDatabase();
            related_tracks = new List<Track>();
            current_relations = new List<Relation>();

            InitializeComponent();

            track_input_handler = new TrackInputHandler(combo_title, combo_artist, combo_label, combo_release, combo_side, button_reset, track_database);
        }
        private void combo_title_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.TitleChanged();
            if (track_input_handler.Current_track != null) {
                current_relations = track_input_handler.Current_track.Relations;
                related_tracks = new List<Track>();
                if (current_relations != null) {
                    foreach (Relation relation in current_relations) {
                        related_tracks.Add(relation.Second_track);
                    }
                }
                data_grid.ItemsSource = related_tracks;
            }
        }
        private void combo_artist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.ArtistChanged();
        }
        private void combo_label_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.LabelChanged();
        }
        private void combo_release_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.ReleaseChanged();
        }
        private void combo_side_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_input_handler.SideChanged();
        }
        private void button_reset_Click(object sender, RoutedEventArgs e) {
            track_input_handler.Reset();
            current_relations = null;
            related_tracks = null;
            data_grid.ItemsSource = related_tracks;
        }
        private void button_relation_Click(object sender, RoutedEventArgs e) {
            CreateRelation relation_window = new CreateRelation(this, track_database);
            relation_window.Show();
        }
    }
}
