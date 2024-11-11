using UnityEngine;
using UnityEngine.EventSystems;

//가상 조이스틱
public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private PlayerMovement pm = null; //플레이어 움직임 담당 컴포넌트

    //가상 조이스틱 및 레버 관련
    public GameObject virtualJoystick = null;
    public RectTransform leverRectTransform = null;
    private RectTransform virtualJoystickRectTransform = null;
    private Vector2 virtualJoystickPosition = Vector2.zero;
    private float virtualJoystickRadius = 0f;

    private bool isInput = false;
    public Vector2 inputVec = Vector2.zero;

    public bool IsInput {
        get { return isInput; }
        set { isInput = value; }
    }

    private void Start() {
        pm = GameManager.Inst.player.GetComponent<PlayerMovement>();
        virtualJoystickRectTransform = virtualJoystick.GetComponent<RectTransform>();
        virtualJoystickRadius = virtualJoystickRectTransform.sizeDelta.x / 2f;
        virtualJoystick.SetActive(false);
    }

    private void Update() {
        //OnDrag 이벤트는 드래그 시작 후 가만히 있을 때 호출이 안되기 때문에
        //드래그 시작부터 끝날 때까지 지속해서 입력 벡터를 전송하기 위해 isInput변수를 사용
        if (isInput) SendInputVector();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        isInput = true;

        virtualJoystickRectTransform.anchoredPosition = eventData.position;
        virtualJoystick.SetActive(true);
        virtualJoystickPosition = eventData.position;

        ControlJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        ControlJoystick(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        isInput = false;

        pm.SendMessage("Stop");

        virtualJoystick.SetActive(false);
        leverRectTransform.anchoredPosition = Vector2.zero;
    }

    //조이스틱 조작
    private void ControlJoystick(PointerEventData eventData) {
        Vector2 inputDir = eventData.position - virtualJoystickPosition;
        //레버가 가상 조이스틱의 경계를 넘지 않아야 하기 때문에 inputDir의 크기가 가상 조이스틱의 반지름 보다 크다면
        //inputDir의 방향벡터에 반지름을 곱해 위치 조정
        Vector2 limitDir = ((inputDir.magnitude < virtualJoystickRadius) ? inputDir : inputDir.normalized * virtualJoystickRadius);
        leverRectTransform.anchoredPosition = limitDir;
        //limitDir을 가상 조이스틱의 반지름으로 나누어 0~1로 정규화
        inputVec = limitDir / virtualJoystickRadius;
    }

    //PlyaerMovement 컴포넌트에 움직여야 할 벡터 전송
    private void SendInputVector() {
        if (pm) {
            pm.Move(inputVec);
        }
    }
}
