using UnityEngine;

namespace UnityEngine.Animations
{
    public static class PositionConstraintExtension 
    {
        public static void ClearSources(this PositionConstraint positionConstraint)
        {
            for(int i = 0; i < positionConstraint.sourceCount; i++)
            {
                positionConstraint.RemoveSource(i);
            }
        }
    }
}
