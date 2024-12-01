using System.Collections.Generic;

namespace Quatrimo
{
    public class updateBoard : scoreOperation
    {
        List<int> updatedRows;

        public updateBoard(List<int> rows)
        {
            updatedRows = rows;
        }

        public override void execute(encounter encounter)
        {
            foreach(int row in updatedRows)
            {
                encounter.updatedRows.Add(row);
            }
        }

    }
}
