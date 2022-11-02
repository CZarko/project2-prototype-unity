void ToonShading_float(in float3 Normal, in float RampSmoothness, in float3 ClipSpacePos, in float3 WorldPos, in float4 RampTinting, in float RampOffset, out float3 RampOutput, out float3 Direction) {
    #ifdef SHADERGRAPH_PREVIEW
      RampOutput = float3(0.5,0.5,0);
      Direction = float3(0.5,0.5,0);
    #else
      #if SHADOWS_SCREEN
        half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
      #else
        half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
      #endif

      #if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
        Light light = GetMainLight(shadowCoord);
      #else
        Light light = GetMainLight();
      #endif

      half d = dot(Normal, light.direction) * 0.5 + 0.5;

      half ramp = smoothstep(RampOffset, RampOffset + RampSmoothness, d);

      ramp *= light.shadowAttenuation;

      RampOutput = light.color * (ramp + RampTinting);

      Direction = light.direction;
    #endif
}
