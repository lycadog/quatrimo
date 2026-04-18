
public class BoardRow
{
    int y;
    int width;

    Cell[] cells;

    int totalScorability;

    public BoardRow(int y, int width, Cell[] cells, int totalScorability)
    {
        this.y = y;
        this.width = width;
        this.cells = cells;
        this.totalScorability = totalScorability;

        foreach(var cell in cells)
        {
            cell.BecameScorable += OnCellBecameScorable;
            cell.BecameNonScorable += OnCellBecameNonscorable;
        }
    }

    public bool Scorable
    {
        get
        {
            return 
                totalScorability >= width - RunStats.EmptySpacesAllowedInScoring - RunStats.EmptySpacesRequiredInScoring 
                &&
                totalScorability <= width - RunStats.EmptySpacesRequiredInScoring;
        }
    }

    public void OnCellBecameScorable()
    {
        totalScorability++;
    }

    public void OnCellBecameNonscorable()
    {
        totalScorability--;
    }




}