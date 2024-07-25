#ifndef US_KEYDEFINES_INCLUDED
#define US_KEYDEFINES_INCLUDED

#define X_FEATURES defined(UBERX)

#define FORWARD_PASS defined(UNITY_PASS_FORWARDBASE)

#define ADDITIVE_PASS defined(UNITY_PASS_FORWARDADD)

#define SHADOW_PASS defined(UNITY_PASS_SHADOWCASTER)

#define OUTLINE_PASS defined(OUTLINE)

#define VERTEX_LIGHT defined(VERTEXLIGHT_ON)

#define ALPHA_TEST defined(_ALPHATEST_ON)

#define ALPHA_BLEND defined(_ALPHABLEND_ON)

#define ALPHA_PREMULTIPLY defined(_ALPHAPREMULTIPLY_ON)

#define NON_OPAQUE_RENDERING defined(_ALPHATEST_ON) || defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)

#define TRANSPARENT_RENDERING defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)

#define SHADING_ENABLED defined(_SHADING_ON)

#define PACKED_WORKFLOW defined(_PACKED_WORKFLOW_ON)

#define SPECULAR_WORKFLOW defined(_SPECULAR_WORKFLOW_ON)

#define DEFAULT_WORKFLOW !defined(_PACKED_WORKFLOW_ON) && !defined(_SPECULAR_WORKFLOW_ON)

#define REFLECTIONS_ENABLED defined(_REFLECTIONS_ON)

#define SSR_ENABLED defined(_SCREENSPACE_REFLECTIONS_ON)

#define CUBEMAP_REFLECTIONS defined(_CUBEMAP_REFLECTIONS_ON)

#define SPECULAR_ENABLED defined(_SPECULAR_ON)

#define ANISO_SPECULAR defined(_SPECULAR_ANISO_ON)

#define COMBINED_SPECULAR defined(_SPECULAR_COMBINED_ON)

#define GGX_SPECULAR !defined(_SPECULAR_ANISO_ON) && !defined(_SPECULAR_COMBINED_ON)

#define NORMALMAP_ENABLED defined(_NORMALMAP_ON)

#define DETAIL_NORMALMAP_ENABLED defined(_DETAIL_NORMALMAP_ON)

#define EMISSION_ENABLED defined(_EMISSION_ON)

#define PULSE_ENABLED defined(_PULSE_ON)

#define PARALLAX_ENABLED defined(_PARALLAXMAP_ON)

#define FILTERING_ENABLED defined(_FILTERING_ON)

#define POST_FILTERING_ENABLED defined(_POST_FILTERING_ON)

#define PBR_PREVIEW_ENABLED defined(_PBR_PREVIEW_ON)

#define SEPARATE_MASKING defined(_SEPARATE_MASKING_ON)

#define PACKED_MASKING defined(_PACKED_MASKING_ON)

#define UV_DISTORTION_ENABLED defined(_UV_DISTORTION_ON)

#define UV_DISTORTION_NORMALMAP defined(_UV_DISTORTION_NORMALMAP_ON)

#define DISSOLVE_TEXTURE defined(_DISSOLVE_TEXTURE_ON)

#define DISSOLVE_GEOMETRY defined(_DISSOLVE_GEOMETRY_ON)

#define DISSOLVE_SIMPLEX defined(_DISSOLVE_SIMPLEX_ON)

#define DISSOLVE_ENABLED defined(_DISSOLVE_TEXTURE_ON) || defined(_DISSOLVE_GEOMETRY_ON) || defined(_DISSOLVE_SIMPLEX_ON)

#define CUBEMAP_ENABLED defined(_CUBEMAP_ON)

#define COMBINED_CUBEMAP_ENABLED defined(_CUBEMAP_COMBINED_ON)

#define MATCAP_ENABLED defined(_MATCAP_ON)

#define ENVIRONMENT_RIM_ENABLED defined(_ENVIRONMENT_RIM_ON)

#define SPRITESHEETS_ENABLED defined(_FLIPBOOK_ON)

#define CLONES_ENABLED defined(_CLONES_ON)

#define REFRACTION_ENABLED defined(_REFRACTION_ON)

#define REFRACTION_CA_ENABLED defined(_CHROMATIC_ABBERATION_ON)

#define VERTEX_MANIP_ENABLED defined(_VERTEX_MANIP_ON)

#define MASK_SOS_ENABLED defined(_MASK_TRANSFORMS_ON)

#define REFLCUBE_EXISTS defined(_REFLECTION_FALLBACK_ON)

#define BCDISSOLVE_ENABLED defined(_BASECOLOR_DISSOLVE_ON)

#define AUDIOLINK_ENABLED defined(_AUDIOLINK_ON)

#define RIM_ENABLED defined(_RIM_ON)

#define SUBSURFACE_ENABLED defined(_SUBSURFACE_ON)

#endif // US_KEYDEFINES_INCLUDED