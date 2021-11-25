using System;

namespace Build
{
    public class Building
    {
        public static uint countBuilding = 0;
        private uint id = 137742; // first ID
        private float height;
        private byte numberStoreys;
        private ushort numberFlats;
        private byte numberEntrance;
        #region GetSet methods
        public uint GetID()
        {
            return id;
        }
        public float GetHeight()
        {
            return height;
        }
        public void SetHeight(float height)
        {
            if (height < 3)
            {
                System.Console.WriteLine("Error. Heightof Buildings > 3m");
            }
            else
            {
                this.height = height;
            }
        }
        public byte GetNummberStoreys()
        {
            return numberStoreys;
        }
        public void SetNumberStoreys(byte numberStoreys)
        {
            this.numberStoreys = numberStoreys;
        }
        public ushort GetNumberFlats()
        {
            return numberFlats;
        }
        public void SetNumberFlats(ushort numberFlats)
        {
            this.numberFlats = numberFlats;
        }
        public byte GetNumberEntrance()
        {
            return numberEntrance;
        }
        public void SetNumberEntrance(byte numberEntrance)
        {
            this.numberEntrance = numberEntrance;
        }
        #endregion

        public float GetHeightOfStory()
        {
            return height / numberStoreys;
        }
        public int GetAverageCountFlatsInEntrance()
        {
            return numberFlats / numberEntrance;
        }
        public int GetAverageCountFlatsOnStorey()
        {
            return numberFlats / numberStoreys;
        }
        internal Building()
        {
            id += countBuilding;
            countBuilding++;
        }
        internal Building(float height, byte numberStoreys)
        {
            this.height = height;
            this.numberEntrance = 1;
            this.numberStoreys = numberStoreys;
            this.numberFlats = numberStoreys;
        }
        internal Building(float height, byte numberStoreys, byte numberEntrance, ushort numberFlats) : this()
        {
            this.height = height;
            this.numberEntrance = numberEntrance;
            this.numberStoreys = numberStoreys;
            this.numberFlats = numberFlats;
        }
        public void Display()
        {
            Console.WriteLine("ID: " + GetID());
            Console.WriteLine("Height: " + GetHeight());
            Console.WriteLine("Entrance: " + GetNumberEntrance());
            Console.WriteLine("Flats: " + GetNumberFlats());
            Console.WriteLine("Storeys: " + GetNummberStoreys());
        }
    }
}
