
namespace PathfindingPCL
{
    public abstract class Node
    {

		private Node precursor = null;//Le précurseur, qui est aussi un nœud.
        internal Node Precursor
        {
            get
            {
                return precursor;
            }
            set
            {
                precursor = value;
            }
        }

		private double distanceFromBegin = double.PositiveInfinity; // La distance depuis le départ.
        internal double DistanceFromBegin
        {
            get
            {
                return distanceFromBegin;
            }
            set
            {
                distanceFromBegin = value;
            }
        }

		internal double EstimatedDistance { get; set; } // La distance estimée à la sortie (si nécessaire).
    }
}
