﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameToJoin {
	public string LocalIp;
	public string roomName;
	public GameToJoin(string ip, string name){
		LocalIp = ip;
		roomName = name;
	}


}
