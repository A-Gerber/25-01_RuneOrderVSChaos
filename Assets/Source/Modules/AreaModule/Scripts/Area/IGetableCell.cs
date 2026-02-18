public interface IGetableCell
{
    bool TryGetCellByCoordinate(out ITakeable cell, LocalPosition position);
}