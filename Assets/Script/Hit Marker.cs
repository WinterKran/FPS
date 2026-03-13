using UnityEngine;
using System.Collections;

public class HitMarkerUI : MonoBehaviour
{
    public GameObject hitMarkerImage;
    public float showTime = 0.1f;

    public void ShowHitMarker()
    {
        StartCoroutine(ShowMarker());
    }

    IEnumerator ShowMarker()
    {
        hitMarkerImage.SetActive(true);

        yield return new WaitForSeconds(showTime);

        hitMarkerImage.SetActive(false);
    }
}