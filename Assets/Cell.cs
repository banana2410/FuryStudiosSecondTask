using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ContentOfCell
{
    Nothing,
    Obstacle,
    Start,
    Finish
}
public enum TypeOfValue
{
    H,
    G,
    F
}

public class Cell : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer => gameObject.GetComponent<SpriteRenderer>();
    public ContentOfCell ContentOfCell;
    private int _hNumber;
    private int _gNumber;
    private int _fNumber;//H + G

    [SerializeField] private TextMeshProUGUI _hNumberText, _gNumberText, _fNumberText;

    private void Awake()
    {
        
    }
    public void SetValue(TypeOfValue typeOfValue, int value)
    {
        switch (typeOfValue)
        {
            case TypeOfValue.H:
                _hNumber = value;
                _hNumberText.text = value.ToString();
                break;
            case TypeOfValue.G:
                _gNumber = value;
                _gNumberText.text = value.ToString();
                break;
            case TypeOfValue.F:
                _fNumber = value;
                _fNumberText.text = value.ToString();
                break;
            default:
                Debug.LogError("Errrrror");
                break;
        }
    }

    public void SetContentOfCell(ContentOfCell contentOfCell)
    {
        switch (contentOfCell)
        {
            case ContentOfCell.Nothing:
                
                break;
            case ContentOfCell.Obstacle:
                _gNumberText.gameObject.SetActive(false);
                _hNumberText.gameObject.SetActive(false);
                _fNumberText.gameObject.SetActive(false);
                _spriteRenderer.color = Color.black;
                break;
            case ContentOfCell.Start:
                _spriteRenderer.color = Color.blue;
                break;
            case ContentOfCell.Finish:
                _spriteRenderer.color = Color.green;
                break;
            default:
                break;
        }
    }



}
