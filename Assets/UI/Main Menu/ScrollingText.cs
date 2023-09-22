using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    //Text variables
    private RectTransform text;
    private RectTransform duplicateText;

    //Speed variable
    public float scrollSpeed = 65;

    //Get the text variable
    private void Start()
    {
        text = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        //If the text overflows from the boundaries, create a duplicate of the text variable
        if (text.rect.width > GetComponent<RectTransform>().rect.width && !duplicateText)
        {
            duplicateText = Instantiate(text);
            duplicateText.transform.SetParent(transform, false);
            duplicateText.localPosition = new Vector3(text.rect.width + 100, text.localPosition.y, text.localPosition.z);
        }
        //Scroll the text and duplicate text variables
        else if (text.rect.width > GetComponent<RectTransform>().rect.width && duplicateText)
        {
            //Scroll text
            text.transform.Translate(new Vector3(-1, 0, 0) * scrollSpeed * Time.deltaTime);
            duplicateText.transform.Translate(new Vector3(-1, 0, 0) * scrollSpeed * Time.deltaTime);

            //If the text is no longer in the boundaries, position the text behind the other text variable
            if (text.localPosition.x + text.rect.width / 2 <= -GetComponent<RectTransform>().rect.width / 2)
            {
                text.localPosition = new Vector3(duplicateText.localPosition.x + duplicateText.rect.width + 100, duplicateText.localPosition.y, duplicateText.localPosition.z);
            }
            if (duplicateText.localPosition.x + duplicateText.rect.width / 2 <= -GetComponent<RectTransform>().rect.width / 2)
            {
                duplicateText.localPosition = new Vector3(text.localPosition.x + text.rect.width + 100, text.localPosition.y, text.localPosition.z);
            }
        }
        //If the text does not overflow but there is still a duplicate text variable, destroy the duplicate text variable and reset the text position
        else if (text.rect.width < GetComponent<RectTransform>().rect.width && duplicateText)
        {
            Destroy(duplicateText.gameObject);
            duplicateText = null;

            text.localPosition = new Vector3(0, 0, 0);
        }
    }
}
