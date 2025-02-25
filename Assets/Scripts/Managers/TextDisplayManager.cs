using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDisplayManager : MonoBehaviour, IManager
{
    [SerializeField]
    private GameObject textPrefab;
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnEnable()
    {
        EnemyBase.OnEnemyDefeated += ShowMoneyText;
    }

    private void OnDisable()
    {
        EnemyBase.OnEnemyDefeated -= ShowMoneyText;
    }

    private void ShowMoneyText(Vector3 position, int money)
    {
        if (textPrefab == null)
        {
            Debug.LogError("MoneyText is null");
        }

        GameObject textObject = Instantiate(textPrefab, position, Quaternion.identity);
        TMP_Text textComponent = textObject.GetComponentInChildren<TMP_Text>();

        if (textComponent != null)
        {
            textComponent.text = $"+{money}";
        } else
        {
            Debug.LogError("TextComponent is null");
            return;
        }

        StartCoroutine(FadeAndMoveText(textObject, textComponent));
    }

    private IEnumerator FadeAndMoveText(GameObject textObj, TMP_Text textComponent)
    {
        float duration = 1.5f;
        float speed = 1.25f;
        float elapsedTime = 0f;

        Color startColor = textComponent.color;
        Vector3 startPosition = textObj.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            textObj.transform.position = startPosition + new Vector3(0, elapsedTime * speed, 0);
            textObj.transform.rotation = Quaternion.LookRotation(textObj.transform.position - Camera.main.transform.position);

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime);
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }
        Destroy(textObj);
    }
}
