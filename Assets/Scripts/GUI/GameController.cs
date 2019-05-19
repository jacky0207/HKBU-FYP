using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private Player player;

    // Move
    private Image moveContainer;
    private Image moveJoystick;

    public float clickPeriod = 0.2f;
    private bool holding;
    private float lastClick;

    // Time stop
    // private bool timeRunning;

    // Reference: http://www.theappguruz.com/blog/beginners-guide-learn-to-make-simple-virtual-joystick-in-unity
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        moveContainer = GetComponent<Image>();
        moveJoystick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
    }

    // Move
    public void OnDrag(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (moveContainer.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out position);

        position.x = (position.x / moveContainer.rectTransform.sizeDelta.x);    // drag position percentage correlated to the drag container

        float x = (moveContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;

		// Correct x to int
		x = (x > 0) ? 1 : -1;

		// Joystick UI Move
        Vector2 InputDirection = new Vector2(x, 0); // (1, 0) or (-1, 0)
        InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;   // May be out of bound

        //to define the area in which moveJoystick can move around
        moveJoystick.rectTransform.anchoredPosition = new Vector2(InputDirection.x * (moveContainer.rectTransform.sizeDelta.x / 3), 0);

		// Update direction
		player.direction = (x == -1) ? Direction.left : Direction.right;
    }

    // Trigger joystick
    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    // Reset joystick
    public void OnPointerUp(PointerEventData ped)
    {
        // InputDirection = Vector3.zero;
		player.direction = Direction.none;
        moveJoystick.rectTransform.anchoredPosition = Vector3.zero;
    }
    // end Move

    public void JumpButton()
    {
		player.Jump();
    }

    public void TimeButton()
    {
        holding = !holding;

        // Throw if click only
        if (!holding)
        {
            if (lastClick + clickPeriod > Time.time)
            {
                return;
            }
        }

        lastClick = Time.time;
        player.TimeStartOrStop();
    }

}
