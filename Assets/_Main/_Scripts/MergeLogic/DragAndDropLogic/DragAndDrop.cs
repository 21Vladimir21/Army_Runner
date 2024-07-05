using UnityEngine;
using UnityEngine.Events;

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
        private bool _isDragged;
        private Transform _draggedObject;
        private CellToMerge _startDragCell;
        private CellToMerge _selectedCell;
        private CellToMerge _lastSelectedCell;

        public UnityEvent<CellToMerge> OnUpObject { get; } = new();
        public UnityEvent<CellToMerge> OnSelectNewObject { get; } = new();
        public UnityEvent OnMouseUp { get; } = new();

        public DragAndDrop(DragConfig config, Camera camera)
        {
            _heightWhenLifting = config.heightWhenLifting;

            _maxXPosition = config.maxXPosition;
            _minXPosition = config.minXPosition;
            _maxZPosition = config.maxZPosition;
            _minZPosition = config.minZPosition;
            _camera = camera;
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
            if (_isDragged == false && cell.IsBusy)
            {
                _startDragCell = cell;
                _draggedObject = cell.currentObject.transform;
                _startDragCell.OnReturnObject.AddListener(ResetDrag);
                OnUpObject.Invoke(cell);
                _isDragged = true;
            }
        }

        private void SelectNewCell(CellToMerge cell)
        {
            if (_startDragCell != cell && cell != _lastSelectedCell)
            {
                _selectedCell = cell;
                _lastSelectedCell = cell;
                OnSelectNewObject.Invoke(_selectedCell);
            }
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

        private void ResetDrag()
        {
            if (_selectedCell != null && _selectedCell.IsBusy == false)
            {
                _selectedCell.AddObject(_startDragCell.currentObject);
                _startDragCell.RemoveObject();
                _selectedCell.ResetCurrentObjectPosition();
            }
            else if (_selectedCell != null && _selectedCell.IsBusy)
            {
                OnMouseUp.Invoke();
            }

            else if (_draggedObject != null)
            {
                _startDragCell.ResetCurrentObjectPosition();
            }

            if (_startDragCell != null)
            {
                _startDragCell.OnReturnObject.RemoveListener(ResetDrag);
            }

            ClearDragState();
        }

        private void ClearDragState()
        {
            _startDragCell = null;
            _draggedObject = null;
            _selectedCell = null;
            _isDragged = false;
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