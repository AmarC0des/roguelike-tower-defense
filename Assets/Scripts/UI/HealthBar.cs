using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RectTransform fill;
    RectTransform tf;
    public bool TruePlayerFalseCastle;
    void Start()
    {
        tf = GetComponent<RectTransform>();
    }
    void Update()
    {
        float fillRatio = TruePlayerFalseCastle
            ? (float)GameManager.Instance.charHP/GameManager.Instance.charMaxHP
            : (float)GameManager.Instance.curCastleHp/GameManager.Instance.maxCastleHp;
        fill.sizeDelta = new Vector2(tf.sizeDelta.x*fillRatio, fill.sizeDelta.y);
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            GameManager.Instance.charHP -= 1;
        }
    }
}
