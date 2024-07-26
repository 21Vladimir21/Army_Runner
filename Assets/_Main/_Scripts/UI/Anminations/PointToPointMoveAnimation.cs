using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PointToPointMoveAnimation : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private Transform moveObject;
    private Sequence _sequence;


    private void OnEnable()
    {
        moveObject.localPosition = points[0].localPosition;
        points.Add(points[0]);
        var path = new Vector3[points.Count];
        for (var i = 0; i < points.Count; i++)
            path[i] = points[i].localPosition;
        
        _sequence = DOTween.Sequence();
        _sequence.Append(moveObject.DOLocalPath(path, 3, PathType.CatmullRom).SetEase(Ease.Linear));
        _sequence.SetLoops(-1);
    }
    private void OnDisable()
    {
        _sequence.Kill();
    }
    
}