using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textcomponent;
    public string[] lines;
    public float textSpeed;
    public float delayBetweenLines = 1f;

    private int index;

    void Start()
    {
        textcomponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textcomponent.text == lines[index])
            {
                StopAllCoroutines();
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textcomponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(delayBetweenLines);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textcomponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
