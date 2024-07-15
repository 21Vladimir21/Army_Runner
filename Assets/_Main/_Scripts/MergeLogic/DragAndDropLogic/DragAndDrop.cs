using _Main._Scripts.CrowdLogic;
using UnityEngine;

namespace _Main._Scripts.MergeLogic.DragAndDropLogic
{
    public class DragAndDrop
    {
        private readonly float _heightWhenLifting;
        private readonly float _maxXPosition;
        private readonly float _minXPosition;
        private readonly float _maxZPosition;
        private readonly float _minZPosition;
        private readonly Camera _camera;
        private readonly RepresentativeOfTheSoldiers _representativeOfTheSoldiers;
        private readonly SoldiersPool _soldiersPool;
        private bool _isDragged;
        private Transform _draggedObject;
        private CellToMerge _startDragCell;
        private CellToMerge _selectedCell;
        private CellToMerge _lastSelectedCell;


        public DragAndDrop(DragConfig config, Camera camera, RepresentativeOfTheSoldiers representativeOfTheSoldiers,
            SoldiersPool soldiersPool)
        {
            _heightWhenLifting = config.heightWhenLifting;

            _maxXPosition = config.maxXPosition;
            _minXPosition = config.minXPosition;
            _maxZPosition = config.maxZPosition;
            _minZPosition = config.minZPosition;
            _camera = camera;
            _representativeOfTheSoldiers = representativeOfTheSoldiers;
            _soldiersPool = soldiersPool;
        }

        public void UpdateDrag()
        {
            HandleMouseDown();
            if (_isDragged) HandleDragging();
            HandleMouseUp();
        }

        private void HandleMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = CastRay();
                if (hit.collider != null && hit.collider.TryGetComponent(out CellToMerge cell)) StartDrag(cell);
            }
        }

        private void HandleDragging()
        {
            var hit = CastRay();
            if (hit.collider != null && hit.collider.TryGetComponent(out CellToMerge cell)) SelectNewCell(cell);

            MoveDraggedObject();
        }

        private void HandleMouseUp()
        {
            if (Input.GetMouseButtonUp(0)) ResetDrag();
        }

        private void StartDrag(CellToMerge cell)
        {
            if (_isDragged || cell.IsBusy == false) return;
            _startDragCell = cell;
            _startDragCell.StartDragObject();
            _draggedObject = cell.currentObject.transform;
            _startDragCell.SelectCell();
            _isDragged = true;
        }

        private void SelectNewCell(CellToMerge cell)
        {
            if (_startDragCell != cell && cell != _lastSelectedCell)
            {
                if (_selectedCell != null) _selectedCell.DeSelectCell();

                _selectedCell = cell;
                _selectedCell.SelectCell();
                _lastSelectedCell = cell;
            }
        }

        private void ResetDrag()
        {
            if (_selectedCell != null && _selectedCell.IsBusy == false) RearrangeSoldier();
            else if (_selectedCell != null && _selectedCell.IsBusy) MergeSoldiers();
            else if (_draggedObject != null) _startDragCell.ResetSoldierPosition();
            ClearDragState();
        }

        private void RearrangeSoldier()
        {
            _selectedCell.AddObject(_startDragCell.currentObject);
            _startDragCell.RemoveObjectData();
            _selectedCell.ResetSoldierPosition();
        }

        private void MergeSoldiers()
        {
            if (_selectedCell.IsBusy == false) return;

            var currentObjectLevel = _startDragCell.currentObject.Level;
            var selectedCellSoldierLevels = _selectedCell.currentObject.Level;
            if (currentObjectLevel != selectedCellSoldierLevels ||
                currentObjectLevel == SoldiersLevels.Level13 || selectedCellSoldierLevels == SoldiersLevels.Level13)
            {
                _startDragCell.ResetSoldierPosition();
                _selectedCell.DeSelectCell();
                return;
            }
            _soldiersPool.ReturnSoldier(_selectedCell.currentObject);
            _soldiersPool.ReturnSoldier(_startDragCell.currentObject);
            _selectedCell.RemoveObjectData();
            _startDragCell.RemoveObjectData();
            _selectedCell.AddObject(_representativeOfTheSoldiers.GetNextObjectLevel(currentObjectLevel), true);
        }

        private void ClearDragState()
        {
            _lastSelectedCell = null;
            _startDragCell = null;
            _draggedObject = null;
            _selectedCell = null;
            _isDragged = false;
        }

        private void MoveDraggedObject()
        {
            if (_draggedObject == null) return;

            var distance = Vector3.Distance(_camera.transform.position, _draggedObject.position);
            var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            var worldPoint = _camera.ScreenToWorldPoint(screenPoint);

            var clampX = Mathf.Clamp(worldPoint.x, _minXPosition, _maxXPosition);
            var clampZ = Mathf.Clamp(worldPoint.z, _minZPosition, _maxZPosition);
            var clampPoint = new Vector3(clampX, _heightWhenLifting, clampZ);
            _draggedObject.position = clampPoint;
        }

        private RaycastHit CastRay()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                return hit;

            return default;
        }
    }
}