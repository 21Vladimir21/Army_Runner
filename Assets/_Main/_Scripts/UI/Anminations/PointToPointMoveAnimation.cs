using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PointToPointMoveAnimation : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private Transform moveObject;
    

    private void OnEnable()
    {
        moveObject.localPosition = points[0].localPosition;
        points.Add(points[0]);
        var path = new Vector3[points.Count];
        for (var i = 0; i < points.Count; i++)
            path[i] = points[i].localPosition;
        
        var seq = DOTween.Sequence();
        seq.Append(moveObject.DOLocalPath(path, 3, PathType.CatmullRom).SetEase(Ease.Linear));
        seq.SetLoops(-1);
    }
}