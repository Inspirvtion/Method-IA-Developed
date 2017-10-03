using System;
using System.Collections.Generic;

namespace MultiAgentSystemPCL
{
    public delegate void EnvironmentUpdated(List<Waste> _waste, List<WasteAgent> _agents);

    public class WasteEnvironment
    {
		List<Waste> wasteList; // La liste des tas de déchets présents dans l’environnement.
		List<WasteAgent> agents; // La liste des agents nettoyeurs.
		public Random randomGenerator; // Un générateur aléatoire.
		double MAX_WIDTH; // La taille de l’espace grâce à MAX_WIDTH et MAX_HEIGHT.
        double MAX_HEIGHT;
		protected int nbIterations = 0; // Le nombre d’itérations depuis le début.

		public event EnvironmentUpdated environmentUpdatedEvent; // un évènement indiquant qu’elle est terminée (la mise à jour).

        public WasteEnvironment(int _nbWaste, int _nbAgents, double _width, double _height, int _nbWasteTypes)
        {
			// Par : paramètre le nombre de déchets, d’agents, la taille de l’environnement et le nombre de types de déchets.
            if (randomGenerator == null)
            {
                randomGenerator = new Random();
            }
            MAX_WIDTH = _width;
            MAX_HEIGHT = _height;

			// Creation des dechets.
            wasteList = new List<Waste>();
            for (int i = 0; i < _nbWaste; i++)
            {
                Waste waste = new Waste(randomGenerator.NextDouble() * MAX_WIDTH, randomGenerator.NextDouble() * MAX_HEIGHT, randomGenerator.Next(_nbWasteTypes));
                wasteList.Add(waste);
            }

			// Creation des agents.
            agents = new List<WasteAgent>();
            for (int i = 0; i < _nbAgents; i++)
            {
                WasteAgent agent = new WasteAgent(randomGenerator.NextDouble() * MAX_WIDTH, randomGenerator.NextDouble() * MAX_HEIGHT, this);
                agents.Add(agent);
            }
        }

        public void Update()
        {
            foreach (WasteAgent agent in agents) // Pour chaque agent.
            {
                agent.UpdateDirection(wasteList); // Calcule de speedX et SpeedY.
				agent.UpdatePosition(MAX_WIDTH, MAX_HEIGHT); // PosX += STEP * speedX ; PosY += STEP * speedY.
            }

            nbIterations++; // On incremente le nombre d'iterations.

			/* Comme les agents sont "aspirés" par le premier tas à portée, il y a un biais. 
			 * Toutes les 500 itérations on inverse donc l’ordre des tas, de manière à contrer ce biais*/
            if (nbIterations % 500 == 0)
            {
                wasteList.Reverse();
            }

            if (environmentUpdatedEvent != null)
            {
                environmentUpdatedEvent(wasteList, agents);
            }
        }

        internal Waste TakeWaste(Waste _goal)
        {
			if (_goal.Size == 1) // s’il n’y a qu’un élément.
            {
				wasteList.Remove(_goal); // On supprime alors le tas de la liste des déchets.
				return _goal; //  l’agent va le récupérer
            }
			else // Si par contre le tas contient plusieurs éléments.
            {
				_goal.Decrease(); // on va juste le décrémenter.
				Waste load = new Waste(_goal); // renvoyer un nouvel élément créé par copie (et donc ayant une charge de 1).
                return load;
            }
        }

		// indiquer qu’un agent a déposé un nouvel élément sur un tas existant : incrémenter la taille du tas visé.
        internal void SetWaste(Waste _goal)
        {
            _goal.Increase();
        }
    }
}
