using UnityEditor;
using UnityEngine;
using PixelCrew.Utils;
using System;
using UnityEngine.Events;
using System.Linq;

public class CheckCircleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private string[] _tags;

    private readonly Collider2D[] _interactResult = new Collider2D[10];

    [SerializeField] private OnOverlapEvent _onOverLap;

    private void OnDrawGizmosSelected()
    {
        Handles.color = HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
    }

    public void Check()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactResult, _mask);

        for (int i = 0; i < size; i++)
        {
            var overlapResult = _interactResult[i];
            var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
            if (isInTags)
            {
                _onOverLap?.Invoke(overlapResult.gameObject);
            }
        }
    }

    [Serializable]
    public class OnOverlapEvent : UnityEvent<GameObject>
    {

    }
}