using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerDownHandler
{
    public TileState state;

    public Vector2 tileCoordinate;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite xColor;
    [SerializeField] Sprite oColor;

    private void Start()
    {
        state = TileState.None;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        GameManager.instance.winnerPanel.SetActive(false);
        GameManager.instance.gameOverPanel.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state != TileState.None)
            return;

        var newState = (GameManager.instance.Turn % 2 == 0 ? TileState.X : TileState.O);
        SetState(newState);
        GameManager.instance.Turn++;

        var result = GameManager.instance.HasWinner();

        if (result.Item1)
        {
            GameManager.instance.winnerPanel.SetActive(true);
            GameManager.instance.winnerText.text="Winner " + result.Item2;

            foreach (var item in GameManager.instance.ListTileController)
            {
                item.enabled = false;
            }
        }

        if (GameManager.instance.GameOver())
            GameManager.instance.gameOverPanel.SetActive(true);



    }

    void SetState(TileState state)
    {
        this.state = state;

        spriteRenderer.sprite = (state == TileState.X) ? xColor : oColor;
    }

    public TileController GetNextTile(Direction direction)
    {
        var nextTileCoordinate = tileCoordinate;

        switch (direction)
        {
            case Direction.Up:
                nextTileCoordinate.y++;
                break;
            case Direction.UpRight:
                nextTileCoordinate.y++;
                nextTileCoordinate.x++;
                break;
            case Direction.Right:
                nextTileCoordinate.x++;
                break;
            case Direction.DownRight:
                nextTileCoordinate.y--;
                nextTileCoordinate.x++;
                break;
            case Direction.Down:
                nextTileCoordinate.y--;
                break;
            case Direction.DownLeft:
                nextTileCoordinate.y--;
                nextTileCoordinate.x--;
                break;
            case Direction.Left:
                nextTileCoordinate.x--;
                break;
            case Direction.UpLeft:
                nextTileCoordinate.y++;
                nextTileCoordinate.x--;
                break;
        }


        return GameManager.instance.ListTileController.Find(x => x.tileCoordinate == nextTileCoordinate);
    }
}

public enum TileState
{
    None,
    X,
    O
}

public enum Direction
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}