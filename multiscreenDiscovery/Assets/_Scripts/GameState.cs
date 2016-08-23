using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameStates  {
	
	public class PlayerDescriptor
	{	
		
		public string playerName;
		public bool playerSeated;
		public int victoryPoints;

		public PlayerDescriptor(){
			playerName = "";
			playerSeated = false;
			victoryPoints = 0;
		}

		public PlayerDescriptor(string name, bool seat, int vP){
			playerName = name;
			playerSeated = seat;
			victoryPoints = vP;
		}

	}

	public class ZoneDescriptor
	{
		public string DataType = "ZoneDescriptor";
		public string gatewayZoneCategory;
		public string zoneGuid;
		public string playerGuid;

		public ZoneDescriptor(){
			gatewayZoneCategory = "none";
			zoneGuid = "none";
			playerGuid = "none";
		}

		public ZoneDescriptor(string gZc, string zG, string pG){
			gatewayZoneCategory = gZc;
			zoneGuid = zG;
			playerGuid = pG;
		}
	}

	public class PieceDescriptor
	{
		public string DataType = "PieceDescriptor";
		public string pieceGuid;
		public string zoneGuid;

		public string prefabDataPath;
		public string spriteDataPath;


		public PieceDescriptor(){
			pieceGuid = "none";
			zoneGuid = "none";

			prefabDataPath  = "none";
			spriteDataPath = "none";
		}

		public PieceDescriptor(string pG, string zG, string pDP, string sDP){
			pieceGuid = pG;
			zoneGuid = zG;

			prefabDataPath = pDP;
			spriteDataPath = sDP;
		}

	}



}
