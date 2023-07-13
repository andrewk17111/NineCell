# NineCell
An automated sudoku solver and helper

## Plans
- User supplied starting cells
- Validate user cell values
- Provide solution one step at a time
- Automatically provide solved board
- Custom board dimensions
- Custom cell restrictions

## Terminology
For the purposes of this project, specific terminology is used in documentation as well as comments.
**Board**: The two dimentional playing area of the game that contains the cells. (9x9 by default)
**Cell**: A single valued point on the board.
**Note**: A list of possible values for a cell.
**Box**: A two dimensional grid of cells within the board. (3x3 of boxes, each 3x3 of cells by default)

## Solving Techniques
- [X] **Elimination**: The notes of a cell cannot contain the value of any of the cells in the same row, column, or box.
- [X] **Naked Singles**: The value of a cell with only one note is the value of that note.
- [X] **Hidden Singles**: The value of a cell with a note that is not present in any other cell in the same row, column, or box is the value of that note.
- [X] **Naked Pairs/Triples/Quads**: If two/three/four cells in the same row, column, or box have the same two/three/four notes, then those notes can be removed from all other cells in the same row, column, or box.
- [X] **Hidden Pairs/Triples/Quads**: If two/three/four cells are the only possible cells for two/three/four notes in the same row, column, or box, then all other notes can be removed from those cells.
- [ ] **X-Wing**: If two rows or columns have only two cells with the same two notes, then those notes can be removed from all other cells in the same two columns or rows.
- [ ] **XY-Wing**: If there are three cells, each with only two notes, and one cell shares a row, column or box with the other two (wings), and shares different notes with the wings, and the unshared note is the same between the wings, then the unshared note can be removed from a fourth cell in common with the wings.
- [ ] **Swordfish**: If three rows or columns have only two or three cells with the same two or three notes, then those notes can be removed from all other cells in the same three columns or rows.
