
public class BoardRow
{
    int y;
    int width;

    Cell[] cells;

    int totalScorability;

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