using UnityEngine;
using UnityEngine.EventSystems;

//���� ���̽�ƽ
public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private PlayerMovement pm = null; //�÷��̾� ������ ��� ������Ʈ

    //���� ���̽�ƽ �� ���� ����
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
        //OnDrag �̺�Ʈ�� �巡�� ���� �� ������ ���� �� ȣ���� �ȵǱ� ������
        //�巡�� ���ۺ��� ���� ������ �����ؼ� �Է� ���͸� �����ϱ� ���� isInput������ ���
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

    //���̽�ƽ ����
    private void ControlJoystick(PointerEventData eventData) {
        Vector2 inputDir = eventData.position - virtualJoystickPosition;
        //������ ���� ���̽�ƽ�� ��踦 ���� �ʾƾ� �ϱ� ������ inputDir�� ũ�Ⱑ ���� ���̽�ƽ�� ������ ���� ũ�ٸ�
        //inputDir�� ���⺤�Ϳ� �������� ���� ��ġ ����
        Vector2 limitDir = ((inputDir.magnitude < virtualJoystickRadius) ? inputDir : inputDir.normalized * virtualJoystickRadius);
        leverRectTransform.anchoredPosition = limitDir;
        //limitDir�� ���� ���̽�ƽ�� ���������� ������ 0~1�� ����ȭ
        inputVec = limitDir / virtualJoystickRadius;
    }

    //PlyaerMovement ������Ʈ�� �������� �� ���� ����
    private void SendInputVector() {
        if (pm) {
            pm.Move(inputVec);
        }
    }
}
