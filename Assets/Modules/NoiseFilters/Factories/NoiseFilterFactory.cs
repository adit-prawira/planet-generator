public static class NoiseFilterFactory
{
   public static INoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings)
   {
      switch (noiseSettings.filterType)
      {
         case NoiseSettings.FilterType.Rigid:
            return new RigidNoiseFilter(noiseSettings.rigidNoiseSettings);
         case NoiseSettings.FilterType.Simple:
            return new SimpleNoiseFilter(noiseSettings.simpleNoiseSettings);
         default:
            return null;
      }
   }
}
