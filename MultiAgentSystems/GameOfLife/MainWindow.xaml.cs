using MultiAgentSystemPCL;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        DispatcherTimer updateTimer; // timer pour lancer les mises à jour
        bool running = false; //indiquant si celles-ci sont lancées ou en pause (car on pourra arrêter ou relancer la simulation d’un clic gauche
        GoLGrid grid; // une référence vers la grille.

        public MainWindow()
        {
            InitializeComponent(); // initialise les composants graphiques.
            Loaded += MainWindow_Loaded; // le chargement de la fenêtre est terminé.
            gridCanvas.MouseDown += gridCanvas_MouseDown; // le clic de la souris (droit ou gauche).
        }

        void gridCanvas_MouseDown(object _sender, MouseButtonEventArgs _mouseEvent)
        {
            // change l’état de la cellule située sous le clic (en prenant en compte le fait que chaque cellule est représentée par 3 px en largeur et 3 px en hauteur)
            if (_mouseEvent.LeftButton == MouseButtonState.Pressed)
            {
                grid.ChangeState((int)(_mouseEvent.GetPosition(gridCanvas).X / 3), (int)(_mouseEvent.GetPosition(gridCanvas).Y / 3));
                grid.Update(false); // On demande alors une mise à jour de l’affichage, sans recalculer les cellules vivantes ou mortes.
            }
            // on lance ou stoppe la simulation selon son état actuel. 
            else if (_mouseEvent.RightButton == MouseButtonState.Pressed)
            {
                if (running)
                {
                    updateTimer.Stop();
                }
                else
                {
                    updateTimer.Start();
                }
                running = !running;
            }

        }

        // Lorsque la fenêtre est chargée, on crée une nouvelle grille, et on s’abonne à son évènement de mise à jour.
        void MainWindow_Loaded(object _sender, RoutedEventArgs _e)
        {
            grid = new GoLGrid((int) gridCanvas.ActualWidth / 3, (int) gridCanvas.ActualHeight / 3, 0.1);
            grid.gridUpdatedEvent += grid_gridUpdatedEvent;
            
            // On crée aussi un timer qui se lancera toutes les 500ms.
            updateTimer = new DispatcherTimer(); 
            updateTimer.Tick += updateTimer_Tick;
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            
            // On lance la simulation.
            updateTimer.Start();
            running = true;
        }

        // On efface l’écran actuel, puis on parcourt la grille, et on dessine les cellules vivantes.
        void grid_gridUpdatedEvent(bool[][] _grid)
        {
            gridCanvas.Children.Clear();

            for(int row = 0; row < _grid.Count(); row++) {
                for (int col = 0; col < _grid[0].Count(); col++)
                {
                    if (_grid[row][col])
                    {
                        DrawCell(row, col);
                    }
                }
            }
        }
 
        // Pour les méthodes graphiques, le dessin d’une cellule vivante consiste juste à dessiner un carré de 3 px de côté en noir.
        private void DrawCell(int _row, int _col)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 3;
            rect.Height = 3;
            rect.Margin = new Thickness(3 * _row, 3 * _col, 0, 0);
            rect.Stroke = Brushes.Black;
            rect.Fill = Brushes.Black;

            gridCanvas.Children.Add(rect);
        }

        // À chaque fois que le timer se déclenche, on lance la mise à jour de la grille.
        void updateTimer_Tick(object _sender, EventArgs _e)
        {
            grid.Update();
        }
    }
}
