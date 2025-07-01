using UnityEngine;
using UnityEngine.EventSystems;

//Virtual joystick
public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    //Player field
    private PlayerState ps = null; //Player state
    public State playerIdle; //Player idle state
    public State playerMove; //Player move state

    //Virtual joystick field
    public GameObject virtualJoystick = null; //Virtual joystick UI
    public RectTransform leverTransform = null; //Lever transform
    private RectTransform virtualJoystickTransform = null; //Virtual joystick transform
    private Vector2 virtualJoystickPos = Vector2.zero; //Virtual joystick position
    private float virtualJoystickRadius = 0f; //virtual joystick radius

    private bool isInput = false; //Input frag
    public Vector2 inputVec = Vector2.zero; //Input vector

    private void Start() {
        ps = GameManager.Inst.ps;
        virtualJoystickTransform = virtualJoystick.GetComponent<RectTransform>();
        virtualJoystickRadius = virtualJoystickTransform.sizeDelta.x / 2f;
    }

    private void OnEnable() {
        inputVec = Vector2.zero;
    }

    private void Update() {
        if (isInput) SendInputVector();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        isInput = true;

        virtualJoystickTransform.anchoredPosition = eventData.position; //가상 조이스틱 드래그 시작 지점으로 이동
        virtualJoystickPos = eventData.position; //가상 조이스틱 중심 좌표 저장
        virtualJoystick.SetActive(true); //가상 조이스틱 UI 활성화

        GetInputVec(eventData);

        ps.SetState(playerMove);
        ps.Action();
    }

    public void OnDrag(PointerEventData eventData) {
        GetInputVec(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        isInput = false;

        ps.SetState(playerIdle);
        ps.Action();
        virtualJoystick.SetActive(false);
    }

    //Get input vector
    private void GetInputVec(PointerEventData eventData) {
        Vector2 inputDir = eventData.position - virtualJoystickPos; //드래그 방향 벡터
        //inputDir의 벡터 크기가 가상 조이스틱 반지름을 넘어가면 벡터 크기를 반지름으로 고정
        if (inputDir.sqrMagnitude > virtualJoystickRadius * virtualJoystickRadius) {
            inputDir = inputDir.normalized * virtualJoystickRadius;
        }
        leverTransform.anchoredPosition = inputDir; //레버 UI 위치 설정
        inputVec = inputDir / virtualJoystickRadius; //inputDir을 가상 조이스틱 반지름 기준으로 정규화
    }

    //Send vector to player
    private void SendInputVector() {
        if (ps is null) return;

        ps.inputVec = inputVec;
    }
}
