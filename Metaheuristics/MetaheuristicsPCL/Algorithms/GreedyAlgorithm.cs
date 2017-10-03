
namespace MetaheuristicsPCL
{
    public abstract class GreedyAlgorithm : Algorithm
    {
        public override sealed void Solve(IProblem _pb, IHM _ihm)
        {
            base.Solve(_pb, _ihm); // Appel de la methode Solve du parent Classe Algorithme.
            ConstructSolution(); // Construction de la solution.
            SendResult(); // Envoie du resultat.
        }

        protected abstract void ConstructSolution();
    }
}
