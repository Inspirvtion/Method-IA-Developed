
namespace MetaheuristicsPCL
{
    public abstract class Algorithm
    {
        protected IProblem pb; // Probleme à resoudre.
        protected IHM ihm; // Resultat de l'algorithme.
        public virtual void Solve(IProblem _pb, IHM _ihm) // Resolution de l'algorithme.
        {
            pb = _pb;
            ihm = _ihm;
        }

        protected abstract void SendResult(); // Envoi du résultat. 
    }
}
