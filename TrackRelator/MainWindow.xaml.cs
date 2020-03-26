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
        private List<Release> list_release;
        public MainWindow() {
            LoadReleases();
            InitializeComponent();
        }
        private void LoadReleases() {

        }
    }
}
