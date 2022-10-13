using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace WG_Game
{
    public enum ResultType { Good = 0, Bad = 1}
    public enum ResultAction { Direct = 0, Indirect = 1 }
    public struct InteractionResult
    {
        public ResultType rType;

        public float HealthImpact;
        public float SpeedImpact;
        public float EnergyImpact;

        public string Message;
    }

    public static class ResultsModel
    {
        private static InteractionResult CalculateResult(string criteria)
        {
            InteractionResult result = new InteractionResult();
            result.rType = Random.Range(0.0f, 1.0f) < 0.5f ? ResultType.Bad : ResultType.Good;
            result.HealthImpact = 0.0f;
            result.SpeedImpact = 0.0f;
            result.EnergyImpact = Random.Range(30.0f, 50.0f) * (result.rType == ResultType.Good ? 0.1f : -1);
            result.Message = criteria + $". Energy {result.EnergyImpact:F2}";

            return result;
        }

        public static (InteractionResult?, InteractionResult?) CalculateResults(string inputCriteria)
        {
            //TODO generate result based on criteria

            int q = Random.Range(0, 2);

            if(q == 0)
            {
                return Random.Range(0, 2) == 0 ? (CalculateResult(inputCriteria + ". Type: Direct"), null) : (null, CalculateResult(inputCriteria + ". Type: Indirect"));
            }
            else
            {
                return (CalculateResult(inputCriteria + ". Type: Direct"), CalculateResult(inputCriteria + ". Type: Inirect"));
            }
        }
    }
}
