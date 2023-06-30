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
**Box**: A two dimensional grid of cells within the board. (3x3 of boxes, each 3x3 of cells by default)
**Naked Pair**: Cells from the same box in a line (row or column) that are the only cells that can have the value of their notes
