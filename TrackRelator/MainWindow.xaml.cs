using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TrackRelator {
    public partial class MainWindow : Window {
        private bool track_select = false;
        private bool reset = false;
        private Release current_release;
        private Track current_track;
        private string current_artist;
        private string current_label;
        private Relation current_relation;
        private List<Track> related_tracks;
        private List<string> list_artists;
        private List<Release> list_release;
        private List<Release> list_combo_release;
        private List<Track> list_tracks;
        private List<string> list_labels;
        private List<string> list_combo_label;
        private List<Relation> list_relations;
        public MainWindow() {
            reset = true;
            list_release = new List<Release>();
            list_combo_release = new List<Release>();
            list_tracks = new List<Track>();
            list_artists = new List<string>();
            list_labels = new List<string>();
            list_combo_label = new List<string>();
            list_relations = new List<Relation>();
            related_tracks = new List<Track>();

            LoadReleases();
            LoadRelations();
            InitializeComponent();
            FillCombos();

            reset = false;
        }
        private void LoadReleases() {
            Import import = new Import();
            list_release = import.GetReleases();
            foreach (Release rel in list_release) {
                foreach (Track tr in rel.Tracks) {
                    list_tracks.Add(tr);
                }
                if (list_labels.Count == 0) {
                    list_labels.Add(rel.Label);
                } else {
                    var add = true;
                    foreach (string label in list_labels) {
                        if (label == rel.Label) {
                            add = false;
                            break;
                        }
                    }
                    if (add) list_labels.Add(rel.Label);
                }
            }
            foreach (Track tr in list_tracks) {
                if (list_artists.Count == 0) {
                    list_artists.Add(tr.Artist);
                } else {
                    var add = true;
                    foreach (string artist in list_artists) {
                        if (artist == tr.Artist) {
                            add = false;
                            break;
                        }
                    }
                    if (add) list_artists.Add(tr.Artist);
                }
            }
            /*for (int j = 0; j < 20; j++) {
                var rel = new Release();
                for (int i = 0; i < 4; i++) {
                    var tr = new Track {
                        Artist = "Artist" + (j + 1),
                        Title = "Release " + (j + 1) + " Title" + (i + 1),
                        Side = (i < 2) ? ("A" + (i + 1)) : ("B" + (i - 1)),
                        Release = rel
                    };
                    rel.AddTrack(tr);
                }
                rel.Name = "RLS" + (j + 1);
                rel.Label = "Label" + (j + 1);
                list_labels.Add(rel.Label);
                list_release.Add(rel);
            }
            foreach (Track tr in list_tracks) {
                if (list_artists.Count == 0) {
                    list_artists.Add(tr.Artist);
                } else {
                    var add = true;
                    foreach (string artist in list_artists) {
                        if (artist == tr.Artist) {
                            add = false;
                            break;
                        }
                    }
                    if (add) list_artists.Add(tr.Artist);
                }
            }*/
        }
        private void LoadRelations() {
            /*Relation rel;
            for (int j = 0; j < 4; j += 2) {
                rel = new Relation();
                rel.First_track = list_tracks.ToArray()[j];
                for (int i = 1; i < 11; i++) {
                    rel.Second_tracks.Add(list_tracks.ToArray()[i]);
                    list_relations.Add(rel);
                }
                list_tracks.ToArray()[j].Relation = rel;
            }
            list_relations.Add(list_tracks.ToArray()[0].Relation);
            current_relation = list_tracks.ToArray()[0].Relation;
            related_tracks = current_relation.Second_tracks;*/
        }
        private void FillCombos() {
            combo_artist.ItemsSource = list_artists;
            combo_title.ItemsSource = list_tracks;
            combo_title.DisplayMemberPath = "Title";
            combo_release.ItemsSource = list_release;
            combo_release.DisplayMemberPath = "Name";
            combo_label.ItemsSource = list_labels;
            combo_side.IsEnabled = false;
        }
        private void combo_artist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!reset) {
                current_artist = (string)combo_artist.SelectedItem;
                list_combo_release = new List<Release>();
                list_combo_label = new List<string>();
                foreach (Release rel in list_release) {
                    if (rel != null) {
                        if (rel.ContainsArtist(current_artist)) {
                            list_combo_release.Add(rel);
                            list_combo_label.Add(rel.Label);
                        }
                    }
                }
                if (!track_select) {
                    combo_title.ItemsSource = list_tracks.Where(track => track.Artist == current_artist);
                    combo_release.ItemsSource = list_combo_release;
                    combo_label.ItemsSource = list_combo_label;
                }
            }
        }

        private void button_reset_Click(object sender, RoutedEventArgs e) {
            reset = true;
            track_select = false;
            current_artist = null;
            current_release = null;
            current_track = null;
            current_label = null;
            current_relation = null;
            related_tracks = null;
            data_grid.ItemsSource = related_tracks;
            FillCombos();
            combo_artist.SelectedIndex = -1;
            combo_release.SelectedIndex = -1;
            combo_title.SelectedIndex = -1;
            combo_side.SelectedIndex = -1;
            combo_label.SelectedIndex = -1;
            reset = false;
        }

        private void combo_release_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!reset && ! track_select) {
                current_release = (Release)combo_release.SelectedItem;
                if (combo_release.SelectedIndex != -1) {
                    combo_side.IsEnabled = true;
                    list_combo_label = new List<string>();
                    list_combo_label.Add(current_release.Label);
                    combo_label.ItemsSource = list_combo_label;
                    combo_label.SelectedItem = current_release.Label;
                    combo_side.IsEnabled = true;
                    if (combo_title.SelectedIndex == -1) {
                        combo_side.ItemsSource = current_release.GetSides();
                        combo_artist.ItemsSource = current_release.GetArtists();
                        if (!track_select) combo_title.ItemsSource = list_tracks.Where(track => track.Release == current_release);
                    } else {
                        combo_title.ItemsSource = list_tracks.Where(track => track.Release == current_release && track.Artist == current_artist);
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

        private void combo_title_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!reset) {
                track_select = true;
                current_track = (Track)combo_title.SelectedItem;
                if (current_track != null) {
                    current_release = current_track.Release;
                    combo_artist.ItemsSource = list_artists.Where(artist => artist == current_track.Artist);
                    combo_artist.SelectedItem = current_track.Artist;
                    list_combo_release = new List<Release>();
                    list_combo_release.Add(current_release);
                    combo_release.ItemsSource = list_combo_release;
                    combo_release.SelectedItem = current_release;
                    combo_side.IsEnabled = true;
                    var lst = new List<string>();
                    lst.Add(current_track.Side);
                    combo_side.ItemsSource = lst;
                    combo_side.SelectedItem = current_track.Side;
                    list_combo_label = new List<string>();
                    list_combo_label.Add(current_release.Label);
                    combo_label.ItemsSource = list_combo_label;
                    combo_label.SelectedItem = current_release.Label;
                    current_relation = current_track.Relation;
                    if (current_relation != null) {
                        related_tracks = current_relation.Second_tracks;
                        data_grid.ItemsSource = related_tracks;
                    } else {
                        data_grid.ItemsSource = null;
                    }
                }
            }
            track_select = false;
        }

        private void combo_label_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!reset && !track_select) {
                current_label = (string)combo_label.SelectedItem;
                combo_release.ItemsSource = list_release.Where(release => release.Label == current_label);
                combo_title.ItemsSource = list_tracks.Where(track => track.Release.Label == current_label);
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

        private void combo_side_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!reset && !track_select) {
                var current_side = (string)combo_side.SelectedItem;
                if (combo_title.SelectedIndex == -1 || combo_artist.SelectedIndex == -1) {
                    var list_combo_artists = new List<string>();
                    foreach (Track tr in current_release.Tracks) {
                        if (tr.Side == current_side) list_combo_artists.Add(tr.Artist);
                    }
                    combo_artist.ItemsSource = list_combo_artists;
                    combo_artist.SelectedIndex = 0;
                    combo_title.ItemsSource = current_release.Tracks.Where(track => track.Side == current_side);
                    combo_title.SelectedIndex = 0;
                }
            }
        }
    }
}
