using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// JoyStickZone에 적용
// joyStickBG에 JoyStickBG 할당
// joyStickCtrl에 JoyStickCtrl 할당

// JoyStick의 움직임을 구현

public class JoyStickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region 변수
    [SerializeField] RectTransform joyStickBG;
    [SerializeField] RectTransform joyStickCtrl;
    private RectTransform baseRect = null;

    private float deadZone = 0;
    private float ctrlZone = 1;
    private Vector2 touchPosition;

    private Canvas canvas;
    private Camera camera;

    public float Horizontal { get { return touchPosition.x; } }
    public float Vertical { get { return touchPosition.y; } }

    #endregion // 변수

    #region 함수
    private void Awake()
    {
        // 조이스틱 이미지 비활성화
        joyStickBG.gameObject.SetActive(false);

        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    // 조이스틱을 움직이기 시작할 때
    public void OnPointerDown(PointerEventData _eventData)
    {
        joyStickBG.anchoredPosition = ScreenPointPosition(_eventData.position);

        // 조이스틱 이미지 활성화
        joyStickBG.gameObject.SetActive(true);

        OnDrag(_eventData);
    }

    // 조이스틱을 움직이고 있을 때
    public void OnDrag(PointerEventData _eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(camera, joyStickBG.position);
        Vector2 radius = joyStickBG.sizeDelta / 2;
        touchPosition = (_eventData.position) / (radius * canvas.scaleFactor);
        CtrlInput(touchPosition.magnitude, touchPosition.normalized);
        joyStickCtrl.anchoredPosition = touchPosition * radius * ctrlZone;
    }

    private void CtrlInput(float _magnitude, Vector2 _normalized)
    {
        if(_magnitude > deadZone)
        {
            if (_magnitude > 1)
            {
                touchPosition = _normalized;
            }
        }
        else
        {
            touchPosition = Vector2.zero;
        }
    }

    // 조이스틱을 움직이기를 끝냈을 때
    public void OnPointerUp(PointerEventData _eventData)
    {
        // 조이스틱 이미지 비활성화
        joyStickBG.gameObject.SetActive(false);

        // joyStickCtrl 위치 초기화
        joyStickCtrl.anchoredPosition = Vector2.zero;
    }

    // 조이스틱 터치 부분
    public Vector2 ScreenPointPosition(Vector2 _pointPosition)
    {
        Vector2 localPoint = Vector2.zero;

        // 조이스틱을 터치한 포지션이 JoyStickZone 이내일 때
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, _pointPosition, camera, out localPoint))
        {
            return localPoint - (joyStickBG.anchorMax * baseRect.sizeDelta) + (baseRect.pivot * baseRect.sizeDelta);
        }
        return Vector2.zero;
    }
    #endregion // 함수
}
