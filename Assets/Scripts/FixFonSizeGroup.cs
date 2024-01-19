using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FixFonSizeGroup : MonoBehaviour
{
    [SerializeField]
    TMP_Text[] _text = new TMP_Text[0];

    // Start is called before the first frame update
    void Start()
    {
        if (_text.Length < 1)
        {
            return;
        }

        StartCoroutine(FixSize());
    }

    IEnumerator FixSize()
    {
        yield return null;

        float size = -1;

        for (int i = 0; i < _text.Length; i++)
        {
            if (size < 0)
            {
                size = _text[i].fontSize;
                continue;
            }

            if (_text[i].fontSize < size)
            {
                size = _text[i].fontSize;
            }
        }

        for (int i = 0; i < _text.Length; i++)
        {
            _text[i].enableAutoSizing = false;
            _text[i].fontSize = size;
        }
    }
}
