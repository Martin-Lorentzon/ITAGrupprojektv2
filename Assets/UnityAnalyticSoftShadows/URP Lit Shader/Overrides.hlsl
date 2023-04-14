TEXTURE2D(_UASSTexture);
SAMPLER(sampler_UASSTexture);


half3 LightingPhysicallyBased_(BRDFData brdfData, BRDFData brdfDataClearCoat, Light light, half3 normalWS, half3 viewDirectionWS, half clearCoatMask, bool specularHighlightsOff, float2 screenUV)
{
	float4 uass = SAMPLE_TEXTURE2D_X(_UASSTexture, sampler_UASSTexture, screenUV);
	if (uass.a == 1.0)
		light.shadowAttenuation *= uass.x;
	return LightingPhysicallyBased(brdfData, brdfDataClearCoat, light.color, light.direction, light.distanceAttenuation * light.shadowAttenuation, normalWS, viewDirectionWS, clearCoatMask, specularHighlightsOff);
}


half4 UniversalFragmentPBR_(InputData inputData, SurfaceData surfaceData)
{
#if defined(_SPECULARHIGHLIGHTS_OFF)
	bool specularHighlightsOff = true;
#else
	bool specularHighlightsOff = false;
#endif
	BRDFData brdfData;

	// NOTE: can modify "surfaceData"...
	InitializeBRDFData(surfaceData, brdfData);

#if defined(DEBUG_DISPLAY)
	half4 debugColor;

	if (CanDebugOverrideOutputColor(inputData, surfaceData, brdfData, debugColor))
	{
		return debugColor;
	}
#endif

	// Clear-coat calculation...
	BRDFData brdfDataClearCoat = CreateClearCoatBRDFData(surfaceData, brdfData);
	half4 shadowMask = CalculateShadowMask(inputData);
	AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
	uint meshRenderingLayers = GetMeshRenderingLightLayer();
	Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);

	// NOTE: We don't apply AO to the GI here because it's done in the lighting calculation below...
	MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);

	LightingData lightingData = CreateLightingData(inputData, surfaceData);

	lightingData.giColor = GlobalIllumination(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask,
		inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS,
		inputData.normalWS, inputData.viewDirectionWS);

	if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
	{
		lightingData.mainLightColor = LightingPhysicallyBased_(brdfData, brdfDataClearCoat,
			mainLight,
			inputData.normalWS, inputData.viewDirectionWS,
			surfaceData.clearCoatMask, specularHighlightsOff, inputData.normalizedScreenSpaceUV);
	}

#if defined(_ADDITIONAL_LIGHTS)
	uint pixelLightCount = GetAdditionalLightsCount();

#if USE_CLUSTERED_LIGHTING
	for (uint lightIndex = 0; lightIndex < min(_AdditionalLightsDirectionalCount, MAX_VISIBLE_LIGHTS); lightIndex++)
	{
		Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

		if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
		{
			lightingData.additionalLightsColor += LightingPhysicallyBased_(brdfData, brdfDataClearCoat, light,
				inputData.normalWS, inputData.viewDirectionWS,
				surfaceData.clearCoatMask, specularHighlightsOff, inputData.normalizedScreenSpaceUV);
		}
	}
#endif

	LIGHT_LOOP_BEGIN(pixelLightCount)
		Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

	if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
	{
		lightingData.additionalLightsColor += LightingPhysicallyBased_(brdfData, brdfDataClearCoat, light,
			inputData.normalWS, inputData.viewDirectionWS,
			surfaceData.clearCoatMask, specularHighlightsOff, inputData.normalizedScreenSpaceUV);
	}
	LIGHT_LOOP_END
#endif

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
		lightingData.vertexLightingColor += inputData.vertexLighting * brdfData.diffuse;
#endif

	return CalculateFinalColor(lightingData, surfaceData.alpha);
}