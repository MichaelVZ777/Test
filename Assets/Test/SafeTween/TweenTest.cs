using UnityEngine;
using System.Collections;
using SafeTween;

public class TweenTest : MonoBehaviour
{
    public Easings.Functions function;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        var target = startPosition + Vector3.right*10;
       
        var tweener = new Tweener();
        tweener.Add(new TweenLocalPosition(transform, startPosition, target, 0, 1).SetInterpolation(function));
        tweener.Add(new TweenLocalPosition(transform, target, startPosition, 1, 2).SetInterpolation(function));
        tweener.repeat = true;
        tweener.Play();
    }
}
