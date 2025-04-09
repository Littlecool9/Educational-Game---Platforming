using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public List<Trophy> trophies;
    public float flyTime;
    public TextMeshPro goalText;

    // Start is called before the first frame update
    void Start()
    {
        if (flyTime == 0) flyTime = 1f;
        StartCoroutine(StartAni());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartAni()
    {
        goalText.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        foreach (Trophy trophy in trophies)
        {
            trophy.StartFlyToSlot(trophy.targetSlotPosition);
        }
        yield return new WaitForSeconds(1f);
        goalText.gameObject.SetActive(true);
        yield return new WaitForSeconds(flyTime);
        foreach (Trophy trophy in trophies)
        {
            Destroy(trophy.gameObject);
        }
    }
}
