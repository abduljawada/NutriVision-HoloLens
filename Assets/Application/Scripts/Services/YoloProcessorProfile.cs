
using System;
using MixedReality.Toolkit;
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using Unity.Sentis;
using UnityEngine;

namespace YoloHolo.Services
{
    [CreateAssetMenu(menuName = "YoloProcessorProfile", fileName = "YoloProcessorProfile",
        order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class YoloProcessorProfile : BaseServiceProfile<IServiceModule>
    {
        [SerializeField]
        private ModelAsset model;
        public ModelAsset Model => model;

        [SerializeField]
        private float minimumProbability = 0.65f;
        public float MinimumProbability => minimumProbability;

        [SerializeField]
        private float overlapThreshold = 0.5f;
        public float OverlapThreshold => overlapThreshold;

        [SerializeField] 
        private int channels = 3;
        public int Channels => channels;

        [SerializeField]
        [Implements(typeof(IYoloClassTranslator), TypeGrouping.ByNamespaceFlat)]
        private SystemType classTranslator;

        [SerializeField]
        [Implements(typeof(IYoloClassTranslator), TypeGrouping.ByNamespaceFlat)]
        private SystemType arabicClassTranslator;

        private static IYoloClassTranslator translator;
        private static IYoloClassTranslator arabicTranslator;

        public IYoloClassTranslator ClassTranslator => 
            translator ??= (IYoloClassTranslator)Activator.CreateInstance(classTranslator);

        public IYoloClassTranslator ArabicClassTranslator =>
            arabicTranslator ??= (IYoloClassTranslator)Activator.CreateInstance(arabicClassTranslator);
    }
}
