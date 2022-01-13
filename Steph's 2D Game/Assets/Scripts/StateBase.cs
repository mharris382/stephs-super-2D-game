using UnityEngine;

namespace DefaultNamespace
{
    public class StateBase : IState
    {
        public virtual void Tick()
        {
            Debug.Log("Tick:" + this.GetType().Name);
        }

        public virtual void FixedTick()
        {
            Debug.Log("Fixed Tick:" + this.GetType().Name);
        }

        public virtual void OnEnter()
        {
            Debug.Log("Enter:" + this.GetType().Name);
        }

        public virtual void OnExit()
        {
            Debug.Log("Exit:" + this.GetType().Name);
        }
    }
}