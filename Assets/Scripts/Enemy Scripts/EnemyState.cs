﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyState
{
    void Enter();
    void Execute();
    void Exit();
}
