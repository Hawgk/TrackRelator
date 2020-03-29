using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace TrackRelator {
    class TrackInputHandler {
        public ComboBox combo_title;
        public ComboBox combo_artist;
        public ComboBox combo_label;
        public ComboBox combo_release;
        public ComboBox combo_side;
        public Button button_reset;

        private bool reset = true;
        private bool track_select = false;
        private bool side_select = false;

        private List<string> list_combo_artists;
        private List<Track> list_combo_tracks;
        private List<Release> list_combo_releases;
        private List<string> list_combo_labels;

        private TrackDatabase track_database;
        public Track Current_track { get; set; }
        public string Current_artist { get; set; }
        public Release Current_release { get; set; }
        private string current_label;
        public TrackInputHandler(ComboBox combo_title, ComboBox combo_artist, ComboBox combo_label,
            ComboBox combo_release, ComboBox combo_side, Button button_reset, TrackDatabase track_database) {
            this.combo_title = combo_title;
            this.combo_artist = combo_artist;
            this.combo_label = combo_label;
            this.combo_release = combo_release;
            this.combo_side = combo_side;
            this.button_reset = button_reset;
            this.track_database = track_database;
            Reset();
            reset = false;
        }
        public void TitleChanged() {
            if (!reset) {
                track_select = true;
                Current_track = (Track)combo_title.SelectedItem;

                if (Current_track != null) {
                    Current_artist = Current_track.Artist;
                    Current_release = Current_track.Release;
                    combo_artist.ItemsSource = track_database.List_artists.Where(artist => artist == Current_track.Artist).ToList();
                    combo_artist.SelectedItem = Current_artist;
                    list_combo_releases = new List<Release>();
                    list_combo_releases.Add(Current_release);
                    combo_release.ItemsSource = list_combo_releases;
                    combo_release.SelectedItem = Current_release;
                    combo_side.IsEnabled = true;
                    if (!side_select) {
                        var lst = new List<string>();
                        lst.Add(Current_track.Side);
                        combo_side.ItemsSource = lst;
                    }
                    combo_side.SelectedItem = Current_track.Side;
                    list_combo_labels = new List<string>();
                    list_combo_labels.Add(Current_release.Label);
                    combo_label.ItemsSource = list_combo_labels;
                    combo_label.SelectedItem = Current_release.Label;
                }
            }
            track_select = false;
        }
        public void ArtistChanged() {
            if (!reset && !track_select) {
                Current_artist = combo_artist.SelectedItem.ToString();
                list_combo_releases = new List<Release>();
                list_combo_labels = new List<string>();
                foreach (Release rel in track_database.List_releases) {
                    if (rel != null) {
                        if (rel.ContainsArtist(Current_artist)) {
                            list_combo_releases.Add(rel);
                            list_combo_labels.Add(rel.Label);
                        }
                    }
                }
                list_combo_tracks = (List<Track>)combo_title.ItemsSource;
                combo_title.ItemsSource = list_combo_tracks.Where(track => track.Artist == Current_artist).ToList();
                combo_release.ItemsSource = list_combo_releases;
                combo_label.ItemsSource = list_combo_labels;
            }
        }
        public void LabelChanged() {
            if (!reset && !track_select) {
                current_label = (string)combo_label.SelectedItem;
                combo_release.ItemsSource = track_database.List_releases.Where(release => release.Label == current_label).ToList();
                combo_title.ItemsSource = track_database.List_tracks.Where(track => track.Release.Label == current_label).ToList();
                var list_combo_artists = new List<string>();
                foreach (Track tr in combo_title.ItemsSource) {
                    if (list_combo_artists.Count == 0) {
                        list_combo_artists.Add(tr.Artist);
                    } else {
                        var add = true;
                        foreach (string artist in list_combo_artists) {
                            if (artist == tr.Artist) {
                                add = false;
                                break;
                            }
                        }
                        if (add) list_combo_artists.Add(tr.Artist);
                    }
                }
                combo_artist.ItemsSource = list_combo_artists;
            }
        }
        public void ReleaseChanged() {
            if (!reset && !track_select) {
                Current_release = (Release)combo_release.SelectedItem;
                if (combo_release.SelectedIndex != -1) {
                    combo_side.IsEnabled = true;
                    list_combo_labels = new List<string>();
                    list_combo_labels.Add(Current_release.Label);
                    combo_label.ItemsSource = list_combo_labels;
                    combo_label.SelectedItem = Current_release.Label;
                    combo_side.IsEnabled = true;
                    if (combo_title.SelectedIndex == -1) {
                        combo_side.ItemsSource = Current_release.GetSides();
                        combo_artist.ItemsSource = Current_release.GetArtists();
                        if (!track_select) combo_title.ItemsSource = track_database.List_tracks.Where(track => track.Release == Current_release).ToList();
                    } else {
                        combo_title.ItemsSource = track_database.List_tracks.Where(track => track.Release == Current_release && track.Artist == Current_artist).ToList();
                        var list_combo_side = new List<string>();
                        foreach (Track track in combo_title.ItemsSource) {
                            list_combo_side.Add(track.Side);
                        }
                        combo_side.ItemsSource = list_combo_side;
                    }
                } else {
                    combo_side.IsEnabled = false;
                }
            }
        }
        public void SideChanged() {
            if (!reset && !track_select) {
                side_select = true;
                var current_side = (string)combo_side.SelectedItem;
                var list_combo_artists = new List<string>();
                foreach (Track tr in Current_release.Tracks) {
                    if (tr.Side == current_side) list_combo_artists.Add(tr.Artist);
                }
                combo_artist.ItemsSource = list_combo_artists;
                combo_artist.SelectedIndex = 0;
                combo_title.ItemsSource = Current_release.Tracks.Where(track => track.Side == current_side).ToList();
                combo_title.SelectedIndex = 0;
                side_select = false;
            }
        }
        private void SetItemsSources() {
            combo_title.ItemsSource = list_combo_tracks;
            combo_title.DisplayMemberPath = "Title";
            combo_artist.ItemsSource = list_combo_artists;
            combo_label.ItemsSource = list_combo_labels;
            combo_release.ItemsSource = list_combo_releases;
            combo_release.DisplayMemberPath = "Name";
            combo_side.IsEnabled = false;
        }
        public void Reset() {
            reset = true;
            track_select = false;
            Current_artist = null;
            Current_release = null;
            Current_track = null;
            current_label = null;
            ResetLists();
            SetItemsSources();
            combo_artist.SelectedIndex = -1;
            combo_release.SelectedIndex = -1;
            combo_title.SelectedIndex = -1;
            combo_side.SelectedIndex = -1;
            combo_label.SelectedIndex = -1;
            reset = false;
        }
        public void ResetLists() {
            list_combo_releases = track_database.List_releases.OrderBy(o => o.Name).ToList();
            list_combo_tracks = track_database.List_tracks.OrderBy(o => o.Title).ToList();
            list_combo_labels = track_database.List_labels.OrderBy(o => o).ToList();
            list_combo_artists = track_database.List_artists.OrderBy(o => o).ToList();
        }
    }
}
