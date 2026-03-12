using System;
using System.Collections.Generic;
using UnityEngine;

public class TransparentChanger
{
    private const float TransparentValue = 0.6f;
    private const float FrozenTransparentValue = 0.4f;
    private const float OpaqueValue = 1f;
    private const int OpaqueRenderingMode = 0;
    private const int TransparentRenderingMode = 3;

    private readonly Renderer _cubeRenderer;
    private readonly Renderer _snowRenderer;
    private readonly List<Material> _materials = new();

    public TransparentChanger(Renderer cubeRenderer, Renderer snowRenderer)
    {
        _cubeRenderer = cubeRenderer != null ? cubeRenderer : throw new InvalidOperationException("cubeRenderer is null");
        _snowRenderer = snowRenderer != null ? snowRenderer : throw new InvalidOperationException("snowRenderer is null");

        _materials.Add(_cubeRenderer.materials[0]);
        _materials.Add(_snowRenderer.materials[0]);
    }

    internal void ChangeTransparent(bool isTransparent, bool isFrozen)
    {
        float transparentValue;

        if (isFrozen)
            transparentValue = FrozenTransparentValue;
        else 
            transparentValue = TransparentValue;

        if (isTransparent)
        {
            foreach (var material in _materials)
                BecomeTransparent(material, transparentValue);
        }
        else
        {
            foreach (var material in _materials)
                BecomeOpaque(material);
        }
    }

    private void BecomeTransparent(Material material, float transparentValue)
    {
        Color color = material.color;
        color.a = transparentValue;
        material.color = color;

        material.SetInt("_Mode", TransparentRenderingMode);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //_materials[0].SetInt("_ZWrite", 0); Z‑буфер (буфер глубины) если 0 то записи не будет в буфер (партиклы будут рисоваться поверх куба)
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private void BecomeOpaque(Material material)
    {
        Color color = material.color;
        color.a = OpaqueValue;
        material.color = color;

        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        //_materials[0].SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.SetInt("_Mode", OpaqueRenderingMode);

    }
}