using BCIT615.Assessment1.GamePlayer;

namespace BCIT615.Assessment1.GamePlayer.Model;

public class GamePlayer : IGamePlayer
{
    private Position _currentPosition;
    private bool _isComplete;
    private readonly List<MoveRecord> _moveHistory = new();


    private readonly int _rows;
    private readonly int _columns;
    private readonly IReadOnlyDictionary<Position, PieceType> _pieces;


    public Position StartPosition { get; }

    public Position TargetPosition { get; }

    public Position CurrentPosition => _currentPosition;

    public bool IsComplete => _isComplete;

    //public IReadOnlyList<MoveRecord> MoveHistory => _moveHistory;
    public IReadOnlyList<MoveRecord> MoveHistory =>
    _moveHistory.AsReadOnly();

    public GamePlayer()
    {
        _rows = ReferenceBoardData.Rows;
        _columns = ReferenceBoardData.Columns;
        _pieces = ReferenceBoardData.Pieces;

        StartPosition = ReferenceBoardData.Start;
        TargetPosition = ReferenceBoardData.Target;

        _currentPosition = StartPosition;
        _isComplete = false;
    }

    public GamePlayer(
    int rows,
    int columns,
    IReadOnlyDictionary<Position, PieceType> pieces,
    Position startPosition,
    Position targetPosition)
    {

        if (pieces == null)
        {
            throw new ArgumentNullException(nameof(pieces));
        }

        if (rows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows));
        }

        if (columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(columns));
        }


        if (startPosition.Row < 0 ||
       startPosition.Row >= rows ||
       startPosition.Column < 0 ||
       startPosition.Column >= columns)
        {
            throw new ArgumentOutOfRangeException(nameof(startPosition));
        }

        // Validate target position
        if (targetPosition.Row < 0 ||
            targetPosition.Row >= rows ||
            targetPosition.Column < 0 ||
            targetPosition.Column >= columns)
        {
            throw new ArgumentOutOfRangeException(nameof(targetPosition));
        }

        // Start and target cannot be the same
        if (startPosition == targetPosition)
        {
            throw new ArgumentException(
                "Start position and target position cannot be the same.");
        }

        if (!pieces.ContainsKey(startPosition))
        {
            throw new ArgumentException(
                "The start position must contain a piece.");
        }


        foreach (Position position in pieces.Keys)
        {
            if (position.Row < 0 ||
                position.Row >= rows ||
                position.Column < 0 ||
                position.Column >= columns)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pieces),
                    "A piece is outside the board.");
            }

            PieceType piece = pieces[position];

            if (!Enum.IsDefined(typeof(PieceType), piece))
            {
                throw new ArgumentException(
                    "The board contains an invalid piece type.",
                    nameof(pieces));
            }
        }



        _rows = rows;
        _columns = columns;

        //_pieces = pieces;
        _pieces = new Dictionary<Position, PieceType>(pieces);

        StartPosition = startPosition;
        TargetPosition = targetPosition;

        _currentPosition = StartPosition;
        _isComplete = false;
    }

    public PieceType? GetPieceAt(Position position)
    {
        if (position.Row < 0 ||
            position.Row >= _rows ||
            position.Column < 0 ||
            position.Column >= _columns)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        if (_pieces.TryGetValue(position, out PieceType piece))
        {
            return piece;
        }

        return null;
    }

    public MoveResult TryMove(Position destination)
    {
        if (_isComplete)
        {
            return MoveResult.GameAlreadyCompleted;
        }

        if (destination.Row < 0 ||
            destination.Row >= _rows ||
            destination.Column < 0 ||
            destination.Column >= _columns)
        {
            return MoveResult.OutOfBounds;
        }

        if (destination == _currentPosition)
        {
            return MoveResult.InvalidMovement;
        }

        PieceType? piece = GetPieceAt(_currentPosition);

        // 4. Rook movement
        if (piece == PieceType.Rook)
        {
            bool sameRow = destination.Row == _currentPosition.Row;
            bool sameColumn = destination.Column == _currentPosition.Column;

            if (!sameRow && !sameColumn)
            {
                return MoveResult.InvalidMovement;
            }

            int rowStep = Math.Sign(destination.Row - _currentPosition.Row);
            int columnStep = Math.Sign(destination.Column - _currentPosition.Column);

            int row = _currentPosition.Row + rowStep;
            int column = _currentPosition.Column + columnStep;

            while (row != destination.Row || column != destination.Column)
            {
                if (GetPieceAt(new Position(row, column)) != null)
                {
                    return MoveResult.PathBlocked;
                }

                row += rowStep;
                column += columnStep;
            }
        }


        if (piece == PieceType.Bishop)
        {
            int rowDifference = Math.Abs(destination.Row - _currentPosition.Row);
            int columnDifference = Math.Abs(destination.Column - _currentPosition.Column);

            if (rowDifference != columnDifference)
            {
                return MoveResult.InvalidMovement;
            }

            int rowStep = Math.Sign(destination.Row - _currentPosition.Row);
            int columnStep = Math.Sign(destination.Column - _currentPosition.Column);

            int row = _currentPosition.Row + rowStep;
            int column = _currentPosition.Column + columnStep;

            while (row != destination.Row || column != destination.Column)
            {
                if (GetPieceAt(new Position(row, column)) != null)
                {
                    return MoveResult.PathBlocked;
                }

                row += rowStep;
                column += columnStep;
            }
        }



        // Knight movement
        if (piece == PieceType.Knight)
        {
            int rowDifference = Math.Abs(destination.Row - _currentPosition.Row);
            int columnDifference = Math.Abs(destination.Column - _currentPosition.Column);

            bool validKnightMove =
                (rowDifference == 2 && columnDifference == 1) ||
                (rowDifference == 1 && columnDifference == 2);

            if (!validKnightMove)
            {
                return MoveResult.InvalidMovement;
            }
        }


        // King movement
        if (piece == PieceType.King)
        {
            int rowDifference = Math.Abs(destination.Row - _currentPosition.Row);
            int columnDifference = Math.Abs(destination.Column - _currentPosition.Column);

            if (rowDifference > 1 || columnDifference > 1)
            {
                return MoveResult.InvalidMovement;
            }
        }

        if (destination != TargetPosition && GetPieceAt(destination) == null)
        {
            return MoveResult.InvalidDestination;
        }

        MoveRecord move = new(
            _moveHistory.Count + 1,
            _currentPosition,
            destination,
            piece!.Value);

        _moveHistory.Add(move);

        _currentPosition = destination;

        if (destination == TargetPosition)
        {
            _isComplete = true;
            return MoveResult.GameCompleted;
        }

        return MoveResult.Success;

    }

    public void Restart()
    {
        _currentPosition = StartPosition;
        _isComplete = false;
        _moveHistory.Clear();
    }
}