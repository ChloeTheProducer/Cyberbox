using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    Player player;

    AudioClip hover;
    AudioClip pressed;

    Texture2D defaultCur;
    Texture2D selectCur;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        hover = Resources.Load<AudioClip>("UI/Command");
        pressed = Resources.Load<AudioClip>("UI/Click");

        defaultCur = Resources.Load<Texture2D>("cursor");
        selectCur = Resources.Load<Texture2D>("cursorSelect");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        player.audioSource.PlayOneShot(hover);
        Cursor.SetCursor(defaultCur, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(selectCur, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        player.audioSource.PlayOneShot(pressed);
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }

}
