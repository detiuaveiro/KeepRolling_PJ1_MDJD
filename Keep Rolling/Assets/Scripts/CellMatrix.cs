using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds the matrix of cells in the scene
/// </summary>
public class CellMatrix
{
    public Cell[,] cells;
    private int min_x;
    private int min_y;
    private int max_x;
    private int max_y;

    public CellMatrix(int min_x, int min_y, int max_x, int max_y) {
        this.min_x = min_x;
        this.min_y = min_y;
        this.max_x = max_x;
        this.max_y = max_y;
        cells = new Cell[max_x-min_x + 1,max_y-min_y + 1];
    }

    /// <summary>
    /// Adds the cell to the matrix, if there is another cell there it substitutes it. 
    /// </summary>
    /// <param name="cell">cell to be added</param>
    public void AddCell(Cell cell) {
        cells[cell.getVisualX() - min_x, cell.getVisualY() - min_y] = cell;
    }

    /// <summary>
    /// Gets the cell in the given position
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>The cell in that position, can return null</returns>
    public Cell GetCell(int x, int y) {
        return cells[x - min_x, y - min_y];
    }

    /// <summary>
    /// Checks if there is a cell in the given position
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>boolean true if different than null</returns>
    public bool HasCell(int x, int y) {
        return !(cells[x - min_x, y - min_y] is null);
    }

    /// <summary>
    /// Gets all the cells in the matrix
    /// </summary>
    /// <returns>A list with all the non null cells in the matrix</returns>
    public List<Cell> GetAllCells() {
        List<Cell> cell_list = new List<Cell>();
        foreach (var cell in cells) {
            if (!(cell is null)) {
                cell_list.Add(cell);
            }
        }
        return cell_list;
    }
}
