using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite poisonImage;
    public Sprite moveImage;
    public Sprite UselessImage;
    public Sprite lungeImage;
    public GameObject cardLeft;
    public GameObject cardMid;
    public GameObject cardRight;
    public Unit player;
    Sprite leftSprite;
    Sprite midSprite;
    Sprite rightSprite;
    Vector3 leftDown = new Vector3(45,-280,0);
    Vector3 midDown = new Vector3(0,-280,0);
    Vector3 rightDown = new Vector3(-20,-280,0);
    Vector3 rightUp = new Vector3(-20,-68,0);
    Vector3 midUp = new Vector3(0,-68,0);
    Vector3 leftUp = new Vector3(45,-68,0);
    Vector3 rightDes;
    Vector3 leftDes;
    Vector3 midDes;
    public void Start()
    {
        leftDes = leftDown;
        rightDes = rightDown;
        midDes = midDown;
    }
    public void Action(int ID)
    {
        if (ID == 1)
        {
            Move();
        }
        if (ID == 2)
        {
            Attack();
        }
        if (ID == 3)
        {
            Lunge();
        }
    }

    public void DisplayCard(int ID, int pos)
    {
        Sprite newImage = UselessImage;
        if (ID == 1)
        {
            newImage = moveImage;
        }
        else if (ID == 2)
        {
            newImage = poisonImage;
        }
        else if (ID == 3)
        {
            newImage = lungeImage;
        }
        if (pos == 0)
        {
            cardLeft.GetComponentInChildren<Image>().sprite = newImage;
        }
        else if (pos == 1)
        {
            cardMid.GetComponentInChildren<Image>().sprite = newImage;
        }
        else if (pos == 2)
        {
            cardRight.GetComponentInChildren<Image>().sprite = newImage;
        }

    }

    public void Attack()
    {
        player.Attack();
    }
    public void Lunge()
    {
        print("Lungetriggered");
        ClickableTile.active = true;
        ClickableTile.range = 2;
        ClickableTile.stab = true;
    }

    public void Move()
    {
        ClickableTile.active = true;
        ClickableTile.range = 1;
    }
    public void CardPopUpLeft()
    {
        leftDes = leftUp;
        cardLeft.GetComponent<RectTransform>().anchoredPosition3D=leftDes;
    }
    public void CardPopUpRight()
    {
        rightDes = rightUp;
        cardRight.GetComponent<RectTransform>().anchoredPosition3D=rightDes;
    }
    public void CardPopUpMid()
    {
        midDes = midUp;
        cardMid.GetComponent<RectTransform>().anchoredPosition3D=midDes;
    }
    public void CardDropLeft()
    {
        leftDes = leftDown;
        cardLeft.GetComponent<RectTransform>().anchoredPosition3D=leftDes;
    }
    public void CardDropMid()
    {
        midDes = midDown;
        cardMid.GetComponent<RectTransform>().anchoredPosition3D=midDes;
    }
    public void CardDropRight()
    {
        rightDes = rightDown;
        cardRight.GetComponent<RectTransform>().anchoredPosition3D=rightDes;
    }
}
