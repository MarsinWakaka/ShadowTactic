// #define InputTest
//
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
//
// public class InteractionTest : MonoBehaviour
// {
//     public Player player;
//
//     private HealthController HitTest;
//
//     void Start()
//     {
//         HitTest = player.healthController;
//         if (HitTest == null)
//         {
//             Debug.LogWarning($"{HitTest} didn't has Component of HealthController");
//         }
//     }
//
//     void Update()
//     {
// #if (InputTest)
// if (Input.GetKeyDown(KeyCode.J))
//         {
//             HitTest?.Hurt(10);
//         }
//         if (Input.GetKeyDown(KeyCode.H))
//         {
//             HitTest?.Recover(50);
//         }
//         if (Input.GetKeyDown(KeyCode.N))
//         {
//             GameManager.Instance.NextScene();
//         }
//         if (Input.GetKeyDown(KeyCode.P))
//         {
//             GameObject.Instantiate(player.HurtFX, player.transform, true);
//         }
// #endif
//     }
// }
