﻿using System;

namespace DefaultNamespace
{
    
    public interface IState
    {
        public void Tick();
        public void FixedTick();
        public void OnEnter();
        public void OnExit();
    }

    
}