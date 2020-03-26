using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackRelator {
    public partial class MainWindow : Window {
        private Release current_release;
        private Track current_track;
        private string current_artist;
        private List<string> list_artists;
        private List<Release> list_release;
        private List<Release> list_combo_release;
        private List<Track> list_tracks;
        private List<Track> list_combo_tracks;
        public MainWindow() {
            list_release = new List<Release>();
            list_combo_release = new List<Release>();
            list_tracks = new List<Track>();
            list_combo_tracks = new List<Track>();
            list_artists = new List<string>();
            LoadReleases();
            InitializeComponent();
            FillCombos();
        }
        private void LoadReleases() {
            for (int j = 0; j < 20; j++) {
                var rel = new Release();
                for (int i = 0; i < 4; i++) {
                    var tr = new Track {
                        Artist = "Artist" + (j + 1),
                        Title = "Title" + (i + 1),
                        Side = (i < 2) ? ("A" + (i + 1)) : ("B" + (i - 1)),
                        Release = rel
                    };
                    rel.AddTrack(tr);
                }
                rel.Name = "RLS" + (j + 1);
                rel.Label = "Label" + (j + 1);
                list_release.Add(rel);
            }
            foreach (Release rel in list_release) {
                foreach (Track tr in rel.Tracks) {
                    list_tracks.Add(tr);
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
        }
        private void FillCombos() {
            combo_artist.ItemsSource = list_artists;
            combo_title.ItemsSource = list_tracks;
            combo_title.DisplayMemberPath = "Title";
            combo_release.ItemsSource = list_release;
            combo_release.DisplayMemberPath = "Name";
            combo_side.IsEnabled = false;
        }
        private void combo_artist_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            current_artist = (string)combo_artist.SelectedItem;
            list_combo_release = new List<Release>();
            foreach (Release rel in list_release) {
                if (rel != null) {
                    if (rel.ContainsArtist(current_artist)) list_combo_release.Add(rel);
                }
            }
            combo_title.ItemsSource = list_tracks.Where(track => track.Artist == current_artist).ToList();
            combo_release.ItemsSource = list_combo_release;
        }

        private void button_reset_Click(object sender, RoutedEventArgs e) {
            current_artist = null;
            current_release = null;
            current_track = null;
            combo_artist.ItemsSource = list_artists;
            combo_title.ItemsSource = list_tracks;
            combo_release.ItemsSource = list_release;
            combo_artist.SelectedIndex = -1;
            combo_release.SelectedIndex = -1;
            combo_title.SelectedIndex = -1;
            combo_side.SelectedIndex = -1;
        }

        private void combo_release_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            current_release = (Release)combo_release.SelectedItem;
            if (combo_release.SelectedIndex != -1 && combo_artist.SelectedIndex == -1 && combo_title.SelectedIndex == -1) {
                combo_side.IsEnabled = true;
                combo_side.ItemsSource = current_release.GetSides();
                combo_artist.ItemsSource = current_release.GetArtists();
                combo_title.ItemsSource = list_tracks.Where(track => track.Release == current_release);
            } else {
                combo_side.IsEnabled = false;
            }
        }
    }
}
