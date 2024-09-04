using System.Collections.Generic;
using System.Linq;
using _Main._Scripts.CrowdLogic;
using Kimicu.YandexGames;
using SoundService.Scripts;

namespace _Main._Scripts.MergeLogic
{
    public class AutoMerger
    {
        private List<CellToMerge> _gameCells;
        private List<CellToMerge> _reserveCells;
        private RepresentativeOfTheSoldiers _representativeOfTheSoldiers;
        private SoldiersPool _soldiersPool;

        public AutoMerger(List<CellToMerge> gameCells, List<CellToMerge> reserveCells,
            RepresentativeOfTheSoldiers representativeOfTheSoldiers, SoldiersPool soldiersPool)
        {
            _gameCells = gameCells;
            _reserveCells = reserveCells;
            _representativeOfTheSoldiers = representativeOfTheSoldiers;
            _soldiersPool = soldiersPool;
        }

        public void AutoMergeAndMoveToGameCells()
        {
            Merge();
            MoveSoldierToGameCells();
            AlignSoldiersWithOutSpaces();
        }

        private void AlignSoldiersWithOutSpaces()
        {
            List<DraggableObject> solders = new();
            foreach (var cell in _gameCells)
            {
                if (!cell.IsBusy) continue;
                solders.Add(cell.currentObject);
                cell.RemoveObjectData();
            }

            for (int i = 0; i < solders.Count; i++) _gameCells[i].AddObject(solders[i]);
        }

        private void MoveSoldierToGameCells()
        {
            foreach (var cell in _reserveCells)
            {
                if (cell.IsBusy == false) continue;

                var index = TryGetFreeIndexInList(_gameCells);
                if (index == null)
                    break;
                var soldier = cell.currentObject;
                cell.RemoveObjectData();
                _gameCells[(int)index].AddObject(soldier);
            }
        }

        private void Merge()
        {
            List<CellToMerge> allCells = new();
            allCells.AddRange(_gameCells);
            allCells.AddRange(_reserveCells);
            while (true)
            {
                var groups = allCells
                    .Where(cell =>
                        cell.currentObject != null && !cell.currentObject.Level.Equals(SoldiersLevels.Level13))
                    .GroupBy(s => s.currentObject.Level)
                    .ToList();

                bool hasSoldiersForMerge = groups.Any(x => x.Count() > 1);
                if (hasSoldiersForMerge == false) break;

                foreach (var group in groups)
                {
                    var toMerges = group;

                    while (toMerges.Count() >= 2)
                    {
                        if (toMerges.Key.Equals(SoldiersLevels.Level13)) break;

                        var firstSoldier = toMerges.First();
                        _soldiersPool.ReturnSoldier(firstSoldier.currentObject);
                        allCells.First(cell => cell.currentObject == firstSoldier.currentObject)
                            .RemoveObjectData();

                        var secondSoldier = toMerges.Skip(1).First();
                        _soldiersPool.ReturnSoldier(secondSoldier.currentObject);
                        allCells.First(cell => cell.currentObject == secondSoldier.currentObject)
                            .RemoveObjectData();


                        var index = TryGetFreeIndexInList(allCells);
                        if (index == null)
                            return;

                        var nextLevel = _representativeOfTheSoldiers.GetNextObjectLevel(toMerges.Key);

                        allCells[(int)index].AddObject(nextLevel, true);

                        toMerges = allCells
                            .Where(cell => cell.currentObject != null)
                            .GroupBy(s => s.currentObject.Level)
                            .FirstOrDefault(g => g.Key == group.Key);


                        if (toMerges == null || toMerges.Count() < 2)
                            break;
                    }
                }
            }
        }

        private int? TryGetFreeIndexInList(List<CellToMerge> list)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].IsBusy == false)
                    return i;

            return null;
        }
    }
}