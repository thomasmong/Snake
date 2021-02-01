using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public serpent Serpent;
        public int Dim = 32;
        public int Intervalle = 160;
        public bool GameOn;
        public Timer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ActionUser(object sender, KeyEventArgs e)
        {
            var touche = e.Key;
            switch (touche)
            {
                case Key.Up:
                {
                    if (!Serpent.Vect.SequenceEqual(new int[] {0, 1}))
                    {
                        Serpent.Vect = new int[] {0, -1};
                    }
                    break;
                }
                case Key.Down:
                {
                    if (!Serpent.Vect.SequenceEqual(new int[] {0, -1}))
                    {
                        Serpent.Vect = new int[] {0, 1};
                    }
                    break;
                }
                case Key.Left:
                {
                    if (!Serpent.Vect.SequenceEqual(new int[] {1, 0}))
                    {
                        Serpent.Vect = new int[] {-1, 0};
                    }
                    break;
                }
                case Key.Right:
                {
                    if (!Serpent.Vect.SequenceEqual(new int[] {-1, 0}))
                    {
                        Serpent.Vect = new int[] {1, 0};
                    }
                    break;
                }
                case Key.Space:
                {
                    GameOn = !GameOn;
                    break;
                }
                case Key.Escape:
                {
                    GameOn = false;
                    timer.Enabled = false;
                    break;
                }
            }
        }

        private void NouvellePartie(object sender, RoutedEventArgs e)
        {
            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = Intervalle;
            Canvas.Children.Clear();
            Serpent = new serpent(Dim,Canvas);
            Canvas.IsEnabled = true;
            Canvas.Focus();
            timer.Elapsed += Update;
            timer.AutoReset = true;
            timer.Enabled = true;
            GameOn = true;
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            if (GameOn && !Serpent.IsDead)
            {
                Dispatcher.Invoke(() => { Serpent.Update(); });
            }
        }
    }
}