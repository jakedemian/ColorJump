using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalPlatformData  {
	public static int lastActivePlatformId = -1;

	// platforms will notify this class when the player has landed on them
	public static bool NotifyLanded(GameObject platform){
		bool isNewActivePlatform = false;

		int thisPlatformId = platform.GetInstanceID();
		if(lastActivePlatformId != thisPlatformId){
			isNewActivePlatform = true;
			lastActivePlatformId = thisPlatformId;
		}
		
		return isNewActivePlatform;
	}
}
