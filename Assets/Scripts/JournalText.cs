using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public string[] lines;
    public int textSpeed;

    private int index;
    public Button[] activeButton;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        //startDialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startDialogue(int textTodisplay)
    {
        textComponent.text = string.Empty;

        index = textTodisplay;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(1 / textSpeed);
        }
    }

    public void resetAll()
    {
        foreach(Button entry in activeButton)
        {
            entry.interactable = false;
        }
    }

    /*
    void NextLine(int textToDisplay)
    {

        StartCoroutine(TypeLine());
    }
    */
}
