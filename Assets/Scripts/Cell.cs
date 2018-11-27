using UnityEngine;

public enum Type { King, Rook, Bishop, Queen, Knight, Pawn, Clear }
public class Cell : MonoBehaviour {




    public ChessMan ch;

    public GridDraw gridDraw;
    Color col;
    Renderer rend;

    public int x = 0;
    public int y = 0;

    public bool IsClear() {
        if (ch != null && ch.type1 != Type.Clear) return false;
        else return true;
    }


    public Type type
    {
        get {
            if (ch == null) return Type.Clear;
            return ch.type1;
        }
    }


    Color defaultCol;
    private void OnEnable() {
        ch = new ChessMan();
        rend = transform.GetChild(0).GetComponent<Renderer>();
        col = rend.material.color;
        rend.material.color = Color.clear;
        defaultCol = rend.material.GetColor("_Emission");
    }

    private void OnDisable() {
        rend.material.color = col;
    }

    public void OnMouseEnter() {
        if (gridDraw.GamePause) return;
        if (!select)
            pickOut();
    }
    public void OnMouseExit() {
        if (gridDraw.GamePause) return;
        if (!select)
            rend.material.color = Color.clear;
    }




    void MutationPawn() {
        if (type == Type.Pawn && (ch.player && y == 7 || !ch.player && y == 0)) {
            gridDraw.MutationPawn(ch);
        }
    }

    void InterceptionPawn() {
        if (gridDraw.lastHit.type == Type.Pawn && gridDraw.enPassant != null && gridDraw.enPassant.x == x && gridDraw.enPassant.y == y) {
            gridDraw.enPassantChessMan.type1 = Type.Clear;
            gridDraw.enPassantChessMan.ReDraw();
        }
        gridDraw.enPassant = null;
        gridDraw.enPassantChessMan = null;

        //Фиксировать проход пешки
        if (gridDraw.lastHit.type == Type.Pawn && Mathf.Abs(gridDraw.lastHit.y - y) == 2) {

            gridDraw.enPassant = gridDraw.GetByIndex(gridDraw.lastHit.x, gridDraw.lastHit.y + (gridDraw.lastHit.ch.player ? 1 : -1));
            gridDraw.enPassantChessMan = gridDraw.lastHit.ch;
        }
    }


    private void OnMouseUp() {
        
        if (gridDraw.GamePause) return;
        
        // lastHit - ячейка с которой происходит перемещение в эту ячейку

        if (gridDraw.lastHit != null && select) {
            if (gridDraw.lastHit.ch.player != gridDraw.activePlayer) return;


            InterceptionPawn();


            ChessMan.MoveFromTo(gridDraw.lastHit, this);
            MutationPawn();

            gridDraw.lastHit = null;
            gridDraw.DeselectAll();
            ch.pawnOne = false;
            gridDraw.activePlayer = !ch.player;


            gridDraw.FindPaths();
            gridDraw.DeselectAll();
        }
        else {


            gridDraw.DeselectAll();
            ParseSelect();
        }
        

    }
    bool select;

    public void SelectCell(bool red = false) {

        if (!select) {
            select = true;

            pickOut(red);
        }
    }


    void pickOut(bool red = false) {

        rend.material.color = col;

        rend.material.SetColor("_Emission", red ? Color.red : defaultCol);

    }


    public void DeSelectCell() {
        if (select) {
            select = false;
            rend.material.color = Color.clear;
        }
    }

    public Cell[] array;
    public void FindPath() {

        if (type == Type.Pawn) {
            array = gridDraw.Pawn(this, ch.player, ch.pawnOne);
            if (gridDraw.enPassant !=null
                && gridDraw.enPassant.ch.player != ch.player 
                && y == gridDraw.enPassant.y + 
                (!gridDraw.lastHit.ch.player ? 1 : -1)) {
                array[4] = gridDraw.enPassant;
            }
        }

        else if (type == Type.Bishop) {
            array = (gridDraw.Bishop(this));
        }
        else if (type == Type.Queen) {
            array = (gridDraw.Queen(this));
        }
        else if (type == Type.Rook) {
            array = (gridDraw.Rook(this));
        }
        else if (type == Type.King) {
            array = (gridDraw.King(this));
        }
        else if (type == Type.Knight) {
            array = (gridDraw.Knight(this));
        }
        

    }

    private void ParseSelect() {
        if (ch.player != gridDraw.activePlayer) return;
        if (array == null) return;
        if (!IsClear()) gridDraw.lastHit = this;
        foreach (var i in array) {

            if (i != null) {




                i.SelectCell();
            }
        }
    }
}

public class ChessMan {
    /// <summary>Чья фигура. true - игрок, false - противник</summary>
    public bool player;
    public Transform transform;
    public GridDraw gridDraw;
    public Cell cell;

    public Type type1;
    public bool pawnOne = true;
    public int x { get { return cell.x; } }
    public int y { get { return cell.y; } }




    public void ReDraw() {
        if (transform != null) {
            Object.Destroy(transform.gameObject);
            transform = null;
        }
        if (type1 == Type.Clear) return;


        if (transform == null && gridDraw) {
            Transform chessman;
            if (player) {
                chessman = gridDraw.playerColor ? gridDraw.whiteChessmans[(int)type1] : gridDraw.blackChessmans[(int)type1];
            }
            else {
                chessman = !gridDraw.playerColor ? gridDraw.whiteChessmans[(int)type1] : gridDraw.blackChessmans[(int)type1];
            }

            transform = Object.Instantiate(chessman, new Vector3(x + gridDraw.start.x + 0.5f, gridDraw.start.y, gridDraw.start.z + y + 0.5f), new Quaternion());
        }
    }


    public static void MoveFromTo(Cell from, Cell to) {
        if (!to.IsClear()) {

            if (to.ch.type1 == Type.King) {
                to.gridDraw.Win(!to.ch.player);
            }

            to.ch.type1 = Type.Clear;
            to.ch.ReDraw();
        }
        to.ch = from.ch;
        from.ch = null;
        to.ch.cell = to;
        to.ch.ReDraw();
    }
}

