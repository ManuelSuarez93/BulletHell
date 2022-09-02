using UnityEngine; 

namespace Pooling
{
    public interface IPoolable
    {   
        public void SetPool(PrefabPool newPool);
    }

}

 