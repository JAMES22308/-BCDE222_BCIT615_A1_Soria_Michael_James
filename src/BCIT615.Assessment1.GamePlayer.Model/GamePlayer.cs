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

    public IReadOnlyList<MoveRecord> MoveHistory => _moveHistory;

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
        _rows = rows;
        _columns = columns;
        _pieces = pieces;

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

        return MoveResult.Success;
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }
}