using UnityEngine;
using DG;
using DG.Tweening;

public class MinionVfx : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer SR{get;set;}

    public void TakeDamage(Color cor, float CD)
    {
        Sequence seq = DOTween.Sequence();
        seq.Insert(0f, SR.DOColor(cor, CD));
        seq.Append(SR.DOColor(Color.white, 0f));
        seq.Play();
    }
}
