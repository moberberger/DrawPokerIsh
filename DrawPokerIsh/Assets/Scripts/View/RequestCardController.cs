public class RequestCardController
{
    public int RowIndex;
    public int ColumnIndex;
    public CardController CardController;

    public RequestCardController( int rowIndex, int columnIndex )
    {
        this.RowIndex = rowIndex;
        this.ColumnIndex = columnIndex;
    }
}