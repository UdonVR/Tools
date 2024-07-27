
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Toolkit
{
    public class PrivacyZone : UdonSharpBehaviour
    {
        [Tooltip("default volume range outside of trigger.")]
        public float playerVolume = 25;

        [Tooltip("volume range inside of trigger.")]
        public float playerMin = 0;

        private bool localEnter = false;

        private int[] inside;
        private int[] outside;

        private int tmpPlayer = -1;

        public bool off = false;

        private void Start()
        {
            inside = new int[100];
            outside = new int[100];

            for (int i = 0; i < inside.Length; i++)
            {
                inside[i] = -1;
                outside[i] = -1;
            }
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (player == Networking.LocalPlayer)
            {
                localEnter = true;
                if (!off) LocalPlayerSwap();
                return;
            }

            playerEnter(player);
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (player == Networking.LocalPlayer)
            {
                localEnter = false;
                if (!off) LocalPlayerSwap();
                return;
            }

            playerLeave(player);
        }

        private void playerEnter(VRCPlayerApi player)
        {
            tmpPlayer = player.playerId;
            for (int i = 0; i < inside.Length; i++)
            {
                if (inside[i] == -1)
                {
                    inside[i] = tmpPlayer;
                    break;
                }
            }

            for (int i = 0; i < outside.Length; i++)
            {
                if (outside[i] == tmpPlayer)
                {
                    outside[i] = -1;
                    break;
                }
            }

            if (!off)
                player.SetVoiceDistanceFar(localEnter ? playerVolume : playerMin);
        }

        private void playerLeave(VRCPlayerApi player)
        {
            tmpPlayer = player.playerId;
            for (int i = 0; i < outside.Length; i++)
            {
                if (outside[i] == -1)
                {
                    outside[i] = tmpPlayer;
                    break;
                }
            }

            for (int i = 0; i < inside.Length; i++)
            {
                if (inside[i] == tmpPlayer)
                {
                    inside[i] = -1;
                    break;
                }
            }

            if (!off)
                player.SetVoiceDistanceFar(localEnter ? playerMin : playerVolume);
        }

        private VRCPlayerApi _targetPlayer;

        private void LocalPlayerSwap()
        {
            if (localEnter)
            {
                foreach (var _playerID in inside)
                {
                    if (_playerID == -1) continue;
                    _targetPlayer = VRCPlayerApi.GetPlayerById(_playerID);
                    if (_targetPlayer == null) continue;
                    if (VRC.SDKBase.Utilities.IsValid(_targetPlayer))
                    {
                        _targetPlayer.SetVoiceDistanceFar(playerVolume);
                    }
                }

                foreach (var _playerID in outside)
                {
                    if (_playerID == -1) continue;
                    _targetPlayer = VRCPlayerApi.GetPlayerById(_playerID);
                    if (_targetPlayer == null) continue;
                    if (VRC.SDKBase.Utilities.IsValid(_targetPlayer))
                    {
                        _targetPlayer.SetVoiceDistanceFar(playerMin);
                    }
                }
            }
            else
            {
                foreach (var _playerID in inside)
                {
                    if (_playerID == -1) continue;
                    _targetPlayer = VRCPlayerApi.GetPlayerById(_playerID);
                    if (VRC.SDKBase.Utilities.IsValid(_targetPlayer))
                    {
                        _targetPlayer.SetVoiceDistanceFar(playerMin);
                    }
                }

                foreach (var _playerID in outside)
                {
                    if (_playerID == -1) continue;
                    _targetPlayer = VRCPlayerApi.GetPlayerById(_playerID);
                    if (VRC.SDKBase.Utilities.IsValid(_targetPlayer))
                    {
                        _targetPlayer.SetVoiceDistanceFar(playerVolume);
                    }
                }
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            playerLeave(player);
        }

        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            playerLeave(player);
        }

        public void ToggleCollider(bool _state)
        {
            off = _state;

            if (off)
            {
                foreach (var _player in inside)
                {
                    if (VRC.SDKBase.Utilities.IsValid(_targetPlayer))
                        _targetPlayer.SetVoiceDistanceFar(playerVolume);
                }

                foreach (var _player in outside)
                {
                    if (VRC.SDKBase.Utilities.IsValid(_targetPlayer))
                        _targetPlayer.SetVoiceDistanceFar(playerVolume);
                }
            }
            else
            {
                LocalPlayerSwap();
            }
        }
    }
}