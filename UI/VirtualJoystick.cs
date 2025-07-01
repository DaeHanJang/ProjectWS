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

        virtualJoystickTransform.anchoredPosition = eventData.position; //���� ���̽�ƽ �巡�� ���� �������� �̵�
        virtualJoystickPos = eventData.position; //���� ���̽�ƽ �߽� ��ǥ ����
        virtualJoystick.SetActive(true); //���� ���̽�ƽ UI Ȱ��ȭ

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
        Vector2 inputDir = eventData.position - virtualJoystickPos; //�巡�� ���� ����
        //inputDir�� ���� ũ�Ⱑ ���� ���̽�ƽ �������� �Ѿ�� ���� ũ�⸦ ���������� ����
        if (inputDir.sqrMagnitude > virtualJoystickRadius * virtualJoystickRadius) {
            inputDir = inputDir.normalized * virtualJoystickRadius;
        }
        leverTransform.anchoredPosition = inputDir; //���� UI ��ġ ����
        inputVec = inputDir / virtualJoystickRadius; //inputDir�� ���� ���̽�ƽ ������ �������� ����ȭ
    }

    //Send vector to player
    private void SendInputVector() {
        if (ps is null) return;

        ps.inputVec = inputVec;
    }
}
