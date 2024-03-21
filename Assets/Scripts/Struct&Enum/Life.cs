namespace Roguelike
{
    [System.Serializable]
    public struct Life
    {
        public uint Health;
        /*
        public uint Armor;
        public uint Shield;
        */
        public Life(Life copyData)
        {
            this.Health = copyData.Health;
            /*
            this.Armor = copyData.Armor;
            this.Shield = copyData.Shield;
            */
        }
    }
}
