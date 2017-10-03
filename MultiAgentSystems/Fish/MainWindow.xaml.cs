using MultiAgentSystemPCL;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fish
{
    public partial class MainWindow : Window
    {
        Ocean myOcean; // Reference vers l'objet Ocean.

        public MainWindow()
        {
            InitializeComponent(); // Initialisation des composants.
            Loaded += MainWindow_Loaded; // Evenement indiquant que la fenetre  été chargée.
        }

        void MainWindow_Loaded(object _sender, RoutedEventArgs _e) // Methode lancée sur l'évènement loaded.
        {
            oceanCanvas.MouseDown += oceanCanvas_MouseDown; // Clics de la souris : permet de rajouter des zones à éviter.
            
            // Créer un océan et l’initialiser
            myOcean = new Ocean(250, oceanCanvas.ActualWidth, oceanCanvas.ActualHeight);
            myOcean.oceanUpdatedEvent += myOcean_oceanUpdatedEvent;
            
            // Créer un timer qui nous permettra de lancer de manière régulière le même code
            DispatcherTimer dispatcherTimer = new DispatcherTimer(); 
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 15); // On lance le timer toutes les 15 ms.
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            myOcean.UpdateEnvironnement(); // Mise à jour de l’océan à chaque fois que le timer est lancé.
        }

        // La dernière méthode est celle qui est lancée lorsque l’on reçoit l’évènement de fin de mise à jour de l’océan. Dans ce cas-là, on efface le canvas, puis on dessine tous les poissons puis tous les obstacles. On met enfin à jour l’affichage.
        void myOcean_oceanUpdatedEvent(FishAgent[] _fish, List<BadZone> _obstacles)
        {
            oceanCanvas.Children.Clear();

            foreach (FishAgent fish in _fish)
            {
                DrawFish(fish);
            }

            foreach (BadZone obstacle in _obstacles)
            {
                DrawObstacle(obstacle);
            }
            oceanCanvas.UpdateLayout();
        }

        // partant de la tête du poisson et allant vers sa queue dont les coordonnées sont calculées à partir de la direction du poisson (et donc sa vitesse en x et en y)
        private void DrawObstacle(BadZone _obstacle)
        {
            Ellipse circle = new Ellipse();
            circle.Stroke = Brushes.Black;
            circle.Width = 2 * _obstacle.Radius;
            circle.Height = 2 * _obstacle.Radius;
            circle.Margin = new Thickness(_obstacle.PosX - _obstacle.Radius, _obstacle.PosY - _obstacle.Radius, 0, 0);
            oceanCanvas.Children.Add(circle);
        }

        // Poisson représenté par un trait de 10px partant de la tête du poisson et allant vers sa queue dont les coordonnées sont calculées à partir de la direction du poisson (et donc sa vitesse en x et en y).
        private void DrawFish(FishAgent _fish)
        {
            Line body = new Line();
            body.Stroke = Brushes.Black;
            body.X1 = _fish.PosX;
            body.Y1 = _fish.PosY;
            body.X2 = _fish.PosX - 10 * _fish.SpeedX;
            body.Y2 = _fish.PosY - 10 * _fish.SpeedY;
            oceanCanvas.Children.Add(body);
        }
 
        // On rajoute une zone à éviter à l’endroit du clic.
        void oceanCanvas_MouseDown(object _sender, MouseButtonEventArgs _mouseEvent)
        {
            myOcean.AddObstacle(_mouseEvent.GetPosition(oceanCanvas).X, _mouseEvent.GetPosition(oceanCanvas).Y, 10);
        }
    }
}
