using _Main._Scripts.CrowdLogic;

namespace _Main._Scripts.MergeLogic
{
    public class RepresentativeOfTheSoldiers
    {
        private readonly SoldiersPool _soldiersPool;

        public RepresentativeOfTheSoldiers(SoldiersPool soldiersPool)
        {
            _soldiersPool = soldiersPool;
        }


        public DraggableObject GetNextObjectLevel(SoldiersLevels level) => GetSoldier(level + 1);

        public DraggableObject GetSoldier(SoldiersLevels level) =>
            _soldiersPool.GetSoldierFromLevel<DraggableObject>(level);
    }
}