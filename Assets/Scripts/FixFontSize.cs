using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FixFontSize : MonoBehaviour
{
    TMP_Text _text;

    private void Awake()
    {
        TryGetComponent<TMP_Text>(out _text);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_text == null)
        {
            return;
        }

        StartCoroutine(FixSize());
    }

    IEnumerator FixSize()
    {
        yield return null;
        float size = _text.fontSize;
        yield return null;
        _text.enableAutoSizing = false;
        _text.fontSize = size;
    }
}
