using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    public bool IsInThePresent = true;
    bool switchedAlready = false;
    void update()
    {
        switchedAlready = false;
    }
    public void setSwitchedAlready(bool alreadySwitched)
    {
        switchedAlready = alreadySwitched;
    }
    public bool getSwitchedAlready()
    {
        return switchedAlready;
    }
}
