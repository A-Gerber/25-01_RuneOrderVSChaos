internal interface ICellGetable
{
    public bool TryGetCellByPosition(out ITakeable cell, LocalPosition localPosition);
}