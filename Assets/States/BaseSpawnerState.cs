using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawnerState {

    public abstract void EnterState(SpawnerScript ss);
    public abstract void UpdaterState(SpawnerScript ss);

}
