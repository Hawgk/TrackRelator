using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TrackRelator {
    public partial class CreateRelation : Window {
        private TrackInputHandler track_1, track_2;
        private TrackDatabase track_database;

        public CreateRelation(MainWindow window, TrackDatabase track_database) {
            this.Owner = window;
            this.track_database = track_database;
            InitializeComponent();
            track_1 = new TrackInputHandler(combo_title1, combo_artist1, combo_label1, combo_release1, combo_side1, button_reset1, track_database);
            track_2 = new TrackInputHandler(combo_title2, combo_artist2, combo_label2, combo_release2, combo_side2, button_reset2, track_database);
        }
        private void combo_title1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_1.TitleChanged();
        }
        private void combo_artist1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_1.ArtistChanged();
        }
        private void combo_label1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_1.LabelChanged();
        }
        private void combo_release1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_1.ReleaseChanged();
        }
        private void combo_side1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_1.SideChanged();
        }
        private void button_reset1_Click(object sender, RoutedEventArgs e) {
            track_1.Reset();
        }
        private void combo_title2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_2.TitleChanged();
        }
        private void combo_artist2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_2.ArtistChanged();
        }
        private void combo_label2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_2.LabelChanged();
        }
        private void combo_release2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_2.ReleaseChanged();
        }
        private void combo_side2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            track_2.SideChanged();
        }
        private void button_reset2_Click(object sender, RoutedEventArgs e) {
            track_2.Reset();
        }
        private void button_save_Click(object sender, RoutedEventArgs e) {
            Relation relation = new Relation();
            relation.First_track = track_1.Current_track;
            relation.Second_track = track_2.Current_track;
            relation.First_track.Relations.Add(relation);
            relation = new Relation();
            relation.First_track = track_2.Current_track;
            relation.Second_track = track_1.Current_track;
            relation.First_track.Relations.Add(relation);
            track_database.SaveRelations();
            this.Close();
            MessageBox.Show("Successfully saved!");
        }
        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
