/*
 * Author: Tygo de Vries
 * Contributors: See version control history
 *
 * Copyright (c) Tygo de Vries.
 * All rights reserved unless otherwise specified.
 *
 * Licensing and usage of this file are governed by the LICENSE
 * file in the root of the project, if present.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <Summary>
/// TODO: Add a description
/// </Summary>
[DisallowMultipleComponent]
public class ImageRotator : MonoBehaviour
{
    [SerializeField] private List<Texture> textures;
    private float timer;
    public void Update()
    {
        timer += Time.deltaTime * 0.5f;



        GetComponent<RawImage>().texture = textures[Mathf.FloorToInt(timer % textures.Count)];
    }
}
