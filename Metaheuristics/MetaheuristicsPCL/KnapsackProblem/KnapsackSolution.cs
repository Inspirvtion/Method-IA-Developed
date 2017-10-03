using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaheuristicsPCL
{
    public class KnapsackSolution : ISolution
    {
        public double Weight 
        {
			/* Poids contenu dans le sac à dos. Celui-ci est recalculé à chaque fois, comme étant la somme 
			 * des poids des boîtes contenues.*/
            get
            {
                return LoadedContent.Sum(x => x.Weight);
            }
        }

        public double Value
        {
			// Valeur totale du contenu du sac. Celle-ci est aussi recalculée à chaque demande.
            get
            {
                return LoadedContent.Sum(x => x.Value);
            }
        }

		List<Box> loadedContent; // correspond au contenu chargé dans le sac à dos et est donc une liste de boîtes.
        public List<Box> LoadedContent
        {
            get
            {
                return loadedContent; // getter.
            }
            set
            {
                loadedContent = value; // Setter.
            }
        }

        public KnapsackSolution()
        {
			// Constructeur par défaut, qui ne fait que créer une nouvelle liste vide de contenu.
			loadedContent = new List<Box> ();
        }

        public KnapsackSolution(KnapsackSolution _solution)
        {
			// Constructeur qui copie le contenu de la solution passée en paramètre.
            loadedContent = new List<Box>();
            loadedContent.AddRange(_solution.loadedContent);
        }

        public override string ToString() // Conversion en chaine de caractéres.
        {
			// Value : 54 - Weight : 19Kg
			// Loaded : A - D - E - G.
            String res = "Value : " + Value + " - Weight : " + Weight + "\n";
            res += "Loaded : ";
            res += String.Join(" - ", loadedContent);
            return res;
        }

        public override bool Equals(object _object) // Comparaison de deux solutions.
        {
            KnapsackSolution solution = (KnapsackSolution)_object;
            if (solution.loadedContent.Count != loadedContent.Count || solution.Value != Value || solution.Weight != Weight)
            {
				// Si Nombre de box différent | Valeur differente | Poids différent.
                return false;
            }
            else
            {
                foreach (Box box in loadedContent) {
                    if (!solution.loadedContent.Contains(box))
                    {
						// On teste alors si chaque boîte contenue dans le premier sac à dos se retrouve dans le deuxième.
                        return false;
                    }
                }

            }
            return true;
        }

        public override int GetHashCode()
        {
			// Différencier rapidement des solutions si elles sont utilisées dans un dictionnaire.
            return (int) (Value * Weight);
        }
    }
}
