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
            [new Position(3, 3)] = PieceType.Bishop
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
}