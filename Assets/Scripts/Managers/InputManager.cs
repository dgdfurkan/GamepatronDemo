using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    #region Serializable Variables

    #region Private Variables

    [Header("Data")] private InputData _data;
    private bool _isAvailableForTouch;

    #endregion

    #endregion

    #endregion

    #endregion

    private void Awake()
    {
        _data = GetInputData();
    }

    private InputData GetInputData() => Resources.Load<CD_Input>("Data/CD_Input").InputData;

    private void Update()
    {
        if (!_isAvailableForTouch) return;

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.CompareTag("NPC"))
                {
                    //hit.collider.GetComponent<NPCManager>().Interact();
                }
                else if (hit.collider.CompareTag("Breakable"))
                {
                    //hit.collider.GetComponent<BreakableObject>().Interact();
                }
                else
                {
                    //var bullet = Instantiate(_data.bulletPrefab, hit.point, Quaternion.identity);
                    //bullet.GetComponent<Bullet>().Fire(hit.point);
                }
            }
        }
    }

    private bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}