using UnityEngine;
using UnityEngine.UI;
public class GridDraw : MonoBehaviour {

    [SerializeField] public Vector3 start;
    [SerializeField] Vector2Int size;
    [SerializeField] Transform cell;


    Cell[,] _cells2;

    Type[,] state;
    [Space]
    [Space]
    public Transform[] blackChessmans;
    public Transform[] whiteChessmans;

    public bool playerColor;

    private bool gamePause;
    public Cell lastHit;
    public Cell enPassant;
    public ChessMan enPassantChessMan;
    public bool activePlayer = true;

    [SerializeField] Transform panel;
    [SerializeField] Transform colorPanel;
    [SerializeField] Transform winPanel;

    int i = 0;
    Cell[] cl;

    int[,] king = new int[,] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

    public bool GamePause
    {
        get {
            return gamePause;
        }

        set {
            //DeselectAll();
            gamePause = value;
        }
    }

    ChessMan mutPawn;
    public void MutationPawn(ChessMan ch) {
        GamePause = true;
        mutPawn = ch;
        panel.gameObject.SetActive(true);
    }
    public void SetPawnType(int type) {
        mutPawn.type1 = (Type)type;
        mutPawn.ReDraw();
        GamePause = false;
        panel.gameObject.SetActive(false);
    }

    public void FindPaths() {
        foreach (Cell c in _cells2) {
            c.FindPath();
        }
    }


    public void Win(bool winner) {
        winPanel.gameObject.SetActive(true);
        winPanel.Find("Text").GetComponent<Text>().text = winner ? "White win" : "Black win";
    }



    void PlayerColor() {
        panel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        //GamePause = true;
        colorPanel.gameObject.SetActive(true);
    }

    public void SetPlayerColor(bool color) {
        playerColor = color;
        colorPanel.gameObject.SetActive(false);
        // GamePause = false;

        activePlayer = playerColor;


        _cells2 = new Cell[size.x, size.y];

        state = Standart.GetStandart(playerColor, size.x, size.y);

        int x = 0, y = 0;

        while (x < size.x) {
            while (y < size.y) {
                Cell c = Instantiate(cell, new Vector3(x, 0, y) + start, new Quaternion()).gameObject.AddComponent<Cell>();
                c.ch.gridDraw = this;
                c.ch.player = y < 4;
                c.ch.cell = c;
                c.x = x;
                c.y = y;
                c.gridDraw = this;

                c.ch.type1 = state[x, y];
                c.ch.ReDraw();
                _cells2[x, y] = c;
                y++;
            }
            x++; y = 0;
        }
        FindPaths();

    }



    private void OnEnable() {
        PlayerColor();
    }

    public void Reload() {
        if (_cells2 == null) return;
        OnDisable();
        OnEnable();
    }


    private void Update() {
        if (Input.GetKeyUp(KeyCode.R)) {
            Reload();

        }
    }

    private void OnDisable() {
        if (panel) panel.gameObject.SetActive(false);
        if (colorPanel) colorPanel.gameObject.SetActive(false);
        GamePause = false;
        if (_cells2 != null)
            foreach (Cell c in _cells2) {
                if (c.ch != null && c.ch.transform != null) Destroy(c.ch.transform.gameObject);
                if (c != null) Destroy(c.gameObject);


            }
    }
    public void DeselectAll() {
        foreach (var c in _cells2) {
            c.DeSelectCell();
        }
    }

    public Cell GetByIndex(int x, int y) {
        return Isset(x, y) ? _cells2[x, y] : null;
    }



    int[,] knight = new int[,] { { 1, 2 }, { 1, -2 }, { 2, 1 }, { 2, -1 }, { -1, 2 }, { -1, -2 }, { -2, 1 }, { -2, -1 } };
    public Cell[] Knight(Cell cell) {
        cl = new Cell[8];
        for (int i = 0; i < 8; i++) {
            int x = cell.x + knight[i, 0],
                y = cell.y + knight[i, 1];
            if (IssetB(x, y))
                cl[i] = _cells2[x, y];
        }
        return cl;
    }




    public Cell[] Pawn(Cell cell, bool vector, bool one) {
        cl = new Cell[5];
        int x, y;

        x = cell.x;
        y = cell.y + 1 * (vector ? 1 : -1);
        if (IssetB(x, y) && _cells2[x, y].IsClear()) cl[0] = _cells2[x, y];

        if (one && cl[0] != null) {
            x = cell.x;
            y = cell.y + 2 * (vector ? 1 : -1);
            if (IssetB(x, y) && _cells2[x, y].IsClear()) cl[1] = _cells2[x, y];
        }

        x = cell.x + 1;
        y = cell.y + 1 * (vector ? 1 : -1);
        if (Isset(x, y) && !_cells2[x, y].IsClear() && _cells2[x, y].ch.player != lastHit.ch.player)
            cl[2] = _cells2[x, y];

        x = cell.x - 1;
        y = cell.y + 1 * (vector ? 1 : -1);
        if (Isset(x, y) && !_cells2[x, y].IsClear() && _cells2[x, y].ch.player != lastHit.ch.player)
            cl[3] = _cells2[x, y];

        return cl;
    }

    private bool IssetB(int x, int y) {
        return Isset(x, y) && (_cells2[x, y].IsClear() || (lastHit && _cells2[x, y].ch.player != lastHit.ch.player));
    }



    public Cell[] King(Cell cell) {
        Cell[] cl = new Cell[8];
        for (int i = 0; i < 8; i++) {
            int x = cell.x + king[i, 0],
                y = cell.y + king[i, 1];
            if (IssetB(x, y)) cl[i] = _cells2[x, y];

        }
        return cl;
    }

    public Cell[] Bishop(Cell cell) {
        cl = new Cell[13]; i = 0;
        _Rook(1, 1, cell);
        _Rook(1, -1, cell);
        _Rook(-1, 1, cell);
        _Rook(-1, -1, cell);
        return cl;
    }

    public Cell[] Rook(Cell cell) {
        cl = new Cell[14]; i = 0;
        _Rook(-1, 0, cell);
        _Rook(0, -1, cell);
        _Rook(1, 0, cell);
        _Rook(0, 1, cell);

        return cl;
    }

    public Cell[] Queen(Cell cell) {
        cl = new Cell[27]; i = 0;
        _Rook(-1, 0, cell);
        _Rook(0, -1, cell);
        _Rook(1, 0, cell);
        _Rook(0, 1, cell);

        _Rook(-1, 1, cell);
        _Rook(1, -1, cell);
        _Rook(1, 1, cell);
        _Rook(-1, -1, cell);

        return cl;
    }


    bool Isset(int x, int y) {
        if (size.x <= x || size.y <= y || x < 0 || y < 0) return false;
        return true;
    }

    private Cell[] _Rook(int xMod, int yMod, Cell cell) {
        int x = cell.x, y = cell.y;
        while (true) {
            x += xMod; y += yMod;
            if (Isset(x, y)) {
                if (_cells2[x, y].IsClear()) {
                    cl[i] = _cells2[x, y]; i++;
                }
                else if (lastHit != null && _cells2[x, y].ch.player != lastHit.ch.player) {
                    cl[i] = _cells2[x, y]; i++;
                    break;
                }
                else {
                    break;
                }
            }
            else break;
        }
        return cl;
    }
}



