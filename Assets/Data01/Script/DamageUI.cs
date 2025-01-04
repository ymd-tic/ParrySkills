using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("フェードする時間")][SerializeField] private float feadSpeed = 10.0f;


    //-----privateField--------------------------------------------------------------
    private TextMeshProUGUI damageText;
    private float startAlpha;
    private float curTime = 0;

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

    void Start()
    {
        damageText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        startAlpha = damageText.color.a;
    }


    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.rotation = Camera.main.transform.rotation;

        curTime += Time.deltaTime;
        float colorAlpha = Mathf.Lerp(startAlpha, 0, curTime / feadSpeed);
        Color newColor = damageText.color;
        newColor.a = colorAlpha;
        damageText.color = newColor;

        if (colorAlpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion

}
