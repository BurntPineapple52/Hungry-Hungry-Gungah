using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusTile : MonoBehaviour
{
    [SerializeField]
    float _timeOnScreen = 4f;

    [SerializeField]
    Image _fill;

    float _timer = 0;

    bool _active = false;

    private void Update()
    {
        if (!_active)
        {
            if (_timer >= _timeOnScreen)
            {
                ManagerLocator.Instance.Game.RemoveBonusTile();
            }

            return;
        }

        _fill.fillAmount = Mathf.Lerp(1, 0, _timer / _timeOnScreen);
        _timer += Time.deltaTime;

        if (_timer >= _timeOnScreen)
        {
            _active = false;
            _fill.fillAmount = 1;
        }
    }

    public void StartOnPosition(Vector3 pos)
    {
        transform.position = pos;
        _active = true;
    }
}
