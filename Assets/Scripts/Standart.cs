public static class Standart {


    public static Type[,] standart;


    public static Type[,] GetStandart(bool playerColor, int x, int y) {
        Type[,] state = new Type[x, y];

        state[0, 0] = Type.Rook;
        state[1, 0] = Type.Knight;
        state[2, 0] = Type.Bishop;
        state[3, 0] = playerColor ? Type.Queen : Type.King;
        state[4, 0] = !playerColor ? Type.Queen : Type.King;
        state[5, 0] = Type.Bishop;
        state[6, 0] = Type.Knight;
        state[7, 0] = Type.Rook;

        state[0, 1] = Type.Pawn;
        state[1, 1] = Type.Pawn;
        state[2, 1] = Type.Pawn;
        state[3, 1] = Type.Pawn;
        state[4, 1] = Type.Pawn;
        state[5, 1] = Type.Pawn;
        state[6, 1] = Type.Pawn;
        state[7, 1] = Type.Pawn;

        state[0, 2] = Type.Clear;
        state[1, 2] = Type.Clear;
        state[2, 2] = Type.Clear;
        state[3, 2] = Type.Clear;
        state[4, 2] = Type.Clear;
        state[5, 2] = Type.Clear;
        state[6, 2] = Type.Clear;
        state[7, 2] = Type.Clear;

        state[0, 3] = Type.Clear;
        state[1, 3] = Type.Clear;
        state[2, 3] = Type.Clear;
        state[3, 3] = Type.Clear;
        state[4, 3] = Type.Clear;
        state[5, 3] = Type.Clear;
        state[6, 3] = Type.Clear;
        state[7, 3] = Type.Clear;

        state[0, 4] = Type.Clear;
        state[1, 4] = Type.Clear;
        state[2, 4] = Type.Clear;
        state[3, 4] = Type.Clear;
        state[4, 4] = Type.Clear;
        state[5, 4] = Type.Clear;
        state[6, 4] = Type.Clear;
        state[7, 4] = Type.Clear;

        state[0, 5] = Type.Clear;
        state[1, 5] = Type.Clear;
        state[2, 5] = Type.Clear;
        state[3, 5] = Type.Clear;
        state[4, 5] = Type.Clear;
        state[5, 5] = Type.Clear;
        state[6, 5] = Type.Clear;
        state[7, 5] = Type.Clear;

        state[0, 6] = Type.Pawn;
        state[1, 6] = Type.Pawn;
        state[2, 6] = Type.Pawn;
        state[3, 6] = Type.Pawn;
        state[4, 6] = Type.Pawn;
        state[5, 6] = Type.Pawn;
        state[6, 6] = Type.Pawn;
        state[7, 6] = Type.Pawn;

        state[0, 7] = Type.Rook;
        state[1, 7] = Type.Knight;
        state[2, 7] = Type.Bishop;
        state[3, 7] = playerColor ? Type.Queen : Type.King;
        state[4, 7] = !playerColor ? Type.Queen : Type.King;
        state[5, 7] = Type.Bishop;
        state[6, 7] = Type.Knight;
        state[7, 7] = Type.Rook;

        return state;
    }

}
