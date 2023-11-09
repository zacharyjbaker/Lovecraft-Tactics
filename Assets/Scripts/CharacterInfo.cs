using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile standingOnTile;

    public OverlayTile getCharTile()
    {
        return standingOnTile;
    }
}