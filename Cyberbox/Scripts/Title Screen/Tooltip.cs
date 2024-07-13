using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Card;
    public Text Title;
    public Text Description;
    public Text Year;
    public Image Thumbnail;

    public string stringTitle;
    public string stringDescription;
    public string stringYear;
    public Sprite assetthumbnail;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Card.transform.position = Input.mousePosition;  
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Card.SetActive(true);
        Title.GetComponent<Text>().text = stringTitle.ToString();
        Description.GetComponent<Text>().text = stringDescription.ToString();
        Year.GetComponent<Text>().text = stringYear.ToString();
        Thumbnail.GetComponent<Image>().sprite = assetthumbnail;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Card.SetActive(false);
    }
}
