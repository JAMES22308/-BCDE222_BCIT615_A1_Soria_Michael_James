using BCIT615.Assessment1.GamePlayer;
using BCIT615.Assessment1.GamePlayer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BCIT615.Assessment1.GamePlayer.ModelTests;

[TestClass]
public class GamePlayerTests
{
    [TestMethod]
    public void NewGame_StartsAtCorrectPosition()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Assert
        Assert.AreEqual(new Position(5, 0), player.StartPosition);
        Assert.AreEqual(new Position(5, 0), player.CurrentPosition);
        Assert.AreEqual(new Position(0, 5), player.TargetPosition);
        Assert.IsFalse(player.IsComplete);
        Assert.AreEqual(0, player.MoveHistory.Count);
    }

    [TestMethod]
    public void GetPieceAt_ReturnsCorrectPiece()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        PieceType? piece = player.GetPieceAt(new Position(5, 0));

        // Assert
        Assert.AreEqual(PieceType.Rook, piece);
    }

    [TestMethod]
    public void TryMove_RookLegalMove_ReturnsSuccess()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult result = player.TryMove(new Position(5, 3));

        // Assert
        Assert.AreEqual(MoveResult.Success, result);
    }

    [TestMethod]
    public void TryMove_OutOfBounds_ReturnsOutOfBounds()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult result = player.TryMove(new Position(6, 0));

        // Assert
        Assert.AreEqual(MoveResult.OutOfBounds, result);
    }


    [TestMethod]
    public void TryMove_RookDiagonalMove_ReturnsInvalidMovement()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult result = player.TryMove(new Position(4, 1));

        // Assert
        Assert.AreEqual(MoveResult.InvalidMovement, result);
    }


    [TestMethod]
    public void TryMove_SamePosition_ReturnsInvalidMovement()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult result = player.TryMove(new Position(5, 0));

        // Assert
        Assert.AreEqual(MoveResult.InvalidMovement, result);
    }

    [TestMethod]
    public void TryMove_RookBlockedPath_ReturnsPathBlocked()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = PieceType.Rook,
            [new Position(5, 1)] = PieceType.Bishop
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(5, 0),
            new Position(0, 5));

        // Act
        MoveResult result = player.TryMove(new Position(5, 3));

        // Assert
        Assert.AreEqual(MoveResult.PathBlocked, result);
    }


    [TestMethod]
    public void TryMove_BishopLegalMove_ReturnsSuccess()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(3, 3)] = PieceType.Bishop,
            [new Position(1, 5)] = PieceType.Rook
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(3, 3),
            new Position(0, 5));

        // Act
        MoveResult result = player.TryMove(new Position(1, 5));

        // Assert
        Assert.AreEqual(MoveResult.Success, result);
    }


    [TestMethod]
    public void TryMove_BishopBlockedPath_ReturnsPathBlocked()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = PieceType.Bishop,
            [new Position(4, 1)] = PieceType.Rook
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(5, 0),
            new Position(2, 3));

        // Act
        MoveResult result = player.TryMove(new Position(2, 3));

        // Assert
        Assert.AreEqual(MoveResult.PathBlocked, result);
    }






    [TestMethod]
    public void TryMove_KingLegalMove_ReturnsSuccess()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(3, 3)] = PieceType.King,
            [new Position(4, 4)] = PieceType.Rook
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(3, 3),
            new Position(0, 5));

        // Act
        MoveResult result = player.TryMove(new Position(4, 4));

        // Assert
        Assert.AreEqual(MoveResult.Success, result);
    }


    [TestMethod]
    public void TryMove_KingInvalidDistance_ReturnsInvalidMovement()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(3, 3)] = PieceType.King
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(3, 3),
            new Position(0, 5));

        // Act
        MoveResult result = player.TryMove(new Position(3, 5));

        // Assert
        Assert.AreEqual(MoveResult.InvalidMovement, result);
    }





    [TestMethod]
    public void TryMove_SuccessfulMove_UpdatesCurrentPositionAndHistory()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult result = player.TryMove(new Position(5, 3));

        // Assert
        Assert.AreEqual(MoveResult.Success, result);
        Assert.AreEqual(new Position(5, 3), player.CurrentPosition);

        Assert.AreEqual(1, player.MoveHistory.Count);

        MoveRecord move = player.MoveHistory[0];

        Assert.AreEqual(1, move.SequenceNumber);
        Assert.AreEqual(new Position(5, 0), move.From);
        Assert.AreEqual(new Position(5, 3), move.To);
        Assert.AreEqual(PieceType.Rook, move.MovementPiece);
    }


    [TestMethod]
    public void TryMove_EmptyNonTarget_ReturnsInvalidDestination()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult result = player.TryMove(new Position(4, 0));

        // Assert
        Assert.AreEqual(MoveResult.InvalidDestination, result);

        // Make sure the rejected move did not change state
        Assert.AreEqual(new Position(5, 0), player.CurrentPosition);
        Assert.AreEqual(0, player.MoveHistory.Count);
    }



    [TestMethod]
    public void TryMove_ToTarget_ReturnsGameCompleted()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(1, 4)] = PieceType.King
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(1, 4),
            new Position(0, 5));

        // Act
        MoveResult result = player.TryMove(new Position(0, 5));

        // Assert
        Assert.AreEqual(MoveResult.GameCompleted, result);
        Assert.AreEqual(new Position(0, 5), player.CurrentPosition);
        Assert.IsTrue(player.IsComplete);
        Assert.AreEqual(1, player.MoveHistory.Count);
    }




    [TestMethod]
    public void TryMove_AfterCompletion_ReturnsGameAlreadyCompleted()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(1, 4)] = PieceType.King
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(1, 4),
            new Position(0, 5));

        // Complete the game first
        MoveResult firstResult = player.TryMove(new Position(0, 5));

        // Act
        MoveResult secondResult = player.TryMove(new Position(1, 4));

        // Assert
        Assert.AreEqual(MoveResult.GameCompleted, firstResult);
        Assert.AreEqual(MoveResult.GameAlreadyCompleted, secondResult);

        // State must not change
        Assert.AreEqual(new Position(0, 5), player.CurrentPosition);
        Assert.IsTrue(player.IsComplete);
        Assert.AreEqual(1, player.MoveHistory.Count);
    }


    [TestMethod]
    public void Restart_RestoresInitialGameState()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Make a successful move
        MoveResult result = player.TryMove(new Position(5, 3));

        // Act
        player.Restart();

        // Assert
        Assert.AreEqual(MoveResult.Success, result);
        Assert.AreEqual(new Position(5, 0), player.CurrentPosition);
        Assert.IsFalse(player.IsComplete);
        Assert.AreEqual(0, player.MoveHistory.Count);

        // Board should still exist
        Assert.AreEqual(PieceType.Rook, player.GetPieceAt(new Position(5, 0)));
        Assert.AreEqual(PieceType.Bishop, player.GetPieceAt(new Position(5, 3)));
    }

    [TestMethod]
    public void MoveHistory_RecordsSuccessfulMovesInOrder()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        // Act
        MoveResult firstResult = player.TryMove(new Position(5, 3));
        MoveResult secondResult = player.TryMove(new Position(3, 5));

        // Assert
        Assert.AreEqual(MoveResult.Success, firstResult);
        Assert.AreEqual(MoveResult.Success, secondResult);

        Assert.AreEqual(2, player.MoveHistory.Count);

        MoveRecord firstMove = player.MoveHistory[0];
        MoveRecord secondMove = player.MoveHistory[1];

        Assert.AreEqual(1, firstMove.SequenceNumber);
        Assert.AreEqual(new Position(5, 0), firstMove.From);
        Assert.AreEqual(new Position(5, 3), firstMove.To);
        Assert.AreEqual(PieceType.Rook, firstMove.MovementPiece);

        Assert.AreEqual(2, secondMove.SequenceNumber);
        Assert.AreEqual(new Position(5, 3), secondMove.From);
        Assert.AreEqual(new Position(3, 5), secondMove.To);
        Assert.AreEqual(PieceType.Bishop, secondMove.MovementPiece);
    }


    [TestMethod]
    public void MoveHistory_CannotBeModifiedThroughReturnedCollection()
    {
        // Arrange
        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new();

        player.TryMove(new Position(5, 3));

        // Act
        IReadOnlyList<MoveRecord> history = player.MoveHistory;

        // Assert
        Assert.AreEqual(1, history.Count);

        Assert.ThrowsException<NotSupportedException>(() =>
            ((IList<MoveRecord>)history).Clear());

        Assert.AreEqual(1, player.MoveHistory.Count);
    }


    [TestMethod]
    public void Board_CannotBeModifiedAfterGameCreation()
    {
        // Arrange
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = PieceType.Rook
        };

        BCIT615.Assessment1.GamePlayer.Model.GamePlayer player = new(
            6,
            6,
            pieces,
            new Position(5, 0),
            new Position(0, 5));

        // Act
        pieces[new Position(5, 0)] = PieceType.King;
        pieces[new Position(3, 3)] = PieceType.Bishop;

        // Assert
        Assert.AreEqual(
            PieceType.Rook,
            player.GetPieceAt(new Position(5, 0)));

        Assert.IsNull(
            player.GetPieceAt(new Position(3, 3)));
    }

    [TestMethod]
    public void Constructor_NullPieces_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                null!,
                new Position(5, 0),
                new Position(0, 5)));
    }







    [TestMethod]
    public void Constructor_ZeroRows_ThrowsArgumentOutOfRangeException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(0, 0)] = PieceType.Rook
        };

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                0,
                6,
                pieces,
                new Position(0, 0),
                new Position(0, 5)));
    }

    [TestMethod]
    public void Constructor_ZeroColumns_ThrowsArgumentOutOfRangeException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(0, 0)] = PieceType.Rook
        };

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                0,
                pieces,
                new Position(0, 0),
                new Position(5, 0)));
    }




    [TestMethod]
    public void Constructor_StartOutsideBoard_ThrowsArgumentOutOfRangeException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(0, 0)] = PieceType.Rook
        };

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                pieces,
                new Position(6, 0),
                new Position(0, 5)));
    }

    [TestMethod]
    public void Constructor_TargetOutsideBoard_ThrowsArgumentOutOfRangeException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = PieceType.Rook
        };

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                pieces,
                new Position(5, 0),
                new Position(6, 5)));
    }

    [TestMethod]
    public void Constructor_StartAndTargetSame_ThrowsArgumentException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = PieceType.Rook
        };

        Assert.ThrowsException<ArgumentException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                pieces,
                new Position(5, 0),
                new Position(5, 0)));
    }

    [TestMethod]
    public void Constructor_StartPositionWithoutPiece_ThrowsArgumentException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(2, 2)] = PieceType.Rook
        };

        Assert.ThrowsException<ArgumentException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                pieces,
                new Position(5, 0),
                new Position(0, 5)));
    }




    [TestMethod]
    public void Constructor_PieceOutsideBoard_ThrowsArgumentOutOfRangeException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = PieceType.Rook,
            [new Position(6, 0)] = PieceType.Bishop
        };

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                pieces,
                new Position(5, 0),
                new Position(0, 5)));
    }




    [TestMethod]
    public void Constructor_InvalidPieceType_ThrowsArgumentException()
    {
        Dictionary<Position, PieceType> pieces = new()
        {
            [new Position(5, 0)] = (PieceType)99
        };

        Assert.ThrowsException<ArgumentException>(() =>
            new BCIT615.Assessment1.GamePlayer.Model.GamePlayer(
                6,
                6,
                pieces,
                new Position(5, 0),
                new Position(0, 5)));
    }


}