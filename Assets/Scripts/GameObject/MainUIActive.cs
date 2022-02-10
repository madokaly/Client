using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[NotKeyable]
public class MainUIActive : PlayableAsset
{
    public bool active = true;
    public ActivationControlPlayable.PostPlaybackState postPlayback;
	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        var playable = ScriptPlayable<MainUIActivation>.Create(graph);
        playable.GetBehaviour().active = active;
        playable.GetBehaviour().postPlayback = postPlayback;
        return playable;

	}
}
