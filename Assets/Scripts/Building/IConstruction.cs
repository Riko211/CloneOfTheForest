namespace Building
{
    public interface IConstruction
    {
        public void MoveBlueprint();
        public void Construct();
        public bool CanBeConstructed();
    }
}