using System;

namespace Wpf2p2p
{
	public class Region
    {
		public int ID { get; set; }
		public string Name { get; set; }

		public Region(string ID, string Name)
		{
			this.ID = Convert.ToInt32(ID);
			this.Name = Name;
		}
    }
}