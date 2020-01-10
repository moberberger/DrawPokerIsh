public class SetInitialCard
{
    public int RowIndex;
    public int ColumnIndex;
    public int CardId;

    public SetInitialCard( int rowIndex, int columnIndex, int cardId )
    {
        this.RowIndex = rowIndex;
        this.ColumnIndex = columnIndex;
        this.CardId = cardId;
    }

    public int CardIndex => RowIndex * 5 + ColumnIndex;
}