using UnityEngine; 

public interface IPoolable
{ 
    public PrefabPool Pool { get; }    
    public GameObject GameObject { get; }
    public void SetPool(PrefabPool newPool);
}

 