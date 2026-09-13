# BCIT615 Assessment 1 - Learner Starter v0.5
## Author

Michael James Soria

BCDE222 / BCIT615
Ara Institute of Canterbury

This project implements the Game Player model for BCDE222 / BCIT615 Assessment 1. The player moves through a supplied 6 × 6 board using movement rules based on the piece located at the player's current position. The implementation includes Rook, Bishop, Knight and King movement, move validation, move history, game completion and restart functionality.

## Board Definition

The supplied reference board is 6 × 6 using zero-based row and column positions.

| Position | Piece |
|----------|-------|
| `(5,0)`  | Rook |
| `(5,3)`  | Bishop |
| `(3,5)`  | Knight |
| `(1,4)`  | King |
| `(0,5)`  | Target |

The starting position is `(5,0)` and the target position is `(0,5)`.

The reference route is:

`(5,0) → (5,3) → (3,5) → (1,4) → (0,5)`



## Movement Rules

- **Rook** – moves horizontally or vertically and cannot jump over occupied positions.
- **Bishop** – moves diagonally and cannot jump over occupied positions.
- **Knight** – moves in an L-shape (2 positions in one direction and 1 in the other) and can jump over occupied positions.
- **King** – moves exactly one square in any direction.



## Game Rules

- The game starts at the supplied start position.
- The player can only make a valid movement according to the piece at the current position.
- The destination must be within the board.
- Rook and Bishop paths cannot contain another piece.
- A destination must contain a piece or be the target position.
- A successful move updates the current position and move history.
- Reaching the target completes the game.
- Moves cannot be made after the game is completed.
- Restart returns the player to the start position and clears the move history.
- Invalid moves do not change the game state.



## Implementation

### GamePlayer

`GamePlayer` implements `IGamePlayer` and is responsible for:

- tracking the current position
- validating moves
- applying movement rules
- tracking completion state
- recording successful moves
- restarting the game

### ReferenceBoardData

`ReferenceBoardData` contains the supplied 6 × 6 reference board, including the start position, target position and fixed piece locations.

### State Protection

The implementation prevents callers from directly modifying the game state.

`MoveHistory` is exposed as a read-only collection, and supplied board data is defensively copied when passed into the parameterised constructor.




## Move Results

The `TryMove` method can return:

- `GameAlreadyCompleted`
- `OutOfBounds`
- `InvalidMovement`
- `PathBlocked`
- `InvalidDestination`
- `GameCompleted`
- `Success`





## Constructor Validation

The parameterised constructor validates:

- null board data
- invalid board dimensions
- start and target positions outside the board
- start and target being the same position
- start position containing no piece
- pieces outside the board
- invalid piece types



## Testing

Automated tests are implemented using MSTest.

Some of the tests cover:

- initial game state
- Rook movement
- Bishop movement
- Knight movement and jumping
- King movement
- invalid movements
- blocked paths
- out-of-bounds destinations
- invalid destinations
- game completion
- moves after completion
- restart
- move history
- read-only/state protection
- constructor validation



## Build and Verification

The project targets .NET 9 and uses SDK version 9.0.316.

Restore dependencies:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build




## Scope and Limitations

This assessment implements the required Game Player model and console demonstration.

The project does not include:

- WPF or graphical user interface
- database persistence
- saved game files
- normal chess rules such as capturing or check/checkmate

The pieces act as fixed movement-rule markers on the supplied board.