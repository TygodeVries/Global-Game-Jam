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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <Summary>
/// TODO: Add a description
/// </Summary>
[DisallowMultipleComponent]
public class JoinHint : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private TMP_Text text;

    private float alpha = 0;
    public void Update()
    {
        float alphaGoal;
        PlayerMovement[] players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        if (players.Length == 0)
        {
            alphaGoal = 1;
        }
        else
        {
            alphaGoal = 0.4f;
        }

        alpha = Mathf.Lerp(alpha, alphaGoal, Time.deltaTime);
        image.color = Color.white * alpha;
        text.color = Color.white * alpha;

    }
}
